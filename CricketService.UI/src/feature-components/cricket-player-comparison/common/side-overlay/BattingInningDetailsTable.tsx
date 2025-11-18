import React from "react";
import { BattingInningsStat } from "../../hook/useFetchPlayerAllMatches";
import "./BattingInningDetailsTable.scss";

interface BattingInningDetailsTableProps {
  battingInningsStats: BattingInningsStat[];
}

export const BattingInningDetailsTable: React.FC<BattingInningDetailsTableProps> = ({ battingInningsStats }) => {
  const innings = battingInningsStats || [];
  if (!innings.length) {
    return <div className="batting-innings-table-empty">No batting innings data available.</div>;
  }

  return (
    <div className="batting-innings-table-wrapper">
      <table className="batting-innings-table">
        <thead>
          <tr>
            <th>#</th>
            <th>Match #</th>
            <th>Date</th>
            <th>Inning</th>
            <th>Pos</th>
            <th>Runs</th>
            <th>Balls</th>
            <th>4s</th>
            <th>6s</th>
            <th>Out</th>
            <th>Team</th>
            <th>Opposition</th>
          </tr>
        </thead>
        <tbody>
          {innings.map((inning, idx) => (
            <tr key={inning.uuid || idx}>
              <td>{idx + 1}</td>
              <td>{inning.matchNumber || "-"}</td>
              <td>{inning.matchDate ? new Date(inning.matchDate).toLocaleDateString() : "-"}</td>
              <td>{inning.inning ?? "-"}</td>
              <td>{inning.battingPosition ?? "-"}</td>
              <td>{inning.runScored ?? "-"}</td>
              <td>{inning.ballsFaced ?? "-"}</td>
              <td>{inning.foursInInning ?? "-"}</td>
              <td>{inning.sixesInInning ?? "-"}</td>
              <td>{inning.outStatus || "-"}</td>
              <td>{inning.teamName || "-"}</td>
              <td>{inning.oppositionTeamName || "-"}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};
