import axios, { AxiosResponse } from "axios";
import { useQuery } from "react-query";
import { Player } from "../../models/espn-cricinfo-models/CricketMatchModels";
import { ApiData } from "../../models/Api";
import { useESPNPlayerInfo } from "../../hooks/espn-cricinfo-hooks/usePlayerInfo";
import { toCapitalCase } from "../../utils/ReusableFuctions";

export interface CricketSquad {
  teamName: string;
  isDomestic: boolean;
  country?: string;
  players: SquadPlayer[];
}

export type SquadPlayer = Player & {
  imageUrl: string;
  age: string;
  batting: string;
  bowling: string;
  role: string;
  isWithdrawn: boolean;
};

const fetchESPNSquadInfo = async (
  url: string
): Promise<AxiosResponse<ApiData>> => {
  return await axios.get(url);
};

export const useCustomSquadInfo = (url: string) => {
  const queryOptions = {
    refetchOnWindowFocus: false,
    refetchOnMount: false,
    enabled: true,
    cacheTime: 60 * 60 * 1000,
    retry: true,
  };

  const { data } = useQuery([url], () => fetchESPNSquadInfo(url), queryOptions);

  const squadInfo: CricketSquad = {
    teamName:
      (url
        ?.split("/")
        .find((x) => x.includes("-squad-"))
        ?.split("-squad-")[0]
        ?.replace("t20i", "")
        ?.replace("-test", "")
        ?.trim() as string) || "guyana amazon warriors",
    isDomestic: true,
    players: [],
  };

  const divElementCus = document.createElement("div");
  divElementCus.innerHTML = data?.data.toString() as string;

  const playerGridSelector = divElementCus.querySelectorAll("div.ds-p-0");

  const hrefs: string[] = [];

  playerGridSelector?.forEach((pg) => {
    const playerCardSelector = pg?.querySelectorAll(".ds-grid > div");

    playerCardSelector?.forEach((pcs, i) => {
      const href = pcs?.querySelectorAll("a")[1]?.getAttribute("href") || "";
      hrefs.push(href);
    });
  });

  useUpdatePlayer(squadInfo, hrefs[0], true);
  useUpdatePlayer(squadInfo, hrefs[1]);
  useUpdatePlayer(squadInfo, hrefs[2]);
  useUpdatePlayer(squadInfo, hrefs[3]);
  useUpdatePlayer(squadInfo, hrefs[4]);
  useUpdatePlayer(squadInfo, hrefs[5]);
  useUpdatePlayer(squadInfo, hrefs[6]);
  useUpdatePlayer(squadInfo, hrefs[7]);
  useUpdatePlayer(squadInfo, hrefs[8]);
  useUpdatePlayer(squadInfo, hrefs[9]);
  useUpdatePlayer(squadInfo, hrefs[10]);
  useUpdatePlayer(squadInfo, hrefs[11]);
  useUpdatePlayer(squadInfo, hrefs[12]);
  useUpdatePlayer(squadInfo, hrefs[13]);
  useUpdatePlayer(squadInfo, hrefs[14]);
  useUpdatePlayer(squadInfo, hrefs[15]);
  useUpdatePlayer(squadInfo, hrefs[16]);
  useUpdatePlayer(squadInfo, hrefs[17]);
  useUpdatePlayer(squadInfo, hrefs[18]);
  useUpdatePlayer(squadInfo, hrefs[19]);
  useUpdatePlayer(squadInfo, hrefs[20]);
  useUpdatePlayer(squadInfo, hrefs[21]);
  useUpdatePlayer(squadInfo, hrefs[22]);
  useUpdatePlayer(squadInfo, hrefs[23]);
  useUpdatePlayer(squadInfo, hrefs[24]);
  useUpdatePlayer(squadInfo, hrefs[25]);
  useUpdatePlayer(squadInfo, hrefs[26]);

  return squadInfo;
};

const useUpdatePlayer = (
  squadInfo: CricketSquad,
  hrefStr: string,
  isCaptain: boolean = false
) => {
  const { battingStyle, bowlingStyle, playingRole, name, age, href } =
    useESPNPlayerInfo(hrefStr);

  name &&
    squadInfo.players.push({
      name: `${toCapitalCase(name)}${isCaptain ? " (c)" : ""}`,
      href,
      imageUrl: "",
      role: playingRole,
      age,
      batting: battingStyle,
      bowling: bowlingStyle,
      isWithdrawn: false,
    });
};
