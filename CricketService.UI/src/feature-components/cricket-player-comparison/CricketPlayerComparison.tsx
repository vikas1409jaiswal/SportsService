import React, { useState } from "react";
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
import { useMenuToggle } from "../../hooks/useMenuToggle";
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
  const { isMenuOpened } = useMenuToggle();

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
    };
    $(document).on("keydown", handler);
    return () => {
      $(document).off("keydown", handler);
    };
  }, [player1Data?.fullName, player2Data?.fullName]);

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

  // Create speed function that halves speed when batting stats bogies start appearing
  // Accounts for different bogie lengths/heights
  const createSpeedFunction = () => {
    const introBogieCount = 1;
    const profileBogieCount = profileInfoBogies.length;
    const battingStatsBogieCount = battingStatsBogies.length;
    
    // Estimate relative visual weight/height of each section
    // Assuming batting stats bogies are typically larger/taller than others
    const introWeight = introBogieCount * 840; // Standard height
    const profileWeight = profileBogieCount * 476; // Standard height
    const battingStatsWeight = battingStatsBogieCount * 492; // Larger height for stats
    
    const totalWeight = introWeight + profileWeight + battingStatsWeight + 91;
    const battingStatsStartWeight = introWeight + profileWeight;
    const battingStatsStartRatio = battingStatsStartWeight / totalWeight;

    return (time: number): number => {
      if (time < battingStatsStartRatio) {
        // Normal speed before batting stats section
        return 5;
      } else {
        // Half speed when batting stats bogies appear (they're more content-heavy)
        return 0.5;
      }
    };
  };

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
                speedFunction={createSpeedFunction()}
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
