import React, { useMemo, useState } from "react";
import { ReactTable } from "../../../common/ReactTable";
import { Cell } from "react-table";
import { CricketMatch } from "../Models/Interface";
import {
  ReactSlidingSidePanel,
  SidePanelType,
} from "../../../common/ReactSlidingSidePanel";
import { MatchDetails } from "../MatchDetails/MatchDetails";

import "./MatchRecordsTable.scss";

export interface MatchRecordsTableProps {
  matchData: CricketMatch[];
}

export const MatchRecordsTable: React.FunctionComponent<
  MatchRecordsTableProps
> = ({ matchData }) => {
  const columns = useMemo(
    () => [
      {
        Header: "Match SN",
        accessor: "matchNo",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
      },
      {
        Header: "Team 1",
        accessor: "team1.teamName",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        width: 150,
      },
      {
        Header: "Team 2",
        accessor: "team2.teamName",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        width: 150,
      },
      {
        Header: "Date",
        accessor: "matchDate",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        width: 150,
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
      },
      {
        Header: "Overs",
        accessor: "matchDays",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        width: 200,
      },
    ],
    []
  );

  const [isOpenSidePanel, toggleSideOpenPanel] = useState(false);
  const [selectedMatchUuid, setSelectedmatchUuid] = useState("");

  return (
    <ReactTable
      className={"t20-match-records"}
      data={matchData}
      columns={columns}
      perPages={[10, 25, 50, 100]}
      children={
        <ReactSlidingSidePanel
          isOpen={isOpenSidePanel}
          setIsOpen={toggleSideOpenPanel}
          sidePanelType={SidePanelType.Right}
          panelWidth={100}
          children={
            <MatchDetails
              matchData={
                matchData.find(
                  (m) => m.matchUuid === selectedMatchUuid
                ) as CricketMatch
              }
            />
          }
        />
      }
      handleRowClick={(e, r) => {
        setSelectedmatchUuid(r.original.matchUuid);
        toggleSideOpenPanel(!isOpenSidePanel);
      }}
    />
  );
};
