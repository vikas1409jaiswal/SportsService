import axios, { AxiosResponse } from "axios";
import { useQueries, useQuery } from "react-query";
import { MatchDetailInfo } from "./useCustomPlayerInfo";
import { ApiData, ApiResponse } from "../../../models/Api";

export interface PlayerInfo {
  playerUuid: string;
  name: string;
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
  career: CareerDetailInfo | null;
  matchDetail: MatchDetailInfo | null;
}

export interface DebutDetailInfo {
  odiMatches: DebutInfo;
  testMatches: DebutInfo;
  t20IMatches: DebutInfo;
}

export interface CareerDetailInfo {
  odiMatches: CareerDetailStats | null;
  testMatches: CareerDetailStats | null;
  t20IMatches: CareerDetailStats | null;
}

export interface CareerDetailStats {
  battingStats: BattingStatistics | null;
  bowlingStats: BowlingStatistics | null;
}

export interface BattingStatistics {
  matches: number;
  innings: number;
  runs: number;
  ballsFaced: number;
  centuries: number;
  halfCenturies: number;
  highestScore: string;
  fours: number;
  sixes: number;
  strikeRate: number;
}

export interface BowlingStatistics {
  matches: number;
  innings: number;
  wickets: number;
  runsConceded: number;
  bbi: string;
  bbm: string;
  economy: number;
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
  enabled: boolean
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
      queryKey: ["player-data", players[i][0]],
      queryFn: () => fetchPlayeInfo(players[i][0]),
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

    const careerSelector = divElement.querySelectorAll(
      ".ds-w-full.ds-table.ds-table-md.ds-table-bordered.ds-border-collapse.ds-border.ds-border-line.ds-table-auto.ds-overflow-scroll"
    );

    const isBowler = careerSelector
      ?.item(0)
      ?.querySelector("tr")
      ?.textContent?.includes("Wkts");

    const batAndFieldingRows = careerSelector
      .item(isBowler ? 1 : 0)
      ?.querySelectorAll("tr");
    const bowlingRows = careerSelector
      .item(isBowler ? 0 : 1)
      ?.querySelectorAll("tr");

    const indexBFArr = {
      odiMatches: -1,
      t20IMatches: -1,
      testMatches: -1,
    };

    const indexBowArr = {
      odiMatches: -1,
      t20IMatches: -1,
      testMatches: -1,
    };

    batAndFieldingRows?.forEach((x, i) => {
      const format = x?.querySelector("td")?.textContent;
      if (format === "ODI") {
        indexBFArr.odiMatches = i;
      }
      if (format === "Test") {
        indexBFArr.testMatches = i;
      }
      if (format === "T20I") {
        indexBFArr.t20IMatches = i;
      }
    });

    bowlingRows?.forEach((x, i) => {
      const format = x?.querySelector("td")?.textContent;
      if (format === "ODI") {
        indexBowArr.odiMatches = i;
      }
      if (format === "Test") {
        indexBowArr.testMatches = i;
      }
      if (format === "T20I") {
        indexBowArr.t20IMatches = i;
      }
    });

