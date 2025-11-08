import React, { useMemo, useState } from "react";
import { ReactTable, tableOptions } from "../../../common/ReactTable";
import { Cell } from "react-table";
import { CricketTeam, TestCricketMatch } from "../Models/Interface";
import {
  ReactSlidingSidePanel,
  SidePanelType,
} from "../../../common/ReactSlidingSidePanel";
import { TestMatchDetails } from "./../TestMatchDetails/TestMatchDetails";

import "./TestMatchRecordsTable.scss";
import {
  NumberRangeColumnFilter,
  SelectColumnFilter,
  SelectYearColumnFilter,
} from "../../../common/Filter";

export interface TestMatchRecordsTableProps {
  isLoading: boolean;
  teamData: CricketTeam[];
  matchData: TestCricketMatch[];
}

export const TestMatchRecordsTable: React.FunctionComponent<
  TestMatchRecordsTableProps
> = ({ isLoading, teamData, matchData }) => {
  const columns = useMemo(
    () => [
      {
        Header: "Match SN",
        accessor: "matchNo",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        width: 200,
        Filter: NumberRangeColumnFilter,
      },
      {
        Header: "Series",
        accessor: "series",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        width: 300,
        Filter: SelectColumnFilter,
      },
      {
        Header: "Team 1",
        accessor: "team1.teamName",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        width: 150,
        Filter: SelectColumnFilter,
      },
      {
        Header: "Team 2",
        accessor: "team2.teamName",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        width: 150,
        Filter: SelectColumnFilter,
      },
      {
        Header: "Date",
        accessor: "matchDate",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        width: 150,
        Filter: SelectYearColumnFilter,
      },
      {
        Header: "Result",
        accessor: "result",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        width: 250,
      },
      {
        Header: "Venue",
        accessor: "venue",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        width: 200,
        Filter: SelectColumnFilter,
      },
    ],
    []
  );

  const [isOpenSidePanel, toggleSideOpenPanel] = useState(false);
  const [selectedMatchUuid, setSelectedmatchUuid] = useState("");

  const currentMatchData = matchData.find(
    (m) => m.matchUuid === selectedMatchUuid
  ) as TestCricketMatch;

  return (
    <ReactTable
      className={`international-match-records`}
      data={matchData}
      columns={columns}
      perPages={[10, 25, 50, 100]}
      options={{
        ...tableOptions,
        isRowSelect: false,
      }}
      children={
        <ReactSlidingSidePanel
          isOpen={isOpenSidePanel}
          setIsOpen={toggleSideOpenPanel}
          sidePanelType={SidePanelType.Right}
          panelWidth={100}
          children={
            <TestMatchDetails
              teamData={teamData.filter(
                (t) =>
                  t.teamName === currentMatchData?.team1.teamName ||
                  t.teamName === currentMatchData?.team2.teamName
              )}
              matchData={currentMatchData}
            />
          }
        />
      }
      handleRowClick={(e, r) => {
        setSelectedmatchUuid(r.original.matchUuid);
        toggleSideOpenPanel(!isOpenSidePanel);
      }}
      isLoading={isLoading}
    />
  );
};
