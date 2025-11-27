import React, { useEffect, useState, useCallback } from "react";
import { useAnimation } from "framer-motion";
import { ESPNTableRow } from "../../hook/useCustomESPNTable";
import $ from "jquery";
import { PlayerDetailsContainer } from "./PlayerDetailsContainer";
import { PlayerExtraInfo } from "../../../common/PlayerExtraInfo";
import { PlayerImageContainer } from "../../../common/PlayerImageContainer";
import { speeches } from "../../speech-management/SpeechManagement";
import { config } from "../../../../configs";
import { useProfileContext } from "../../ProfileContext";
import { YouTubeInteraction } from "../../../common/yt-interaction";

import "./IndividualPage.scss";

const speechMapping: Record<string, keyof typeof speeches> = {
  MostWicketsInCareer: "most-career-wickets-speech",
  MostRunsInCareer: "most-career-runs-speech",
  MostSixesInCareer: "most-career-sixes-speech",
};

type IndividualPageProps = {
  row: ESPNTableRow;
  selectedRowIndex: number;
  setSelectedRowIndex: (i: number) => void;
};

export const IndividualPage: React.FunctionComponent<IndividualPageProps> = ({
  row,
  selectedRowIndex,
  setSelectedRowIndex,
}) => {
  const [showYouTubeInteraction, setShowYouTubeInteraction] = useState(false);
  
  const playerHref =
    row?.data.find((x) => x.key === "Player Href")?.value || "";
  const teamName =
    row?.data.find((x) => x.key === "Team Name")?.value || "";
  const playerName =
    playerHref
      ?.split("/")[2]
      ?.split("-")
      ?.filter((x) => Number.isNaN(parseInt(x)))
      ?.join(" ") || "";

  const isFirstPlayer = selectedRowIndex === 0;
  const isLastPlayer = selectedRowIndex === 9;

  const playerDetailsControl = useAnimation();

  // Custom speech function with event handling for YouTube interaction
  const speakVideoEndMessage = useCallback(() => {
    const speechMappingJson = require("../../speech-management/speechMapping.json");
    const msg = speechMappingJson["video-end-message"][config.language] || "Thanks for watching! Please like, share and subscribe for more cricket content.";
    
    if ("speechSynthesis" in window) {
      const synthesis = window.speechSynthesis;
      const utterance = new SpeechSynthesisUtterance(msg);
      utterance.lang = "hi-IN";
      utterance.volume = 1;
      utterance.rate = 0.9;
      
      utterance.onstart = () => {
        setShowYouTubeInteraction(true);
      };
      
      utterance.onend = () => {
        setShowYouTubeInteraction(false);
      };
      
      synthesis.speak(utterance);
    }
  }, []);

  useEffect(() => {
    config.isAnimation &&
      playerDetailsControl.start({
        x: ["1000px", "0px"],
        transition: {
          type: "spring",
          stiffness: 10,
          damping: 100,
        },
      });
  }, [selectedRowIndex, playerDetailsControl]);

  $(document).on({
    keydown: (event) => {
      if (event.originalEvent?.key === "ArrowRight" && !isLastPlayer) {
        setSelectedRowIndex(selectedRowIndex + 1);
      }
      if (event.originalEvent?.key === "ArrowLeft" && !isFirstPlayer) {
        setSelectedRowIndex(selectedRowIndex - 1);
      }
    },
  });

  const { selectedProfile } = useProfileContext();

  useEffect(() => {
    if (selectedRowIndex === 9) {
      speeches["top-10-players-intro"]();
    }

    if (row?.data) {
      const speechKey = speechMapping[selectedProfile];
      if (speechKey && speeches[speechKey]) {
        speeches[speechKey](selectedRowIndex, playerName, row, teamName);
      }
    }

    if (selectedRowIndex === 1) {
      speakVideoEndMessage();
    }

    return () => window.speechSynthesis.cancel();
  }, [selectedRowIndex, selectedProfile, row, playerName, teamName, speakVideoEndMessage]);

  return (
    <>
      <div className="cricket-player-container">
        <PlayerImageContainer
          playerHref={playerHref}
          selectedRowIndex={selectedRowIndex}
          teamName={teamName}
          extraInfo={[
            <PlayerExtraInfo
              key="player-extra-info"
              playerHref={playerHref}
            />
          ]}
        />
        <PlayerDetailsContainer
          playerName={playerName}
          playerHref={playerHref}
          row={row}
          playerDetailsControl={playerDetailsControl}
          teamName={teamName}
        />
      </div>
      <YouTubeInteraction 
        show={showYouTubeInteraction}
        duration={6000}
        onComplete={() => 
          setShowYouTubeInteraction(false)
        }
      />
    </>
  );
};
