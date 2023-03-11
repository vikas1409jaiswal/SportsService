import axios, { AxiosResponse } from "axios";
import { useQueries, useQuery } from "react-query";

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

export interface PlayerInfo {
  playerUuid: string;
  fullName: string;
  href: string;
  birth: string;
  teamNames: string[];
  imageSrc: string;
  battingStyle: string;
  bowlingStyle: string;
  playingRole: string;
  height: string;
  education: string;
  content: string[];
}

const fetchPlayeInfo = (href: string): Promise<AxiosResponse<ApiData>> => {
  return axios.get(`https://www.espncricinfo.com${href}`);
};

export const usePlayerInfo = (
  players: string[][],
  enable: boolean
): PlayerInfo[] => {
  const queries = [];

  const queryOptions = {
    refetchOnWindowFocus: false,
    refetchOnMount: false,
    enabled: enable,
    cacheTime: 60 * 60 * 1000,
    retry: true,
  };

  for (let i = 0; i < players.length; i++) {
    queries.push({
      queryKey: ["player-data", players[i][1]],
      queryFn: () => fetchPlayeInfo(players[i][1]),
      ...queryOptions,
    });
  }

  const result = useQueries(queries);

  const playerInfo: PlayerInfo[] = [];

  result.map((r, i) => {
    const divElement = document.createElement("div");
    divElement.innerHTML = (r.data as ApiResponse)?.data.toString() as string;

    const infoGridRows = divElement.querySelectorAll(".ds-p-4 .ds-grid > div");
    const teamNames = divElement
      .querySelectorAll(".ds-p-4 > div > div")
      ?.item(1)
      ?.querySelectorAll("a");
    const contents = divElement.querySelectorAll(".ci-player-bio-content > p");

    const p: PlayerInfo = {
      playerUuid: "",
      fullName: "",
      href: "",
      birth: "",
      imageSrc: "",
      battingStyle: "",
      bowlingStyle: "",
      playingRole: "",
      height: "",
      education: "",
      teamNames: [],
      content: [],
    };

    infoGridRows.forEach((r) => {
      const pSelector = r.querySelector("p")?.textContent;
      const spanSelector = r.querySelector("span")?.textContent as string;
      if (pSelector === "Full Name") {
        p.fullName = spanSelector;
      }
      if (pSelector === "Born") {
        p.birth = spanSelector;
      }
      if (pSelector === "Batting Style") {
        p.battingStyle = spanSelector;
      }
      if (pSelector === "Bowling Style") {
        p.bowlingStyle = spanSelector;
      }
      if (pSelector === "Playing Role") {
        p.playingRole = spanSelector;
      }
      if (pSelector === "Height") {
        p.height = spanSelector;
      }
      if (pSelector === "Education") {
        p.education = spanSelector;
      }
    });

    teamNames?.forEach((x) => p.teamNames.push(x?.textContent as string));

    contents?.forEach((x) => p.content.push(x?.textContent as string));

    playerInfo.push({
      ...p,
      playerUuid: players[i][0],
      fullName: p.fullName,
      href: players[i][1],
      birth: p.birth,
      battingStyle: p.battingStyle,
      bowlingStyle: p.bowlingStyle,
      playingRole: p.playingRole,
      height: p.height,
      education: p.education,
      imageSrc: divElement
        .querySelector(".ds-w-48 img")
        ?.getAttribute("src") as string,
      teamNames: p.teamNames,
      content: p.content,
    });
  });

  // const fetchedLength = playerInfo.filter(x => x.fullName.length > 0).length;

  // console.log(fetchedLength, '/', playerInfo.length);

  //if (fetchedLength === playerInfo.length) {
  //     console.log(playerInfo)
  //}

  return playerInfo;
};
