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
  died: string;
  teamNames: string[];
  imageSrc: string;
  photosSrc: string[];
  battingStyle: string;
  bowlingStyle: string;
  playingRole: string;
  height: string;
  education: string;
  debutDetails: DebutDetailInfo;
  content: string[];
}

interface DebutDetailInfo {
  odiMatches: DebutInfo;
  testMatches: DebutInfo;
  t20IMatches: DebutInfo;
}

interface DebutInfo {
  first: string;
  last: string;
}

const fetchPlayeInfo = async (
  href: string
): Promise<AxiosResponse<ApiData>> => {
  return await axios.get(`https://www.espncricinfo.com${href}`);
};

export const usePlayerInfo = (
  players: string[][],
  enable: boolean
): PlayerInfo[] => {
  const queries = [];

  const queryOptions = {
    refetchOnWindowFocus: false,
    refetchOnMount: false,
    enabled: true,
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
    const debutSelector = divElement.querySelectorAll(
      ".ds-grow .ds-w-full .ds-p-0 .ds-p-4 h5"
    );
    const debutSelector2 = divElement.querySelector(
      ".ds-grow .ds-w-full .ds-p-0 .ds-p-4 h5"
    )?.parentNode?.parentNode?.parentNode;

    const p: PlayerInfo = {
      playerUuid: "",
      fullName: "",
      href: "",
      birth: "",
      died: "",
      imageSrc: "",
      photosSrc: [],
      battingStyle: "",
      bowlingStyle: "",
      playingRole: "",
      height: "",
      education: "",
      teamNames: [],
      content: [],
      debutDetails: {
        t20IMatches: {
          first: "",
          last: "",
        },
        odiMatches: {
          first: "",
          last: "",
        },
        testMatches: {
          first: "",
          last: "",
        },
      },
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
      if (pSelector === "Died") {
        p.died = spanSelector;
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

    debutSelector.forEach((x, i) => {
      const h5Selector = x?.textContent as string;
      if (h5Selector === "Test Matches") {
        p.debutDetails.testMatches = {
          first: debutSelector2?.querySelectorAll("a")?.item(2 * i)
            ?.textContent as string,
          last: debutSelector2?.querySelectorAll("a")?.item(2 * i + 1)
            ?.textContent as string,
        };
      }
      if (h5Selector === "ODI Matches") {
        p.debutDetails.odiMatches = {
          first: debutSelector2?.querySelectorAll("a")?.item(2 * i)
            ?.textContent as string,
          last: debutSelector2?.querySelectorAll("a")?.item(2 * i + 1)
            ?.textContent as string,
        };
      }
      if (h5Selector === "T20I Matches") {
        p.debutDetails.t20IMatches = {
          first: debutSelector2?.querySelectorAll("a")?.item(2 * i)
            ?.textContent as string,
          last: debutSelector2?.querySelectorAll("a")?.item(2 * i + 1)
            ?.textContent as string,
        };
      }
    });

    teamNames?.forEach((x) => p.teamNames.push(x?.textContent as string));

    contents?.forEach((x) => p.content.push(x?.textContent as string));

    let imageSrc = "";

    // divElement.querySelectorAll('.ds-mb-4 img')
    //   .forEach(y => {
    //     p.photosSrc.push(y?.getAttribute("src") as string)
    //   })

    playerInfo.push({
      playerUuid: players[i][0],
      fullName: p.fullName,
      href: players[i][1],
      birth: p.birth,
      died: p.died,
      battingStyle: p.battingStyle,
      bowlingStyle: p.bowlingStyle,
      playingRole: p.playingRole,
      height: p.height,
      education: p.education,
      imageSrc: document
        .querySelector(".ds-w-48 img")
        ?.getAttribute("src") as string,
      photosSrc: p.photosSrc,
      teamNames: p.teamNames,
      content: p.content,
      debutDetails: {
        t20IMatches: p.debutDetails.t20IMatches,
        odiMatches: p.debutDetails.odiMatches,
        testMatches: p.debutDetails.testMatches,
      },
    });
  });

  const fetchedLength = playerInfo.filter((x) => x.fullName.length > 0).length;

  console.log(fetchedLength, "/", playerInfo.length);

  if (fetchedLength === playerInfo.length) {
    console.log(playerInfo);
  }

  return playerInfo;
};
