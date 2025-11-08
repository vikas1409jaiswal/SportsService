import React from "react";
import {
  BattingStatistics,
  FieldingStatistics,
  PlayerInfo,
} from "../../../../CricketComponents/CricketPlayerInfoFetch/useCustomPlayerInfo";
import { featureToggle } from "../feature-toggle";

interface FieldingCareerStatsTableProps {
  player: PlayerInfo;
  hideColumns?: number[];
  gridColumns?: string;
  hideHeader?: boolean;
}

export const FieldingCareerStatsTable: React.FC<
  FieldingCareerStatsTableProps
> = ({ player, hideColumns, gridColumns, hideHeader }) => {
  const getCareerFieldingStatsByFormat = (
    format: string,
    fieldingStats: FieldingStatistics
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
          fieldingStats?.matches || "-",
          fieldingStats?.innings || "-",
          fieldingStats?.catches || "-",
          fieldingStats?.maxCatches || "-",
          fieldingStats?.catchesPerInning || "-",
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
          {["Format", "M", "Inn", "Cat", "maxCat", "Cat/Inn"].map(
            (column, colIndex) =>
              !hideColumns?.includes(colIndex + 1) && <th>{column}</th>
          )}
        </tr>
      </thead>
      <tbody>
        {featureToggle.showOdiData &&
          getCareerFieldingStatsByFormat(
            "ODI",
            player.career?.odiMatches?.fieldingStats as FieldingStatistics
          )}
        {featureToggle.showTestData &&
          getCareerFieldingStatsByFormat(
            "Test",
            player.career?.testMatches?.fieldingStats as FieldingStatistics
          )}
        {featureToggle.showT20IData &&
          getCareerFieldingStatsByFormat(
            "T20I",
            player.career?.t20IMatches?.fieldingStats as FieldingStatistics
          )}
      </tbody>
    </>
  );
};
