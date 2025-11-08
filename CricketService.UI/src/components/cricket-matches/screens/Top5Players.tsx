import React, { useEffect, useState } from "react";
import { useFetchTopPlayers } from "../../../hooks/espn-cricinfo-hooks/useFetchTopPlayers";
import { PlayerImage } from "./elements/PlayerImage";
import teamLogos from "./../../../data/StaticData/teamLogos.json";
import { AnimatedNumber } from "../../common/AnimatedNumber";
import { speakText } from "../../common/SpeakText";
import $ from "jquery";

import "./Top5Players.scss";

interface Top5PlayersProps {
  tournamentId: string;
}

export const Top5Players: React.FC<Top5PlayersProps> = ({ tournamentId }) => {
  const { runScorers, wicketTakers, mostSixes, mostCatches } =
    useFetchTopPlayers(tournamentId);
  const [selectedSubScreenIndex, setSubScreenIndex] = useState(0);

  const getNameFromHref = (href: string) => {
    const nameStr = href?.split("/")[2];
    const nameArr = nameStr?.split("-");
    nameArr?.pop();
    return nameArr?.length > 0
      ? nameArr
          .map((x, i, a) =>
            i !== a?.length - 1 && a.join(" ")?.length > 13 ? `${x[0]}.` : x
          )
          ?.join(" ")
          .toUpperCase()
      : "";
  };

  $(document).on({
    keydown: (event) => {
      if (event.originalEvent?.key === "n") {
        setSubScreenIndex((selectedSubScreenIndex + 1) % 2);
        event.preventDefault();
      }
    },
  });

  useEffect(() => {
    if (runScorers && wicketTakers && selectedSubScreenIndex === 0) {
      speakText(
        `Caribbean premier league 2023 season's, Top Run Scorer: ${getNameFromHref(
          runScorers[0]?.player.href
        )}, ${runScorers[0]?.runs} Runs`
      );
      speakText(
        `Top Wicket Taker: ${getNameFromHref(wicketTakers[0]?.player.href)}, ${
          wicketTakers[0]?.wickets
        } Wicket`
      );
    }
    if (mostSixes && mostCatches && selectedSubScreenIndex === 1) {
      speakText(
        `Top Six Hitter: ${getNameFromHref(mostSixes[0]?.player.href)}, ${
          mostSixes[0]?.sixes
        } Sixes`
      );
      speakText(
        `Top Catch Taker: ${getNameFromHref(mostCatches[0]?.player.href)}, ${
          mostCatches[0]?.catches
        } Catches`
      );
    }
  }, [selectedSubScreenIndex]);

  return (
    <>
      {selectedSubScreenIndex === 0 && (
        <div className="top-5-container">
          <h3 className="top-5-header">Top 5 Run Scorer</h3>
          <div className="top-5-row">
            {runScorers?.slice(0, 5).map((p) => (
              <Top5PlayerCard
                href={p.player.href}
                name={getNameFromHref(p.player.href)?.replace(
                  "S. SAMARAWICKRAMA",
                  "SAMARAWICKRAMA"
                )}
                matches={p.matches}
                stat={p.runs}
                teamShortName={p.player.name?.split("(")[1]?.replace(")", "")}
              />
            ))}
          </div>
          <h3 className="top-5-header">Top 5 Wicket Takers</h3>
          <div className="top-5-row">
            {wicketTakers?.slice(0, 5).map((p) => (
              <Top5PlayerCard
                href={p.player.href}
                name={getNameFromHref(p.player.href)}
                matches={p.matches}
                stat={p.wickets}
                teamShortName={p.player.name?.split("(")[1]?.replace(")", "")}
              />
            ))}
          </div>
        </div>
      )}
      {selectedSubScreenIndex === 1 && (
        <div className="top-5-container">
          <h3 className="top-5-header">Top 5 Six Hitters</h3>
          <div className="top-5-row">
            {mostSixes?.slice(0, 5).map((p) => (
              <Top5PlayerCard
                href={p.player.href}
                name={getNameFromHref(p.player.href)}
                matches={p.matches}
                stat={p.sixes}
                teamShortName={p.player.name?.split("(")[1]?.replace(")", "")}
              />
            ))}
          </div>
          <h3 className="top-5-header">Top 5 Most Catches</h3>
          <div className="top-5-row">
            {mostCatches?.slice(0, 5).map((p) => (
              <Top5PlayerCard
                href={p.player.href}
                name={getNameFromHref(p.player.href)}
                matches={p.matches}
                stat={p.catches}
                teamShortName={p.player.name?.split("(")[1]?.replace(")", "")}
              />
            ))}
          </div>
        </div>
      )}
    </>
  );
};

interface Top5PlayerCardProps {
  href: string;
  name: string;
  matches: number;
  stat: number;
  teamShortName: string;
}

export const Top5PlayerCard: React.FC<Top5PlayerCardProps> = ({
  href,
  name,
  matches,
  stat,
  teamShortName,
}) => {
  return (
    <div className="top-player-card">
      <PlayerImage
        href={href}
        alt={href}
        playerInfos={[]}
        height={330}
        width={200}
        teamName={
          teamLogos.find((tl) => tl.shortName === teamShortName)?.teamName
        }
      />
      <AnimatedNumber value={stat} duration={3000} className="stat-number" />
      <p className="name-header">
        <a
          href={`https://www.espncricinfo.com/${href}`}
          style={{ color: "white", textDecoration: "none" }}
        >
          {name}
        </a>
      </p>
      <img
        className="team-logo"
        src={teamLogos.find((tl) => tl.shortName === teamShortName)?.logoUrl}
        alt={href}
        height={150}
        width={150}
      />
    </div>
  );
};
