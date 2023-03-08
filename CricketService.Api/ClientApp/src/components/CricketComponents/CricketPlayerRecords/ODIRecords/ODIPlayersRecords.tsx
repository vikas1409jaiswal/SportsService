import React, { useContext, useState } from "react";
import { Cell } from "react-table";
import { Col } from "reactstrap";
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

import "./ODIPlayersRecords.scss";

export interface ODIPlayersRecordsProps {
  isLoading: boolean;
  players: PlayerData[];
}

export const ODIPlayersRecords: React.FunctionComponent<
  ODIPlayersRecordsProps
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
      accessor: "careerDetails.odiCareer.battingStatistics.matches",
      Footer: (info: any) => {
        const total = React.useMemo(
          () =>
            info.rows.reduce(
              (sum: number, row: any) =>
                row.values[
                  "careerDetails.odiCareer.battingStatistics.matches"
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
      accessor: "careerDetails.odiCareer.battingStatistics.runs",
      Footer: (info: any) => {
        const total = React.useMemo(
          () =>
            info.rows.reduce(
              (sum: number, row: any) =>
                row.values["careerDetails.odiCareer.battingStatistics.runs"] +
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
      accessor: "careerDetails.odiCareer.battingStatistics.ballsFaced",
      Footer: (info: any) => {
        const total = React.useMemo(
          () =>
            info.rows.reduce(
              (sum: number, row: any) =>
                row.values[
                  "careerDetails.odiCareer.battingStatistics.ballsFaced"
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
      accessor: "careerDetails.odiCareer.battingStatistics.centuries",
      Footer: (info: any) => {
        const total = React.useMemo(
          () =>
            info.rows.reduce(
              (sum: number, row: any) =>
                row.values[
                  "careerDetails.odiCareer.battingStatistics.centuries"
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
      accessor: "careerDetails.odiCareer.battingStatistics.halfCenturies",
      Footer: (info: any) => {
        const total = React.useMemo(
          () =>
            info.rows.reduce(
              (sum: number, row: any) =>
                row.values[
                  "careerDetails.odiCareer.battingStatistics.halfCenturies"
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
      accessor: "careerDetails.odiCareer.battingStatistics.sixes",
      Footer: (info: any) => {
        const total = React.useMemo(
          () =>
            info.rows.reduce(
              (sum: number, row: any) =>
                row.values["careerDetails.odiCareer.battingStatistics.sixes"] +
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
      accessor: "careerDetails.odiCareer.battingStatistics.fours",
      Footer: (info: any) => {
        const total = React.useMemo(
          () =>
            info.rows.reduce(
              (sum: number, row: any) =>
                row.values["careerDetails.odiCareer.battingStatistics.fours"] +
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
      accessor: "careerDetails.odiCareer.bowlingStatistics.wickets",
      Footer: (info: any) => {
        const total = React.useMemo(
          () =>
            info.rows.reduce(
              (sum: number, row: any) =>
                row.values[
                  "careerDetails.odiCareer.battingStatistics.wickets"
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
                  format={CricketFormat.ODI}
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
