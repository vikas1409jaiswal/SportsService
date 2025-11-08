import axios, { AxiosResponse } from "axios";
import { useQuery } from "react-query";
import { ApiKey } from "./ApiKey/ApiKey";

export interface ApiData {
  data: Player[];
  meta: any;
}

export interface ApiDataById {
  data: Player;
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

export interface Player {
  birthcountry: string;
  birthdate: string;
  birthplace: string;
  common_name: string;
  country_id: number;
  display_name: string;
  firstname: string;
  fullname: string;
  height: string;
  image_path: string;
  lastname: string;
  nationality: string;
  player_id: number;
  position_id: number;
  team_id: number;
  weight: string;
}

const fetchPlayersByCountryId = (
  countryId: number
): Promise<AxiosResponse<ApiData>> => {
  return axios.get(
    `https://soccer.sportmonks.com/api/v2.0/countries/${countryId}/players?page=100&api_token=${ApiKey}`
  );
};

const fetchPlayerByName = (name: string): Promise<AxiosResponse<ApiData>> => {
  return axios.get(
    `https://soccer.sportmonks.com/api/v2.0/players/search/${name}?api_token=${ApiKey}`
  );
};

const fetchPlayerByPlayerId = (
  playerId: number
): Promise<AxiosResponse<ApiDataById>> => {
  return axios.get(
    `https://soccer.sportmonks.com/api/v2.0/players/${playerId}?api_token=${ApiKey}`
  );
};

export const usePlayersByCountryId = (countryId: number): Player[] => {
  const initialPlayer: Player[] = [
    {
      birthcountry: "",
      birthdate: "",
      birthplace: "",
      common_name: "",
      country_id: 0,
      display_name: "",
      firstname: "",
      fullname: "",
      height: "",
      image_path: "",
      lastname: "",
      nationality: "",
      player_id: 0,
      position_id: 0,
      team_id: 0,
      weight: "",
    },
  ];
  const { data } = useQuery(
    ["players", countryId],
    () => fetchPlayersByCountryId(countryId),
    { refetchOnWindowFocus: false }
  );

  return data ? data.data.data : initialPlayer;
};

export const usePlayersByName = (name: string): Player[] => {
  const initialPlayer: Player[] = [
    {
      birthcountry: "",
      birthdate: "",
      birthplace: "",
      common_name: "",
      country_id: 0,
      display_name: "",
      firstname: "",
      fullname: "",
      height: "",
      image_path: "",
      lastname: "",
      nationality: "",
      player_id: 0,
      position_id: 0,
      team_id: 0,
      weight: "",
    },
  ];
  const { data } = useQuery(["player", name], () => fetchPlayerByName(name));

  return data ? data.data.data : initialPlayer;
};

export const usePlayerByPlayerId = (playerId: number): Player => {
  const initialPlayer: Player = {
    birthcountry: "",
    birthdate: "",
    birthplace: "",
    common_name: "",
    country_id: 0,
    display_name: "",
    firstname: "",
    fullname: "",
    height: "",
    image_path: "",
    lastname: "",
    nationality: "",
    player_id: 0,
    position_id: 0,
    team_id: 0,
    weight: "",
  };
  const { data } = useQuery(
    ["player", playerId],
    () => fetchPlayerByPlayerId(playerId),
    { refetchOnWindowFocus: false }
  );

  return data ? data.data.data : initialPlayer;
};
