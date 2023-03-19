import axios, { AxiosResponse } from "axios";
import { useQueries, useQuery } from "react-query";
import { CricketFormat } from "../CricketMatchRecords/Models/Interface";

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

interface CricketMatchDetail {
  team1: string;
  team2: string;
  winner: string;
  margin: string;
  ground: string;
  matchDate: string;
  matchNo: string;
  href: string;
}

export interface CricketMatchesBySeason {
  season: number;
  matchDetails: CricketMatchDetail[];
}

export interface CricketMatch {
  matchUuid: string;
  season: string;
  series: string;
  playerOfTheMatch: string;
  matchNo: string;
  matchDays: string;
  matchTitle: string;
  venue: string;
  matchDate: string;
  tossWinner: string;
  tossDecision: string;
  result: string;
  team1: Team;
  team2: Team;
  tvUmpire: string;
  matchReferee: string;
  reserveUmpire: string;
  umpires: string[];
  internationalDebut: string[];
  formatDebut: string[];
}

export interface Team {
  teamName: string;
  battingScorecard: Batsman[];
  bowlingScorecard: Bowler[];
  extras: string;
  fallOfWickets: string[];
  didNotBat: Player[];
}

interface Player {
  name: string;
  href: string;
}

interface Batsman {
  playerName: Player;
  outStatus: string;
  runsScored: number;
  ballsFaced: number;
  minutes: number;
  fours: number;
  sixes: number;
}

interface Bowler {
  playerName: Player;
  oversBowled: number;
  maidens: number;
  runsConceded: number;
  wickets: number;
  wideBall: number;
  noBall: number;
  dots: number;
  fours: number;
  sixes: number;
}

export interface CricketMatchTest {
  matchUuid: string;
  season: string;
  series: string;
  seriesResult: string;
  playerOfTheMatch: string;
  matchNo: string;
  matchDays: string;
  matchTitle: string;
  venue: string;
  matchDate: string;
  tossWinner: string;
  tossDecision: string;
  result: string;
  team1: TestTeam;
  team2: TestTeam;
  tvUmpire: string;
  matchReferee: string;
  reserveUmpire: string;
  umpires: string[];
  internationalDebut: string[];
  formatDebut: string[];
}

export interface TestTeam {
  teamName: string;
  inning1: InningDetail;
  inning2: InningDetail;
}

export interface InningDetail {
  battingScorecard: Batsman[];
  bowlingScorecard: Bowler[];
  extras: string;
  fallOfWickets: string[];
  didNotBat: Player[];
}

const fetchCricketMatches = (
  format: CricketFormat,
  year: number
): Promise<AxiosResponse<ApiData>> => {
  let classNo = 0;
  if (format === CricketFormat.Test) {
    classNo = 1;
  }
  if (format === CricketFormat.ODI) {
    classNo = 2;
  }
  if (format === CricketFormat.T20I) {
    classNo = 3;
  }

  return axios.get(
    `https://stats.espncricinfo.com/ci/engine/records/team/match_results.html?class=${classNo};id=${year};type=year`
  );
};

const fetchCricketMatch = (url: string): Promise<AxiosResponse<ApiData>> => {
  return axios.get(`https://stats.espncricinfo.com${url}`);
};

export const useCricketMatchesBySeason = (
  format: CricketFormat,
  years: number[]
): CricketMatchesBySeason[] => {
  const queries = [];

  const queryOptions = {
    refetchOnWindowFocus: false,
    refetchOnMount: false,
    enabled: true,
    cacheTime: 60 * 60 * 1000,
    retry: true,
  };

  for (let i = 0; i < years.length; i++) {
    queries.push({
      queryKey: ["matches-all", years[i]],
      queryFn: () => fetchCricketMatches(format, years[i]),
      ...queryOptions,
    });
  }

  const result = useQueries(queries);

  const cricketMatchesAll: CricketMatchesBySeason[] = [];

  result.map((r, i) => {
    const divElement = document.createElement("div");

    divElement.innerHTML = (r.data as ApiResponse)?.data.toString() as string;

    const allTableRows = divElement
      .querySelector(".div630Pad table.engineTable")
      ?.querySelectorAll("tr.data1");

    const cricketMatches: CricketMatchesBySeason = {
      season: years[i],
      matchDetails: [],
    };

    allTableRows?.forEach((x) => {
      const tdSelector = x?.querySelectorAll("td");
      cricketMatches.matchDetails.push({
        team1: tdSelector[0]?.textContent,
        team2: tdSelector[1]?.textContent,
        winner: tdSelector[2]?.textContent,
        margin: tdSelector[3]?.textContent,
        ground: tdSelector[4]?.textContent,
        matchDate: tdSelector[5]?.textContent,
        matchNo: tdSelector[6]?.textContent,
        href: tdSelector[6]?.querySelector("a")?.getAttribute("href"),
      } as CricketMatchDetail);
    });

    cricketMatchesAll.push(cricketMatches);
  });

  return cricketMatchesAll;
};

