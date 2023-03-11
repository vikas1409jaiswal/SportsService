import React, { useContext, useMemo, useState } from "react";
import { ReactTable, tableOptions } from "../../../common/ReactTable";
import { Cell } from "react-table";
import {
  NumberRangeColumnFilter,
  SliderColumnFilter,
} from "../../../common/Filter";
import {
  ReactSlidingSidePanel,
  SidePanelType,
} from "../../../common/ReactSlidingSidePanel";
import { TeamDetails } from "../TeamDetails/TeamDetails";
import { CricketTeamData } from "../Models/Interface";
import { CricketFormat } from "../../CricketMatchRecords/Models/Interface";

import "./ODIRecords.scss";
import { TailSpinLoader } from "../../../common/Loader";

export interface ODIRecordsProps {
  isLoading: boolean;
  teamData: CricketTeamData[];
}

export const ODIRecords: React.FunctionComponent<ODIRecordsProps> = ({
  teamData,
  isLoading,
}) => {
  const columns = useMemo(
    () => [
      {
        Header: "Team Name",
        Footer: "Total",
        accessor: "teamName",
        Cell: (cell: Cell) => (
          <div>
            <img
              src={
                teamData.find((t) => t.teamName === cell.row.values.teamName)
                  ?.flagUri
              }
              style={{ width: 80, height: 50, border: "2px solid black" }}
            />
            <h6>{cell.value}</h6>
          </div>
        ),
        width: 300,
      },
      {
        Header: "Matches",
        accessor: "teamRecordDetails.odiResults.matches",
        Footer: (info: any) => {
          const total = React.useMemo(
            () =>
              info.rows.reduce(
                (sum: number, row: any) =>
                  row.values["teamRecordDetails.odiResults.matches"] + sum,
                0
              ),
            [info.rows]
          );

          return <>{total / 2}</>;
        },
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        Filter: NumberRangeColumnFilter,
        filter: "between",
      },
      {
        Header: "Won",
        accessor: "teamRecordDetails.odiResults.won",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        Footer: (info: any) => {
          const total = React.useMemo(
            () =>
              info.rows.reduce(
                (sum: number, row: any) =>
                  row.values["teamRecordDetails.odiResults.won"] + sum,
                0
              ),
            [info.rows]
          );

          return <>{total}</>;
        },
        Filter: NumberRangeColumnFilter,
        filter: "between",
      },
      {
        Header: "Lost",
        accessor: "teamRecordDetails.odiResults.lost",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        Footer: (info: any) => {
          const total = React.useMemo(
            () =>
              info.rows.reduce(
                (sum: number, row: any) =>
                  row.values["teamRecordDetails.odiResults.lost"] + sum,
                0
              ),
            [info.rows]
          );

          return <>{total}</>;
        },
        Filter: NumberRangeColumnFilter,
        filter: "between",
      },
      {
        Header: "Tied and Won",
        accessor: "teamRecordDetails.odiResults.tiedAndWon",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        Footer: (info: any) => {
          const total = React.useMemo(
            () =>
              info.rows.reduce(
                (sum: number, row: any) =>
                  row.values["teamRecordDetails.odiResults.tiedAndWon"] + sum,
                0
              ),
            [info.rows]
          );

          return <>{total}</>;
        },
      },
      {
        Header: "Tied and Lost",
        accessor: "teamRecordDetails.odiResults.tiedAndLost",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        Footer: (info: any) => {
          const total = React.useMemo(
            () =>
              info.rows.reduce(
                (sum: number, row: any) =>
                  row.values["teamRecordDetails.odiResults.tiedAndLost"] + sum,
                0
              ),
            [info.rows]
          );

          return <>{total}</>;
        },
      },
      {
        Header: "No Result",
        accessor: "teamRecordDetails.odiResults.noResult",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        Footer: (info: any) => {
          const total = React.useMemo(
            () =>
              info.rows.reduce(
                (sum: number, row: any) =>
                  row.values["teamRecordDetails.odiResults.noResult"] + sum,
                0
              ),
            [info.rows]
          );

          return <>{total / 2}</>;
        },
        Filter: SliderColumnFilter,
        filter: "equals",
      },
      {
        Header: "Win %",
        accessor: "teamRecordDetails.odiResults.winPercentage",
        Cell: (cell: Cell) => <div>{cell.value?.toPrecision(4)}</div>,
      },
    ],
    []
  );

  const [isOpenSidePanel, toggleSideOpenPanel] = useState(false);
  const [selectedTeamUuid, setSelectedTeamUuid] = useState("0");

  const filteredTeamData = teamData?.filter(
    (x) => x.teamRecordDetails.odiResults !== null
  );
  const teamSelectedData = filteredTeamData?.find(
    (x) => x.teamUuid === selectedTeamUuid
  );

  return (
    <>
      <ReactTable
        className={"t20-team-records"}
        isLoading={isLoading}
        data={filteredTeamData}
        columns={columns}
        perPages={[10, 25, 50, 100]}
        options={{
          ...tableOptions,
          isFooter: true,
        }}
        children={
          <ReactSlidingSidePanel
            isOpen={isOpenSidePanel}
            setIsOpen={toggleSideOpenPanel}
            sidePanelType={SidePanelType.Left}
            panelWidth={100}
            children={
              <TeamDetails
                cricketFormat={CricketFormat.ODI}
                teamName={teamSelectedData?.teamName as string}
                flagUrl={teamSelectedData?.flagUri as string}
              />
            }
          />
        }
        handleRowClick={(e, r) => {
          setSelectedTeamUuid(r.original.teamUuid);
          toggleSideOpenPanel(!isOpenSidePanel);
        }}
      />
    </>
  );
};
