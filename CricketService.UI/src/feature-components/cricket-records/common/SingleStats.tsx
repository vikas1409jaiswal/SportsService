import React from "react";
import { AnimatedNumber } from "../../../components/common/AnimatedNumber";
import { config } from "../../../configs";
import engToHindiJson from "../../../data/StaticData/englishToHindi.json";

import "./SingleStats.scss";

interface SingleStatsProps {
  singleStat: {
    key: string;
    value: string;
  };
}

export const SingleStats: React.FC<SingleStatsProps> = ({ singleStat }) => {
  return (
    <div className="single-stats">
      <div className="single-stat-title text-3d">
         {config.language === "hindi"
          ? (engToHindiJson as any)["cricket-words"][singleStat?.key]
          : singleStat.key?.toUpperCase()}
      </div>
      <div className="single-stat-value text-3d">
        <AnimatedNumber value={parseInt(singleStat.value)} duration={3000} />
      </div>
    </div>
  );
};
