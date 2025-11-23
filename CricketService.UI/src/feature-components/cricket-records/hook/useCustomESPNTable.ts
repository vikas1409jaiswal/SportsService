import { useQuery } from "react-query";
import axios from "axios";
import { ApiData } from "../../../models/Api";
import { dataGenerator } from "./dataGenerator";

export interface ESPNTableRow {
  data: {
    key: string;
    value: string;
  }[];
}

const ONE_HOUR_IN_MS = 60 * 60 * 1000; // 1 hour in milliseconds

const fetchESPNTable = async (): Promise<ApiData> => {
  try {
    const response = await axios.get(
      `https://www.espncricinfo.com/records/most-runs-in-career-83548`,
      {
        headers: {
          "User-Agent":
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36",
          Accept:
            "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8",
        },
      }
    );
    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error) && error.response?.status === 403) {
      const fallbackHtml = await fetch("/espnFallback.html").then(r => r.text());
      return fallbackHtml as unknown as ApiData;
    }
    throw error;
  }
};


export const useCustomESPNTable = (profile: string = "MostWicketsInCareer", maxLimit: number = 10): ESPNTableRow[] => {
  const { data } = useQuery(["espn-table"], () => fetchESPNTable(), {
    staleTime: ONE_HOUR_IN_MS,
    cacheTime: ONE_HOUR_IN_MS * 2,
    refetchOnWindowFocus: false,
    refetchOnReconnect: false,
    refetchOnMount: false,
  });

  const divElement = document.createElement("div");
  divElement.innerHTML = data?.toString() as string;

  let espnTableSelector = divElement?.querySelector(
    ".ds-w-full.ds-table.ds-table-xs.ds-table-auto.ds-w-full.ds-overflow-scroll.ds-scrollbar-hide"
  );

  if (!espnTableSelector) {
    const xhr = new XMLHttpRequest();
    xhr.open("GET", "/espnFallback.html", false);
    xhr.send(null);
    if (xhr.status === 200) {
      divElement.innerHTML = xhr.responseText;
      espnTableSelector = divElement.querySelector(
        ".ds-w-full.ds-table.ds-table-xs.ds-table-auto.ds-w-full.ds-overflow-scroll.ds-scrollbar-hide"
      );
    }
  }

  const espnTable: ESPNTableRow[] = [];
  const tableRowsSelector = espnTableSelector?.querySelectorAll("tbody > tr");

  tableRowsSelector?.forEach((tr, i) => {
    const tdsSelector = tr?.querySelectorAll("td");
    if (i < maxLimit) {
      if (profile === "MostWicketsInCareer") {
        dataGenerator.mostWicketsInCareer(espnTable, tdsSelector, false);
      } else if (profile === "MostRunsInCareer") {
        dataGenerator.mostRunsInCareer(espnTable, tdsSelector, false);
      }
    }
  });

  return espnTable;
};
