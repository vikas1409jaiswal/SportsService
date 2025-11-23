import { ESPNTableRow } from "./useCustomESPNTable";
import teamLogos from "../../../data/StaticData/teamLogos.json";

const getFullTeamName = (shortName: string) => {
  switch (shortName) {
    case "IND":
      return "India";
    case "AUS":
      return "Australia";
    case "ENG":
      return "England";
    case "NZ":
      return "New Zealand";
    case "SA":
      return "South Africa";
    case "WI":
      return "West Indies";
    case "PAK":
      return "Pakistan";
    case "SL":
      return "Sri Lanka";
    case "BAN":
      return "Bangladesh";
    case "ZIM":
      return "Zimbabwe";
    case "AFG":
      return "Afghanistan";
    case "IRE":
      return "Ireland";
    case "NED":
      return "Netherlands";
    case "SCO":
      return "Scotland";
    default:
      return shortName;
  }
};

export const mostWicketsInCareer = (
  espnTableRows: ESPNTableRow[],
  tdsSelector: NodeListOf<HTMLTableCellElement>,
  thSelector: NodeListOf<HTMLTableCellElement>,
  isCalendarYear?: boolean
) => {
  const headerKeyMapping: Record<string, string> = {
    "Mat": "Matches",
    "Inns": "Innings",
    "Wkts": "Wickets",
    "Runs": "Conceded",
    "Mdns": "Maidens",
    "BBI": "Best Bowling In Innings",
    "Ave": "Average",
    "Econ": "Economy Rate",
    "SR": "Strike Rate",
    "4": "4 Wicket Hauls",
    "5": "5 Wicket Hauls",
    "10": "10 Wicket Hauls",
  };

  const headers: string[] = [];
  if (thSelector) {
    thSelector.forEach((th) => {
     headers.push(th.textContent?.trim() || "");
    });
  }

  const rowData: { key: string; value: string }[] = [];

  rowData.push({
    key: "Player Href",
    value: tdsSelector[0]?.querySelector("a")?.getAttribute("href")?.trim() || "",
  });

  rowData.push({
    key: "Team Name",
    value: getFullTeamName(
      tdsSelector[0]?.textContent?.split(" (")[1]?.replace(")", "").trim()?.replace("ICC", "")?.replace("Asia", "")?.replace("Afr", "")?.replaceAll("/", "") || ""
    ),
  });

  for (let i = 1; i < tdsSelector.length; i++) {
    let key = headers[i] || `Column${i+1}`;
    if (headerKeyMapping[key]) {
      key = headerKeyMapping[key];
    }
    rowData.push({ key, value: tdsSelector[i]?.textContent?.trim() || "" });
  }

  espnTableRows.push({ data: rowData });
};


export const mostRunsInCareer = (
  espnTableRows: ESPNTableRow[],
  tdsSelector: NodeListOf<HTMLTableCellElement>,
  thSelector: NodeListOf<HTMLTableCellElement>,
  isCalendarYear?: boolean
) => {
  // Use thSelector for dynamic key mapping
  const headers: string[] = [];
  if (thSelector) {
    thSelector.forEach((th) => {
      headers.push(th.textContent?.trim() || "");
    });
  }

  const rowData: { key: string; value: string }[] = [];
  for (let i = 0; i < tdsSelector.length; i++) {
    const key = headers[i] || `Column${i+1}`;
    rowData.push({ key, value: tdsSelector[i]?.textContent?.trim() || "" });
  }

  // Add Player Href if available
  rowData.push({
    key: "Player Href",
    value: tdsSelector[0]?.querySelector("a")?.getAttribute("href")?.trim() || "",
  });

  // Add Team Name using getFullTeamName logic
  rowData.push({
    key: "Team Name",
    value: getFullTeamName(
      tdsSelector[0]?.textContent?.split(" (")[1]?.replace(")", "").trim()?.replace("ICC", "")?.replace("Asia", "")?.replace("Afr", "")?.replaceAll("/", "") || ""
    ),
  });

  // Add xyz (logo team name)
  rowData.push({
    key: "xyz",
    value:
      teamLogos.find(
        (x) =>
          x.shortName ===
          tdsSelector[0]?.textContent?.split("(")[1]?.replace(")", "")
      )?.teamName || "",
  });

  espnTableRows.push({ data: rowData });

  if (!isCalendarYear) {
    espnTableRows[0].data.push({
      key: "Span",
      value: tdsSelector[1]?.textContent || "",
    });
  }
};

