import React from "react";
import { Batsman } from "../../../../models/espn-cricinfo-models/CricketMatchModels";

import "./BBShortInfo.scss";

interface BattingShortInfoProps {
  battingScoreCard: Batsman[];
  href: string;
}

export const BattingShortInfo: React.FC<BattingShortInfoProps> = ({
  battingScoreCard,
  href,
}) => {
  const player = battingScoreCard.find((bs) => bs.playerName.href === href);
  return (
    <div className="bsi-container">
      {player?.ballsFaced ? (
        <>
          <span style={{ fontSize: 25 }}>Runs</span>
          &nbsp;
          <span style={{ fontSize: 50 }}>
            {player?.ballsFaced ? player?.runsScored : "dn Bat"}
            {player?.outStatus?.includes("not out") ? "*" : ""}
          </span>
          &nbsp;&nbsp;
          <span style={{ fontSize: 20 }}>{player?.ballsFaced}</span>
          &nbsp;
          <span>balls</span>
        </>
      ) : (
        <span style={{ fontSize: 50 }}>dn Bat</span>
      )}
      &nbsp;&nbsp;
      {player?.ballsFaced && (
        <>
          <span className="fours-info">{player?.fours}</span>
          &nbsp;&nbsp;
          <span className="sixes-info">{player?.sixes}</span>
        </>
      )}
    </div>
  );
};
