import axios from "axios";
import { useQuery } from "react-query";

export interface PlayerInfo {
  uuid: string;
  fullName: string;
  playerHref: string;
}

export interface AllPlayersApiResponse {
  data: PlayerInfo[];
  status: number;
  config: any;
  headers: any;
  request: any;
  statusText: string;
}

const fetchAllPlayersInfo = async (): Promise<PlayerInfo[]> => {
  const response = await axios.get<PlayerInfo[]>(
    'http://localhost:5001/CricketPlayer/players/all?format=2'
  );
  return response.data;
};

export const useFetchAllPlayersInfo = (): { 
  data: PlayerInfo[] | undefined; 
  isLoading: boolean; 
  error: any 
} => {
  const { data, isLoading, error } = useQuery(
    ["all-players-info"],
    fetchAllPlayersInfo,
    {
      staleTime: 1000 * 60 * 30, // 30 minutes
      cacheTime: 1000 * 60 * 60, // 1 hour
      refetchOnWindowFocus: false,
      refetchOnMount: false,
      refetchOnReconnect: false,
    }
  );

  console.log("Fetched All Players Info:", data);

  return { data, isLoading, error };
};