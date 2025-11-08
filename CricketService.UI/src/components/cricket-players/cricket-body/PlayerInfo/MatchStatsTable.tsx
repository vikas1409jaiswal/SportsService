import { AnimationControls, motion } from "framer-motion";
import React from "react";
import {
  CareerDetailStats,
  MatchDetailStats,
  PlayerInfo,
} from "../../../CricketComponents/CricketPlayerInfoFetch/useCustomPlayerInfo";
import { featureToggle } from "./feature-toggle";

import "./MatchStatsTable.scss";
import { BowlingMatchStatsTable } from "./match-stats-tables/BowlingMatchStatsTable";

interface MatchStatsTableProps {
  player: PlayerInfo;
  hideColumns?: number[];
  gridColumns?: string;
  animate?: AnimationControls;
}

export const MatchStatsTable: React.FC<MatchStatsTableProps> = ({
  player,
  animate,
  hideColumns,
  gridColumns,
}) => {
  const getMatchBattingStatsByFormat = (
    format: string,
    matchDetail: MatchDetailStats
  ) => {
    return (
      <tr
        style={
          gridColumns
            ? { display: "grid", gridTemplateColumns: gridColumns }
            : {}
        }
      >
        <td>{format}</td>
        {!hideColumns?.includes(1) && (
          <td>{matchDetail.battingStats?.runs || "-"}</td>
        )}
        <td>{matchDetail.battingStats?.balls || "-"}</td>
        {!hideColumns?.includes(3) && (
          <td>{matchDetail.battingStats?.opps || "-"}</td>
        )}
        <td>{matchDetail.battingStats?.date || "-"}</td>
        <td>{matchDetail.battingStats?.fours || "-"}</td>
        {!hideColumns?.includes(6) && (
          <td>{matchDetail.battingStats?.sixes || "-"}</td>
        )}
        {!hideColumns?.includes(7) && (
          <td>{matchDetail.battingStats?.strikeRate || "-"}</td>
        )}
        <td>{matchDetail.battingStats?.venue || "-"}</td>
      </tr>
    );
  };

  return (
    <motion.table className="match-stats-table" animate={animate}>
      {featureToggle.showBattingStats && (
        <>
          <thead>
            <tr
              style={
                gridColumns
                  ? { display: "grid", gridTemplateColumns: gridColumns }
                  : {}
              }
            >
              <th>Format</th>
              {!hideColumns?.includes(1) && <th>Runs</th>}
              <th>Balls</th>
              {!hideColumns?.includes(3) && <th>Opp</th>}
              <th>Date</th>
              <th>4s</th>
              {!hideColumns?.includes(6) && <th>6s</th>}
              {!hideColumns?.includes(7) && <th>SR</th>}
              <th>Venue</th>
            </tr>
          </thead>
          <tbody>
            {featureToggle.showOdiData &&
              getMatchBattingStatsByFormat(
                "ODI",
                player.matchDetail?.odiMatches as MatchDetailStats
              )}
            {featureToggle.showTestData &&
              getMatchBattingStatsByFormat(
                "Test",
                player.matchDetail?.testMatches as MatchDetailStats
              )}
            {featureToggle.showT20IData &&
              getMatchBattingStatsByFormat(
                "T20",
                player.matchDetail?.t20IMatches as MatchDetailStats
              )}
          </tbody>
        </>
      )}
      {featureToggle.showBowlingStats && (
        <BowlingMatchStatsTable
          player={player}
          hideColumns={[3, 4]}
          gridColumns={"1fr 1fr 1fr 2fr 2fr 2fr 1fr"}
          hideHeader
        />
      )}
    </motion.table>
  );
};