    const p: PlayerInfo = {
      playerUuid: "",
      name: "",
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
      career: null,
      matchDetail: null,
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

    console.log(debutSelector2);

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

    const odiBandFSelectors = batAndFieldingRows
      ?.item(indexBFArr.odiMatches)
      ?.querySelectorAll("td");

    const odiBowSelectors = bowlingRows
      ?.item(indexBowArr.odiMatches)
      ?.querySelectorAll("td");

    const testBandFSelectors = batAndFieldingRows
      ?.item(indexBFArr.testMatches)
      ?.querySelectorAll("td");

    const testBowSelectors = bowlingRows
      ?.item(indexBowArr.testMatches)
      ?.querySelectorAll("td");

    const t20IBandFSelectors = batAndFieldingRows
      ?.item(indexBFArr.t20IMatches)
      ?.querySelectorAll("td");

    const t20IBowSelectors = bowlingRows
      ?.item(indexBowArr.t20IMatches)
      ?.querySelectorAll("td");

    let name = players[i][0].split("/")[2].split("-");

    name.pop();

    playerInfo.push({
      playerUuid: players[i][0],
      name: name.join(" "),
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
      career: {
        odiMatches: {
          battingStats: {
            matches: parseInt(
              odiBandFSelectors?.item(1)?.textContent as string
            ),
            innings: parseInt(
              odiBandFSelectors?.item(2)?.textContent as string
            ),
            runs: parseInt(odiBandFSelectors?.item(4)?.textContent as string),
            ballsFaced: parseInt(
              odiBandFSelectors?.item(7)?.textContent as string
            ),
            centuries: parseInt(
              odiBandFSelectors?.item(9)?.textContent as string
            ),
            halfCenturies: parseInt(
              odiBandFSelectors?.item(10)?.textContent as string
            ),
            highestScore: odiBandFSelectors?.item(5)?.textContent as string,
            fours: parseInt(odiBandFSelectors?.item(11)?.textContent as string),
            sixes: parseInt(odiBandFSelectors?.item(12)?.textContent as string),
            strikeRate: parseFloat(
              odiBandFSelectors?.item(8)?.textContent as string
            ),
          },
          bowlingStats: {
            matches: parseInt(odiBowSelectors?.item(1)?.textContent as string),
            innings: parseInt(odiBowSelectors?.item(2)?.textContent as string),
            wickets: parseInt(odiBowSelectors?.item(5)?.textContent as string),
            runsConceded: parseInt(
              odiBowSelectors?.item(4)?.textContent as string
            ),
            bbi: odiBowSelectors?.item(6)?.textContent as string,
            bbm: odiBowSelectors?.item(6)?.textContent as string,
            economy: parseFloat(
              odiBowSelectors?.item(9)?.textContent as string
            ),
          },
        },
        testMatches: {
          battingStats: {
            matches: parseInt(
              testBandFSelectors?.item(1)?.textContent as string
            ),
            innings: parseInt(
              testBandFSelectors?.item(2)?.textContent as string
            ),
            runs: parseInt(testBandFSelectors?.item(4)?.textContent as string),
            ballsFaced: parseInt(
              testBandFSelectors?.item(7)?.textContent as string
            ),
            centuries: parseInt(
              testBandFSelectors?.item(9)?.textContent as string
            ),
            halfCenturies: parseInt(
              testBandFSelectors?.item(10)?.textContent as string
            ),
            highestScore: testBandFSelectors?.item(5)?.textContent as string,
            fours: parseInt(
              testBandFSelectors?.item(11)?.textContent as string
            ),
            sixes: parseInt(
              testBandFSelectors?.item(12)?.textContent as string
            ),
            strikeRate: parseFloat(
              testBandFSelectors?.item(8)?.textContent as string
            ),
          },
          bowlingStats: {
            matches: parseInt(testBowSelectors?.item(1)?.textContent as string),
            innings: parseInt(testBowSelectors?.item(2)?.textContent as string),
            wickets: parseInt(testBowSelectors?.item(5)?.textContent as string),
            runsConceded: parseInt(
              testBowSelectors?.item(4)?.textContent as string
            ),
            bbi: testBowSelectors?.item(6)?.textContent as string,
            bbm: testBowSelectors?.item(7)?.textContent as string,
            economy: parseFloat(
              testBowSelectors?.item(9)?.textContent as string
            ),
          },
        },
        t20IMatches: {
          battingStats: {
            matches: parseInt(
              t20IBandFSelectors?.item(1)?.textContent as string
            ),
            innings: parseInt(
              t20IBandFSelectors?.item(2)?.textContent as string
            ),
            runs: parseInt(t20IBandFSelectors?.item(4)?.textContent as string),
            ballsFaced: parseInt(
              t20IBandFSelectors?.item(7)?.textContent as string
            ),
            centuries: parseInt(
              t20IBandFSelectors?.item(9)?.textContent as string
            ),
            halfCenturies: parseInt(
              t20IBandFSelectors?.item(10)?.textContent as string
            ),
            highestScore: t20IBandFSelectors?.item(5)?.textContent as string,
            fours: parseInt(
              t20IBandFSelectors?.item(11)?.textContent as string
            ),
            sixes: parseInt(
              t20IBandFSelectors?.item(12)?.textContent as string
            ),
            strikeRate: parseFloat(
              t20IBandFSelectors?.item(8)?.textContent as string
            ),
          },
          bowlingStats: {
            matches: parseInt(t20IBowSelectors?.item(1)?.textContent as string),
            innings: parseInt(t20IBowSelectors?.item(2)?.textContent as string),
            wickets: parseInt(t20IBowSelectors?.item(5)?.textContent as string),
            runsConceded: parseInt(
              t20IBowSelectors?.item(4)?.textContent as string
            ),
            bbi: t20IBowSelectors?.item(6)?.textContent as string,
            bbm: t20IBowSelectors?.item(6)?.textContent as string,
            economy: parseFloat(
              t20IBowSelectors?.item(9)?.textContent as string
            ),
          },
        },
      },
      matchDetail: null,
    });
  });

  const fetchedLength = playerInfo.filter((x) => x.fullName.length > 0).length;

  console.log(fetchedLength, "/", playerInfo.length);

  if (fetchedLength === playerInfo.length) {
    console.log(playerInfo);
  }

  return playerInfo;
};