export const useCricketMatch = (url: string): CricketMatch => {
  const { data } = useQuery([url], () => fetchCricketMatch(url));

  const divElement = document.createElement("div");

  divElement.innerHTML = data?.data.toString() as string;

  const headerData = divElement.querySelector(
    ".ds-w-full .ds-text-compact-xxs"
  );
  const tableData = divElement.querySelector("table.ds-table.ds-table-sm");
  const tableRows = tableData?.querySelectorAll("tr");

  const teamsData = divElement.querySelectorAll(".ds-rounded-lg.ds-mt-2");

  const team1Data = teamsData[0];
  const team2Data = teamsData[1];
  const tablesTeam1Data = team1Data?.querySelectorAll("table.ds-w-full");
  const tablesTeam2Data = team2Data?.querySelectorAll("table.ds-w-full");

  // console.log(tablesTeam1Data, tablesTeam2Data)

  const teamNames = headerData?.querySelectorAll(
    ".ci-team-score > div:first-child"
  );
  const team1Name = teamNames?.item(0)?.getAttribute("title") as string;
  const team2Name = teamNames?.item(1)?.getAttribute("title") as string;
  const matchShortInfo = headerData?.querySelector(".ds-truncate")
    ?.innerHTML as string;

  let tossDetail = "";
  let season = "";
  let series = "";
  let playerOfTheMatch = "";
  let matchNo = "";
  let matchDays = "";
  let tvUmpire = "";
  let matchReferee = "";
  let reserveUmpire = "";
  let umpires: string[] = [];
  let formatDebut: string[] = [];
  let internationalDebut: string[] = [];

  for (let i = 1; i < (tableRows?.length as number); i++) {
    const tableRow = tableRows?.item(i);
    const rowHeader = tableRow?.querySelectorAll("td span")[0]?.innerHTML;
    const rowValue = tableRow?.querySelectorAll("td span")[1]
      ?.textContent as string;
    if (rowHeader === "Toss") {
      tossDetail = rowValue;
    }
    if (rowHeader === "Season") {
      season = rowValue;
    }
    if (rowHeader === "Series") {
      series = rowValue;
    }
    if (rowHeader === "Player Of The Match") {
      playerOfTheMatch = rowValue;
    }
    if (rowHeader === "Match number") {
      matchNo = rowValue;
    }
    if (rowHeader === "Match days") {
      matchDays = rowValue;
    }
    if (rowHeader === "TV Umpire") {
      tvUmpire = rowValue;
    }
    if (rowHeader === "Reserve Umpire") {
      reserveUmpire = rowValue;
    }
    if (rowHeader === "Match Referee") {
      matchReferee = rowValue;
    }
    if (rowHeader === "Umpires") {
      const umpiresRows = tableRow?.querySelectorAll("td span");
      umpiresRows?.forEach(
        (x, i) => i > 0 && umpires.push(x?.textContent as string)
      );
      umpires = Array.from(new Set(umpires));
    }
    if (rowHeader === "T20I debut" || rowHeader === "ODI debut") {
      const iDebutRows = tableRow?.querySelectorAll("td span");
      iDebutRows?.forEach(
        (x, i) => i > 0 && internationalDebut.push(x?.textContent as string)
      );
      internationalDebut = Array.from(new Set(internationalDebut));
    }
    if (rowHeader === "T20 debut") {
      const debutRows = tableRow?.querySelectorAll("td span");
      debutRows?.forEach(
        (x, i) => i > 0 && formatDebut.push(x?.textContent as string)
      );
      formatDebut = Array.from(new Set(formatDebut));
    }
  }

  const cricketMatch: CricketMatch = {
    matchUuid: "",
    season,
    series,
    playerOfTheMatch,
    matchNo,
    matchDays,
    matchTitle: `${team1Name} vs ${team2Name}`,
    venue: tableData?.querySelector("span")?.innerHTML as string,
    matchDate: matchShortInfo,
    tossWinner: tossDetail.split(",")[0],
    tossDecision: tossDetail,
    result: headerData?.querySelector("p > span")?.innerHTML as string,
    team1: {
      teamName: team1Name,
      battingScorecard: [],
      bowlingScorecard: [],
      extras: "",
      fallOfWickets: [],
      didNotBat: [],
    },
    team2: {
      teamName: team2Name,
      battingScorecard: [],
      bowlingScorecard: [],
      extras: "",
      fallOfWickets: [],
      didNotBat: [],
    },
    tvUmpire,
    matchReferee,
    reserveUmpire,
    umpires,
    formatDebut,
    internationalDebut,
  };

  const setTeamScoreData = (
    tablesTeamData: NodeListOf<Element>,
    teamName: string
  ) => {
    const teamDetails = [cricketMatch.team1, cricketMatch.team2].find(
      (x) => x.teamName === teamName
    );

    tablesTeamData
      ?.item(0)
      ?.querySelectorAll("tbody tr")
      .forEach((tr) => {
        const scoreSelector = tr?.querySelectorAll(
          "td"
        ) as NodeListOf<HTMLTableCellElement>;
        const pName = tr?.querySelector("td")?.textContent as string;
        const pHref = tr?.querySelector("td a")?.getAttribute("href") as string;
        !pName.includes("TOTAL") &&
          !pName.includes("Fall of wickets:") &&
          !pName.includes("Extras") &&
          !pName.includes("Did not bat:") &&
          pName.length > 0 &&
          teamDetails?.battingScorecard.push({
            playerName: {
              name: pName,
              href: pHref,
            },
            outStatus: scoreSelector[1]?.textContent as string,
            runsScored: parseInt(
              tr?.querySelector("td strong")?.textContent as string
            ),
            ballsFaced: parseInt(scoreSelector[3]?.textContent as string),
            minutes: parseInt(scoreSelector[4]?.textContent as string),
            fours: parseInt(scoreSelector[5]?.textContent as string),
            sixes: parseInt(scoreSelector[6]?.textContent as string),
          } as Batsman);

        if (pName.includes("Extra") && teamDetails) {
          teamDetails["extras"] = scoreSelector?.item(1)?.innerHTML;
        }

        if (pName.includes("Fall of wickets:") && teamDetails) {
          scoreSelector
            ?.item(0)
            ?.querySelectorAll("span")
            .forEach((x, i) => {
              i === 0 &&
                teamDetails["fallOfWickets"].push(x?.textContent as string);
              i > 0 &&
                teamDetails["fallOfWickets"].push(
                  x?.textContent?.slice(2) as string
                );
            });
        }

        if (pName.includes("Did not bat: ") && teamDetails) {
          scoreSelector
            ?.item(0)
            ?.querySelectorAll("a")
            .forEach((x) => {
              const dnpPlayerName = x
                ?.querySelector("span")
                ?.textContent?.replace(",", "")
                .trim() as string;
              const href = x?.getAttribute("href") as string;
              teamDetails["didNotBat"]
                .map((x) => x.name)
                ?.indexOf(dnpPlayerName) === -1 &&
                teamDetails["didNotBat"].push({
                  name: dnpPlayerName,
                  href,
                });
            });
        }
      });

    tablesTeamData
      ?.item(1)
      ?.querySelectorAll("tbody tr:not(.ds-hidden)")
      .forEach((tr) => {
        const scoreSelector = tr?.querySelectorAll(
          "td"
        ) as NodeListOf<HTMLTableCellElement>;
        const pName = tr?.querySelector("td")?.textContent as string;
        const pHref = tr?.querySelector("td a")?.getAttribute("href") as string;
        !pName.includes("Team:") &&
          teamDetails?.bowlingScorecard.push({
            playerName: {
              name: pName,
              href: pHref,
            },
            oversBowled: parseFloat(scoreSelector[1]?.textContent as string),
            maidens: parseInt(scoreSelector[2]?.textContent as string),
            runsConceded: parseInt(scoreSelector[3]?.textContent as string),
            wickets: parseInt(
              tr?.querySelector("td strong")?.textContent as string
            ),
            dots: parseInt(scoreSelector[6]?.textContent as string),
            fours: parseInt(scoreSelector[7]?.textContent as string),
            sixes: parseInt(scoreSelector[8]?.textContent as string),
            wideBall: parseInt(scoreSelector[9]?.textContent as string),
            noBall: parseInt(scoreSelector[10]?.textContent as string),
          } as Bowler);
      });
  };

  setTeamScoreData(tablesTeam1Data, team1Name);
  setTeamScoreData(tablesTeam2Data, team2Name);

  return cricketMatch;
};

