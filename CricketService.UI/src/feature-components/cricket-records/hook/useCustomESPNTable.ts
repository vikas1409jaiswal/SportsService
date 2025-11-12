import { useQuery } from "react-query";
import axios from "axios";
import { ApiData } from "../../../models/Api";
import { dataGenerator, mostRunsInTestMatch } from "./dataGenerator";

// Import your fallback HTML content
import { espnFallbackHtml } from "./espnFallback";

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
      return espnFallbackHtml as unknown as ApiData;
    }
    throw error;
  }
};

export const useCustomESPNTable = (maxLimit: number = 10): ESPNTableRow[] => {
  const { data } = useQuery(["espn-table"], () => fetchESPNTable(), {
    staleTime: ONE_HOUR_IN_MS, // Data will be fresh for 1 hour
    cacheTime: ONE_HOUR_IN_MS * 2, // Cache will persist for 2 hours
    refetchOnWindowFocus: false, // Don't refetch when window regains focus
    refetchOnReconnect: false, // Don't refetch when regaining network connection
    refetchOnMount: false, // Only refetch when stale (after 1 hour)
  });

  const divElement = document.createElement("div");
  divElement.innerHTML = data?.toString() as string;

  let espnTableSelector = divElement?.querySelector(
    ".ds-w-full.ds-table.ds-table-xs.ds-table-auto.ds-w-full.ds-overflow-scroll.ds-scrollbar-hide"
  );

  // If the expected table isn't found in the fetched HTML, use the bundled
  // fallback HTML and re-run the query against the parser div.
  if (!espnTableSelector) {
    divElement.innerHTML = (espnFallbackHtml as unknown) as string;
    espnTableSelector = divElement.querySelector(
      ".ds-w-full.ds-table.ds-table-xs.ds-table-auto.ds-w-full.ds-overflow-scroll.ds-scrollbar-hide"
    );
  }

  const espnTable: ESPNTableRow[] = [];
  const tableRowsSelector = espnTableSelector?.querySelectorAll("tbody > tr");

  tableRowsSelector?.forEach((tr, i) => {
    const tdsSelector = tr?.querySelectorAll("td");
    i < maxLimit &&
      //dataGenerator.mostWicketsInCareer(espnTable, tdsSelector, true);
      //dataGenerator.mostRunsInTestMatch(espnTable, tdsSelector, false);
      dataGenerator.mostRunsInCareer(espnTable, tdsSelector, false);

  });

  return espnTable;
};
