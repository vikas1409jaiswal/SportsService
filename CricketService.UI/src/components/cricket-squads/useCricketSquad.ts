import axios, { AxiosResponse } from "axios";
import { useQuery } from "react-query";
import { Player } from "../../models/espn-cricinfo-models/CricketMatchModels";
import { ApiData } from "../../models/Api";
import {
  ESPNPlayerInfo,
  useESPNPlayerInfo,
} from "../../hooks/espn-cricinfo-hooks/usePlayerInfo";
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
        ?.replace("-t20i", "")
        ?.replace("-1st-test", "")
        ?.replace("-odi", "")
        ?.trim() as string) || "guyana amazon warriors",
    isDomestic: true,
    players: [],
  };

  const divElementCus = document.createElement("div");
  divElementCus.innerHTML = data?.data.toString() as string;

  const playerGridSelector = divElementCus.querySelectorAll("div.ds-p-0");

  const hrefs: string[] = [];

  // playerGridSelector?.forEach((pg) => {
  //   const playerCardSelector = pg?.querySelectorAll(".ds-grid > div");

  //   playerCardSelector?.forEach((pcs, i) => {
  //     const href = pcs?.querySelectorAll("a")[1]?.getAttribute("href") || "";
  //     hrefs.push(href);
  //   });
  // });

  playerGridSelector?.forEach((pg) => {
    const playerCardSelector = pg?.querySelectorAll(".ds-grid > div");

    playerCardSelector?.forEach((pcs, i) => {
      const href = pcs?.querySelectorAll("a")[1]?.getAttribute("href") || "";
      hrefs.push(href);
      const detailSelector = pcs?.querySelectorAll(".ds-flex.ds-space-x-1");
      const isAgeMissing = detailSelector[0]?.textContent?.includes("Batting");
      ![-1]?.includes(i) &&
        squadInfo.players.push({
          name: `${pcs
            ?.querySelectorAll("a")[1]
            ?.textContent?.replace("\n", "")}${i === 6 ? " (c)" : ""}${
            i === 155 ? " (vc)" : ""
          }`,
          href,
          imageUrl: "",
          role: pcs?.querySelector("p")?.textContent || "",
          age: isAgeMissing
            ? ""
            : detailSelector[0]?.textContent?.replace("Age:", "") || "",
          batting: isAgeMissing
            ? detailSelector[0]?.textContent?.replace("Batting:", "") || ""
            : detailSelector[1]?.textContent?.replace("Batting:", "") || "",
          bowling: isAgeMissing
            ? detailSelector[1]?.textContent?.replace("Bowling:", "") || ""
            : detailSelector[2]?.textContent?.replace("Bowling:", "") || "",
          isWithdrawn:
            pcs.querySelector("span.ds-text-tight-xs")?.textContent ===
            "Withdrawn",
        });
    });
  });

    // useUpdatePlayer(squadInfo, '/cricketers/iftikhar-ahmed-480603');
    // useUpdatePlayer(squadInfo, '/cricketers/ben-mcdermott-603410');

  return squadInfo;
};

const useUpdatePlayer = (
  squadInfo: CricketSquad,
  hrefStr: string,
  isCaptain: boolean = false
) => {
  const { battingStyle, bowlingStyle, playingRole, name, age, href } =
    useESPNPlayerInfo(hrefStr) as ESPNPlayerInfo;

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
