import React from "react";
import { MatchesResultSummary } from "../../../models/espn-cricinfo-models/H2HMatches";
import { TopPlayerCard } from "./TopPlayerCard";
import { H2HPageHeader } from "./H2HPageHeader";
import { MovingTrain } from "../../common/MovingTrain";
import teamLogos from "./../../../data/StaticData/teamLogos.json";

interface H2HHIScoresProps {
  team1Name: string;
  team2Name: string;
  resultSummary1: MatchesResultSummary;
}

export const H2HHIScores: React.FC<H2HHIScoresProps> = ({
  team1Name,
  team2Name,
  resultSummary1,
}) => {
  const playerCards = resultSummary1?.hIScores
    ?.slice(0, 5)
    ?.map((b, i) => (
      <TopPlayerCard
        href={b?.href}
        name={b?.name}
        matches={0}
        stat={b?.runs}
        teamShortName={b?.name?.split("(")[1]?.replace(")", "")}
        className={`top-run-scorer`}
        isStar={b?.notOut}
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
