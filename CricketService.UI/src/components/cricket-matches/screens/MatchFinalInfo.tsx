import React, { useEffect } from "react";
import { useAllPlayersUuids } from "../../hooks/useAllPlayersUuids";
import { AnimatedValueContent } from "../../cricket-players/cricket-body/PlayerInfo/AnimatedValueContent";
import playerPictures from "./../../../data/StaticData/playerPictures.json";
import { speakText } from "../../common/SpeakText";
import {
  Batsman,
  Bowler,
} from "../../../models/espn-cricinfo-models/CricketMatchModels";

import "./MatchFinalInfo.scss";
import "./../../../components/CommonCss.scss";

interface MatchFinalInfoProps {
  playerOfTheMatch: string;
  matchResult: string;
  potmBattingStats: Batsman[];
  potmBowlingStats: Bowler[];
}

export const MatchFinalInfo: React.FC<MatchFinalInfoProps> = ({
  playerOfTheMatch,
  matchResult,
  potmBattingStats,
  potmBowlingStats,
}) => {
  const { isLoading, playerInfos } = useAllPlayersUuids();

  const matchResultArr = matchResult?.replace("(W)", "Women")?.split("(");

  ////New zealand had taken 1-0 lead in 3 T Twenty International series

  useEffect(() => {
    speakText(`${matchResult
      ?.replace("(W)", "Women")
      ?.replace("WMN", "Women")
      ?.replace("S Africa", "South Africa")
      ?.replace("AUS", "Australia")
      ?.replace("NZ", "New Zealand")
      ?.replace("W Indies", "West Indies")}.
    `);
    speakText(
      `Player of the match, ${playerOfTheMatch} for 92 runs in just 41 balls.`
    );

    return () => window.speechSynthesis.cancel();
  }, []);

  return (
    <div
      className="match-final-info-container"
      style={{
        height: 860,
        background: 'url("https://wallpapercave.com/wp/wp3049846.jpg")',
        backgroundSize: "100% 100%",
      }}
    >
      <div className="match-result text-3d-2">
        {matchResultArr[0]}
        <span style={{ fontSize: 15 }}>
          {matchResultArr[1] && `(${matchResultArr[1]}`}
        </span>
      </div>
      {true && (
        <>
          <div className="potm-container" style={{ background: "none" }}>
            <img
              alt={playerOfTheMatch}
              src={
                `http://localhost:3013/images/nepal/${
                  potmBattingStats[0]?.playerName.href.split("/")[2]
                }.png` ||
                playerPictures.find(
                  (x) =>
                    x.href === potmBattingStats[0]?.playerName.href ||
                    x.href === potmBowlingStats[0]?.playerName.href
                )?.profilePics[0] ||
                playerInfos?.find(
                  (p) =>
                    p.href === potmBattingStats[0]?.playerName.href ||
                    p.href === potmBowlingStats[0]?.playerName.href
                )?.imageUrl ||
                "https://t4.ftcdn.net/jpg/02/23/50/73/360_F_223507349_F5RFU3kL6eMt5LijOaMbWLeHUTv165CB.jpg"
              }
              height={720}
              width={600}
            />
            <div style={{ background: "none" }}>
              <div className="potm-stats">
                {potmBattingStats.length > 0 && (
                  <h1>
                    <span style={{ fontSize: 50 }}>Runs</span>
                    <span style={{ fontSize: 200 }}>
                      <AnimatedValueContent
                        value={potmBattingStats
                          .map((x) => x?.runsScored)
                          .reduce((a, b) => a + b, 0)}
                        duration={3000}
                        player={null}
                        showStar={potmBattingStats[0]?.outStatus?.includes(
                          "not out"
                        )}
                      />
                    </span>
                    <span>{"   "}</span>
                    <span style={{ fontSize: 50 }}>
                      <AnimatedValueContent
                        value={potmBattingStats
                          .map((x) => x?.ballsFaced)
                          .reduce((a, b) => a + b, 0)}
                        duration={3000}
                        player={null}
                      />
                    </span>
                    <span>{"  "}</span>
                    <span>Balls</span>
                  </h1>
                )}
              </div>
              {/* <div className="potm-stats">
                {potmBowlingStats.length > 0 && (
                  <h1>
                    <span style={{ fontSize: 50 }}>Wickets</span>
                    <span style={{ fontSize: 200 }}>
                      <AnimatedValueContent
                        value={potmBowlingStats
                          .map((x) => x?.wickets)
                          .reduce((a, b) => a + b, 0)}
                        duration={3000}
                        player={null}
                      />
                    </span>
                    <span>{"   "}</span>
                    <span style={{ fontSize: 50 }}>
                      <AnimatedValueContent
                        value={potmBowlingStats
                          .map((x) => x?.runsConceded)
                          .reduce((a, b) => a + b, 0)}
                        duration={3000}
                        player={null}
                      />
                    </span>
                    <span>{"  "}</span>
                    <span>Conceded</span>
                  </h1>
                )}
              </div> */}
            </div>
          </div>
          <div className="potm">
            <p>Player Of The Match</p>
            <div className="potm-name">
              {playerOfTheMatch?.replace("Sadeera", "S.")}
            </div>
          </div>
        </>
      )}
    </div>
  );
};
