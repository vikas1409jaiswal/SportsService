import React from "react";
import {
  BowlingMatchStatistics,
  BowlingStatistics,
  PlayerInfo,
} from "../../../../CricketComponents/CricketPlayerInfoFetch/useCustomPlayerInfo";
import { featureToggle } from "../feature-toggle";

interface BowlingMatchStatsTableProps {
  player: PlayerInfo;
  hideColumns?: number[];
  gridColumns?: string;
  hideHeader?: boolean;
}

export const BowlingMatchStatsTable: React.FC<BowlingMatchStatsTableProps> = ({
  player,
  hideColumns,
  gridColumns,
  hideHeader,
}) => {
  console.log(player.matchDetail?.t20IMatches?.bowlingStats);
  const getMatchBowlingStatsByFormat = (
    format: string,
    bowlingStats: BowlingMatchStatistics
  ) => {
    return (
      <tr
        style={
          gridColumns
            ? { display: "grid", gridTemplateColumns: gridColumns }
            : {}
        }
      >
        {[
          format,
          bowlingStats?.overs || "-",
          bowlingStats?.maidens || "-",
          bowlingStats?.wickets || "-",
          bowlingStats?.runsConceded || "-",
          bowlingStats?.date || "-",
          bowlingStats?.forTeam || "-",
          bowlingStats?.opps || "-",
          bowlingStats?.economy || "-",
        ].map(
          (column, colIndex) =>
            !hideColumns?.includes(colIndex) && <td>{column}</td>
        )}
      </tr>
    );
  };
  return (
    <>
      <thead>
        {!hideHeader && (
          <tr>
            <th
              style={{
                backgroundColor: "greenyellow",
                color: "black",
                fontSize: 20,
                fontWeight: 900,
              }}
            >
              Bowling Statistics
            </th>
          </tr>
        )}
        <tr
          style={
            gridColumns
              ? { display: "grid", gridTemplateColumns: gridColumns }
              : {}
          }
        >
          <th>Format</th>
          {!hideColumns?.includes(1) && <th>Overs</th>}
          {!hideColumns?.includes(2) && <th>Maidens</th>}
          {!hideColumns?.includes(3) && <th>Wkts</th>}
          {!hideColumns?.includes(4) && <th>RC</th>}
          {!hideColumns?.includes(5) && <th>Date</th>}
          {!hideColumns?.includes(6) && <th>Team</th>}
          {!hideColumns?.includes(7) && <th>Against</th>}
          {!hideColumns?.includes(8) && <th>Eco</th>}
        </tr>
      </thead>
      <tbody>
        {featureToggle.showOdiData &&
          getMatchBowlingStatsByFormat(
            "ODI",
            player.matchDetail?.odiMatches
              ?.bowlingStats as BowlingMatchStatistics
          )}
        {featureToggle.showTestData &&
          getMatchBowlingStatsByFormat(
            "Test",
            player.matchDetail?.testMatches
              ?.bowlingStats as BowlingMatchStatistics
          )}
        {featureToggle.showT20IData &&
          getMatchBowlingStatsByFormat(
            "T20",
            player.matchDetail?.t20IMatches
              ?.bowlingStats as BowlingMatchStatistics
          )}
      </tbody>
    </>
  );
};
