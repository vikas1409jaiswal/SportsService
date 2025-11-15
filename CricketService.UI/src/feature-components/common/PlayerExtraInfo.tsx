import React, { useMemo } from "react";
import { config } from "../../configs";
import engToHiJson from "../../data/StaticData/englishToHindi.json";
import { ESPNPlayerInfo, useESPNPlayerInfo } from "../../hooks/espn-cricinfo-hooks/usePlayerInfo";
import playersRolesInfo from "./players-roles-info.json";

interface PlayerExtraInfoProps {
  playerHref: string;
}

export const PlayerExtraInfo: React.FC<PlayerExtraInfoProps> = ({
  playerHref
}) => {
  const cricketPositions = (engToHiJson as any)["cricket-positions"];

  const { playingRole: apiPlayingRole, battingStyle: apiBattingStyle, bowlingStyle: apiBowlingStyle } =
    useESPNPlayerInfo(playerHref) as ESPNPlayerInfo;

  // Fallback to JSON if API values are undefined
  const { playingRole, battingStyle, bowlingStyle } = useMemo(() => {
    if (apiPlayingRole && apiBattingStyle && apiBowlingStyle) {
      return {
        playingRole: apiPlayingRole,
        battingStyle: apiBattingStyle,
        bowlingStyle: apiBowlingStyle,
      };
    }
    const match = playersRolesInfo.find(
      (p) => p.playerHref && playerHref && p.playerHref === playerHref
    );
    return {
      playingRole: apiPlayingRole || match?.playingRole || "",
      battingStyle: apiBattingStyle || match?.battingStyle || "",
      bowlingStyle: apiBowlingStyle || match?.bowlingStyle || "",
    };
  }, [apiPlayingRole, apiBattingStyle, apiBowlingStyle, playerHref]);

  if (config.language === "hindi") {
    return (
      <>
        <p className="extra-info">{cricketPositions[playingRole]}</p>
        <p className="extra-info">{cricketPositions[battingStyle]}</p>
        <p className="extra-info">
          {bowlingStyle
            .split(", ")
            .map((x) => cricketPositions[x])
            .join(", ")}
        </p>
      </>
    );
  } else {
    return (
      <>
        <p className="extra-info">{playingRole}</p>
        <p className="extra-info">{battingStyle}</p>
        <p className="extra-info">{bowlingStyle}</p>
      </>
    );
  }
};
