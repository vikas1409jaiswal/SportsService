import React from "react";
import { useICCRankings } from "../../hooks/icc-rankings-hooks/useICCRankings";
import RotatingCircle from "../cricket-records/common/RotatingCircle";
import { useAllPlayersUuids } from "../hooks/useAllPlayersUuids";
import { CountryFlag } from "../cricket-matches/screens/elements/CountryFlag";
import { FaThumbsUp, FaShareAlt } from "react-icons/fa";
import { PlayerImage } from "../cricket-matches/screens/elements/PlayerImage";

import "./../CommonCss.scss";
import "./ICCRankings.scss";
import { CricketFormat } from "../../models/enums/CricketFormat";

interface ICCRankingsProps {}

export const ICCRankings: React.FC<ICCRankingsProps> = ({}) => {
  const { isLoading, playerRankings } = useICCRankings(
    2005,
    CricketFormat.Test
  );

  const { playerInfos } = useAllPlayersUuids();

  const obj: { href: string; profilePics: string[] }[] = [];

  !isLoading &&
    playerRankings.battingRanking.forEach((n) =>
      obj.push({
        href: playerInfos?.find((x) => x.playerName === n.playerName)
          ?.href as string,
        profilePics: [""],
      })
    );

  !isLoading && console.log(obj.slice(0, 25));

  return (
    <>
      <div className="icc-players">
        {isLoading ? (
          <div>..............</div>
        ) : (
          <div className="icc-players-list slide-from-right">
            {playerRankings.battingRanking.slice(0, 25).map((n, i) => (
              <div className="icc-player-card">
                <PlayerImage
                  alt={n.playerName}
                  href={
                    playerInfos?.find((x) => x.playerName === n.playerName)
                      ?.href as string
                  }
                  playerInfos={[]}
                  width={400}
                  height={500}
                />
                <h5>{n.playerName.toUpperCase()}</h5>
                <CountryFlag
                  countryCode={n.teamName}
                  height={100}
                  width={130}
                />
                <h6>{n.teamName}</h6>
                <h2>{n.rating}</h2>
                <h6>
                  <b>Career Best: </b>
                  {n.careerBestRanking}
                </h6>
                <RotatingCircle number={n.rank} />
              </div>
            ))}
          </div>
        )}
      </div>
      <h4 style={{ textAlign: "center" }}>
        Like <FaThumbsUp color="blue" />, Share <FaShareAlt color="green" />,
        and Subscribe{" "}
        <img
          alt="subs-icon"
          src="https://cdn.pixabay.com/photo/2020/07/15/21/04/subscribe-5408999_1280.png"
          height={50}
          width={100}
        />
        <span> </span>to Data plus Animation to stay connected.
      </h4>
    </>
  );
};
