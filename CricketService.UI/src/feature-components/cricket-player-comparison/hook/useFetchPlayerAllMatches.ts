import axios from "axios";
import { useQuery } from "react-query";
import { BattingStatistics } from "../../../components/CricketComponents/CricketPlayerInfoFetch/useCustomPlayerInfo";

export interface ApiData {
  data: any;
  meta: any;
}

export interface ApiResponse {
  data: ApiData;
  status: number;
  config: any;
  headers: any;
  request: any;
  statusText: string;
}

export interface BattingInningsStat {
  uuid: string;
  matchNumber: string;
  matchDate: string;
  inning: number;
  battingPosition: number;
  playerHref: string;
  playerName: string;
  runScored: number;
  ballsFaced: number;
  sixesInInning: number;
  foursInInning: number;
  outStatus: string;
  teamName: string;
  oppositionTeamName: string;
}

export interface CricketPlayerResponse {
  uuid: string;
  fullName: string;
  playerHref: string;
  dateOfBirth: string;
  dateOfDeath: string;
  teams: any;
  careerStatistics: any;
  birthPlace: string;
  internationalFormats: string[];
  teamNames: string[];
  content: any;
  overallStats: PlayerOverallStats;
}

export interface PlayerOverallStats {
  playerODIStats: PlayerFormatStats;
  playerT20IStats: PlayerFormatStats;
}

export interface PlayerFormatStats {
  battingInningsStats: BattingInningsStat[];
  battingOverallStats: BattingStatistics;
}

const fetchPlayerData = async (
  playerUuid: string
): Promise<CricketPlayerResponse> => {
  const response = await axios.get<CricketPlayerResponse>(
    `http://localhost:5001/CricketPlayer/player/${playerUuid}`
  );
  return response.data;
};

export const useFetchPlayerAllMatches = (
  playerUuid: string
): { data: CricketPlayerResponse | undefined; isLoading: boolean; error: any } => {
  const { data, isLoading, error } = useQuery(
    ["player-all-matches-data", playerUuid],
    () => fetchPlayerData(playerUuid),
    {
      enabled: !!playerUuid,
      staleTime: 1000 * 60 * 60, // 1 hour
      cacheTime: 1000 * 60 * 60, // 1 hour
    }
  );

  console.log("Fetched Player All Matches Data:", data);

  return { data, isLoading, error };
};
