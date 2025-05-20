import axios, { AxiosResponse } from "axios";
import { useQuery } from "react-query";

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

export interface MatchesSchedule {
  date: string;
  team1: string;
  team2: string;
  venue: string;
}

const fetchMatchesSchedule = async (): Promise<AxiosResponse<ApiData>> => {
  return await axios.get(`http://localhost:3011`);
};

export const useMatchesSchedule = () => {
  const { data } = useQuery(["schedule"], fetchMatchesSchedule);
  const divElement = document.createElement("div");
  divElement.innerHTML = (data as ApiResponse)?.data.toString() as string;

  const matches: MatchesSchedule[] = [];

  const allMatchesRows = divElement?.querySelectorAll("tbody > tr");

  allMatchesRows?.forEach((x) => {
    const tdsSelector = x?.querySelectorAll("td");
    const teams = tdsSelector[1]?.textContent?.split(",")[0]?.split("vs") || [
      "",
      "",
    ];
    const date = tdsSelector[0]?.querySelector(".dtstart")?.textContent;
    const time = tdsSelector[0]?.querySelector(".match-time")?.textContent;
    matches.push({
      date: `${date}, ${time}`,
      team1: teams[0]?.trim() || "",
      team2: teams[1]?.trim() || "",
      venue: tdsSelector[1]?.querySelector(".match-venue")?.textContent || "",
    });
  });
  return matches;
};
