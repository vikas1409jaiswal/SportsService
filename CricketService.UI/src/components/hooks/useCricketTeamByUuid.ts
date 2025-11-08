import axios, { AxiosResponse } from "axios";
import { useQuery } from "react-query";

export interface CricketTeamDetails {
  teamUuid: string;
  teamName: string;
  flagUri: string;
}

const BASE_URL = "http://localhost:5104";

const fetchTeamByUuid = (
  teamUuid: string
): Promise<AxiosResponse<CricketTeamDetails>> => {
  return axios.get(`${BASE_URL}/cricketteam/team/${teamUuid}`);
};

export const useTeamByUuid = (teamUuid: string) => {
  const { isLoading, data } = useQuery(
    ["team-details", teamUuid],
    () => fetchTeamByUuid(teamUuid),
    { cacheTime: 0 }
  );

  return { teamDetails: data?.data, isLoading };
};
