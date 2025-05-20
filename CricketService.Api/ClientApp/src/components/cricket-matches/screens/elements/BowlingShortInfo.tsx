import React from "react";
import { Bowler } from "../../../../models/espn-cricinfo-models/CricketMatchModels";

import "./BBShortInfo.scss";

interface BowlingShortInfoProps {
  bowlingScoreCard: Bowler[];
  href: string;
}

export const BowlingShortInfo: React.FC<BowlingShortInfoProps> = ({
  bowlingScoreCard,
  href,
}) => {
  const player = bowlingScoreCard.find((bs) => bs.playerName.href === href);
  return (
    <div className="bsi-container">
      {player?.oversBowled ? (
        <>
          <span style={{ fontSize: 25 }}>Wickets</span>
          &nbsp;
          <span style={{ fontSize: 50 }}>{player?.wickets}</span>
          &nbsp; &nbsp;
          <span style={{ fontSize: 20 }}>{player?.runsConceded}</span>
          &nbsp;
          <span>Conceded</span>
        </>
      ) : (
        <span style={{ fontSize: 50 }}>dn Bowl</span>
      )}
    </div>
  );
};
