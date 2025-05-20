import { useQuery } from "react-query";
import axios, { AxiosResponse } from "axios";
import { ApiData } from "../../../models/Api";
import { dataGenerator } from "./dataGenerator";

export interface ESPNTableRow {
  data: {
    key: string;
    value: string;
  }[];
}

const fetchESPNTable = (): Promise<AxiosResponse<ApiData>> => {
  return axios.get(
    `https://www.espncricinfo.com/records/year/bowling-most-wickets-career/2024-2024/test-matches-1`
  );
};

export const useCustomESPNTable = (): ESPNTableRow[] => {
  const { data } = useQuery(["espn-table"], () => fetchESPNTable(), {
    cacheTime: 0,
  });

  const divElement = document.createElement("div");

  divElement.innerHTML = data?.data.toString() as string;

  const espnTableSelector = divElement?.querySelector(
    ".ds-w-full.ds-table.ds-table-xs.ds-table-auto.ds-w-full.ds-overflow-scroll.ds-scrollbar-hide"
  );

  const espnTable: ESPNTableRow[] = [];

  const tableRowsSelector = espnTableSelector?.querySelectorAll("tbody > tr");

  tableRowsSelector?.forEach((tr, i) => {
    const tdsSelector = tr?.querySelectorAll("td");
    dataGenerator.mostWicketsInCareer(espnTable, tdsSelector, true);
    //dataGenerator.mostRunsInCareer(espnTable, tdsSelector, true);
  });

  return espnTable;
};