export const mostRunsInTestMatch = (
  espnTableRows: ESPNTableRow[],
  tdsSelector: NodeListOf<HTMLTableCellElement>,
  isCalendarYear?: boolean
) => {
  espnTableRows.push({
    data: [
      { key: "Player Name", value: tdsSelector[0]?.textContent || "" },
      {
        key: "Player Href",
        value: tdsSelector[0]?.querySelector("a")?.getAttribute("href") || "",
      },
      { key: "Runs", value: tdsSelector[3]?.textContent || "" },
      {
        key: "Matches",
        value: tdsSelector[2]?.textContent || "",
      },
      { key: "1st Ing Score", value: tdsSelector[1]?.textContent || "" },
      { key: "2nd Ing Score", value: tdsSelector[2]?.textContent || "" },
      { key: "Against", value: tdsSelector[5]?.textContent?.replace("v ", "") || "" },
      { key: "Match Date", value: tdsSelector[7]?.textContent || "" },
      {
        key: "xyz",
        value:
          teamLogos.find(
            (x) =>
              x.shortName ===
              tdsSelector[0]?.textContent?.split("(")[1]?.replace(")", "")
          )?.teamName || "",
      },
    ],
  });

  if (!isCalendarYear) {
    espnTableRows[0].data.push({
      key: "Span",
      value: tdsSelector[1]?.textContent || "",
    });
  }
};

export const mostSixesInCareer = (
  espnTableRows: ESPNTableRow[],
  tdsSelector: NodeListOf<HTMLTableCellElement>
) => {
  espnTableRows.push({
    data: [
      { key: "Player Name", value: tdsSelector[0]?.textContent || "" },
      {
        key: "Player Href",
        value: tdsSelector[0]?.querySelector("a")?.getAttribute("href") || "",
      },
      {
        key: "Sixes",
        value: tdsSelector[14]?.textContent || "",
      },
      {
        key: "Matches",
        value: tdsSelector[2]?.textContent || "",
      },
      { key: "Span", value: tdsSelector[1]?.textContent || "" },
      { key: "H.Score", value: tdsSelector[6]?.textContent || "" },
      { key: "Runs", value: tdsSelector[5]?.textContent || "" },
      {
        key: "Fours",
        value: tdsSelector[13]?.textContent || "",
      },
      {
        key: "xyz",
        value:
          teamLogos.find(
            (x) =>
              x.shortName ===
              tdsSelector[0]?.textContent?.split("(")[1]?.replace(")", "")
          )?.teamName || "",
      },
    ],
  });
};

export const allDoubleCenturies = (
  espnTableRows: ESPNTableRow[],
  tdsSelector: NodeListOf<HTMLTableCellElement>
) => {
  espnTableRows.push({
    data: [
      { key: "Player Name", value: tdsSelector[0]?.textContent || "" },
      {
        key: "Player Href",
        value: tdsSelector[0]?.querySelector("a")?.getAttribute("href") || "",
      },
      { key: "Runs", value: tdsSelector[1]?.textContent || "" },
      {
        key: "Balls",
        value: tdsSelector[3]?.textContent || "",
      },
      {
        key: "Sixes",
        value: tdsSelector[5]?.textContent || "",
      },
      {
        key: "Against",
        value: tdsSelector[8]?.textContent?.replace("v ", "") || "",
      },
      { key: "Venue", value: tdsSelector[9]?.textContent || "" },
      {
        key: "Date",
        value: tdsSelector[10]?.textContent || "",
      },
      {
        key: "xyz",
        value: tdsSelector[7]?.textContent || "",
      },
    ],
  });
};

export const dataGenerator = {
  mostWicketsInCareer,
  mostRunsInCareer,
  mostRunsInTestMatch,
  mostSixesInCareer,
  allDoubleCenturies,
};
