import axios, { AxiosResponse } from "axios";
import { useQuery } from "react-query";
import { CricketFormat, CricketTeam } from "../Models/Interface";

const fetchCricketTeams = (): Promise<AxiosResponse<CricketTeam[]>> => {
  return axios.get(`http://localhost:5104/cricketteam/teams/all`);
};

export const useCricketTeamInfo = () => {
  const { isLoading, data } = useQuery(["cricket-teams"], () =>
    fetchCricketTeams()
  );

  return { teamData: data?.data, isLoading };
};
