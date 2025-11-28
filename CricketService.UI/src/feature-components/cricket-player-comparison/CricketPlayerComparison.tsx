import React, { useState } from "react";
import { PlayerProfileContainer } from "./common/PlayerProfileContainer";
import { MovingTrain } from "../../components/common/MovingTrain";
import {
  CricketPlayerResponse,
  useFetchPlayerAllMatches,
} from "./hook/useFetchPlayerAllMatches";
import { PlayerH2HSelection } from "./PlayerH2HSelection";
import { SpeechLanguage, speakText } from "../../components/common/SpeakText";
import $ from "jquery";
import usePlayersIntroJSX from "./hook/bogies-hook/usePlayersIntroJSX";
import useProfileInfoJSX from "./hook/bogies-hook/useProfileInfoJSX";
import useBattingStatsJSX from "./hook/bogies-hook/useBattingStatsJSX";
import { BattingStatistics } from "../../components/CricketComponents/CricketPlayerInfoFetch/useCustomPlayerInfo";
import { useMenuToggle } from "../../hooks/useMenuToggle";
import CenteredSpinner from "../../components/common/CenteredSpinner";

import "./CricketPlayerComparison.scss";

interface CricketPlayerComparisonProps {}

export const CricketPlayerComparison: React.FC<CricketPlayerComparisonProps> = () => {
  const [selectedPlayer1Uuid, setSelectedPlayer1Uuid] = useState<string>("");
  const [selectedPlayer2Uuid, setSelectedPlayer2Uuid] = useState<string>("");
  const [showComparison, setShowComparison] = useState(false);
  const [isMovingTrain, setIsMovingTrain] = useState(false);
  const { isMenuOpened } = useMenuToggle();

  const { data: player1Data, isLoading: isLoadingP1 } =
    useFetchPlayerAllMatches(selectedPlayer1Uuid);
  const { data: player2Data, isLoading: isLoadingP2 } =
    useFetchPlayerAllMatches(selectedPlayer2Uuid);

  const handlePlayersSelected = (player1Uuid: string, player2Uuid: string) => {
    setSelectedPlayer1Uuid(player1Uuid);
    setSelectedPlayer2Uuid(player2Uuid);
    setShowComparison(true);
    setIsMovingTrain(false); // Reset train state when new players are selected
  };

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
    const profileBogieCount = profileInfoBogies?.length || 0;
    const battingStatsBogieCount = battingStatsBogies?.length || 0;
    
    // If no batting stats bogies, use normal speed throughout
    if (battingStatsBogieCount === 0) {
      return (time: number): number => 1.0;
    }
    
    // Estimate relative visual weight/height of each section
    // Assuming batting stats bogies are typically larger/taller than others
    const introWeight = introBogieCount * 840; // Standard height
    const profileWeight = profileBogieCount * 476; // Standard height
    const battingStatsWeight = battingStatsBogieCount * 492; // Larger height for stats
    
    const totalWeight = introWeight + profileWeight + battingStatsWeight + 91;
    const battingStatsStartWeight = introWeight + profileWeight;
    const battingStatsStartRatio = totalWeight > 0 ? battingStatsStartWeight / totalWeight : 0;

    return (time: number): number => {
      if (time < battingStatsStartRatio) {
        // Normal speed before batting stats section
        return 1.0;
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
      <PlayerH2HSelection onPlayersSelected={handlePlayersSelected} isMenuOpened={isMenuOpened} />
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
                trackLength={5000}
                duration={100}
                delay={10}
                isColumn
                popUpIndex={1}
                isMoving={isMovingTrain}
                speedFunction={createSpeedFunction()}
                debug={false}
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
