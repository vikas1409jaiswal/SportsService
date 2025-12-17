import React, { useState } from "react";
import { PlayerProfileContainer } from "./common/PlayerProfileContainer";
import { MovingCarousel } from "../../components/common/moving-carousel";
import {
  CricketPlayerResponse,
  useFetchPlayerAllMatches,
} from "./hook/useFetchPlayerAllMatches";
import { PlayerH2HSelection } from "./PlayerH2HSelection";
import $ from "jquery";
import usePlayersIntroJSX from "./hook/bogies-hook/usePlayersIntroJSX";
import useProfileInfoJSX from "./hook/bogies-hook/useProfileInfoJSX";
import useBattingStatsJSX from "./hook/bogies-hook/useBattingStatsJSX";
import { BattingStatsHeader } from "./hook/bogies-hook/useBattingStatsJSX";
import { BattingStatistics } from "../../components/CricketComponents/CricketPlayerInfoFetch/useCustomPlayerInfo";
import { useMenuToggle } from "../../hooks/useMenuToggle";
import CenteredSpinner from "../../components/common/CenteredSpinner";

import "./CricketPlayerComparison.scss";

type CricketFormat = 'T20I' | 'ODI' | 'Test';

interface CricketPlayerComparisonProps {}

export const CricketPlayerComparison: React.FC<CricketPlayerComparisonProps> = () => {
  const [selectedPlayer1Uuid, setSelectedPlayer1Uuid] = useState<string>("044051e2-7246-448f-826d-ea0e0b07e67c");
  const [selectedPlayer2Uuid, setSelectedPlayer2Uuid] = useState<string>("a012ab17-063b-4cb0-8e12-14a11d38bd4d");
  const [showComparison, setShowComparison] = useState(false);
  const [selectedFormat, setSelectedFormat] = useState<CricketFormat>('T20I');
  const [isFormatDropdownOpen, setIsFormatDropdownOpen] = useState(false);
  const { isMenuOpened } = useMenuToggle();

  const { data: player1Data, isLoading: isLoadingP1 } =
    useFetchPlayerAllMatches(selectedPlayer1Uuid);
  const { data: player2Data, isLoading: isLoadingP2 } =
    useFetchPlayerAllMatches(selectedPlayer2Uuid);

  const handlePlayersSelected = (player1Uuid: string, player2Uuid: string) => {
    setSelectedPlayer1Uuid(player1Uuid);
    setSelectedPlayer2Uuid(player2Uuid);
    setShowComparison(true);
  };

  React.useEffect(() => {
    const handler = (event: any) => {
      if (event.originalEvent?.key === "a") {
        setShowComparison(true);
      }
    };
    $(document).on("keydown", handler);
    return () => {
      $(document).off("keydown", handler);
    };
  }, []);

  // Close dropdown when clicking outside
  React.useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      const target = event.target as HTMLElement;
      if (isFormatDropdownOpen && !target.closest('.format-selector')) {
        setIsFormatDropdownOpen(false);
      }
    };

    document.addEventListener('click', handleClickOutside);
    return () => {
      document.removeEventListener('click', handleClickOutside);
    };
  }, [isFormatDropdownOpen]);

  // Get batting stats based on selected format
  const getBattingStats = React.useCallback((playerData: CricketPlayerResponse | undefined): BattingStatistics | undefined => {
    if (!playerData?.overallStats) return undefined;
    
    console.log('Getting batting stats for format:', selectedFormat);
    console.log('Player data overallStats:', playerData.overallStats);
    
    switch (selectedFormat) {
      case 'ODI':
        console.log('ODI Stats:', playerData.overallStats.playerODIStats?.battingOverallStats);
        return playerData.overallStats.playerODIStats?.battingOverallStats;
      case 'T20I':
        console.log('T20I Stats:', playerData.overallStats.playerT20IStats?.battingOverallStats);
        return playerData.overallStats.playerT20IStats?.battingOverallStats;
      case 'Test':
        console.log('Test Stats:', (playerData.overallStats as any).playerTestStats?.battingOverallStats);
        // Note: Test stats might not be available yet, handle gracefully
        return (playerData.overallStats as any).playerTestStats?.battingOverallStats;
      default:
        return playerData.overallStats.playerT20IStats?.battingOverallStats;
    }
  }, [selectedFormat]);

  const introJSX = usePlayersIntroJSX(player1Data?.fullName || "", player2Data?.fullName || "");
  const profileInfoBogies = useProfileInfoJSX(
    player1Data as CricketPlayerResponse,
    player2Data as CricketPlayerResponse
  );
  
  // Get current batting stats based on selected format
  const player1BattingStats = React.useMemo(() => getBattingStats(player1Data), [player1Data, getBattingStats]);
  const player2BattingStats = React.useMemo(() => getBattingStats(player2Data), [player2Data, getBattingStats]);
  
  const battingStatsBogies = useBattingStatsJSX(
    player1Data?.fullName || "",
    player2Data?.fullName || "",
    player1BattingStats as BattingStatistics,
    player2BattingStats as BattingStatistics,
    (props) => <BattingStatsHeader selectedFormat={selectedFormat} {...props} />
  );
  // const bowlingStatsBogies = useBowlingStatsJSX(
  //   player1Data?.name || "",
  //   player2Data?.name || "",
  //   player1AddData,
  //   player2AddData
  // );

  const bogies = [<div>Let's Start</div>, introJSX, ...profileInfoBogies,...battingStatsBogies ];

  const renderFormatSelector = () => {
    if (!isMenuOpened) return null;
    
    const handleFormatSelect = (format: CricketFormat) => {
      setSelectedFormat(format);
      setIsFormatDropdownOpen(false);
    };
    
    return (
      <div className="format-selector">
        <button 
          className="format-button"
          onClick={() => setIsFormatDropdownOpen(!isFormatDropdownOpen)}
        >
          Format: {selectedFormat}
          {isFormatDropdownOpen && (
            <div className="format-dropdown">
              <button 
                className={`format-option ${selectedFormat === 'T20I' ? 'active' : ''}`}
                onClick={(e) => {
                  e.stopPropagation();
                  handleFormatSelect('T20I');
                }}
              >
                T20I
              </button>
              <button 
                className={`format-option ${selectedFormat === 'ODI' ? 'active' : ''}`}
                onClick={(e) => {
                  e.stopPropagation();
                  handleFormatSelect('ODI');
                }}
              >
                ODI
              </button>
              <button 
                className={`format-option ${selectedFormat === 'Test' ? 'active' : ''}`}
                onClick={(e) => {
                  e.stopPropagation();
                  handleFormatSelect('Test');
                }}
              >
                Test
              </button>
            </div>
          )}
        </button>
      </div>
    );
  };

  return (
    <div className="player-comparison-container">
      <div className="player-selection-wrapper">
        <PlayerH2HSelection onPlayersSelected={handlePlayersSelected} isMenuOpened={isMenuOpened} />
        {renderFormatSelector()}
      </div>
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
              isMenuOpened={isMenuOpened}
              selectedFormat={selectedFormat}
            />
            <div className="comparison-container">
              <MovingCarousel
                key={`carousel-${selectedFormat}`}
                bogies={bogies}
                autoAdvance={false}
                animationConfig={{
                  enterY: 0,
                  exitY: 0,
                  enterScale: 0.9,
                  exitScale: 0.9,
                  enterOpacity: 0,
                  exitOpacity: 0,
                  springStiffness: 200,
                  springDamping: 25,
                  springMass: 0.8,
                  opacityDuration: 0.4,
                  scaleDuration: 0.4,
                  easingFunction: "easeInOut",
                }}
              />
            </div>
            <PlayerProfileContainer
              playerData={player2Data}
              customHeight={845}
              scaleTeamCylinder={0.85}
              className="player-2-container"
              isMenuOpened={isMenuOpened}
              selectedFormat={selectedFormat}
            />
          </>
        )}
    </div>
  );
};
