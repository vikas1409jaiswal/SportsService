import React from "react";
import {
  useReactTable,
  getCoreRowModel,
  getPaginationRowModel,
  flexRender,
  ColumnDef,
  TableOptions,
} from "@tanstack/react-table";
import "./ReactTableV2.scss";

interface ReactTableV2Props<T extends object> {
  columns: ColumnDef<T, any>[];
  data: T[];
  pageSize?: number;
  title?: string;
}

export function ReactTableV2<T extends object>({ columns, data, pageSize = 12, title }: ReactTableV2Props<T>) {
  const table = useReactTable({
    data,
    columns,
    getCoreRowModel: getCoreRowModel(),
    getPaginationRowModel: getPaginationRowModel(),
    initialState: { pagination: { pageSize } },
  } as TableOptions<T>);

  return (
    <div className="common-data-table-container" style={{ maxHeight: "80vh", overflow: "auto" }}>
      {title && <h2>{title}</h2>}
      <div style={{ overflowX: "auto" }}>
        <table>
          <thead>
            {table.getHeaderGroups().map(headerGroup => (
              <tr key={headerGroup.id}>
                {headerGroup.headers.map(header => (
                  <th key={header.id}>
                    {flexRender(header.column.columnDef.header, header.getContext())}
                  </th>
                ))}
              </tr>
            ))}
          </thead>
          <tbody>
            {table.getRowModel().rows.length > 0 ? (
              table.getRowModel().rows.map(row => (
                <tr key={row.id}>
                  {row.getVisibleCells().map(cell => (
                    <td key={cell.id}>
                      {flexRender(
                        cell.column.columnDef.cell ?? (() => cell.getValue()),
                        cell.getContext()
                      )}
                    </td>
                  ))}
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan={columns.length} className="no-matches">No data found.</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
      <div className="pagination">
        <button onClick={() => table.setPageIndex(0)} disabled={!table.getCanPreviousPage()}>&laquo; First</button>
        <button onClick={() => table.previousPage()} disabled={!table.getCanPreviousPage()}>&lsaquo; Prev</button>
        <span>
          Page <strong>{table.getState().pagination.pageIndex + 1} of {table.getPageCount()}</strong>
        </span>
        <button onClick={() => table.nextPage()} disabled={!table.getCanNextPage()}>Next &rsaquo;</button>
        <button onClick={() => table.setPageIndex(table.getPageCount() - 1)} disabled={!table.getCanNextPage()}>Last &raquo;</button>
      </div>
    </div>
  );
}