import React from "react";
import "./SingleStats.scss";
import { AnimatedNumber } from "./../../../components/common/AnimatedNumber";

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
        {singleStat.key?.toUpperCase()}
      </div>
      <div className="single-stat-value text-3d">
        <AnimatedNumber value={parseInt(singleStat.value)} duration={3000} />
      </div>
    </div>
  );
};
