import React from "react";
import { useQuery } from "react-query";
import axios from "axios";
import { ColumnDef } from "@tanstack/react-table";
import { ReactTableV2 } from "./ReactTableV2";

interface CricketMatch {
  matchUuid: string;
  matchDate: string;
  matchTitle: string;
  venue: string;
  result: string;
}

const fetchMatches = async () => {
  const response = await axios.get("http://localhost:5001/cricketmatch/internationalmatches?format=2");
  return response.data;
};

export const CricketMatchesTable: React.FC = () => {
  const { data, isLoading, error } = useQuery(["odi-matches"], fetchMatches);

  const columns = React.useMemo<ColumnDef<CricketMatch, any>[]>(
    () => [
      {
        header: "Date",
        accessorKey: "matchDate",
      },
      {
        header: "Match",
        accessorKey: "matchTitle",
      },
      {
        header: "Venue",
        accessorKey: "venue",
      },
      {
        header: "Result",
        accessorKey: "result",
      },
    ],
    []
  );

  if (isLoading) return <div className="cricket-matches-table-container">Loading...</div>;
  if (error) return <div className="cricket-matches-table-container">Error loading matches.</div>;

  return (
    <ReactTableV2
      columns={columns}
      data={Array.isArray(data) ? data : []}
      pageSize={12}
      title="ODI International Matches"
    />
  );
};
