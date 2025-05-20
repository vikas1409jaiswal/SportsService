import { ESPNTableRow } from "./useCustomESPNTable";
import teamLogos from "./../../../data/StaticData/teamLogos.json";

export const mostWicketsInCareer = (
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
      { key: "Wickets", value: tdsSelector[8]?.textContent || "" },
      {
        key: "Matches",
        value: tdsSelector[2]?.textContent || "",
      },
      { key: "Maiden", value: tdsSelector[6]?.textContent || "" },
      {
        key: "4 Wicket",
        value: tdsSelector[13]?.textContent || "",
      },
      { key: "BBI", value: tdsSelector[9]?.textContent || "" },
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

export const mostRunsInCareer = (
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
      { key: "Runs", value: tdsSelector[5]?.textContent || "" },
      {
        key: "Matches",
        value: tdsSelector[2]?.textContent || "",
      },
      { key: "H.Score", value: tdsSelector[6]?.textContent || "" },
      {
        key: "Hundreds",
        value: tdsSelector[10]?.textContent || "",
      },
      { key: "Fifties", value: tdsSelector[11]?.textContent || "" },
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
  mostSixesInCareer,
  allDoubleCenturies,
};
