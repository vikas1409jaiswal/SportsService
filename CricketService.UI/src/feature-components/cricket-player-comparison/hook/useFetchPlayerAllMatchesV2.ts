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

export interface ESPNPlayerAllMatchesInfo {
  format: string;
  battingCareer: BattingCareer;
  bowlingCareer: BowlingCareer;
  battingPerformances: BattingPerformance[];
  bowlingPerformances: BowlingPerformance[];
  fieldingPerformances: FieldingPerformance[];
}

export interface BattingCareer {
  span: string;
  matches: string;
  innings: string;
  notOuts: string;
  runs: string;
  highScore: string;
  average: string;
  ballsFaced: string;
  strikeRate: string;
  hundreds: string;
  fifties: string;
  ducks: string;
  fours: string;
  sixes: string;
}

export interface BowlingCareer {
  span: string;
  matches: string;
  innings: string;
  overs: string;
  maidens: string;
  runsConceded: string;
  wickets: string;
  bbi: string;
  average: string;
  economy: string;
  strikeRate: string;
  fourWickets: string;
  fiveWickets: string;
}

export interface BattingPerformance {
  runs: string;
  balls: string;
  sixes: string;
  fours: string;
  atPosition: string;
  dismissal: string;
  inning: string;
  opposition: string;
  ground: string;
  date: string;
  matchUrl: string;
}

export interface BowlingPerformance {
  overs: string;
  maidens: string;
  runConceded: string;
  wickets: string;
  atPosition: string;
  inning: string;
  opposition: string;
  ground: string;
  date: string;
  matchUrl: string;
}

export interface FieldingPerformance {
  dismissals: string;
  catches: string;
  stumpings: string;
  catchesWk: string;
  catchesFld: string;
  inning: string;
  opposition: string;
  ground: string;
  date: string;
  matchUrl: string;
}

const fetchESPNPlayerAllMatchesInfo = async (
  hrefNumber: string,
  type: string
): Promise<AxiosResponse<ApiData>> => {
  return await axios.get(
    `https://stats.espncricinfo.com/ci/engine/player/${hrefNumber}.html?class=2;orderby=start;template=results;type=${type};view=innings`
  );
};

const useFetchBattingAllMatchesData = (href: string) => {
  const { data: battingData } = useQuery(
    ["player-all-matches-data", "batting", href],
    () => fetchESPNPlayerAllMatchesInfo(href?.split("-")[2], "batting"),
    { enabled: true }
  );

  const divElement = document.createElement("div");
  divElement.innerHTML = (
    battingData as ApiResponse
  )?.data.toString() as string;

  const selectedTable = Array.from(
    divElement.querySelectorAll(".engineTable")
  ).find((table) =>
    table.querySelector("caption")?.textContent?.includes("Career")
  );

  const dataRow = selectedTable?.querySelector("tr.data1");

  const battingPerformances: BattingPerformance[] = [];

  // dataRows?.forEach((dr) => {
  //   const tdsSelector = dr?.querySelectorAll("td");
  //   const runs = tdsSelector.item(0)?.textContent || "";
  //   runs !== "No records available to match this query" &&
  //     battingPerformances.push({
  //       runs,
  //       balls: tdsSelector.item(2)?.textContent || "",
  //       sixes: tdsSelector.item(4)?.textContent || "",
  //       fours: tdsSelector.item(3)?.textContent || "",
  //       atPosition: tdsSelector.item(6)?.textContent || "",
  //       dismissal: tdsSelector.item(7)?.textContent || "",
  //       inning: tdsSelector.item(8)?.textContent || "",
  //       opposition: tdsSelector.item(10)?.textContent || "",
  //       ground: tdsSelector.item(11)?.textContent || "",
  //       date: tdsSelector.item(12)?.textContent || "",
  //       matchUrl: tdsSelector.item(13)?.textContent || "",
  //     });
  // });

  const tdsSelectors = dataRow?.querySelectorAll("td");

  const battingCareer = {
    span: tdsSelectors?.item(1).textContent || "",
    matches: tdsSelectors?.item(2).textContent || "",
    innings: tdsSelectors?.item(3).textContent || "",
    notOuts: tdsSelectors?.item(4).textContent || "",
    runs: tdsSelectors?.item(5).textContent || "",
    highScore: tdsSelectors?.item(6).textContent || "",
    average: tdsSelectors?.item(7).textContent || "",
    ballsFaced: tdsSelectors?.item(8).textContent || "",
    strikeRate: tdsSelectors?.item(9).textContent || "",
    hundreds: tdsSelectors?.item(10).textContent || "",
    fifties: tdsSelectors?.item(11).textContent || "",
    ducks: tdsSelectors?.item(12).textContent || "",
    fours: tdsSelectors?.item(13).textContent || "",
    sixes: tdsSelectors?.item(14).textContent || "",
  };

  return { battingPerformances, battingCareer };
};

