import React, { useContext, useEffect, useState } from "react";
import { GenericModifiers } from "../../Types/SoccerTypes";
import { CricketProfileContext } from "../../CricketPlayersProfile";
import { motion, useAnimation } from "framer-motion";
import {
  BattingMatchStatistics,
  BattingStatistics,
  BowlingMatchStatistics,
  BowlingStatistics,
  FieldingStatistics,
  PlayerInfo,
} from "../../../CricketComponents/CricketPlayerInfoFetch/useCustomPlayerInfo";
import { SingleStats } from "./SingleStats";
import { CountryContent } from "./CountryContent";
import RotatingCircle from "../../../cricket-records/common/RotatingCircle";
import { featureToggle } from "./feature-toggle";
import $ from "jquery";

import "./PlayerDetails.scss";
import { PlayerImage } from "../../../cricket-matches/screens/elements/PlayerImage";
import { speakText } from "../../../common/SpeakText";

type PlayerDetailsProps = {
  player: PlayerInfo;
  selectedPlayerIndex: number;
  imageUrl: string;
  setSelectedPlayerIndex: GenericModifiers<number>;
};

export const PlayerDetails: React.FunctionComponent<PlayerDetailsProps> = ({
  player,
  selectedPlayerIndex,
  imageUrl,
  setSelectedPlayerIndex,
}) => {
  const { playersForShow, showHeader } = useContext(CricketProfileContext);

  const [flipBodyColumns, setFlipBodyColumns] = useState(false);
  const [showIndicatorColumn, setShowIndicatorColumn] = useState(false);

  const isFirstPlayer = selectedPlayerIndex === 0;

  const isLastPlayer = selectedPlayerIndex === playersForShow.length - 1;

  const playerDetailsControl = useAnimation();
  const playerImageControl = useAnimation();

  console.log(player);

  useEffect(() => {
    playerDetailsControl.start({
      y: ["1000px", "0px"],
      transition: {
        type: "spring",
        stiffness: 10,
        damping: 100,
      },
    });

    playerImageControl.start({
      scale: [0, 1],
      rotate: [0, 720],
      transition: {
        type: "spring",
        stiffness: 100,
        damping: 10,
      },
    });
  }, [player, playerDetailsControl, playerImageControl]);

  // // Press => for next player & <= for previous player.
  // // Press Shift + F for flipping body columns.
  $(document).on({
    keydown: (event) => {
      if (event.originalEvent?.key === "ArrowRight" && !isLastPlayer) {
        setSelectedPlayerIndex(selectedPlayerIndex + 1);
      }
      if (event.originalEvent?.key === "ArrowLeft" && !isFirstPlayer) {
        setSelectedPlayerIndex(selectedPlayerIndex - 1);
      }
      if (event.originalEvent?.key === "F") {
        setFlipBodyColumns(!flipBodyColumns);
      }
    },
  });

  const playerImageContainer = (
    <>
      <motion.div
        className="cricket-player-image"
        animate={playerImageControl}
        whileTap={{
          backgroundColor: "orange",
        }}
      >
        <PlayerImage
          alt={player?.name}
          href={player?.playerUuid}
          playerInfos={[]}
        />
        {player?.playingRole && <h3>{player?.playingRole}</h3>}
        {player?.battingStyle && <h3>{player?.battingStyle?.toUpperCase()}</h3>}
        {player?.bowlingStyle && <h3>{player?.bowlingStyle?.toUpperCase()}</h3>}
      </motion.div>
      <RotatingCircle number={selectedPlayerIndex + 1} />
    </>
  );

  const playerDetailsContainer = (
    <div className="cricket-player-details">
      {/* <SpeechSpeaker text={player?.name} /> */}
      <motion.h1
        animate={playerDetailsControl}
        whileTap={{
          scale: 1.05,
          backgroundColor: "pink",
        }}
      >
        <a
          href={`https://www.espncricinfo.com/${player?.playerUuid}`}
        >{`${player?.name?.toUpperCase()}`}</a>
      </motion.h1>
      <motion.div
        className="cricket-player-details-section"
        animate={playerDetailsControl}
      >
        {player ? (
          <motion.div
            whileTap={{
              scale: 1.05,
              backgroundColor: "pink",
            }}
          >
            <div>
              <span>Full Name </span>
              <span>{player?.fullName}</span>
            </div>
            <div>
              <span>Date Of Birth </span>
              <span>{player?.birth}</span>
            </div>
            {featureToggle.showTeams && (
              <div>
                <span>Teams </span>
                <span>{player?.teamNames.slice(0, 15).join(", ")}</span>
              </div>
            )}
          </motion.div>
        ) : (
          <div className="loading-container">...LOADING</div>
        )}
        {!showHeader && player && (
          <SingleStats
            player={player}
            singleStat={{
              key: "Best Bowling Figure (in CPL)",
              value:
                `${player.matchDetail?.t20IMatches?.bowlingStats?.wickets}/${player.matchDetail?.t20IMatches?.bowlingStats?.runsConceded}` ||
                "",
            }}
          />
        )}
        <CountryContent countryName={player?.teamNames[0]} />
      </motion.div>
    </div>
  );

  useEffect(() => {
    if (selectedPlayerIndex === 9) {
      speakText(
        "In this video we will see top 10 players who has best bowling figures in caribbean premier league."
      );
    }

    if (player) {
      // const { catches, innings, catchesPerInning } = player.career?.t20IMatches
      //   ?.fieldingStats as FieldingStatistics;
      // const { runs, matches, innings, strikeRate, sixes, fours } = player.career
      //   ?.t20IMatches?.battingStats as BattingStatistics;
      const { wickets, runsConceded, forTeam, opps } = player.matchDetail
        ?.t20IMatches?.bowlingStats as BowlingMatchStatistics;
      speakText(
        `Number ${selectedPlayerIndex + 1}, ${player?.name} from ${
          player?.teamNames[0]
        }, ${wickets} Wickets conceding ${runsConceded} Runs for ${forTeam} against ${opps?.replace(
          "v",
          ""
        )}.`
      );
      // const {
      //   wickets,
      //   economy,
      //   innings: bInnings,
      // } = player.career?.t20IMatches?.bowlingStats as BowlingStatistics;
      // speakText(
      //   `Number ${selectedPlayerIndex + 1}, ${player?.name} from ${
      //     player?.teamNames[0]
      //   }, ${runs} Runs in ${innings} innings with a strike rate of ${strikeRate}.`
      // );
      // speakText(
      //   `Number ${selectedPlayerIndex + 1}, ${player?.name} from ${
      //     player?.teamNames[0]
      //   }, ${sixes} Sixes in ${innings} innings. also scored ${fours} fours`
      // );
      //   speakText(
      //     `Number ${selectedPlayerIndex + 1}, ${player?.name} from ${
      //       player?.teamNames[0]
      //     }, ${catches} Catches in ${innings} innings with average of ${catchesPerInning} catch per inning.`
      //   );
    }

    if (selectedPlayerIndex === 0) {
      speakText(
        "Thanks for watching this video. Please like share and subscribe."
      );
    }

    return () => window.speechSynthesis.cancel();
  }, [player]);

  return (
    <>
      <div
        className="cricket-player"
        style={
          flipBodyColumns
            ? {
                display: "grid",
                gridTemplateColumns: showIndicatorColumn
                  ? "24fr 16fr 1fr"
                  : "3fr 2fr",
              }
            : {
                display: "grid",
                gridTemplateColumns: showIndicatorColumn
                  ? "16fr 24fr 1fr"
                  : "2fr 3fr",
              }
        }
      >
        {!flipBodyColumns && playerImageContainer}
        {playerDetailsContainer}
        {flipBodyColumns && playerImageContainer}
        {showIndicatorColumn && (
          <div className="players-indicators-container">
            <span style={{ backgroundColor: "palegreen" }}>
              {selectedPlayerIndex + 1}
            </span>
            {isFirstPlayer && (
              <span style={{ backgroundColor: "yellowgreen" }}>F</span>
            )}
            {isLastPlayer && <span style={{ backgroundColor: "red" }}>L</span>}
          </div>
        )}
      </div>
    </>
  );
};
