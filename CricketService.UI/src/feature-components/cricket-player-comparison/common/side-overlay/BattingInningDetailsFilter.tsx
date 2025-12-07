import React from "react";
import { BattingInningsStat } from "../../hook/useFetchPlayerAllMatches";
import "./BattingInningDetailsFilter.scss";

interface BattingInningDetailsFilterProps {
  battingInningsStats: BattingInningsStat[];
  oppositionTeam: string;
  milestoneType: string;
  inning: string;
  position: string;
  filteredCount: number;
  totalCount: number;
  onOppositionTeamChange: (value: string) => void;
  onMilestoneTypeChange: (value: string) => void;
  onInningChange: (value: string) => void;
  onPositionChange: (value: string) => void;
}

export const BattingInningDetailsFilter: React.FC<BattingInningDetailsFilterProps> = ({
  battingInningsStats,
  oppositionTeam,
  milestoneType,
  inning,
  position,
  filteredCount,
  totalCount,
  onOppositionTeamChange,
  onMilestoneTypeChange,
  onInningChange,
  onPositionChange,
}) => {
  const milestoneOptions = [
    { value: "", label: "All" },
    { value: "Fifty", label: "Fifty" },
    { value: "Hundred", label: "Hundred" },
    { value: "OneFiftyPlus", label: "OneFiftyPlus" },
    { value: "DoubleHundred", label: "DoubleHundred" },
    { value: "Ducks", label: "Ducks" },
    { value: "RunsMilestone", label: "RunsMilestone" },
  ];

  // Get unique opposition teams from data
  const oppositionTeams = React.useMemo(() => {
    return Array.from(
      new Set(battingInningsStats.map(inning => inning.oppositionTeamName).filter(Boolean))
    ).sort();
  }, [battingInningsStats]);

  const oppositionOptions = React.useMemo(() => [
    { value: "", label: "All Teams" },
    ...oppositionTeams.map(team => ({ value: team, label: team }))
  ], [oppositionTeams]);

  // Get unique innings from data
  const innings = React.useMemo(() => {
    return Array.from(
      new Set(battingInningsStats.map(inning => inning.inning).filter(inning => inning !== null && inning !== undefined))
    ).sort((a, b) => Number(a) - Number(b));
  }, [battingInningsStats]);

  const inningOptions = React.useMemo(() => [
    { value: "", label: "All Innings" },
    ...innings.map(inning => ({ value: String(inning), label: `Inning ${inning}` }))
  ], [innings]);

  // Get unique batting positions from data
  const positions = React.useMemo(() => {
    return Array.from(
      new Set(battingInningsStats.map(inning => inning.battingPosition).filter(pos => pos !== null && pos !== undefined))
    ).sort((a, b) => Number(a) - Number(b));
  }, [battingInningsStats]);

  const positionOptions = React.useMemo(() => [
    { value: "", label: "All Positions" },
    ...positions.map(pos => ({ value: String(pos), label: `Position ${pos}` }))
  ], [positions]);

  return (
    <div>
      {/* Results info banner */}
      <div className="batting-inning-details-filter__info-banner">
        Showing <strong>{filteredCount}</strong> of <strong>{totalCount}</strong> innings
        {filteredCount !== totalCount && " (filtered)"}
      </div>
      
      <div className="batting-inning-details-filter">
      <div className="batting-inning-details-filter__group">
        <label htmlFor="opposition-filter" className="batting-inning-details-filter__label">
          Opposition:
        </label>
        <select
          id="opposition-filter"
          value={oppositionTeam}
          onChange={e => onOppositionTeamChange(e.target.value)}
          className="batting-inning-details-filter__select"
        >
          {oppositionOptions.map(opt => (
            <option key={opt.value} value={opt.value}>{opt.label}</option>
          ))}
        </select>
      </div>
      <div className="batting-inning-details-filter__group">
        <label htmlFor="inning-filter" className="batting-inning-details-filter__label">
          Inning:
        </label>
        <select
          id="inning-filter"
          value={inning}
          onChange={e => onInningChange(e.target.value)}
          className="batting-inning-details-filter__select"
        >
          {inningOptions.map(opt => (
            <option key={opt.value} value={opt.value}>{opt.label}</option>
          ))}
        </select>
      </div>
      <div className="batting-inning-details-filter__group">
        <label htmlFor="position-filter" className="batting-inning-details-filter__label">
          Position:
        </label>
        <select
          id="position-filter"
          value={position}
          onChange={e => onPositionChange(e.target.value)}
          className="batting-inning-details-filter__select"
        >
          {positionOptions.map(opt => (
            <option key={opt.value} value={opt.value}>{opt.label}</option>
          ))}
        </select>
      </div>
      <div className="batting-inning-details-filter__group">
        <label htmlFor="milestone-filter" className="batting-inning-details-filter__label">
          Milestone:
        </label>
        <select
          id="milestone-filter"
          value={milestoneType}
          onChange={e => onMilestoneTypeChange(e.target.value)}
          className="batting-inning-details-filter__select"
        >
          {milestoneOptions.map(opt => (
            <option key={opt.value} value={opt.value}>{opt.label}</option>
          ))}
        </select>
      </div>
    </div>
    </div>
  );
};