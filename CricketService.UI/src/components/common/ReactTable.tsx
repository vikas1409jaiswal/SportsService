import React, {
  Children,
  CSSProperties,
  useEffect,
  useMemo,
  useState,
} from "react";
import {
  TableInstance,
  useTable,
  useSortBy,
  usePagination,
  useFilters,
  useGlobalFilter,
  useRowSelect,
  useColumnOrder,
  useResizeColumns,
  useBlockLayout,
  ColumnInstance,
} from "react-table";
import styled, { CSSProp } from "styled-components";
import { DefaultColumnFilter, GlobalFilter } from "./Filter";
import $ from "jquery";

import "./ReactTable.scss";
import { TailSpinLoader } from "./Loader";

interface ReactTableCssOptions {
  headerCss: CSSProperties;
  footerCss: CSSProperties;
  rowCss: CSSProperties;
  cellCss: CSSProperties;
  highlightedRowCss: CSSProperties;
}

export interface ReactTableOptions {
  css: ReactTableCssOptions;
  isPagination: boolean;
  isFooter: boolean;
  isSelectionPanel: boolean;
  isStickyColumn: boolean;
  isRowSelect: boolean;
  isColumnFilter: boolean;
}

interface ReactTableProps {
  columns: any;
  data: any;
  handleRowClick: (e: any, r: any) => void;
  perPages?: number[];
  options?: ReactTableOptions;
  className?: string;
  children?: any;
  isLoading?: boolean;
}

interface CustomTableInstance<T extends Record<string, unknown>>
  extends TableInstance<T> {
  page: any;
  canPreviousPage: boolean;
  canNextPage: boolean;
  pageOptions: any;
  pageCount: number;
  gotoPage: any;
  nextPage: any;
  previousPage: any;
  setPageSize: any;
  state: any;
  pageIndex: number;
  preGlobalFilteredRows: number;
  setGlobalFilter: any;
  defaultColumn: any;
  selectedFlatRows: any;
  setColumnOrder: any;
  resetResizing: any;
  isAllRowsSelected: boolean;
}

export const tableOptions: ReactTableOptions = {
  css: {
    headerCss: {
      backgroundColor: "white",
      color: "black",
      margin: "20px",
      border: "1px solid black",
    },
    footerCss: {
      backgroundColor: "skyblue",
      color: "white",
      margin: "20px",
      border: "2px solid black",
    },
    rowCss: {
      backgroundColor: "honeydew",
      color: "black",
      margin: "20px",
      border: "2px solid black",
    },
    cellCss: {
      backgroundColor: "honeydew",
      color: "black",
      margin: "20px",
      border: "2px solid black",
      width: 50,
    },
    highlightedRowCss: {
      backgroundColor: "orange",
      color: "white",
      border: "2px solid black",
      width: 50,
    },
  },
  isPagination: true,
  isFooter: false,
  isSelectionPanel: true,
  isStickyColumn: true,
  isRowSelect: true,
  isColumnFilter: true,
};

interface IndeterminateCheckboxProps {
  indeterminate?: any;
  reference?: any;
}

const IndeterminateCheckbox: React.FunctionComponent<IndeterminateCheckboxProps> =
  React.forwardRef(({ indeterminate, ...rest }, reference) => {
    const defaultRef = React.useRef();
    const resolvedRef: any = reference || defaultRef;

    React.useEffect(() => {
      resolvedRef.current.indeterminate = indeterminate;
    }, [resolvedRef, indeterminate]);

    return (
      <>
        <input type="checkbox" ref={resolvedRef} {...rest} />
      </>
    );
  });