const useFetchBowlingAllMatchesData = (href: string) => {
  const { data: bowlingData } = useQuery(
    ["player-all-matches-data", "bowling", href],
    () => fetchESPNPlayerAllMatchesInfo(href?.split("-")[2], "bowling"),
    { enabled: true }
  );

  const divElement = document.createElement("div");
  divElement.innerHTML = (
    bowlingData as ApiResponse
  )?.data.toString() as string;

  const selectedTable = Array.from(
    divElement.querySelectorAll(".engineTable")
  ).find((table) =>
    table.querySelector("caption")?.textContent?.includes("Career")
  );

  const dataRow = selectedTable?.querySelector("tr.data1");

  const bowlingPerformances: BowlingPerformance[] = [];

  // dataRows?.forEach((dr) => {
  //   const tdsSelector = dr?.querySelectorAll("td");
  //   const overs = tdsSelector.item(0)?.textContent || "";
  //   overs !== "No records available to match this query" &&
  //     bowlingPerformances.push({
  //       overs,
  //       maidens: tdsSelector.item(1)?.textContent || "",
  //       runConceded: tdsSelector.item(2)?.textContent || "",
  //       wickets: tdsSelector.item(3)?.textContent || "",
  //       atPosition: tdsSelector.item(5)?.textContent || "",
  //       inning: tdsSelector.item(6)?.textContent || "",
  //       opposition: tdsSelector.item(8)?.textContent || "",
  //       ground: tdsSelector.item(9)?.textContent || "",
  //       date: tdsSelector.item(10)?.textContent || "",
  //       matchUrl: tdsSelector.item(11)?.textContent || "",
  //     });
  // });

  const tdsSelectors = dataRow?.querySelectorAll("td");

  const bowlingCareer = {
    span: tdsSelectors?.item(1).textContent || "",
    matches: tdsSelectors?.item(2).textContent || "",
    innings: tdsSelectors?.item(3).textContent || "",
    overs: tdsSelectors?.item(4).textContent || "",
    maidens: tdsSelectors?.item(5).textContent || "",
    runsConceded: tdsSelectors?.item(6).textContent || "",
    wickets: tdsSelectors?.item(7).textContent || "",
    bbi: tdsSelectors?.item(8).textContent || "",
    average: tdsSelectors?.item(9).textContent || "",
    economy: tdsSelectors?.item(10).textContent || "",
    strikeRate: tdsSelectors?.item(11).textContent || "",
    fourWickets: tdsSelectors?.item(12).textContent || "",
    fiveWickets: tdsSelectors?.item(13).textContent || "",
  };

  return { bowlingPerformances, bowlingCareer };
};

const useFetchFieldingAllMatchesData = (href: string) => {
  const { data: fieldingData } = useQuery(
    ["player-all-matches-data", "fielding", href],
    () => fetchESPNPlayerAllMatchesInfo(href?.split("-")[2], "fielding"),
    { enabled: true }
  );

  const divElement = document.createElement("div");
  divElement.innerHTML = (
    fieldingData as ApiResponse
  )?.data.toString() as string;

  const selectedTable = Array.from(
    divElement.querySelectorAll(".engineTable")
  ).find((table) =>
    table.querySelector("caption")?.textContent?.includes("Innings")
  );

  const dataRows = selectedTable?.querySelectorAll("tr.data1");

  const fieldingPerformances: FieldingPerformance[] = [];

  dataRows?.forEach((dr) => {
    const tdsSelector = dr?.querySelectorAll("td");
    const dismissals = tdsSelector.item(0)?.textContent || "";
    dismissals !== "No records available to match this query" &&
      fieldingPerformances.push({
        dismissals,
        catches: tdsSelector.item(1)?.textContent || "",
        stumpings: tdsSelector.item(2)?.textContent || "",
        catchesWk: tdsSelector.item(3)?.textContent || "",
        catchesFld: tdsSelector.item(4)?.textContent || "",
        inning: tdsSelector.item(5)?.textContent || "",
        opposition: tdsSelector.item(7)?.textContent || "",
        ground: tdsSelector.item(8)?.textContent || "",
        date: tdsSelector.item(9)?.textContent || "",
        matchUrl: tdsSelector.item(10)?.textContent || "",
      });
  });

  return fieldingPerformances;
};

export const useFetchPlayerAllMatches = (
  href: string
): ESPNPlayerAllMatchesInfo => {
  const { battingPerformances, battingCareer } =
    useFetchBattingAllMatchesData(href);
  const { bowlingPerformances, bowlingCareer } =
    useFetchBowlingAllMatchesData(href);
  const allMatchesInfo: ESPNPlayerAllMatchesInfo = {
    format: "ODI",
    battingCareer,
    bowlingCareer,
    battingPerformances: battingPerformances.reverse(),
    bowlingPerformances: bowlingPerformances.reverse(),
    fieldingPerformances: [],
  };

  console.log(allMatchesInfo);

  return allMatchesInfo;
};
