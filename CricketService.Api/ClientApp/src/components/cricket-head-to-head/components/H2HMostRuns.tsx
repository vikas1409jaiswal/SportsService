import React from "react";
import { MatchesResultSummary } from "../../../models/espn-cricinfo-models/H2HMatches";
import { H2HPageHeader } from "./H2HPageHeader";
import { TopPlayerCard } from "./TopPlayerCard";
import { MovingTrain } from "../../common/MovingTrain";

import "./H2HMostRecords.scss";

interface H2HMostRunsProps {
  team1Name: string;
  team2Name: string;
  resultSummary1: MatchesResultSummary;
}

export const H2HMostRuns: React.FC<H2HMostRunsProps> = ({
  team1Name,
  team2Name,
  resultSummary1,
}) => {
  const playerCards = resultSummary1?.mostRuns
    ?.slice(0, 5)
    ?.map((b, i) => (
      <TopPlayerCard
        href={b?.href}
        name={b?.name}
        matches={b?.matches}
        stat={b?.runs}
        teamShortName={b?.name?.split("(")[1]?.replace(")", "")}
        className={`top-run-scorer`}
      />
    ));
  return (
    <>
      <H2HPageHeader
        team1Name={team1Name}
        team2Name={team2Name}
        span={resultSummary1?.span}
      />
      <div className="top-player-cards">
        <MovingTrain
          bogies={playerCards}
          trackLength={700}
          duration={10}
          delay={5}
        />
      </div>
    </>
  );
};
