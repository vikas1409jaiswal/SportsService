import React, { useEffect, useState } from "react";
import { PlayerProfileContainer } from "./common/PlayerProfileContainer";
import { MovingTrain } from "../../components/common/MovingTrain";
import {
  CricketPlayerResponse,
  useFetchPlayerAllMatches,
} from "./hook/useFetchPlayerAllMatches";
import { SpeechLanguage, speakText } from "../../components/common/SpeakText";
import $ from "jquery";
import usePlayersIntroJSX from "./hook/bogies-hook/usePlayersIntroJSX";
import useProfileInfoJSX from "./hook/bogies-hook/useProfileInfoJSX";
import useBattingStatsJSX from "./hook/bogies-hook/useBattingStatsJSX";
import { BattingStatistics } from "../../components/CricketComponents/CricketPlayerInfoFetch/useCustomPlayerInfo";
import "./CricketPlayerComparison.scss";
import CenteredSpinner from "../../components/common/CenteredSpinner";

interface CricketPlayerComparisonProps {}

export const CricketPlayerComparison: React.FC<CricketPlayerComparisonProps> = () => {
  const { data: player1Data, isLoading: isLoadingP1 } =
    useFetchPlayerAllMatches("044051e2-7246-448f-826d-ea0e0b07e67c");
  const { data: player2Data, isLoading: isLoadingP2 } =
    useFetchPlayerAllMatches("a012ab17-063b-4cb0-8e12-14a11d38bd4d");
  const [showComparison, setShowComparison] = useState(true);
  const [isMovingTrain, setIsMovingTrain] = useState(false);
  const [isMenuOpened, setIsMenuOpened] = useState(false);

  React.useEffect(() => {
    const handler = (event: any) => {
      if (event.originalEvent?.key === "a") {
        setShowComparison(true);
      }
      if (
        event.originalEvent?.key === "s" || 
        (event.originalEvent?.key === "S" && event.originalEvent?.shiftKey)
      ) {
        setIsMovingTrain(true);
        // Trigger speech when train starts moving (user interaction context)
        if (player1Data?.fullName && player2Data?.fullName) {
          speakText(
            `Welcome! In this video, we compare ${player1Data.fullName} and ${player2Data.fullName}—breaking down their stats, careers, and strengths.`,
            SpeechLanguage.HindiIndian,
            false
          );
        }
      }
      if (
        event.originalEvent?.key === "m" || event.originalEvent?.key === "M"
      ) {
        setIsMenuOpened((prev: boolean) => !prev);
      }
    };
    $(document).on("keydown", handler);
    return () => {
      $(document).off("keydown", handler);
    };
  }, [player1Data?.fullName, player2Data?.fullName]);

  // useEffect(() => {
  //   if (player1Data?.fullName && player2Data?.fullName) {
  //     speakText(
  //       `Welcome! In this video, we compare ${player1Data.fullName} and ${player2Data.fullName}—breaking down their stats, careers, and strengths.`,
  //       SpeechLanguage.HindiIndian
  //     );
  //   }
  // }, [player1Data?.fullName, player2Data?.fullName]);

  const introJSX = usePlayersIntroJSX(player1Data?.fullName || "", player2Data?.fullName || "", isMovingTrain);
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

  const bogies = [introJSX, ...profileInfoBogies,...battingStatsBogies ];

  const renderBanner = () => (
    <div
      className="train-banner-alert"
      role="alert"
      aria-live="assertive"
    >
      <span className="train-banner-icon">⚠️</span>
      Train is stopped. Press <b>S</b> or <b>Shift+S</b> to start the train animation.
    </div>
  );

  return (
    <div className="player-comparison-container">
      {(isLoadingP1 || isLoadingP2) && <CenteredSpinner />}
      {showComparison &&
        !isLoadingP1 &&
        !isLoadingP2 &&
        player1Data &&
        player2Data && (
          <>
            {!isMovingTrain && renderBanner()}
            <PlayerProfileContainer
              playerData={player1Data}
              customHeight={845}
              scaleTeamCylinder={0.85}
              className="player-1-container"
              isMenuOpened={isMenuOpened}
            />
            <div className="comparison-container">
              <MovingTrain
                bogies={bogies}
                trackLength={100000}
                duration={2000}
                delay={10}
                isColumn
                popUpIndex={1}
                isMoving={isMovingTrain}
              />
            </div>
            <PlayerProfileContainer
              playerData={player2Data}
              customHeight={845}
              scaleTeamCylinder={0.85}
              className="player-2-container"
              isMenuOpened={isMenuOpened}
            />
          </>
        )}
    </div>
  );
};
