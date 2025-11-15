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

  const { playingRole, battingStyle, bowlingStyle } = useMemo(() => {

    const match = playersRolesInfo.find(
      (p) => p.playerHref && playerHref && p.playerHref === playerHref
    );
    return {
      playingRole: match?.playingRole || "",
      battingStyle: match?.battingStyle || "",
      bowlingStyle: match?.bowlingStyle || "",
    };
  }, [playerHref]);

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
