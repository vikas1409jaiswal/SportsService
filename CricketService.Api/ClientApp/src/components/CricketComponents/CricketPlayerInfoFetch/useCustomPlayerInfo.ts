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
  fieldingStats: FieldingStatistics | null;
}

export interface MatchDetailInfo {
  odiMatches: MatchDetailStats | null;
  testMatches: MatchDetailStats | null;
  t20IMatches: MatchDetailStats | null;
}

export interface MatchDetailStats {
  battingStats: BattingMatchStatistics | null;
  bowlingStats: BowlingMatchStatistics | null;
  fieldingStats: FieldingStatistics | null;
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

export interface FieldingStatistics {
  matches: number;
  innings: number;
  catches: number;
  maxCatches: number;
  catchesPerInning: number;
}

export interface BattingMatchStatistics {
  runs: number;
  balls: number;
  opps: string;
  date: string;
  sixes: number;
  fours: number;
  minutes: number;
  strikeRate: number;
  venue: string;
  notOut: boolean;
}

export interface BowlingMatchStatistics {
  overs: number;
  maidens: number;
  wickets: number;
  runsConceded: number;
  economy: number;
  forTeam: string;
  opps: string;
  date: string;
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

const fetchCustomPlayeInfo = async (): Promise<AxiosResponse<ApiData>> => {
  return await axios.get(`http://localhost:3000`);
};

export const useCustomPlayerInfo = (
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
      queryKey: ["player-data", players[i][0]],
      queryFn: () => fetchPlayeInfo(players[i][0]),
      ...queryOptions,
    });
  }

  const result = useQueries(queries);

  const playerInfo: PlayerInfo[] = [];

  const { data: d } = useQuery(["custom-players"], () =>
    fetchCustomPlayeInfo()
  );

  const divElementCus = document.createElement("div");
  divElementCus.innerHTML = d?.data.toString() as string;

  const playersSelector = divElementCus.querySelectorAll("tbody > tr");

  const playerMap = new Map<number, string>();

  playersSelector.forEach((x, i) => {
    let namePlayer = x?.querySelector("td a")?.getAttribute("href") as string;
    const country = x?.querySelectorAll("td")[7]?.textContent || "";
    country?.includes("India") && playerMap.set(i, namePlayer.split("/")[2]);
  });

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

    let name = players[i][0].split("/")[2].split("-");

    name.pop();

    const tdsSelector = playersSelector?.item(i)?.querySelectorAll("td");

    console.log(tdsSelector);

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
            matches: parseInt(tdsSelector?.item(2).textContent as string),
            innings: parseInt(tdsSelector?.item(3).textContent as string),
            runs: parseInt(tdsSelector?.item(5).textContent as string),
            ballsFaced: NaN,
            centuries: parseInt(tdsSelector?.item(0).textContent as string),
            halfCenturies: NaN,
            highestScore: tdsSelector?.item(6).textContent as string,
            fours: parseInt(tdsSelector?.item(3).textContent as string),
            sixes: parseInt(tdsSelector?.item(4).textContent as string),
            strikeRate: parseFloat(tdsSelector?.item(2).textContent as string),
          },
          bowlingStats: {
            matches: parseInt(tdsSelector?.item(2).textContent as string),
            innings: parseInt(tdsSelector?.item(3).textContent as string),
            wickets: parseInt(tdsSelector?.item(3).textContent as string),
            runsConceded: parseInt(tdsSelector?.item(4).textContent as string),
            bbi: tdsSelector?.item(3).textContent as string,
            bbm: tdsSelector?.item(3).textContent as string,
            economy: parseFloat(tdsSelector?.item(2).textContent as string),
          },
          fieldingStats: {
            matches: parseInt(tdsSelector?.item(2).textContent as string),
            innings: parseInt(tdsSelector?.item(3).textContent as string),
            catches: parseInt(tdsSelector?.item(4).textContent as string),
            maxCatches: parseInt(tdsSelector?.item(5).textContent as string),
            catchesPerInning: parseFloat(
              tdsSelector?.item(6).textContent as string
            ),
          },
        },
        testMatches: {
          battingStats: {
            matches: parseInt(tdsSelector?.item(2).textContent as string),
            innings: parseInt(tdsSelector?.item(3).textContent as string),
            runs: parseInt(tdsSelector?.item(0).textContent as string),
            ballsFaced: NaN,
            centuries: NaN,
            halfCenturies: NaN,
            highestScore: tdsSelector?.item(1).textContent as string,
            fours: parseInt(tdsSelector?.item(3).textContent as string),
            sixes: parseInt(tdsSelector?.item(4).textContent as string),
            strikeRate: parseFloat(tdsSelector?.item(0).textContent as string),
          },
          bowlingStats: {
            matches: parseInt(tdsSelector?.item(2).textContent as string),
            innings: parseInt(tdsSelector?.item(3).textContent as string),
            wickets: parseInt(tdsSelector?.item(0).textContent as string),
            runsConceded: parseInt(tdsSelector?.item(0).textContent as string),
            bbi: tdsSelector?.item(0).textContent as string,
            bbm: tdsSelector?.item(0).textContent as string,
            economy: parseFloat(tdsSelector?.item(2).textContent as string),
          },
          fieldingStats: null,
        },
        t20IMatches: {
          battingStats: null,
          // {
          //   matches: parseInt(tdsSelector?.item(2).textContent as string),
          //   innings: parseInt(tdsSelector?.item(3).textContent as string),
          //   runs: parseInt(tdsSelector?.item(5).textContent as string),
          //   ballsFaced: NaN,
          //   centuries: NaN,
          //   halfCenturies: NaN,
          //   highestScore: tdsSelector?.item(6).textContent as string,
          //   fours: parseInt(tdsSelector?.item(13).textContent as string),
          //   sixes: parseInt(tdsSelector?.item(14).textContent as string),
          //   strikeRate: parseFloat(tdsSelector?.item(9).textContent as string),
          // },
          bowlingStats: null,
          // {
          //   matches: parseInt(tdsSelector?.item(2).textContent as string),
          //   innings: parseInt(tdsSelector?.item(3).textContent as string),
          //   wickets: parseInt(tdsSelector?.item(8).textContent as string),
          //   runsConceded: parseInt(tdsSelector?.item(4).textContent as string),
          //   bbi: tdsSelector?.item(3).textContent as string,
          //   bbm: tdsSelector?.item(10).textContent as string,
          //   economy: parseFloat(tdsSelector?.item(12).textContent as string),
          // },
          fieldingStats: {
            matches: parseInt(tdsSelector?.item(2).textContent as string),
            innings: parseInt(tdsSelector?.item(3).textContent as string),
            catches: parseInt(tdsSelector?.item(4).textContent as string),
            maxCatches: parseInt(tdsSelector?.item(5).textContent as string),
            catchesPerInning: parseFloat(
              tdsSelector?.item(6).textContent as string
            ),
          },
        },
      },
      matchDetail: {
        odiMatches: {
          battingStats: {
            runs: parseInt(tdsSelector?.item(1).textContent as string),
            balls: parseInt(tdsSelector?.item(3).textContent as string),
            opps: tdsSelector?.item(3).textContent as string,
            date: tdsSelector?.item(1).textContent as string,
            fours: parseInt(tdsSelector?.item(4).textContent as string),
            sixes: parseInt(tdsSelector?.item(0).textContent as string),
            strikeRate: parseFloat(tdsSelector?.item(0).textContent as string),
            minutes: 0,
            venue: tdsSelector?.item(2).textContent as string,
            notOut: tdsSelector?.item(1).textContent?.includes("*") as boolean,
          },
          bowlingStats: {
            overs: parseFloat(tdsSelector?.item(1).textContent as string),
            maidens: parseInt(tdsSelector?.item(2).textContent as string),
            wickets: parseInt(tdsSelector?.item(0).textContent as string),
            runsConceded: parseInt(tdsSelector?.item(0).textContent as string),
            economy: parseFloat(tdsSelector?.item(2).textContent as string),
            forTeam: tdsSelector?.item(6).textContent as string,
            opps: tdsSelector?.item(7).textContent as string,
            date: tdsSelector?.item(9).textContent as string,
          },
          fieldingStats: {
            matches: parseInt(tdsSelector?.item(2).textContent as string),
            innings: parseInt(tdsSelector?.item(3).textContent as string),
            catches: parseInt(tdsSelector?.item(2).textContent as string),
            maxCatches: parseInt(tdsSelector?.item(3).textContent as string),
            catchesPerInning: parseFloat(
              tdsSelector?.item(3).textContent as string
            ),
          },
        },
        t20IMatches: {
          battingStats: {
            runs: parseInt(tdsSelector?.item(1).textContent as string),
            balls: parseInt(tdsSelector?.item(3).textContent as string),
            opps: tdsSelector?.item(8).textContent as string,
            date: tdsSelector?.item(10).textContent as string,
            fours: parseInt(tdsSelector?.item(4).textContent as string),
            sixes: parseInt(tdsSelector?.item(5).textContent as string),
            strikeRate: parseFloat(tdsSelector?.item(6).textContent as string),
            minutes: 0,
            venue: tdsSelector?.item(9).textContent as string,
            notOut: tdsSelector?.item(1).textContent?.includes("*") as boolean,
          },
          bowlingStats: {
            overs: parseFloat(tdsSelector?.item(1).textContent as string),
            maidens: parseInt(tdsSelector?.item(2).textContent as string),
            wickets: parseInt(tdsSelector?.item(4).textContent as string),
            runsConceded: parseInt(tdsSelector?.item(3).textContent as string),
            economy: parseFloat(tdsSelector?.item(5).textContent as string),
            forTeam: tdsSelector?.item(6).textContent as string,
            opps: tdsSelector?.item(7).textContent as string,
            date: tdsSelector?.item(9).textContent as string,
          },
          fieldingStats: {
            matches: parseInt(tdsSelector?.item(2).textContent as string),
            innings: parseInt(tdsSelector?.item(3).textContent as string),
            catches: parseInt(tdsSelector?.item(2).textContent as string),
            maxCatches: parseInt(tdsSelector?.item(3).textContent as string),
            catchesPerInning: parseFloat(
              tdsSelector?.item(3).textContent as string
            ),
          },
        },
        testMatches: {
          battingStats: {
            runs: parseInt(tdsSelector?.item(5).textContent as string),
            balls: parseInt(tdsSelector?.item(2).textContent as string),
            opps: tdsSelector?.item(0).textContent as string,
            date: tdsSelector?.item(0).textContent as string,
            fours: parseInt(tdsSelector?.item(4).textContent as string),
            sixes: parseInt(tdsSelector?.item(0).textContent as string),
            strikeRate: parseFloat(tdsSelector?.item(0).textContent as string),
            minutes: 0,
            venue: tdsSelector?.item(0).textContent as string,
            notOut: tdsSelector?.item(0).textContent?.includes("*") as boolean,
          },
          bowlingStats: {
            overs: parseFloat(tdsSelector?.item(1).textContent as string),
            maidens: parseInt(tdsSelector?.item(2).textContent as string),
            wickets: parseInt(tdsSelector?.item(0).textContent as string),
            runsConceded: parseInt(tdsSelector?.item(0).textContent as string),
            economy: parseFloat(tdsSelector?.item(2).textContent as string),
            forTeam: tdsSelector?.item(6).textContent as string,
            opps: tdsSelector?.item(7).textContent as string,
            date: tdsSelector?.item(9).textContent as string,
          },
          fieldingStats: null,
        },
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
