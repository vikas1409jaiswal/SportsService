import React from "react";
import { BattingStatistics } from "../../../../CricketComponents/CricketPlayerInfoFetch/useCustomPlayerInfo";
import { PlayerInfo } from "../../../../CricketComponents/CricketPlayerInfoFetch/usePlayerInfo";
import { featureToggle } from "../feature-toggle";

interface BattingCareerStatsTableProps {
  player: PlayerInfo;
  hideColumns?: number[];
  gridColumns?: string;
  hideHeader?: boolean;
}

export const BattingCareerStatsTable: React.FC<
  BattingCareerStatsTableProps
> = ({ player, hideColumns, gridColumns, hideHeader }) => {
  const getCareerBattingStatsByFormat = (
    format: string,
    battingStats: BattingStatistics
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
          battingStats?.matches || "-",
          battingStats?.innings || "-",
          battingStats?.runs || "-",
          battingStats?.highestScore || "-",
          battingStats?.fours || "-",
          battingStats?.sixes || "-",
          battingStats?.strikeRate || "-",
        ].map(
          (column, colIndex) =>
            !hideColumns?.includes(colIndex + 1) && <td>{column}</td>
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
              Batting Statistics
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
          {["Format", "M", "Inn", "R", "HS", "4s", "6s", "SR"].map(
            (column, colIndex) =>
              !hideColumns?.includes(colIndex + 1) && <th>{column}</th>
          )}
        </tr>
      </thead>
      <tbody>
        {featureToggle.showOdiData &&
          getCareerBattingStatsByFormat(
            "ODI",
            player.career?.odiMatches?.battingStats as BattingStatistics
          )}
        {featureToggle.showTestData &&
          getCareerBattingStatsByFormat(
            "Test",
            player.career?.testMatches?.battingStats as BattingStatistics
          )}
        {featureToggle.showT20IData &&
          getCareerBattingStatsByFormat(
            "T20",
            player.career?.t20IMatches?.battingStats as BattingStatistics
          )}
      </tbody>
    </>
  );
};
