import React, { useEffect, useState } from "react";
import { PlayerProfileContainer } from "./common/PlayerProfileContainer";
import { MovingTrain } from "../../components/common/MovingTrain";
import {
  CricketPlayerResponse,
  useFetchPlayerAllMatches,
} from "./hook/useFetchPlayerAllMatches";
import { SpeechLanguage, speakText } from "../../components/common/SpeakText";
import $ from "jquery";
import { useBattingStatsJSX, useProfileInfoJSX } from "./useBogies";
import { BattingStatistics } from "../../components/CricketComponents/CricketPlayerInfoFetch/useCustomPlayerInfo";

import "./CricketPlayerComparison.scss";
import CenteredSpinner from "../../components/common/CenteredSpinner";

interface CricketPlayerComparisonProps {}

export const CricketPlayerComparison: React.FC<
  CricketPlayerComparisonProps
> = () => {
  const { data: player1Data, isLoading: isLoadingP1 } =
    useFetchPlayerAllMatches("044051e2-7246-448f-826d-ea0e0b07e67c");
  const { data: player2Data, isLoading: isLoadingP2 } =
    useFetchPlayerAllMatches("a012ab17-063b-4cb0-8e12-14a11d38bd4d");
  const [showComparison, setShowComparison] = useState(true);

  $(document).on({
    keydown: (event) => {
      if (event.originalEvent?.key === "a") {
        setShowComparison(true);
      }
    },
  });

  useEffect(() => {
    if (player1Data?.fullName && player2Data?.fullName) {
      speakText(
        `${player1Data.fullName} versus ${player2Data.fullName} comparison`,
        SpeechLanguage.HindiIndian
      );
    }
  }, [player1Data?.fullName, player2Data?.fullName]);

  const profileInfoBogies = useProfileInfoJSX(
    player1Data as CricketPlayerResponse,
    player2Data as CricketPlayerResponse
  );

  const battingStatsBogies = useBattingStatsJSX(
    player1Data?.fullName || "",
    player2Data?.fullName || "",
    player1Data?.overallStats?.playerODIStats?.battingOverallStats as BattingStatistics,
    player2Data?.overallStats?.playerODIStats?.battingOverallStats as BattingStatistics
  );
  // const bowlingStatsBogies = useBowlingStatsJSX(
  //   player1Data?.name || "",
  //   player2Data?.name || "",
  //   player1AddData,
  //   player2AddData
  // );

  const bogies = [ ...battingStatsBogies ];

  return (
    <div className="player-comparison-container">
      {(isLoadingP1 || isLoadingP2) && <CenteredSpinner />}
      {showComparison &&
        !isLoadingP1 &&
        !isLoadingP2 &&
        player1Data &&
        player2Data && (
          <>
            <PlayerProfileContainer
              playerData={player1Data}
              customHeight={845}
              scaleTeamCylinder={0.85}
              className="player-1-container"
            />
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
            <PlayerProfileContainer
              playerData={player2Data}
              customHeight={845}
              scaleTeamCylinder={0.85}
              className="player-2-container"
            />
          </>
        )}
    </div>
  );
};
