import React, { useEffect } from "react";
import { PointsTableRow } from "../../../models/espn-cricinfo-models/CricketMatchModels";
import logos from "./../../../data/StaticData/teamLogos.json";

import "./PointsTable.scss";
import { speakText } from "../../common/SpeakText";

interface PointsTableProps {
  pointsTableRows: PointsTableRow[];
}

export const PointsTable: React.FC<PointsTableProps> = ({
  pointsTableRows,
}) => {
  useEffect(() => {
    speakText(
      `Points table, ${pointsTableRows[0].teamName?.replace(
        "St",
        "Saint"
      )} is at top with ${pointsTableRows[0].points} Points. `
    );
  }, []);
  return (
    <div className="points-table-container">
      <table>
        <thead>
          <tr>
            <th>Rank</th>
            <th colSpan={2}>Team</th>
            <th>Matches</th>
            <th>Won</th>
            <th>Lost</th>
            <th>Tied</th>
            <th>No Result</th>
            <th>Points</th>
            <th>NRR</th>
          </tr>
        </thead>
        <tbody>
          {pointsTableRows?.slice(0, 6).map((tr) => (
            <tr className="points-table-row">
              <td>{tr.rank}</td>
              <td>
                <img
                  src={logos.find((x) => x.teamName === tr.teamName)?.logoUrl}
                  alt={tr.teamName}
                  height={80}
                  width={80}
                />
              </td>
              <td>{tr.teamName}</td>
              <td>{tr.matches}</td>
              <td>{tr.won}</td>
              <td>{tr.lost}</td>
              <td>{tr.tied}</td>
              <td>{tr.noResult}</td>
              <td>{tr.points}</td>
              <td>{tr.netRR}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};
