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

import "./T20Records.scss";
import { CricketFormat } from "../../../../models/enums/CricketFormat";

export interface T20RecordsProps {
  isLoading: boolean;
  teamData: CricketTeamData[];
}

export const T20Records: React.FunctionComponent<T20RecordsProps> = ({
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
        accessor: "teamRecordDetails.t20IResults.matches",
        Footer: (info: any) => {
          const total = React.useMemo(
            () =>
              info.rows.reduce(
                (sum: number, row: any) =>
                  row.values["teamRecordDetails.t20IResults.matches"] + sum,
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
        accessor: "teamRecordDetails.t20IResults.won",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        Footer: (info: any) => {
          const total = React.useMemo(
            () =>
              info.rows.reduce(
                (sum: number, row: any) =>
                  row.values["teamRecordDetails.t20IResults.won"] + sum,
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
        accessor: "teamRecordDetails.t20IResults.lost",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        Footer: (info: any) => {
          const total = React.useMemo(
            () =>
              info.rows.reduce(
                (sum: number, row: any) =>
                  row.values["teamRecordDetails.t20IResults.lost"] + sum,
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
        Header: "Tied",
        accessor: "teamRecordDetails.t20IResults.tied",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        Footer: (info: any) => {
          const total = React.useMemo(
            () =>
              info.rows.reduce(
                (sum: number, row: any) =>
                  row.values["teamRecordDetails.t20IResults.tied"] + sum,
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
        Header: "No Result",
        accessor: "teamRecordDetails.t20IResults.noResult",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        Footer: (info: any) => {
          const total = React.useMemo(
            () =>
              info.rows.reduce(
                (sum: number, row: any) =>
                  row.values["teamRecordDetails.t20IResults.noResult"] + sum,
                0
              ),
            [info.rows]
          );

          return <>{total / 2}</>;
        },
        Filter: NumberRangeColumnFilter,
        filter: "between",
      },
      {
        Header: "Win %",
        accessor: "teamRecordDetails.t20IResults.winPercentage",
        Cell: (cell: Cell) => <div>{cell.value?.toPrecision(4)}</div>,
        Filter: () => <></>,
      },
      {
        Header: "Debut",
        accessor: "teamRecordDetails.t20IResults.debut",
        Cell: (cell: Cell) => (
          <div>
            <h6>{cell.value.date}</h6>
            <h6>vs</h6>
            <h6>{cell.value.opponent}</h6>
          </div>
        ),
        width: 200,
      },
    ],
    []
  );

  const [isOpenSidePanel, toggleSideOpenPanel] = useState(false);
  const [selectedTeamUuid, setSelectedTeamUuid] = useState("0");

  const filteredTeamData = teamData?.filter(
    (x) => x.teamRecordDetails.t20IResults !== null
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
          isRowSelect: false,
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
                cricketFormat={CricketFormat.T20I}
                teamUuid={teamSelectedData?.teamUuid as string}
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
