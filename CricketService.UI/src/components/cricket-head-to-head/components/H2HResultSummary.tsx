import React from "react";
import { AnimatedValueContent } from "../../cricket-players/cricket-body/PlayerInfo/AnimatedValueContent";
import { MatchesResultSummary } from "../../../models/espn-cricinfo-models/H2HMatches";
import { CricketFormat } from "../../../models/enums/CricketFormat";
import { H2HPageHeader } from "./H2HPageHeader";

import "./H2HResultSummary.scss";

interface H2HResultSummaryProps {
  format: CricketFormat;
  team1Name: string;
  team2Name: string;
  resultSummary1: MatchesResultSummary;
  resultSummary2: MatchesResultSummary;
}

export const H2HResultSummary: React.FC<H2HResultSummaryProps> = ({
  format,
  team1Name,
  team2Name,
  resultSummary1,
  resultSummary2,
}) => {
  const dataMap = [
    {
      key: "Matches",
      value: resultSummary1?.matches,
    },
    {
      key: `${resultSummary1?.team
        ?.replace("United States of America", "USA")
        ?.replace("United Arab Emirates", "UAE")} Won`,
      value: resultSummary1?.won,
    },
    {
      key: `${resultSummary2?.team
        ?.replace("United States of America", "USA")
        ?.replace("United Arab Emirates", "UAE")} Won`,
      value: resultSummary1?.lost,
    },
    {
      key: "Tied",
      value: resultSummary1?.tied,
    },
    {
      key: "No Result",
      value: resultSummary1?.noResult,
    },
  ];
  return (
    <>
      <H2HPageHeader
        team1Name={team1Name}
        team2Name={team2Name}
        span={resultSummary1?.span}
      />
      <div className="h2h-result-summary-info" style={{ background: "none" }}>
        {dataMap.map((c, i) => (
          <div className="h2h-match">
            <span className="h2h-match-key">{c.key}</span>
            <AnimatedValueContent
              value={c.value}
              duration={5000}
              className="h2h-match-value"
            />
          </div>
        ))}
        {format === CricketFormat.Test && (
          <div className="h2h-match">
            <span>Draw</span>
            <span>{resultSummary1?.draw}</span>
          </div>
        )}
      </div>
    </>
  );
};
