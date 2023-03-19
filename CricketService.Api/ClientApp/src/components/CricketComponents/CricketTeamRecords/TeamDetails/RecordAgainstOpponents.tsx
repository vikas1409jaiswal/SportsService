import React, { useContext, useMemo, useState } from "react";
import { ReactTable, tableOptions } from "../../../common/ReactTable";
import {
  NumberRangeColumnFilter,
  SliderColumnFilter,
} from "../../../common/Filter";
import { GridLoader } from "../../../common/Loader";
import axios, { AxiosResponse } from "axios";
import { useQuery } from "react-query";
import { Cell } from "react-table";

import "./RecordAgainstOpponents.scss";
import { CricketFormat } from "../../CricketMatchRecords/Models/Interface";

interface AgainstTeamData {
  opponent: string;
  matches: number;
  won: number;
  lost: number;
  noResult: number;
}

const fetchAllAgainstTeamData = (
  teamUuid: string,
  format: CricketFormat
): Promise<AxiosResponse<AgainstTeamData[]>> => {
  const formatId = CricketFormat.T20I === format ? 0 : 1;
  return axios.get(
    `http://localhost:5104/cricketteam/teams/${teamUuid}/records/against/all?format=${formatId}`
  );
};

export const useTeamAgainstRecords = (
  teamUuid: string,
  format: CricketFormat
) => {
  const { isLoading, data } = useQuery(
    [teamUuid, "against-opponent"],
    () => fetchAllAgainstTeamData(teamUuid, format),
    { cacheTime: 0 }
  );

  console.log(data);

  return { data: data?.data, isLoading };
};

export interface RecordAgainstOpponentsProps {
  teamUuid: string;
  teamName: string;
  format: CricketFormat;
}

export const RecordAgainsOpponents: React.FunctionComponent<
  RecordAgainstOpponentsProps
> = ({ teamUuid, teamName, format }) => {
  const { isLoading, data } = useTeamAgainstRecords(teamUuid, format);

  const columns = useMemo(
    () => [
      {
        Header: "Team Name",
        Footer: "Total",
        accessor: "opponent",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        width: 250,
      },
      {
        Header: "Matches",
        accessor: "matches",
        Footer: (info: any) => {
          const total = React.useMemo(
            () =>
              info.rows.reduce(
                (sum: number, row: any) => row.values["matches"] + sum,
                0
              ),
            [info.rows]
          );

          return <>{total}</>;
        },
        Cell: (cell: Cell) => <div>{cell.value}</div>,
      },
      {
        Header: "Won",
        accessor: "won",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        Footer: (info: any) => {
          const total = React.useMemo(
            () =>
              info.rows.reduce(
                (sum: number, row: any) => row.values["won"] + sum,
                0
              ),
            [info.rows]
          );

          return <>{total}</>;
        },
      },
      {
        Header: "Lost",
        accessor: "lost",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        Footer: (info: any) => {
          const total = React.useMemo(
            () =>
              info.rows.reduce(
                (sum: number, row: any) => row.values["lost"] + sum,
                0
              ),
            [info.rows]
          );

          return <>{total}</>;
        },
      },
      {
        Header: "Tied",
        accessor: "tied",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        Footer: (info: any) => {
          const total = React.useMemo(
            () =>
              info.rows.reduce(
                (sum: number, row: any) => row.values["tied"] + sum,
                0
              ),
            [info.rows]
          );

          return <>{total}</>;
        },
      },
      {
        Header: "No Result",
        accessor: "noResult",
        Cell: (cell: Cell) => <div>{cell.value}</div>,
        Footer: (info: any) => {
          const total = React.useMemo(
            () =>
              info.rows.reduce(
                (sum: number, row: any) => row.values["noResult"] + sum,
                0
              ),
            [info.rows]
          );

          return <>{total}</>;
        },
      },
    ],
    []
  );

  return (
    <div className="against-record-table">
      {isLoading && <GridLoader size={120} color={"orange"} />}
      {!isLoading && data && (
        <ReactTable
          className={"t20-team-records"}
          data={data}
          columns={columns}
          perPages={[10, 25, 50, 100]}
          options={{
            ...tableOptions,
            isPagination: false,
            isSelectionPanel: false,
            isStickyColumn: false,
            isFooter: true,
            isRowSelect: false,
            isColumnFilter: false,
          }}
          handleRowClick={() => {}}
        />
      )}
    </div>
  );
};
