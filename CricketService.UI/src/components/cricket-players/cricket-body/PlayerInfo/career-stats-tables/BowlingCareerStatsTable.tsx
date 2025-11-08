import React from "react";
import {
  BowlingStatistics,
  PlayerInfo,
} from "../../../../CricketComponents/CricketPlayerInfoFetch/useCustomPlayerInfo";
import { featureToggle } from "../feature-toggle";

interface BowlingCareerStatsTableProps {
  player: PlayerInfo;
  hideColumns?: number[];
  gridColumns?: string;
  hideHeader?: boolean;
}

export const BowlingCareerStatsTable: React.FC<
  BowlingCareerStatsTableProps
> = ({ player, hideColumns, gridColumns, hideHeader }) => {
  const getCareerBowlingStatsByFormat = (
    format: string,
    bowlingStats: BowlingStatistics
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
          bowlingStats?.matches || "-",
          bowlingStats?.innings || "-",
          bowlingStats?.wickets || "-",
          bowlingStats?.runsConceded || "-",
          bowlingStats?.bbi || "-",
          bowlingStats?.bbm || "-",
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
          <th>M</th>
          <th>Inn</th>
          {!hideColumns?.includes(3) && <th>Wkts</th>}
          <th>RC</th>
          {!hideColumns?.includes(5) && <th>BBI</th>}
          {!hideColumns?.includes(6) && <th>BBM</th>}
          <th>Eco</th>
        </tr>
      </thead>
      <tbody>
        {featureToggle.showOdiData &&
          getCareerBowlingStatsByFormat(
            "ODI",
            player.career?.odiMatches?.bowlingStats as BowlingStatistics
          )}
        {featureToggle.showTestData &&
          getCareerBowlingStatsByFormat(
            "Test",
            player.career?.testMatches?.bowlingStats as BowlingStatistics
          )}
        {featureToggle.showT20IData &&
          getCareerBowlingStatsByFormat(
            "T20I",
            player.career?.t20IMatches?.bowlingStats as BowlingStatistics
          )}
      </tbody>
    </>
  );
};
