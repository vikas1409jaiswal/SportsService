import React, { useEffect, useState } from "react";
import {
  ICCPlayerInfo,
  ICCRanking,
  useICCRankings,
} from "../../hooks/icc-rankings-hooks/useICCRankings";
import { CricketFormat } from "../../models/enums/CricketFormat";

interface PlayersRaceBarProps {
  players: ICCPlayerInfo[];
}

export const PlayersRaceBar: React.FC<PlayersRaceBarProps> = ({ players }) => {
  const [currentYear, setCurrentYear] = useState(1950);

  const { isLoading, playerRankings } = useICCRankings(2007, CricketFormat.ODI);

  console.log(playerRankings);

  return (
    <>
      {!isLoading && (
        <PlayersRaceInfo
          currentYear={currentYear}
          setCurrentYear={setCurrentYear}
          playerRankings={playerRankings as ICCRanking}
        />
      )}
    </>
  );
};

interface PlayersRaceInfoProps {
  currentYear: number;
  setCurrentYear: (y: number) => void;
  playerRankings: ICCRanking;
}

const PlayersRaceInfo: React.FC<PlayersRaceInfoProps> = ({
  currentYear,
  setCurrentYear,
  playerRankings,
}) => {
  const [currentPlayers, setCurrentPlayers] = useState<ICCPlayerInfo[]>(
    playerRankings.battingRanking.slice(0, 5)
  );

  // Find the minimum and maximum rankings
  const minRank = Math.min(...currentPlayers.map((player) => player.rank));
  const maxRank = Math.max(...currentPlayers.map((player) => player.rank));

  // Calculate the range of the race bar
  const range = maxRank - minRank + 1;

  useEffect(() => {
    // // Update the players for the current year
    // const updatedPlayers = currentPlayers.filter(
    //   (player) => player.rank <= currentYear
    // );
    // setCurrentPlayers(updatedPlayers);

    // Advance to the next year
    const timer = setTimeout(() => {
      setCurrentYear(currentYear + 1);
    }, 1000);

    // Clean up the timer
    return () => clearTimeout(timer);
  }, []);

  return (
    <div>
      {currentPlayers.map((player) => {
        const position = ((player.rank - minRank) / range) * 100;

        return (
          <div key={player.rank}>
            <div>
              <span>{player.playerName}</span>
              <span>{player.teamName}</span>
            </div>
            <div>
              <div
                style={{
                  width: `${position}%`,
                  background: "blue",
                  height: "10px",
                }}
              />
            </div>
          </div>
        );
      })}
    </div>
  );
};
