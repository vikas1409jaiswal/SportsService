import React from "react";

import "./StatRow.scss";
import { AnimatedValueContent } from "../../../../components/cricket-players/cricket-body/PlayerInfo/AnimatedValueContent";
import { config } from "../../../../configs";
import engToHindiJson from "../../../../data/StaticData/englishToHindi.json";

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
        .map((x, i) => (i === 1 ? (engToHindiJson as any)["months"][x] : x))
        .join(" ");
    } else if (key === "Against") {
      return (engToHindiJson as any)["team-names"][value];
    } else if (key === "Venue") {
      return (engToHindiJson as any)["cricket-venues"][value];
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
          ? (engToHindiJson as any)["cricket-words"][singleStat?.key]
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
