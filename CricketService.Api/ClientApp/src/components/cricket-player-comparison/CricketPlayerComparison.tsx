import React, { useEffect, useState } from "react";
import { PlayerImageContainer } from "../cricket-records/common/PlayerImageContainer";
import { useESPNPlayerInfo } from "../../hooks/espn-cricinfo-hooks/usePlayerInfo";
import { ArrowDataComparer } from "./common/ArrowDataComparer";
import { MovingTrain } from "../common/MovingTrain";
import { useFetchPlayerAllMatches } from "./hook/useFetchPlayerAllMatches";
import { SpeechLanguage, speakText } from "../common/SpeakText";
import $ from "jquery";

import "./CricketPlayerComparison.scss";
import {
  useBattingStatsJSX,
  useBowlingStatsJSX,
  useProfileInfoJSX,
} from "./useBogies";

const hrefs = ["kusal-mendis-629074", "virat-kohli-253802"];

interface CricketPlayerComparisonProps {}

export const CricketPlayerComparison: React.FC<
  CricketPlayerComparisonProps
> = ({}) => {
  const player1Data = useESPNPlayerInfo(`/cricketers/${hrefs[0]}`);
  const player2Data = useESPNPlayerInfo(`/cricketers/${hrefs[1]}`);
  const player1AddData = useFetchPlayerAllMatches(`/cricketers/${hrefs[0]}`);
  const player2AddData = useFetchPlayerAllMatches(`/cricketers/${hrefs[1]}`);
  const [showComparison, setShowComparison] = useState(false);

  $(document).on({
    keydown: (event) => {
      if (event.originalEvent?.key === "a") {
        setShowComparison(true);
      }
    },
  });

  useEffect(() => {
    speakText(
      `${player1Data.name} versus ${player2Data.name} comparison`,
      SpeechLanguage.HindiIndian
    );
  });

  const profileInfoBogies = useProfileInfoJSX(player1Data, player2Data);
  const battingStatsBogies = useBattingStatsJSX(
    player1Data.name,
    player2Data.name,
    player1AddData,
    player2AddData
  );
  const bowlingStatsBogies = useBowlingStatsJSX(
    player1Data.name,
    player2Data.name,
    player1AddData,
    player2AddData
  );

  const bogies = [...bowlingStatsBogies];

  return (
    <div className="player-comparison-container">
      {showComparison && (
        <>
          <div className="player-container player-1-container">
            {player1Data && (
              <PlayerImageContainer
                playerHref={`/cricketers/${hrefs[0]}`}
                selectedRowIndex={0}
                teamName={player1Data.teamNames[0]}
                hideRotatingCircle
                skipAnimation
                extraInfo={[
                  <p className="player-name-header">
                    {player1Data.name.toUpperCase()}
                  </p>,
                  <p>{player1Data.playingRole}</p>,
                  <p>{player1Data.battingStyle}</p>,
                  <p>{player1Data.bowlingStyle}</p>,
                ]}
                customHeight={845}
                scaleTeamCylinder={0.85}
              />
            )}
          </div>
          <div className="comparison-container">
            <MovingTrain
              bogies={bogies}
              trackLength={100000}
              duration={5000}
              delay={10}
              isColumn
              popUpIndex={1}
            />
          </div>
          <div className="player-container player-2-container">
            <PlayerImageContainer
              playerHref={`/cricketers/${hrefs[1]}`}
              selectedRowIndex={0}
              teamName={player2Data.teamNames[0]}
              hideRotatingCircle
              skipAnimation
              extraInfo={[
                <p className="player-name-header">
                  {player2Data.name.toUpperCase()}
                </p>,
                <p>{player2Data.playingRole}</p>,
                <p>{player2Data.battingStyle}</p>,
                <p>{player2Data.bowlingStyle}</p>,
              ]}
              customHeight={845}
              scaleTeamCylinder={0.85}
            />
          </div>
        </>
      )}
    </div>
  );
};
