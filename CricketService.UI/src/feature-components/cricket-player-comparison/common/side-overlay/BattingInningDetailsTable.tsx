import React, { useState } from "react";
import { BattingInningsStat } from "../../hook/useFetchPlayerAllMatches";
import { BattingInningDetailsFilter } from "./BattingInningDetailsFilter";
import "./BattingInningDetailsTable.scss";

interface BattingInningDetailsTableProps {
  battingInningsStats: BattingInningsStat[];
}

export const BattingInningDetailsTable: React.FC<BattingInningDetailsTableProps> = ({ battingInningsStats }) => {
  const innings = battingInningsStats || [];
  const [milestoneType, setMilestoneType] = useState<string>("");
  const [oppositionTeam, setOppositionTeam] = useState<string>("");
  const [inning, setInning] = useState<string>("");
  const [position, setPosition] = useState<string>("");

  // Helper to match milestone type
  const milestoneTypeMap: Record<string, string> = {
    Fifty: "ODI Fifty",
    Hundred: "ODI Hundred",
    OneFiftyPlus: "ODI 150+ Score",
    DoubleHundred: "ODI Double Hundred",
    Ducks: "ODI Duck",
    RunsMilestone: "Career Runs Completed",
  };

  const filteredInnings = innings.filter((inningData: BattingInningsStat) => {
    // Filter by opposition team
    const teamMatch = !oppositionTeam || inningData.oppositionTeamName === oppositionTeam;
    
    // Filter by milestone type
    const milestoneMatch = !milestoneType || 
      (inningData.milestones && inningData.milestones.some((milestone: string) => 
        milestone.includes(milestoneTypeMap[milestoneType])
      ));
    
    // Filter by inning
    const inningMatch = !inning || String(inningData.inning) === inning;
    
    // Filter by batting position
    const positionMatch = !position || String(inningData.battingPosition) === position;
    
    return teamMatch && milestoneMatch && inningMatch && positionMatch;
  });

  if (!innings.length) {
    return <div className="batting-innings-table-empty">No batting innings data available.</div>;
  }

  return (
    <div className="batting-innings-table-wrapper">
      <BattingInningDetailsFilter
        battingInningsStats={innings}
        oppositionTeam={oppositionTeam}
        milestoneType={milestoneType}
        inning={inning}
        position={position}
        filteredCount={filteredInnings.length}
        totalCount={innings.length}
        onOppositionTeamChange={setOppositionTeam}
        onMilestoneTypeChange={setMilestoneType}
        onInningChange={setInning}
        onPositionChange={setPosition}
      />
      <table className="batting-innings-table">
        <thead>
          <tr>
            <th>#</th>
            <th><b>Match #</b></th>
            <th>Date</th>
            <th>Inning</th>
            <th>Pos</th>
            <th><b>Runs</b></th>
            <th>Balls</th>
            <th>4s</th>
            <th>6s</th>
            <th>Out</th>
            <th>Team</th>
            <th>Opposition</th>
            <th>Milestones</th>
          </tr>
        </thead>
        <tbody>
          {filteredInnings.map((inning: BattingInningsStat, idx: number) => (
            <tr key={inning.uuid || idx}>
              <td>{idx + 1}</td>
              <td><b>{inning.matchNumber || "-"}</b></td>
              <td>{inning.matchDate ? new Date(inning.matchDate).toLocaleDateString() : "-"}</td>
              <td>{inning.inning ?? "-"}</td>
              <td>{inning.battingPosition ?? "-"}</td>
              <td><b>{inning.runScored ?? "-"}</b></td>
              <td>{inning.ballsFaced ?? "-"}</td>
              <td>{inning.foursInInning ?? "-"}</td>
              <td>{inning.sixesInInning ?? "-"}</td>
              <td>{inning.outStatus || "-"}</td>
              <td>{inning.teamName || "-"}</td>
              <td>{inning.oppositionTeamName || "-"}</td>
              <td>
                {inning.milestones && inning.milestones.length > 0 ? (
                  <div className="milestones-container">
                    {inning.milestones.map((milestone: string, milestoneIdx: number) => {
                      let badgeClass = "milestone-badge";
                      if (milestone.includes("ODI Fifty")) badgeClass += " milestone-fifty";
                      else if (milestone.includes("ODI Hundred")) badgeClass += " milestone-hundred";
                      else if (milestone.includes("ODI 150+ Score")) badgeClass += " milestone-onefiftyplus";
                      else if (milestone.includes("ODI Double Hundred")) badgeClass += " milestone-doublehundred";
                      else if (milestone.includes("ODI Duck")) badgeClass += " milestone-duck";
                      else if (milestone.includes("Career Runs Completed")) badgeClass += " milestone-career";
                      return (
                        <span key={milestoneIdx} className={badgeClass}>
                          {milestone}
                        </span>
                      );
                    })}
                  </div>
                ) : (
                  "-"
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};
