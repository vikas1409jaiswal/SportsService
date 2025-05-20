import React from "react";

import "./StatRow.scss";
import { AnimatedValueContent } from "../../../cricket-players/cricket-body/PlayerInfo/AnimatedValueContent";
import { config } from "../../../../configs";
import engtohindi from "./../../../../data/StaticData/englishToHindi.json";

interface StatRowProps {
  singleStat: {
    key: string;
    value: string;
  };
  isAnimation?: boolean;
}

export const StatRow: React.FC<StatRowProps> = ({
  singleStat,
  isAnimation,
}) => {
  const getHindiValue = (key: string, value: string) => {
    if (key === "Date") {
      return value
        .split(" ")
        .map((x, i) => (i === 1 ? (engtohindi as any)["months"][x] : x))
        .join(" ");
    } else if (key === "Against") {
      return (engtohindi as any)["team-names"][value];
    } else if (key === "Venue") {
      return (engtohindi as any)["cricket-venues"][value];
    }
    return value;
  };
  return (
    <div
      className="stat-row"
      style={singleStat.key === "xyz" ? { visibility: "hidden" } : {}}
    >
      <div className="single-stat-title text-3d">
        {config.language === "hindi"
          ? (engtohindi as any)["cricket-words"][singleStat?.key]
          : singleStat.key?.toUpperCase()}
      </div>
      <div className="single-stat-value text-3d">
        {isAnimation && (
          <AnimatedValueContent
            value={parseInt(singleStat?.value) || 0}
            duration={3000}
            player={null}
          />
        )}
        {!isAnimation && (
          <span>
            {config.language === "hindi"
              ? getHindiValue(singleStat?.key, singleStat?.value)
              : singleStat?.value}
          </span>
        )}
      </div>
    </div>
  );
};
