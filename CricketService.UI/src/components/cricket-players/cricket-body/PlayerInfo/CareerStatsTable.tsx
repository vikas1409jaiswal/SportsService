import { AnimationControls, motion } from "framer-motion";
import React from "react";
import { PlayerInfo } from "../../../CricketComponents/CricketPlayerInfoFetch/useCustomPlayerInfo";
import { BattingCareerStatsTable } from "./career-stats-tables/BattingCareerStatsTable";
import { BowlingCareerStatsTable } from "./career-stats-tables/BowlingCareerStatsTable";
import { featureToggle } from "./feature-toggle";

import "./CareerStatsTable.scss";
import { FieldingCareerStatsTable } from "./career-stats-tables/FieldingCareerStatsTable";

interface CareerStatsTableProps {
  player: PlayerInfo;
  animate?: AnimationControls;
}

export const CareerStatsTable: React.FC<CareerStatsTableProps> = ({
  player,
  animate,
}) => {
  return (
    <motion.table className="career-stats-table" animate={animate}>
      {featureToggle.showBattingStats && (
        <BattingCareerStatsTable
          player={player}
          hideColumns={[7]}
          gridColumns={"repeat(7, 1fr)"}
          hideHeader
        />
      )}
      {featureToggle.showBowlingStats && (
        <BowlingCareerStatsTable
          player={player}
          hideColumns={[3, 5]}
          gridColumns={"repeat(6, 1fr)"}
          hideHeader
        />
      )}
      {featureToggle.showFieldingStats && (
        <FieldingCareerStatsTable
          player={player}
          hideColumns={[4]}
          gridColumns={"repeat(5, 1fr)"}
          hideHeader
        />
      )}
    </motion.table>
  );
};
