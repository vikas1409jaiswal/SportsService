import React, { useEffect } from "react";
import { useAnimation } from "framer-motion";
import { ESPNTableRow } from "../../hook/useCustomESPNTable";
import $ from "jquery";
import { PlayerDetailsContainer } from "./PlayerDetailsContainer";
import { PlayerExtraInfo } from "../../../common/PlayerExtraInfo";
import { PlayerImageContainer } from "../../../common/PlayerImageContainer";
import { speeches } from "../../speech-management/SpeechManagement";
import { config } from "../../../../configs";

import "./IndividualPage.scss";

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
  }, [selectedRowIndex]);

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

  useEffect(() => {
    if (selectedRowIndex === 9) {
      speeches["top-10-players-intro"]();
    }

    if (row?.data) {
      speeches["most-career-runs-speech"](
        selectedRowIndex,
        playerName,
        row,
        teamName
      );
    }

    if (selectedRowIndex === 1) {
      speeches["video-end-message"]();
    }

    return () => window.speechSynthesis.cancel();
  }, [selectedRowIndex]);

  return (
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
  );
};
