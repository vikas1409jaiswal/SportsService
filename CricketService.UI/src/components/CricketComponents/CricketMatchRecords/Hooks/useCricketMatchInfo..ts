import axios, { AxiosResponse } from "axios";
import { useQuery } from "react-query";
import { CricketMatch, TestCricketMatch } from "../Models/Interface";
import { CricketFormat } from "../../../../models/enums/CricketFormat";

const fetchCricketMatches = (
  format: CricketFormat
): Promise<AxiosResponse<CricketMatch[]>> => {
  const urlsMap = new Map<CricketFormat, number>();

  urlsMap.set(CricketFormat.T20I, 0);
  urlsMap.set(CricketFormat.ODI, 1);

  return axios.get(
    `http://localhost:5104/cricketmatch/internationalMatches?format=${urlsMap.get(
      format
    )}`
  );
};

const fetchTestCricketMatches = (): Promise<
  AxiosResponse<TestCricketMatch[]>
> => {
  return axios.get(
    `http://localhost:5104/cricketmatch/internationalMatches?format=2`
  );
};

export const useCricketMatchInfo = (format: CricketFormat) => {
  const { isLoading, data } = useQuery(
    ["cricket-matches", format],
    () => fetchCricketMatches(format),
    {
      enabled: format === CricketFormat.T20I || format === CricketFormat.ODI,
    }
  );

  console.log(data);

  return { matchData: data?.data, isLoading };
};

export const useTestCricketMatchInfo = (format: CricketFormat) => {
  const { isLoading, data } = useQuery(
    ["cricket-matches", format],
    () => fetchTestCricketMatches(),
    {
      enabled: format === CricketFormat.Test,
    }
  );

  return { testMatchData: data?.data, isLoading };
};
