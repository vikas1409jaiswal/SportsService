import React, { useState } from "react";
import {
  CricketMatch,
  CricketTeam,
  TestCricketMatch,
} from "./Models/Interface";
import { MatchRecordsTable } from "./MatchRecordsTable/MatchRecordsTable";
import {
  useCricketMatchInfo,
  useTestCricketMatchInfo,
} from "./Hooks/useCricketMatchInfo.";
import { useCricketTeamInfo } from "./Hooks/useCricketTeamInfo.";
import { TestMatchRecordsTable } from "./TestMatchRecordsTable/TestMatchRecordsTable";
import { CricketFormat } from "../../../models/enums/CricketFormat";

import "./CricketAnimations.scss";

export interface CricketAnimationsProps {}

export const CricketAnimations: React.FunctionComponent<
  CricketAnimationsProps
> = () => {
  const [selectedFormat, setSelectedFormat] = useState<CricketFormat>(
    CricketFormat.Test
  );

  const { teamData } = useCricketTeamInfo();
  const { isLoading, matchData } = useCricketMatchInfo(selectedFormat);
  const { isLoading: isLoadingTest, testMatchData } =
    useTestCricketMatchInfo(selectedFormat);

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
      {selectedFormat === CricketFormat.Test ? (
        <TestMatchRecordsTable
          isLoading={isLoadingTest}
          teamData={teamData ? (teamData as CricketTeam[]) : []}
          matchData={testMatchData ? (testMatchData as TestCricketMatch[]) : []}
        />
      ) : (
        <MatchRecordsTable
          isLoading={isLoading}
          teamData={teamData ? (teamData as CricketTeam[]) : []}
          matchData={matchData ? (matchData as CricketMatch[]) : []}
          format={selectedFormat}
        />
      )}
    </>
  );
};
