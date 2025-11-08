import axios, { AxiosResponse } from "axios";
import { useQuery } from "react-query";
import { CricketFormat } from "../../models/enums/CricketFormat";

export interface ICCRanking {
  format: string;
  battingRanking: ICCPlayerInfo[];
}

export interface ICCPlayerInfo {
  rank: number;
  playerName: string;
  rating: number;
  careerBestRanking: string;
  teamName: string;
}

const fetchICCRankings = (
  year: number,
  format: CricketFormat
): Promise<AxiosResponse<ICCRanking[]>> => {
  let formatStr;
  if (format === CricketFormat.T20I) {
    formatStr = "t20i";
  } else if (format === CricketFormat.ODI) {
    formatStr = "odi";
  } else if (format === CricketFormat.Test) {
    formatStr = "test";
  }
  return axios.get(
    `https://www.icc-cricket.com/rankings/mens/player-rankings/${formatStr}/batting?at=${year}-01-01`
  );
};

export const useICCRankings = (year: number = 2007, format: CricketFormat) => {
  const { isLoading, data } = useQuery(
    ["icc-rankings", year, format],
    () => fetchICCRankings(2007, format),
    { cacheTime: 0, refetchInterval: 5, retry: true }
  );

  const rankings: ICCRanking = {
    format: "t20I",
    battingRanking: [],
  };

  const divElement = document.createElement("div");

  divElement.innerHTML = data?.data.toString() as string;

  const playerRankTables = divElement?.querySelector(".rankings-table");

  playerRankTables?.querySelectorAll("tbody > tr")?.forEach((x, i) =>
    rankings.battingRanking.push({
      rank: i + 1,
      playerName: x
        ?.querySelectorAll("td")
        ?.item(1)
        ?.textContent?.trim() as string,
      rating: parseInt(
        x?.querySelectorAll("td")?.item(3)?.textContent as string
      ),
      careerBestRanking: x
        ?.querySelectorAll("td")
        ?.item(4)
        ?.textContent?.trim() as string,
      teamName: x
        ?.querySelectorAll("td")
        ?.item(2)
        ?.textContent?.trim() as string,
    })
  );

  return { playerRankings: rankings, isLoading };
};
