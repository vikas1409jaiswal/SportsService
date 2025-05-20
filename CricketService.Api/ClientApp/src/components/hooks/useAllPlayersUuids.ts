import axios, { AxiosResponse } from "axios";
import { useQuery } from "react-query";

export interface PlayerShortInfo {
  uuid: string;
  playerName: string;
  href: string;
  teams: TeamInfo[];
  imageUrl: string;
}

export interface TeamInfo {
  uuid: string;
  name: string;
}

const BASE_URL = "http://localhost:5104";

const fetchPlayersByUuids = (): Promise<AxiosResponse<PlayerShortInfo[]>> => {
  return axios.get(`${BASE_URL}/cricketplayer/players/unique?format=3`);
};

export const useAllPlayersUuids = () => {
  const { isLoading, data } = useQuery(
    ["all-players-uuids"],
    () => fetchPlayersByUuids(),
    { cacheTime: 0 }
  );

  return { playerInfos: data?.data, isLoading };
};
