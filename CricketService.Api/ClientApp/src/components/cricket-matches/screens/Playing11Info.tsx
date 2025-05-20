import React, { useEffect, useState } from "react";
import {
  MatchSquad,
  TeamSquadInfo,
} from "../../../models/espn-cricinfo-models/CricketMatchModels";
import { PlayerImage } from "./elements/PlayerImage";
import { motion, useAnimation } from "framer-motion";
import { RotatingCylinder } from "../../common/RotatingCylinder";
import logos from "./../../../data/StaticData/teamLogos.json";
import $ from "jquery";

import "./Playing11Info.scss";
import { speakText } from "../../common/SpeakText";

interface Playing11InfoProps {
  matchSquad: MatchSquad;
}

export const Playing11Info: React.FC<Playing11InfoProps> = ({ matchSquad }) => {
  const [showTeam1, setShowTeam1] = useState(true);

  $(document).on({
    keydown: (event) => {
      if (event.originalEvent?.key === "n") {
        setShowTeam1(!showTeam1);
        event.preventDefault();
      }
    },
  });

  return (
    <>
      {showTeam1 && (
        <TeamPlaying11Info teamSquadInfo={matchSquad.team1SquadInfo} />
      )}
      {!showTeam1 && (
        <TeamPlaying11Info teamSquadInfo={matchSquad.team2SquadInfo} />
      )}
    </>
  );
};

interface TeamPlaying11InfoProps {
  teamSquadInfo: TeamSquadInfo;
}

export const TeamPlaying11Info: React.FC<TeamPlaying11InfoProps> = ({
  teamSquadInfo,
}) => {
  const controls = useAnimation();

  useEffect(() => {
    controls.start((i) => ({
      scale: [0, 1],
      transition: {
        duration: 3,
        delay: (i - 1) * 0.2,
      },
    }));
  }, []);

  useEffect(() => {
    speakText(`${teamSquadInfo.teamName} Playing 11`);
    return () => window.speechSynthesis.cancel();
  }, []);
  return (
    <>
      <div
        className="team-playing-11-container"
        style={{
          height: 860,
          background: 'url("https://wallpapercave.com/wp/wp3049846.jpg")',
          backgroundSize: "100% 100%",
        }}
      >
        <div className="playing-11-header">{`${teamSquadInfo.teamName} Playing 11`}</div>
        <div
          className="playing-11-squad"
          style={{
            background: "none",
          }}
        >
          {(teamSquadInfo.teamName === "xxxx"
            ? teamSquadInfo.teamSquad
                .slice(0, 7)
                .concat(teamSquadInfo.teamSquad.slice(8, 11))
                .concat([
                  {
                    player: {
                      name: "Josh Hazlewood",
                      href: "/cricketers/josh-hazlewood-288284",
                    },
                    role: "Bowler",
                  },
                ])
            : teamSquadInfo.teamSquad.slice(0, 11)
          ).map((p, i) => (
            <motion.div
              className="player-card"
              animate={controls}
              custom={i + 1}
              style={{
                backgroundColor:
                  logos.find((x) => x.teamName === teamSquadInfo.teamName)
                    ?.primaryColor || "white",
              }}
            >
              <PlayerImage
                href={p.player.href}
                alt={p.player.name}
                playerInfos={[]}
                height={250}
                width={200}
                teamName={teamSquadInfo.teamName}
              />
              <h5>
                <a
                  href={`https://www.espncricinfo.com/${p.player.href}`}
                  style={{
                    color: "black",
                    fontWeight: 700,
                    textDecoration: "none",
                  }}
                >
                  {p.player.name}
                </a>
              </h5>
              <h6>{p.role}</h6>
            </motion.div>
          ))}
        </div>
        <RotatingCylinder
          images={Array.from(
            { length: 5 },
            () =>
              logos.find((t) => t.teamName === teamSquadInfo.teamName)
                ?.logoUrl as string
          )}
          width={120}
          height={120}
          rotationSpeed={8}
          translateZ={80}
          perspective={50000}
        />
      </div>
    </>
  );
};
