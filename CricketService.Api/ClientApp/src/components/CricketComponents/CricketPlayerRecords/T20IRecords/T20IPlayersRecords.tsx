import React, { useContext, useState } from "react";
import { Cell } from "react-table";
import { NumberRangeColumnFilter } from "../../../common/Filter";
import { TailSpinLoader } from "../../../common/Loader";
import {
  ReactSlidingSidePanel,
  SidePanelType,
} from "../../../common/ReactSlidingSidePanel";
import { ReactTable, tableOptions } from "../../../common/ReactTable";
import { CricketFormat } from "../../CricketMatchRecords/Models/Interface";
import { PlayerData } from "../Models/Interface";
import { PlayerDetails } from "../PlayerDetails/PlayerDetails";

import "./T20IPlayersRecords.scss";

export interface T20IPlayersRecordsProps {
  isLoading: boolean;
  players: PlayerData[];
}

export const T20IPlayersRecords: React.FunctionComponent<
  T20IPlayersRecordsProps
> = ({ isLoading, players }) => {
  const columns = [
    {
      Header: "Full Name",
      accessor: "fullName",
      Footer: "Total",
      Cell: (cell: Cell) => <div>{cell.value}</div>,
    },
    {
      Header: "Matches",
      accessor: "careerDetails.t20Career.battingStatistics.matches",
      Footer: (info: any) => {
        const total = React.useMemo(
          () =>
            info.rows.reduce(
              (sum: number, row: any) =>
                row.values[
                  "careerDetails.t20Career.battingStatistics.matches"
                ] + sum,
              0
            ),
          [info.rows]
        );

        return <>{"xxx"}</>;
      },
      Cell: (cell: Cell) => <div>{cell.value}</div>,
      Filter: NumberRangeColumnFilter,
      filter: "between",
    },
    {
      Header: "Runs",
      accessor: "careerDetails.t20Career.battingStatistics.runs",
      Footer: (info: any) => {
        const total = React.useMemo(
          () =>
            info.rows.reduce(
              (sum: number, row: any) =>
                row.values["careerDetails.t20Career.battingStatistics.runs"] +
                sum,
              0
            ),
          [info.rows]
        );

        return <>{total}</>;
      },
      Cell: (cell: Cell) => <div>{cell.value}</div>,
      Filter: NumberRangeColumnFilter,
      filter: "between",
    },
    {
      Header: "Balls",
      accessor: "careerDetails.t20Career.battingStatistics.ballsFaced",
      Footer: (info: any) => {
        const total = React.useMemo(
          () =>
            info.rows.reduce(
              (sum: number, row: any) =>
                row.values[
                  "careerDetails.t20Career.battingStatistics.ballsFaced"
                ] + sum,
              0
            ),
          [info.rows]
        );

        return <>{total}</>;
      },
      Cell: (cell: Cell) => <div>{cell.value}</div>,
      Filter: NumberRangeColumnFilter,
      filter: "between",
    },
    {
      Header: "100s",
      accessor: "careerDetails.t20Career.battingStatistics.centuries",
      Footer: (info: any) => {
        const total = React.useMemo(
          () =>
            info.rows.reduce(
              (sum: number, row: any) =>
                row.values[
                  "careerDetails.t20Career.battingStatistics.centuries"
                ] + sum,
              0
            ),
          [info.rows]
        );

        return <>{total}</>;
      },
      Cell: (cell: Cell) => <div>{cell.value}</div>,
      Filter: NumberRangeColumnFilter,
      filter: "between",
    },
    {
      Header: "50s",
      accessor: "careerDetails.t20Career.battingStatistics.halfCenturies",
      Footer: (info: any) => {
        const total = React.useMemo(
          () =>
            info.rows.reduce(
              (sum: number, row: any) =>
                row.values[
                  "careerDetails.t20Career.battingStatistics.halfCenturies"
                ] + sum,
              0
            ),
          [info.rows]
        );

        return <>{total}</>;
      },
      Cell: (cell: Cell) => <div>{cell.value}</div>,
      Filter: NumberRangeColumnFilter,
      filter: "between",
    },
    {
      Header: "6s",
      accessor: "careerDetails.t20Career.battingStatistics.sixes",
      Footer: (info: any) => {
        const total = React.useMemo(
          () =>
            info.rows.reduce(
              (sum: number, row: any) =>
                row.values["careerDetails.t20Career.battingStatistics.sixes"] +
                sum,
              0
            ),
          [info.rows]
        );

        return <>{total}</>;
      },
      Cell: (cell: Cell) => <div>{cell.value}</div>,
      Filter: NumberRangeColumnFilter,
      filter: "between",
    },
    {
      Header: "4s",
      accessor: "careerDetails.t20Career.battingStatistics.fours",
      Footer: (info: any) => {
        const total = React.useMemo(
          () =>
            info.rows.reduce(
              (sum: number, row: any) =>
                row.values["careerDetails.t20Career.battingStatistics.fours"] +
                sum,
              0
            ),
          [info.rows]
        );

        return <>{total}</>;
      },
      Cell: (cell: Cell) => <div>{cell.value}</div>,
      Filter: NumberRangeColumnFilter,
      filter: "between",
    },
    {
      Header: "Wickets",
      accessor: "careerDetails.t20Career.bowlingStatistics.wickets",
      Footer: (info: any) => {
        const total = React.useMemo(
          () =>
            info.rows.reduce(
              (sum: number, row: any) =>
                row.values[
                  "careerDetails.t20Career.battingStatistics.wickets"
                ] + sum,
              0
            ),
          [info.rows]
        );

        return <>{total}</>;
      },
      Cell: (cell: Cell) => <div>{cell.value}</div>,
      Filter: NumberRangeColumnFilter,
      filter: "between",
    },
  ];

  const [isOpenSidePanel, toggleSideOpenPanel] = useState(false);
  const [selectedPlayerUuid, setSelectedPlayerUuid] = useState("0");

  const playerSelectedData = players?.find(
    (x) => x.playerUuid === selectedPlayerUuid
  );

  return (
    <>
      {isLoading && <TailSpinLoader />}
      {!isLoading && players && (
        <ReactTable
          columns={columns}
          isLoading={isLoading}
          data={players}
          perPages={[5, 10, 20, 50]}
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
                <PlayerDetails
                  format={CricketFormat.T20I}
                  teamName={playerSelectedData?.teamName as string}
                  playerName={playerSelectedData?.fullName as string}
                />
              }
            />
          }
          handleRowClick={(e, r) => {
            setSelectedPlayerUuid(r.original.playerUuid);
            toggleSideOpenPanel(!isOpenSidePanel);
          }}
        />
      )}
    </>
  );
};
