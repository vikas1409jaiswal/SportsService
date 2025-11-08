import React, { useEffect } from "react";
import { RotatingCylinder } from "../../common/RotatingCylinder";
import logos from "./../../../data/StaticData/teamLogos.json";
import { AnimatedValueContent } from "../../cricket-players/cricket-body/PlayerInfo/AnimatedValueContent";
import { GradualText } from "../../common/GradualText";
import { speakText } from "../../common/SpeakText";

import "./MatchBasicInfo.scss";
import "./../../../components/CommonCss.scss";

const VenueCountryMap = {
  Lauderhill: "USA",
};

interface MatchBasicInfoProps {
  href: string;
  matchNumber: string;
  matchTitle: string;
  matchDate: string;
  matchVenue: string;
  matchSeries: string;
  tossWinner: string;
  tossResult: string;
}

export const MatchBasicInfo: React.FC<MatchBasicInfoProps> = ({
  href,
  matchNumber,
  matchTitle,
  matchDate,
  matchVenue,
  matchSeries,
  tossWinner,
  tossResult,
}) => {
  const [team1, team2] = matchTitle.split(" vs ");
  const mNArr = matchNumber.split(" ");

  //  Nepal tri nations series, 2nd t twenty international
  //1st One day international of 4 match series,
  //ICC world cup 2023, warm up match

  useEffect(() => {
    speakText(
      `${matchNumber
        ?.replace("WODI", "Women One day International")
        ?.replace("WT20I", "Women T Twenty International")
        ?.replace("ODI", "One day International")
        ?.replace("T20I", "T Twenty International")
        ?.replace(
          "no.",
          "number"
        )}, ${team1} versus ${team2} - Nepal tri nations series, Match Number 3, ${tossWinner} won the toss, ${tossResult}`
    );

    return () => window.speechSynthesis.cancel();
  }, []);

  return (
    <div
      className="match-basic-info-container"
      style={{
        height: 860,
        background: 'url("https://wallpapercave.com/wp/wp3049846.jpg")',
        backgroundSize: "100% 100%",
      }}
    >
      <div className="basic-match-number">
        {`${mNArr[0]} no.`}
        <AnimatedValueContent
          value={parseInt(mNArr[2])}
          duration={2000}
          player={null}
        />
        {/* Warm Up - Day 4 */}
      </div>
      <div className="basic-match-series">{`${
        matchSeries?.split("[")[0]
      } - 3rd T20I Match`}</div>
      <div className="match-title" style={{ background: "none" }}>
        <div style={{ background: "none" }}>
          <RotatingCylinder
            images={Array.from(
              { length: 5 },
              () => logos.find((t) => t.teamName === team1)?.logoUrl as string
            )}
            width={250}
            height={250}
            rotationSpeed={8}
            translateZ={170}
          />
          <h1 className="text-3d">{team1}</h1>
        </div>
        <div className="vs-sign text-3d">
          <a
            href={`https://stats.espncricinfo.com${href}`}
            rel="noreferrer"
            target="_blank"
            style={{ textDecoration: "none" }}
          >
            vs
          </a>
        </div>
        <div style={{ background: "none" }}>
          <RotatingCylinder
            images={Array.from(
              { length: 5 },
              () => logos.find((t) => t.teamName === team2)?.logoUrl as string
            )}
            width={250}
            height={250}
            rotationSpeed={8}
            translateZ={170}
          />
          <h1 className="text-3d">{team2}</h1>
        </div>
      </div>
      <div className="basic-match-date">
        {matchDate && (
          <GradualText
            id={matchNumber.split(" ")[2]}
            duration={6000}
            text={matchDate[0] + " " + matchDate.slice(1)}
          />
        )}
      </div>
      <div className="basic-match-venue">
        {matchVenue && (
          <GradualText
            id={matchNumber.split(" ")[2]}
            duration={6000}
            text={`${matchVenue[0] + " " + matchVenue.slice(1)} (Nepal)`}
          />
        )}
      </div>
    </div>
  );
};
