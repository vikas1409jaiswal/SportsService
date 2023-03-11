import React, { useContext, useEffect, useState } from "react";
import { CricketMatch, CricketFormat, CricketTeam } from "./Models/Interface";
import { MatchRecordsTable } from "./MatchRecordsTable/MatchRecordsTable";
import { useCricketMatchInfo } from "./Hooks/useCricketMatchInfo.";
import { useCricketTeamInfo } from "./Hooks/useCricketTeamInfo.";

import "./CricketMatchRecords.scss";

export interface CricketMatchRecordsProps {}

export const CricketMatchRecords: React.FunctionComponent<
  CricketMatchRecordsProps
> = () => {
  const [selectedFormat, setSelectedFormat] = useState<CricketFormat>(
    CricketFormat.ODI
  );

  const { teamData } = useCricketTeamInfo();
  const { isLoading, matchData } = useCricketMatchInfo(selectedFormat);

  return (
    <>
      <div className="format-selection">
        {[CricketFormat.ODI, CricketFormat.T20I, CricketFormat.Test].map(
          (x) => (
            <span
              key={x}
              style={{
                backgroundColor: selectedFormat === x ? "blue" : "black",
              }}
              onClick={() => setSelectedFormat(x)}
            >
              {x}
            </span>
          )
        )}
      </div>
      <MatchRecordsTable
        isLoading={isLoading}
        teamData={teamData ? (teamData as CricketTeam[]) : []}
        matchData={matchData ? (matchData as CricketMatch[]) : []}
      />
    </>
  );
};
