import { useQuery } from "react-query";
import { ApiData } from "../../models/Api";
import {
  Batsman,
  Bowler,
  CricketMatch,
  MatchSquad,
  PointsTableRow,
} from "../../models/espn-cricinfo-models/CricketMatchModels";
import axios, { AxiosResponse } from "axios";

const fetchCricketMatch = (url: string): Promise<AxiosResponse<ApiData>> => {
  return axios.get(`https://stats.espncricinfo.com${url}`);
};

const useFetchPlaying11 = (url: string, isEnabled: boolean) => {
  const playing11Url = url?.replace("full-scorecard", "match-playing-xi");
  //const playing11Url = url.replace("full-scorecard", "match-squads");

  const { data: playing11Data } = useQuery(
    [playing11Url],
    () => fetchCricketMatch(playing11Url),
    {
      enabled: isEnabled,
    }
  );

  const playing11DivElement = document.createElement("div");

  playing11DivElement.innerHTML = playing11Data?.data.toString() as string;

  const tableSelector = playing11DivElement.querySelector(
    ".ds-w-full.ds-table.ds-table-sm.ds-table-bordered.ds-border-collapse.ds-border.ds-border-line.ds-table-auto.ds-bg-fill-content-prime"
  );

  const thSelector = tableSelector?.querySelectorAll("thead th");
  const trSelector = tableSelector?.querySelectorAll("tbody > tr");

  const matchSquad: MatchSquad = {
    team1SquadInfo: {
      teamName: thSelector?.item(1)?.textContent || "",
      teamSquad: [],
    },
    team2SquadInfo: {
      teamName: thSelector?.item(2)?.textContent || "",
      teamSquad: [],
    },
  };

  trSelector?.forEach((tr) => {
    matchSquad.team1SquadInfo.teamSquad.push({
      player: {
        name:
          tr?.querySelectorAll("td a").item(0)?.textContent?.trim() ||
          "unknown",
        href: tr?.querySelectorAll("td a").item(0)?.getAttribute("href") || "",
      },
      role: tr?.querySelectorAll("td p").item(0)?.textContent || "",
    });

    matchSquad.team2SquadInfo.teamSquad.push({
      player: {
        name:
          tr?.querySelectorAll("td a").item(1)?.textContent?.trim() ||
          "unknown",
        href: tr?.querySelectorAll("td a").item(1)?.getAttribute("href") || "",
      },
      role: tr?.querySelectorAll("td p").item(1)?.textContent || "",
    });
  });

  return matchSquad;
};

const useFetchPointsTable = (url: string, isEnabled: boolean) => {
  const pointsTableUrl = url?.replace(
    "full-scorecard",
    "points-table-standings"
  );

  const { data: pointsTable } = useQuery(
    [pointsTableUrl],
    () => fetchCricketMatch(pointsTableUrl),
    {
      enabled: isEnabled,
    }
  );

  const ptDivElement = document.createElement("div");

  ptDivElement.innerHTML = pointsTable?.data.toString() as string;

  const tableSelector = ptDivElement.querySelectorAll(
    ".ds-w-full.ds-table > tbody > tr.ds-text-tight-s"
  );

  const pointsTableArr: PointsTableRow[] = [];

  tableSelector.forEach((tr) => {
    const tdsSelector = tr?.querySelectorAll("td");
    pointsTableArr.push({
      rank: parseInt(tr?.querySelector("td > a span")?.textContent || "0"),
      teamName: tr?.querySelector("td > a div")?.textContent || "",
      matches: parseInt(tdsSelector?.item(1).textContent as string),
      won: parseInt(tdsSelector?.item(2).textContent as string),
      lost: parseInt(tdsSelector?.item(3).textContent as string),
      tied: parseInt(tdsSelector?.item(4).textContent as string),
      noResult: parseInt(tdsSelector?.item(5).textContent as string),
      netRR: parseFloat(tdsSelector?.item(7).textContent as string),
      points: parseInt(tdsSelector?.item(6).textContent as string),
    });
  });

  return pointsTableArr;
};

export const useFetchMatchByUrl = (url: string): CricketMatch => {
  const { isLoading, data } = useQuery([url], () => fetchCricketMatch(url));

  const matchSquad = useFetchPlaying11(url, !isLoading);

  const pointsTable = useFetchPointsTable(url, !isLoading);

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
    matchUuid: "00000000-0000-0000-0000-000000000000",
    season,
    series,
    playerOfTheMatch,
    matchNo,
    matchDays,
    matchTitle: `${team1Name} vs ${team2Name}`,
    venue: tableData?.querySelector("span")?.innerHTML as string,
    matchDate: matchDays?.split(" - ")[0],
    tossWinner: tossDetail.split(",")[0],
    tossDecision: tossDetail,
    result: headerData?.querySelector("p > span")?.innerHTML as string,
    matchSquad,
    pointsTable,
    team1: {
      teamName: team1Name,
      team: {
        name: team1Name,
        uuid: "00000000-0000-0000-0000-000000000000",
      },
      battingScorecard: [],
      bowlingScorecard: [],
      extras: "",
      fallOfWickets: [],
      didNotBat: [],
    },
    team2: {
      teamName: team2Name,
      team: {
        name: team2Name,
        uuid: "00000000-0000-0000-0000-000000000000",
      },
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
          !pName.includes("Yet to bat:") &&
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

        if (
          (pName.includes("Did not bat:") || pName.includes("Yet to bat:")) &&
          teamDetails
        ) {
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

  console.log([cricketMatch]);

  return cricketMatch;
};
