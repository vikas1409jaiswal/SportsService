import React from "react";
import { config } from "../../../../configs";
import engToHiJson from "../../../../data/StaticData/englishToHindi.json";

interface PlayerExtraInfoProps {
  playingRole: string;
  battingStyle: string;
  bowlingStyle: string;
}

export const PlayerExtraInfo: React.FC<PlayerExtraInfoProps> = ({
  playingRole,
  battingStyle,
  bowlingStyle,
}) => {
  const cricketPositions = (engToHiJson as any)["cricket-positions"];

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
