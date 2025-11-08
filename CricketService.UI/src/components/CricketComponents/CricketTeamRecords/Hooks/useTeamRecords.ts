import axios, { AxiosResponse } from "axios";
import { useQuery } from "react-query";
import { CricketTeamData } from "../Models/Interface";

const fetchAllTeams = (): Promise<AxiosResponse<CricketTeamData[]>> => {
  return axios.get(`http://localhost:5104/cricketteam/teams/all/records`);
};

export const useTeamRecords = () => {
  const { isLoading, data } = useQuery(["cricket-team"], () => fetchAllTeams());

  return { teamData: data?.data, isLoading };
};
