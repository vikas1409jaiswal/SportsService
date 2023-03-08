import axios, { AxiosResponse } from "axios";
import { useQuery } from "react-query";
import { ApiKey } from "./ApiKey/ApiKey";

export interface ApiData {
  data: Countries[];
  meta: any;
}

export interface ApiDataById {
  data: Countries;
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

export interface Countries {
  id: number;
  name: string;
  image_path: string;
  extra: any;
}

const fetchCountries = (): Promise<AxiosResponse<ApiData>> => {
  return axios.get(
    `https://soccer.sportmonks.com/api/v2.0/countries?api_token=${ApiKey}`
  );
};

const fetchCountry = (
  countryId: number
): Promise<AxiosResponse<ApiDataById>> => {
  return axios.get(
    `https://soccer.sportmonks.com/api/v2.0/countries/${countryId}?api_token=${ApiKey}`
  );
};

export const useCountries = (): Countries[] => {
  const initialCountries: Countries[] = [];
  const { data } = useQuery("countries", fetchCountries, {
    refetchOnWindowFocus: false,
  });

  return data ? data?.data.data : initialCountries;
};

export const useCountryById = (countryId: number): Countries => {
  const initialCountry: Countries = {
    name: "",
    extra: {},
    id: 0,
    image_path: "",
  };
  const { data } = useQuery(
    ["country", countryId],
    () => fetchCountry(countryId),
    { refetchOnWindowFocus: false }
  );

  return data ? data?.data.data : initialCountry;
};
