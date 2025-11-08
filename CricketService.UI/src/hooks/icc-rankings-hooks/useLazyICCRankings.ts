import axios, { AxiosResponse } from "axios";
import cheerio, { html } from "cheerio";
import { ICCRanking } from "./useICCRankings";
import { useQuery } from "react-query";

const fetchICCRankings = (
  year: number
): Promise<AxiosResponse<ICCRanking[]>> => {
  return axios.get(
    `https://www.icc-cricket.com/rankings/mens/player-rankings/t20i/batting?at=${year}-01-01`
  );
};

export const useICCRankings = (year: number = 2007) => {
  const { isLoading, data } = useQuery(
    ["icc-rankings", year],
    () => fetchICCRankings(year),
    { cacheTime: 0 }
  );

  const parseICCRankings = (html: string): ICCRanking => {
    const $ = cheerio.load(html);
    const rankings: ICCRanking = {
      format: "t20I",
      battingRanking: [],
    };

    console.log(html);

    $(".rankings-table tbody tr").each((index, element) => {
      const rank = index + 1;
      const playerName = $(element).find("td:nth-child(2)").text().trim();
      console.log(index);
      const rating = parseInt($(element).find("td:nth-child(4)").text(), 10);
      const careerBestRanking = $(element)
        .find("td:nth-child(5)")
        .text()
        .trim();
      const teamName = $(element).find("td:nth-child(3)").text().trim();

      rankings.battingRanking.push({
        rank,
        playerName,
        rating,
        careerBestRanking,
        teamName,
      });
    });

    return rankings;
  };

  const playerRankings = !isLoading
    ? parseICCRankings(JSON.stringify(data?.data))
    : null;

  return { playerRankings, isLoading };
};