export const useCricketMatchesBySeasonDetails = (
  year: number,
  matchDetails: CricketMatchDetail[]
) => {
  const queries = [];

  const queryOptions = {
    refetchOnWindowFocus: false,
    refetchOnMount: false,
    enabled: true,
    cacheTime: 60 * 60 * 1000,
    retry: true,
  };

  for (let i = 0; i < matchDetails.length; i++) {
    queries.push({
      queryKey: ["matches-season", matchDetails[i].href],
      queryFn: () => fetchCricketMatch(matchDetails[i].href),
      ...queryOptions,
    });
  }

  const result = useQueries(queries);

  const cricketMatches: CricketMatch[] = [];

  result.map((r, i) => {
    const divElement = document.createElement("div");

    divElement.innerHTML = (r.data as ApiResponse)?.data.toString() as string;

    const headerData = divElement.querySelector(
      ".ds-w-full .ds-text-compact-xxs"
    );
    const tableData = divElement.querySelector("table.ds-table.ds-table-sm");
    const tableRows = tableData?.querySelectorAll("tr");

    const teamsData = divElement.querySelectorAll(".ds-rounded-lg.ds-mt-2");

    const team1Data = teamsData[0];
    const team2Data = teamsData[1];
    const tablesTeam1Data = team1Data?.querySelectorAll("table.ds-w-full");
    const tablesTeam2Data = team2Data?.querySelectorAll("table.ds-w-full");

    const teamNames = headerData?.querySelectorAll(
      ".ci-team-score > div:first-child"
    );
    const team1Name = teamNames?.item(0)?.getAttribute("title") as string;
    const team2Name = teamNames?.item(1)?.getAttribute("title") as string;

    let tossDetail = "";
    let season = "";
    let series = "";
    let playerOfTheMatch = "";
    let matchNo = "";
    let matchDays = "";
    let tvUmpire = "";
    let matchReferee = "";
    let reserveUmpire = "";
    let umpires: string[] = [];
    let formatDebut: string[] = [];
    let internationalDebut: string[] = [];

    for (let i = 1; i < (tableRows?.length as number); i++) {
      const tableRow = tableRows?.item(i);
      const rowHeader = tableRow?.querySelectorAll("td span")[0]?.innerHTML;
      const rowValue = tableRow?.querySelectorAll("td span")[1]
        ?.textContent as string;

      if (rowHeader === "Toss") {
        tossDetail = rowValue;
      }
      if (rowHeader === "Season") {
        season = rowValue;
      }
      if (rowHeader === "Series") {
        series = rowValue;
      }
      if (rowHeader === "Player Of The Match") {
        playerOfTheMatch = rowValue;
      }
      if (rowHeader === "Match number") {
        matchNo = rowValue;
      }
      if (rowHeader === "Match days") {
        matchDays = rowValue;
      }
      if (rowHeader === "TV Umpire") {
        tvUmpire = rowValue;
      }
      if (rowHeader === "Reserve Umpire") {
        reserveUmpire = rowValue;
      }
      if (rowHeader === "Match Referee") {
        matchReferee = rowValue;
      }
      if (rowHeader === "Umpires") {
        const umpiresRows = tableRow?.querySelectorAll("td span");
        umpiresRows?.forEach(
          (x, i) => i > 0 && umpires.push(x?.textContent as string)
        );
        umpires = Array.from(new Set(umpires));
      }
      if (rowHeader === "T20I debut" || rowHeader === "ODI debut") {
        const iDebutRows = tableRow?.querySelectorAll("td span");
        iDebutRows?.forEach(
          (x, i) => i > 0 && internationalDebut.push(x?.textContent as string)
        );
        internationalDebut = Array.from(new Set(internationalDebut));
      }
      if (rowHeader === "T20 debut") {
        const debutRows = tableRow?.querySelectorAll("td span");
        debutRows?.forEach(
          (x, i) => i > 0 && formatDebut.push(x?.textContent as string)
        );
        formatDebut = Array.from(new Set(formatDebut));
      }
    }

    const cricketMatch: CricketMatch = {
      matchUuid: "",
      season,
      series,
      playerOfTheMatch,
      matchNo,
      matchDays,
      matchTitle: `${team1Name} vs ${team2Name}`,
      venue: tableData?.querySelector("span")?.innerHTML as string,
      matchDate: matchDetails[i].matchDate,
      tossWinner: tossDetail.split(",")[0],
      tossDecision: tossDetail,
      result: headerData?.querySelector("p > span")?.innerHTML as string,
      team1: {
        teamName: team1Name,
        battingScorecard: [],
        bowlingScorecard: [],
        extras: "",
        fallOfWickets: [],
        didNotBat: [],
      },
      team2: {
        teamName: team2Name,
        battingScorecard: [],
        bowlingScorecard: [],
        extras: "",
        fallOfWickets: [],
        didNotBat: [],
      },
      tvUmpire,
      matchReferee,
      reserveUmpire,
      umpires,
      formatDebut,
      internationalDebut,
    };

    const setTeamScoreData = (
      tablesTeamData: NodeListOf<Element>,
      teamName: string
    ) => {
      const teamDetails = [cricketMatch.team1, cricketMatch.team2].find(
        (x) => x.teamName === teamName
      );

      tablesTeamData
        ?.item(0)
        ?.querySelectorAll("tbody tr")
        .forEach((tr) => {
          const scoreSelector = tr?.querySelectorAll(
            "td"
          ) as NodeListOf<HTMLTableCellElement>;
          const pName = tr?.querySelector("td")?.textContent as string;
          const pHref = tr
            ?.querySelector("td a")
            ?.getAttribute("href") as string;
          !pName.includes("TOTAL") &&
            !pName.includes("Fall of wickets:") &&
            !pName.includes("Extras") &&
            !pName.includes("Did not bat:") &&
            pName.length > 0 &&
            teamDetails?.battingScorecard.push({
              playerName: {
                name: pName,
                href: pHref,
              },
              outStatus: scoreSelector[1]?.textContent as string,
              runsScored: parseInt(
                tr?.querySelector("td strong")?.textContent as string
              ),
              ballsFaced: parseInt(scoreSelector[3]?.textContent as string),
              minutes: parseInt(scoreSelector[4]?.textContent as string),
              fours: parseInt(scoreSelector[5]?.textContent as string),
              sixes: parseInt(scoreSelector[6]?.textContent as string),
            } as Batsman);

          if (pName.includes("Extra") && teamDetails) {
            teamDetails["extras"] = scoreSelector?.item(1)?.innerHTML;
          }

          if (pName.includes("Fall of wickets:") && teamDetails) {
            scoreSelector
              ?.item(0)
              ?.querySelectorAll("span")
              .forEach((x, i) => {
                i === 0 &&
                  teamDetails["fallOfWickets"].push(x?.textContent as string);
                i > 0 &&
                  teamDetails["fallOfWickets"].push(
                    x?.textContent?.slice(2) as string
                  );
              });
          }

          if (pName.includes("Did not bat: ") && teamDetails) {
            scoreSelector
              ?.item(0)
              ?.querySelectorAll("a")
              .forEach((x) => {
                const dnpPlayerName = x
                  ?.querySelector("span")
                  ?.textContent?.replace(",", "")
                  .trim() as string;
                const href = x?.getAttribute("href") as string;
                teamDetails["didNotBat"]
                  .map((x) => x.name)
                  ?.indexOf(dnpPlayerName) === -1 &&
                  teamDetails["didNotBat"].push({
                    name: dnpPlayerName,
                    href,
                  });
              });
          }
        });

      tablesTeamData
        ?.item(1)
        ?.querySelectorAll("tbody tr:not(.ds-hidden)")
        .forEach((tr) => {
          const scoreSelector = tr?.querySelectorAll(
            "td"
          ) as NodeListOf<HTMLTableCellElement>;
          const pName = tr?.querySelector("td")?.textContent as string;
          const pHref = tr
            ?.querySelector("td a")
            ?.getAttribute("href") as string;
          !pName.includes("Team:") &&
            teamDetails?.bowlingScorecard.push({
              playerName: {
                name: pName,
                href: pHref,
              },
              oversBowled: parseFloat(scoreSelector[1]?.textContent as string),
              maidens: parseInt(scoreSelector[2]?.textContent as string),
              runsConceded: parseInt(scoreSelector[3]?.textContent as string),
              wickets: parseInt(
                tr?.querySelector("td strong")?.textContent as string
              ),
              dots: parseInt(scoreSelector[6]?.textContent as string),
              fours: parseInt(scoreSelector[7]?.textContent as string),
              sixes: parseInt(scoreSelector[8]?.textContent as string),
              wideBall: parseInt(scoreSelector[9]?.textContent as string),
              noBall: parseInt(scoreSelector[10]?.textContent as string),
            } as Bowler);
        });
    };

    setTeamScoreData(tablesTeam1Data, team1Name);
    setTeamScoreData(tablesTeam2Data, team2Name);

    cricketMatches.push(cricketMatch);
  });

  const fetchedLength = cricketMatches.filter(
    (x) => x.series.length > 0
  ).length;

  console.log(`Year:${year}::${fetchedLength}/${cricketMatches.length}`);

  if (fetchedLength === cricketMatches.length) {
    console.log(cricketMatches);
  }

  return null;
};

