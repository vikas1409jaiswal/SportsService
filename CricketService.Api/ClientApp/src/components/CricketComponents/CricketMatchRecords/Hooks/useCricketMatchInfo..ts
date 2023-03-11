import axios, { AxiosResponse } from "axios";
import { useQuery } from "react-query";
import { CricketFormat, CricketMatch } from "../Models/Interface";

const fetchCricketMatches = (
  format: CricketFormat
): Promise<AxiosResponse<CricketMatch[]>> => {
  const urlsMap = new Map<CricketFormat, string>();

  urlsMap.set(CricketFormat.T20I, "t20Match/all");
  urlsMap.set(CricketFormat.ODI, "odiMatch/all");

  return axios.get(`http://localhost:5104/cricketmatch/${urlsMap.get(format)}`);
};

export const useCricketMatchInfo = (format: CricketFormat) => {
  const { isLoading, data } = useQuery(["cricket-matches", format], () =>
    fetchCricketMatches(format)
  );

  return { matchData: data?.data, isLoading };
};
