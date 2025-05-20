import React, { useEffect } from "react";
import { PlayerImageContainer } from "./PlayerImageContainer";
import { getNameFromHref } from "../../../utils/ReusableFuctions";
import { SpeechLanguage, speakText } from "../../common/SpeakText";
import { useInView } from "react-intersection-observer";
import { useESPNPlayerInfo } from "../../../hooks/espn-cricinfo-hooks/usePlayerInfo";

interface PlayerBirthDayCardProps {
  playerHref: string;
  dateOfBirth: string;
  index: number;
}

export const PlayerBirthDayCard: React.FC<PlayerBirthDayCardProps> = ({
  playerHref,
  dateOfBirth,
  index,
}) => {
  const playerName = getNameFromHref(playerHref, "eng", 25);
  const [ref, inView] = useInView({
    triggerOnce: true, // Render the component only once
    threshold: 1, // Trigger when 50% of the component is in view
  });

  const { teamNames, span, playingRole } = useESPNPlayerInfo(playerHref);

  // useEffect(() => {
  //   [0, 1, 2]?.includes(index) &&
  //     speakText(
  //       `${playerName}, from ${teamNames[0]} ${
  //         dateOfBirth.split(" ")[0]
  //       } November`
  //     );
  // }, [index]);

  useEffect(() => {
    inView &&
      speakText(
        `${playerName}, from ${teamNames[0]} ${
          dateOfBirth.split(" ")[0]
        } November`
      );
  }, [inView, index]);

  return teamNames.length > 0 ? (
    <div ref={ref}>
      <PlayerImageContainer
        playerHref={playerHref}
        selectedRowIndex={0}
        hideRotatingCircle
        extraInfo={[
          <p style={{ fontWeight: 900, height: 60, position: "relative" }}>
            {playerName}
            <br />
            <span
              style={{
                fontSize: 16,
                position: "absolute",
                marginLeft: -50,
              }}
            >
              {playingRole}
            </span>
          </p>,
          <p>{dateOfBirth}</p>,
          <p>{teamNames[0]}</p>,
        ]}
        teamName={teamNames[0]}
      />
    </div>
  ) : (
    <></>
  );
};
