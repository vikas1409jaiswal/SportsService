import React from "react";
import { PlayerInfo } from "../../../CricketComponents/CricketPlayerInfoFetch/useCustomPlayerInfo";

import { AnimatedValueContent } from "./AnimatedValueContent";
import { MatchStatsTable } from "./MatchStatsTable";
import { CareerStatsTable } from "./CareerStatsTable";

interface SingleStatsProps {
  player: PlayerInfo;
  singleStat: {
    key: string;
    value: string;
  };
  ducks?: number;
}

export const SingleStats: React.FC<SingleStatsProps> = ({
  player,
  singleStat,
}) => {
  const valArr = singleStat.value?.split("/");
  return (
    <div className="single-stats">
      <div className="single-stat-title text-3d">
        {singleStat.key?.toUpperCase()}
      </div>
      <div className="single-stat-value text-3d">
        <AnimatedValueContent
          value={parseInt(valArr[0])}
          duration={3000}
          player={player}
        />
        /
        <AnimatedValueContent
          value={parseInt(valArr[1])}
          duration={3000}
          player={player}
        />
        {player?.matchDetail?.odiMatches?.battingStats?.notOut && (
          <span>*</span>
        )}
      </div>
      {/* <div className="single-stat-value text-3d">
        <div style={{ fontSize: 60 }}>{singleStat.value.split("::")[0]}</div>
        <div style={{ fontSize: 30 }}>{singleStat.value.split("::")[1]}</div>
      </div> */}
      {/* <CareerStatsTable player={player} /> */}
      <MatchStatsTable
        player={player}
        hideColumns={[1]}
        gridColumns={"2fr 2fr 2fr 2fr 1fr 1fr 2fr 2fr"}
      />
    </div>
  );
};