export const ReactTable: React.FunctionComponent<ReactTableProps> = ({
  columns,
  data,
  perPages,
  options = tableOptions,
  className,
  children,
  isLoading,
  handleRowClick,
}) => {
  const {
    css,
    isPagination,
    isFooter,
    isSelectionPanel,
    isStickyColumn,
    isRowSelect,
    isColumnFilter,
  } = options as ReactTableOptions;

  const { headerCss, footerCss, rowCss, cellCss, highlightedRowCss } =
    css as ReactTableCssOptions;

  const [isEnableColumnFilter, toggleStateColumnFilter] =
    useState(isColumnFilter);
  const [isEnableColumnSetting, toggleStateColumnSetting] = useState(false);
  const [highlightedRowIndex, setHighlightedRowIndex] = useState(-1);

  const defaultColumn: any = useMemo(
    () => ({
      Filter: DefaultColumnFilter,
      minWidth: 30,
      width: 100,
      maxWidth: 400,
    }),
    []
  );

  function shuffle(arr: any) {
    arr = [...arr];
    const shuffled = [];
    while (arr.length) {
      const rand = Math.floor(Math.random() * arr.length);
      shuffled.push(arr.splice(rand, 1)[0]);
    }
    return shuffled;
  }

  const randomizeColumns = () => {
    setColumnOrder(shuffle(visibleColumns.map((d) => d.id)));
  };

  const {
    page,
    allColumns,
    getToggleHideAllColumnsProps,
    getTableProps,
    getTableBodyProps,
    headerGroups,
    prepareRow,
    canPreviousPage,
    canNextPage,
    pageOptions,
    pageCount,
    gotoPage,
    nextPage,
    previousPage,
    setPageSize,
    isAllRowsSelected,
    selectedFlatRows,
    state: { pageIndex, pageSize, globalFilter, selectedRowIds },
    preGlobalFilteredRows,
    setGlobalFilter,
    visibleColumns,
    setColumnOrder,
    resetResizing,
  } = useTable(
    {
      columns,
      data,
      defaultColumn,
    },
    useFilters,
    useGlobalFilter,
    useSortBy,
    usePagination,
    useRowSelect,
    (hooks) => {
      isRowSelect &&
        hooks.visibleColumns.push((columns) => [
          {
            id: "selection",
            Header: ({ getToggleAllRowsSelectedProps }: any) => (
              <div style={{ width: 25 }}>
                <IndeterminateCheckbox {...getToggleAllRowsSelectedProps()} />
              </div>
            ),
            Cell: ({ row }: any) => (
              <div style={{ width: 25 }}>
                <IndeterminateCheckbox {...row.getToggleRowSelectedProps()} />
              </div>
            ),
            width: 50,
          },
          ...columns,
        ]);
    },
    useColumnOrder,
    useResizeColumns,
    useBlockLayout
  ) as CustomTableInstance<Record<string, unknown>>;

  useEffect(() => {
    !isPagination && setPageSize(10000);
  }, [isPagination]);

  const footerGroups = headerGroups.slice().reverse();

  $(document).ready(function () {
    $(".react-table-data-rows").each(function (index, element) {
      if (index === highlightedRowIndex) {
        $(element).css(
          "background-color",
          highlightedRowCss.backgroundColor as string
        );
        $(element).css("opacity", "0.8");
      } else {
        $(element).css("background-color", "white");
        $(element).css("opacity", "1");
      }
    });
  });

  return (
    <>
      <div className={`react-table ${className}`}>
        {isSelectionPanel && (
          <div className="react-table-selection-panel">
            <div className="button-panel">
              <button onClick={() => randomizeColumns()}>
                Randomize Columns
              </button>
              <button onClick={resetResizing}>Reset Resizing</button>
              <button
                onClick={() => toggleStateColumnFilter(!isEnableColumnFilter)}
              >
                {`${isEnableColumnFilter ? "Disable" : "Enable"}`} Column Filter
              </button>
            </div>
            <div className="global-search-filter">
              <GlobalFilter
                globalFilter={globalFilter}
                preGlobalFilteredRows={preGlobalFilteredRows}
                setGlobalFilter={setGlobalFilter}
              />
            </div>
          </div>
        )}
        <div className="react-table-container">
          <div className="table-preceder-panel">
            <p>
              {isRowSelect ? (
                <span>
                  {`${Object.keys(selectedRowIds).length}`} rows selected.
                </span>
              ) : (
                <span>Row selection disable.</span>
              )}
              <button
                onClick={() => toggleStateColumnSetting(!isEnableColumnSetting)}
              >
                ...
              </button>
              <div
                className="column-setting-panel"
                style={{
                  visibility: isEnableColumnSetting ? "visible" : "hidden",
                }}
              >
                {isEnableColumnSetting && (
                  <div id="all-select">
                    <IndeterminateCheckbox
                      {...getToggleHideAllColumnsProps()}
                    />{" "}
                    Toggle All
                  </div>
                )}
                {isEnableColumnSetting &&
                  allColumns.map((column, i) => (
                    <div key={column.id}>
                      <label>
                        <input
                          type="checkbox"
                          {...column.getToggleHiddenProps()}
                        />{" "}
                        {isRowSelect
                          ? i > 0
                            ? column.Header?.toString()
                            : "Row Selection"
                          : column.Header?.toString()}
                      </label>
                    </div>
                  ))}
              </div>
            </p>
          </div>
          <table {...getTableProps()}>
            <thead>
              {headerGroups.map((headerGroup) => (
                <tr {...headerGroup.getHeaderGroupProps()}>
                  {headerGroup.headers.map((column: any) => (
                    <th
                      {...column.getHeaderProps(column.getSortByToggleProps())}
                    >
                      <>
                        <div className="column-name-header">
                          {column.render("Header")}
                          {isStickyColumn && (
                            <div
                              {...column.getResizerProps()}
                              className={`resizer ${
                                column.isResizing ? "isResizing" : ""
                              }`}
                            />
                          )}
                          <span>
                            {column.isSorted
                              ? column.isSortedDesc
                                ? " 🔽"
                                : " 🔼"
                              : ""}
                          </span>
                        </div>
                        {isEnableColumnFilter && (
                          <div className="column-filter">
                            {column.canFilter ? column.render("Filter") : null}
                          </div>
                        )}
                      </>
                    </th>
                  ))}
                </tr>
              ))}
            </thead>
            <tbody {...getTableBodyProps()}>
              {isLoading && <TailSpinLoader />}
              {!isLoading &&
                page.map((row: any, i: number) => {
                  prepareRow(row);
                  return (
                    <tr
                      {...row.getRowProps()}
                      onMouseOver={() => setHighlightedRowIndex(i)}
                      onMouseOut={() => setHighlightedRowIndex(-1)}
                      className="react-table-data-rows"
                    >
                      {row.cells.map((cell: any) => {
                        return (
                          <td
                            {...cell.getCellProps()}
                            onClick={(e: any) => {
                              console.log(row);
                              cell.column.id !== "selection" &&
                                handleRowClick(e, row);
                            }}
                          >
                            {cell.render("Cell")}
                          </td>
                        );
                      })}
                    </tr>
                  );
                })}
            </tbody>
            <tfoot>
              {isFooter &&
                footerGroups.map((footerGroup) => (
                  <tr {...footerGroup.getHeaderGroupProps()}>
                    {footerGroup.headers.map((column: any) => (
                      <th
                        {...column.getHeaderProps(
                          column.getSortByToggleProps()
                        )}
                      >
                        {column.render("Footer")}
                      </th>
                    ))}
                  </tr>
                ))}
            </tfoot>
          </table>
        </div>
        {isPagination && (
          <div className="react-pagination-container">
            <button onClick={() => gotoPage(0)} disabled={!canPreviousPage}>
              {"<<"}
            </button>{" "}
            <button onClick={() => previousPage()} disabled={!canPreviousPage}>
              {"<"}
            </button>{" "}
            <button onClick={() => nextPage()} disabled={!canNextPage}>
              {">"}
            </button>{" "}
            <button
              onClick={() => gotoPage(pageCount - 1)}
              disabled={!canNextPage}
            >
              {">>"}
            </button>{" "}
            <span>
              Page{" "}
              <strong>
                {pageIndex + 1} of {pageOptions.length}
              </strong>{" "}
            </span>
            <span>
              | Go to page:{" "}
              <input
                type="number"
                defaultValue={pageIndex + 1}
                onChange={(e) => {
                  const page = e.target.value ? Number(e.target.value) - 1 : 0;
                  gotoPage(page);
                }}
                style={{ width: "100px" }}
              />
            </span>{" "}
            <select
              value={pageSize}
              onChange={(e) => {
                setPageSize(Number(e.target.value));
              }}
            >
              {perPages?.map((pageSize) => (
                <option key={pageSize} value={pageSize}>
                  Show {pageSize}
                </option>
              ))}
            </select>
          </div>
        )}
        {children}
      </div>
    </>
  );
};
