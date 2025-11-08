import React, { useContext, useState } from "react";
import { CricketProfileContext } from "../CricketPlayersProfile";
import { PlayerDetails } from "./PlayerInfo/PlayerDetails";
import { useCustomPlayerInfo } from "../../CricketComponents/CricketPlayerInfoFetch/useCustomPlayerInfo";
import { usePlayerInfo } from "../../CricketComponents/CricketPlayerInfoFetch/usePlayerInfo";

export interface SelectedPlayerIndex {
  selectedFW: number;
  selectedMF: number;
  selectedDF: number;
  selectedGK: number;
}

export interface CricketBodyProps {}

export const CricketBody: React.FunctionComponent<CricketBodyProps> = (
  props
) => {
  const {
    showAllPlayers,
    countries,
    playersForShow: playersByTeam,
  } = useContext(CricketProfileContext);

  const [selectedPlayerIndex, setSelectedPlayerIndex] = useState(0);

  // const players = usePlayerInfo(
  //   (playersByTeam || []).map((x) => [x.href]),
  //   true
  // );

  const players = useCustomPlayerInfo(
    (playersByTeam || []).map((x) => [x.href]),
    true
  );

  const obj: { href: string; profilePics: string[] }[] = [];

  players.forEach((x) =>
    obj.push({
      href: x.playerUuid,
      profilePics: [
        "https://t4.ftcdn.net/jpg/02/23/50/73/360_F_223507349_F5RFU3kL6eMt5LijOaMbWLeHUTv165CB.jpg",
      ],
    })
  );

  console.log(obj);

  return (
    <PlayerDetails
      player={players[selectedPlayerIndex]}
      selectedPlayerIndex={selectedPlayerIndex}
      setSelectedPlayerIndex={setSelectedPlayerIndex}
      imageUrl={playersByTeam[selectedPlayerIndex]?.imageUrl}
    />
  );
};