export const useTestCricketMatchesBySeasonDetails = (
  matchDetails: CricketMatchDetail[]
) => {
  const queries = [];

  const queryOptions = {
    refetchOnWindowFocus: false,
    refetchOnMount: false,
    enabled: true,
    cacheTime: 60 * 60 * 1000,
    retry: true,
  };

  for (let i = 0; i < matchDetails.length; i++) {
    queries.push({
      queryKey: ["test-matches-season", matchDetails[i].href],
      queryFn: () => fetchCricketMatch(matchDetails[i].href),
      ...queryOptions,
    });
  }

  const result = useQueries(queries);

  const cricketMatches: CricketMatchTest[] = [];

  result.map((r, i) => {
    const divElement = document.createElement("div");

    divElement.innerHTML = (r.data as ApiResponse)?.data.toString() as string;

    const headerData = divElement.querySelector(
      ".ds-w-full .ds-text-compact-xxs"
    );
    const tableData = divElement.querySelector("table.ds-table.ds-table-sm");
    const tableRows = tableData?.querySelectorAll("tr");

    const teamsData = divElement.querySelectorAll(".ds-rounded-lg.ds-mt-2");

    const team1Data = [teamsData[0], teamsData[2]];
    const team2Data = [teamsData[1], teamsData[3]];
    const tablesTeam1Data = team1Data.map((td) =>
      td?.querySelectorAll("table.ds-w-full")
    );
    const tablesTeam2Data = team2Data.map((td) =>
      td?.querySelectorAll("table.ds-w-full")
    );

    const teamNames = headerData?.querySelectorAll(
      ".ci-team-score > div:first-child"
    );
    const team1Name = teamNames?.item(0)?.getAttribute("title") as string;
    const team2Name = teamNames?.item(1)?.getAttribute("title") as string;

    let tossDetail = "";
    let season = "";
    let series = "";
    let seriesResult = "";
    let playerOfTheMatch = "";
    let matchNo = "";
    let matchDays = "";
    let tvUmpire = "";
    let matchReferee = "";
    let reserveUmpire = "";
    let umpires: string[] = [];
    let formatDebut: string[] = [];
    let internationalDebut: string[] = [];

    for (let i = 1; i < (tableRows?.length as number); i++) {
      const tableRow = tableRows?.item(i);
      const rowHeader = tableRow?.querySelectorAll("td span")[0]?.innerHTML;
      const rowValue = tableRow?.querySelectorAll("td span")[1]
        ?.textContent as string;

      if (rowHeader === "Toss") {
        tossDetail = rowValue;
      }
      if (rowHeader === "Season") {
        season = rowValue;
      }
      if (rowHeader === "Series result") {
        seriesResult = rowValue;
      }
      if (rowHeader === "Series") {
        series = rowValue;
      }
      if (rowHeader === "Player Of The Match") {
        playerOfTheMatch = rowValue;
      }
      if (rowHeader === "Match number") {
        matchNo = rowValue;
      }
      if (rowHeader === "Match days") {
        matchDays = rowValue;
      }
      if (rowHeader === "TV Umpire") {
        tvUmpire = rowValue;
      }
      if (rowHeader === "Reserve Umpire") {
        reserveUmpire = rowValue;
      }
      if (rowHeader === "Match Referee") {
        matchReferee = rowValue;
      }
      if (rowHeader === "Umpires") {
        const umpiresRows = tableRow?.querySelectorAll("td span");
        umpiresRows?.forEach(
          (x, i) => i > 0 && umpires.push(x?.textContent as string)
        );
        umpires = Array.from(new Set(umpires));
      }
      if (rowHeader === "Test debut") {
        const iDebutRows = tableRow?.querySelectorAll("td span");
        iDebutRows?.forEach(
          (x, i) => i > 0 && internationalDebut.push(x?.textContent as string)
        );
        internationalDebut = Array.from(new Set(internationalDebut));
      }
      if (rowHeader === "T20 debut") {
        const debutRows = tableRow?.querySelectorAll("td span");
        debutRows?.forEach(
          (x, i) => i > 0 && formatDebut.push(x?.textContent as string)
        );
        formatDebut = Array.from(new Set(formatDebut));
      }
    }

    const cricketMatch: CricketMatchTest = {
      matchUuid: "",
      season,
      series,
      seriesResult,
      playerOfTheMatch,
      matchNo,
      matchDays,
      matchTitle: `${team1Name} vs ${team2Name}`,
      venue: tableData?.querySelector("span")?.innerHTML as string,
      matchDate: matchDetails[i].matchDate,
      tossWinner: tossDetail.split(",")[0],
      tossDecision: tossDetail,
      result: headerData?.querySelector("p > span")?.innerHTML as string,
      team1: {
        teamName: team1Name,
        inning1: {
          battingScorecard: [],
          bowlingScorecard: [],
          extras: "",
          fallOfWickets: [],
          didNotBat: [],
        },
        inning2: {
          battingScorecard: [],
          bowlingScorecard: [],
          extras: "",
          fallOfWickets: [],
          didNotBat: [],
        },
      },
      team2: {
        teamName: team2Name,
        inning1: {
          battingScorecard: [],
          bowlingScorecard: [],
          extras: "",
          fallOfWickets: [],
          didNotBat: [],
        },
        inning2: {
          battingScorecard: [],
          bowlingScorecard: [],
          extras: "",
          fallOfWickets: [],
          didNotBat: [],
        },
      },
      tvUmpire,
      matchReferee,
      reserveUmpire,
      umpires,
      formatDebut,
      internationalDebut,
    };

    const setTeamScoreData = (
      tablesTeamData: NodeListOf<Element>[],
      teamName: string
    ) => {
      const teamDetails = [cricketMatch.team1, cricketMatch.team2].find(
        (x) => x.teamName === teamName
      );

      //1st Inning

      tablesTeamData[0]
        ?.item(0)
        ?.querySelectorAll("tbody tr")
        .forEach((tr) => {
          const scoreSelector = tr?.querySelectorAll(
            "td"
          ) as NodeListOf<HTMLTableCellElement>;
          const pName = tr?.querySelector("td")?.textContent as string;
          const pHref = tr
            ?.querySelector("td a")
            ?.getAttribute("href") as string;
          !pName.includes("TOTAL") &&
            !pName.includes("Fall of wickets:") &&
            !pName.includes("Extras") &&
            !pName.includes("Did not bat:") &&
            pName.length > 0 &&
            teamDetails?.inning1.battingScorecard.push({
              playerName: {
                name: pName,
                href: pHref,
              },
              outStatus: scoreSelector[1]?.textContent as string,
              runsScored: parseInt(
                tr?.querySelector("td strong")?.textContent as string
              ),
              ballsFaced: parseInt(scoreSelector[3]?.textContent as string),
              minutes: parseInt(scoreSelector[4]?.textContent as string),
              fours: parseInt(scoreSelector[5]?.textContent as string),
              sixes: parseInt(scoreSelector[6]?.textContent as string),
            } as Batsman);

          if (pName.includes("Extra") && teamDetails) {
            teamDetails.inning1["extras"] = scoreSelector?.item(1)?.innerHTML;
          }

          if (pName.includes("Fall of wickets:") && teamDetails) {
            scoreSelector
              ?.item(0)
              ?.querySelectorAll("span")
              .forEach((x, i) => {
                i === 0 &&
                  teamDetails.inning1["fallOfWickets"].push(
                    x?.textContent as string
                  );
                i > 0 &&
                  teamDetails.inning1["fallOfWickets"].push(
                    x?.textContent?.slice(2) as string
                  );
              });
          }

          if (pName.includes("Did not bat: ") && teamDetails) {
            scoreSelector
              ?.item(0)
              ?.querySelectorAll("a")
              .forEach((x) => {
                const dnpPlayerName = x
                  ?.querySelector("span")
                  ?.textContent?.replace(",", "")
                  .trim() as string;
                const href = x?.getAttribute("href") as string;
                teamDetails.inning1["didNotBat"]
                  .map((x) => x.name)
                  ?.indexOf(dnpPlayerName) === -1 &&
                  teamDetails.inning1["didNotBat"].push({
                    name: dnpPlayerName,
                    href,
                  });
              });
          }
        });

      tablesTeamData[0]
        ?.item(1)
        ?.querySelectorAll("tbody tr:not(.ds-hidden)")
        .forEach((tr) => {
          const scoreSelector = tr?.querySelectorAll(
            "td"
          ) as NodeListOf<HTMLTableCellElement>;
          const pName = tr?.querySelector("td")?.textContent as string;
          const pHref = tr
            ?.querySelector("td a")
            ?.getAttribute("href") as string;
          !pName.includes("Team:") &&
            teamDetails?.inning1.bowlingScorecard.push({
              playerName: {
                name: pName,
                href: pHref,
              },
              oversBowled: parseFloat(scoreSelector[1]?.textContent as string),
              maidens: parseInt(scoreSelector[2]?.textContent as string),
              runsConceded: parseInt(scoreSelector[3]?.textContent as string),
              wickets: parseInt(
                tr?.querySelector("td strong")?.textContent as string
              ),
              dots: parseInt(scoreSelector[6]?.textContent as string),
              fours: parseInt(scoreSelector[7]?.textContent as string),
              sixes: parseInt(scoreSelector[8]?.textContent as string),
              wideBall: parseInt(scoreSelector[9]?.textContent as string),
              noBall: parseInt(scoreSelector[10]?.textContent as string),
            } as Bowler);
        });

      //2nd Inning

      tablesTeamData[1]
        ?.item(0)
        ?.querySelectorAll("tbody tr")
        .forEach((tr) => {
          const scoreSelector = tr?.querySelectorAll(
            "td"
          ) as NodeListOf<HTMLTableCellElement>;
          const pName = tr?.querySelector("td")?.textContent as string;
          const pHref = tr
            ?.querySelector("td a")
            ?.getAttribute("href") as string;
          !pName.includes("TOTAL") &&
            !pName.includes("Fall of wickets:") &&
            !pName.includes("Extras") &&
            !pName.includes("Did not bat:") &&
            pName.length > 0 &&
            teamDetails?.inning2.battingScorecard.push({
              playerName: {
                name: pName,
                href: pHref,
              },
              outStatus: scoreSelector[1]?.textContent as string,
              runsScored: parseInt(
                tr?.querySelector("td strong")?.textContent as string
              ),
              ballsFaced: parseInt(scoreSelector[3]?.textContent as string),
              minutes: parseInt(scoreSelector[4]?.textContent as string),
              fours: parseInt(scoreSelector[5]?.textContent as string),
              sixes: parseInt(scoreSelector[6]?.textContent as string),
            } as Batsman);

          if (pName.includes("Extra") && teamDetails) {
            teamDetails.inning2["extras"] = scoreSelector?.item(1)?.innerHTML;
          }

          if (pName.includes("Fall of wickets:") && teamDetails) {
            scoreSelector
              ?.item(0)
              ?.querySelectorAll("span")
              .forEach((x, i) => {
                i === 0 &&
                  teamDetails.inning2["fallOfWickets"].push(
                    x?.textContent as string
                  );
                i > 0 &&
                  teamDetails.inning2["fallOfWickets"].push(
                    x?.textContent?.slice(2) as string
                  );
              });
          }

          if (pName.includes("Did not bat: ") && teamDetails) {
            scoreSelector
              ?.item(0)
              ?.querySelectorAll("a")
              .forEach((x) => {
                const dnpPlayerName = x
                  ?.querySelector("span")
                  ?.textContent?.replace(",", "")
                  .trim() as string;
                const href = x?.getAttribute("href") as string;
                teamDetails.inning2["didNotBat"]
                  .map((x) => x.name)
                  ?.indexOf(dnpPlayerName) === -1 &&
                  teamDetails.inning2["didNotBat"].push({
                    name: dnpPlayerName,
                    href,
                  });
              });
          }
        });

      tablesTeamData[1]
        ?.item(1)
        ?.querySelectorAll("tbody tr:not(.ds-hidden)")
        .forEach((tr) => {
          const scoreSelector = tr?.querySelectorAll(
            "td"
          ) as NodeListOf<HTMLTableCellElement>;
          const pName = tr?.querySelector("td")?.textContent as string;
          const pHref = tr
            ?.querySelector("td a")
            ?.getAttribute("href") as string;
          !pName.includes("Team:") &&
            teamDetails?.inning2.bowlingScorecard.push({
              playerName: {
                name: pName,
                href: pHref,
              },
              oversBowled: parseFloat(scoreSelector[1]?.textContent as string),
              maidens: parseInt(scoreSelector[2]?.textContent as string),
              runsConceded: parseInt(scoreSelector[3]?.textContent as string),
              wickets: parseInt(
                tr?.querySelector("td strong")?.textContent as string
              ),
              dots: parseInt(scoreSelector[6]?.textContent as string),
              fours: parseInt(scoreSelector[7]?.textContent as string),
              sixes: parseInt(scoreSelector[8]?.textContent as string),
              wideBall: parseInt(scoreSelector[9]?.textContent as string),
              noBall: parseInt(scoreSelector[10]?.textContent as string),
            } as Bowler);
        });
    };

    setTeamScoreData(tablesTeam1Data, team1Name);
    setTeamScoreData(tablesTeam2Data, team2Name);

    cricketMatches.push(cricketMatch);
  });

  const fetchedLength = cricketMatches.filter(
    (x) => x.series.length > 0
  ).length;

  console.log(`${fetchedLength}/${cricketMatches.length}`);

  if (fetchedLength === cricketMatches.length) {
    console.log(cricketMatches);
  }

  return null;
};
