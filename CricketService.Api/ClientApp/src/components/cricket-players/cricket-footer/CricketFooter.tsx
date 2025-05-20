import React, { useContext } from "react";

import "./CricketFooter.scss";
import { PlayerPosition } from "../../CricketHomePage";
import { CricketProfileContext } from "../CricketPlayersProfile";

const alphabets = [
  "A",
  "B",
  "C",
  "D",
  "E",
  "F",
  "G",
  "H",
  "I",
  "J",
  "K",
  "L",
  "M",
  "N",
  "O",
  "P",
  "Q",
  "R",
  "S",
  "T",
  "U",
  "V",
  "W",
  "X",
  "Y",
  "Z",
];

export interface CricketFooterProps {}

export const CricketFooter: React.FunctionComponent<CricketFooterProps> = (
  props
) => {
  const { selectedPlayerType, setSelectedPlayerType } = useContext(
    CricketProfileContext
  );

  // const setButtonBackgroundColor = (playerPosition: PlayerPosition) => {
  //   let bColor = "";
  //   if (playerPosition === selectedPlayerPosition) {
  //     bColor = "red";
  //   }
  //   return bColor;
  // };

  return (
    <>
      <div className="cricket-home-page-footer">
        {/* {selectedSeasonForPlayers === "AtoZ" ? (
          <div className="alphabets-container">
            {alphabets.map((a) => (
              <button
                style={
                  selectedAlphabet === a
                    ? { backgroundColor: "red" }
                    : { backgroundColor: "greenyellow" }
                }
              >
                {a}
              </button>
            ))}
          </div>
        ) : ( */}
        <>
          <button
            // style={{
            //   backgroundColor: setButtonBackgroundColor(PlayerPosition.forward),
            // }}
            disabled={selectedPlayerType === PlayerPosition.batter}
            onClick={() => {
              setSelectedPlayerType(PlayerPosition.batter);
            }}
          >
            {"BATSMAN"}
          </button>
          <button
            // style={{
            //   backgroundColor: setButtonBackgroundColor(
            //     PlayerPosition.midFielder
            //   ),
            // }}
            disabled={selectedPlayerType === PlayerPosition.bowler}
            onClick={() => {
              setSelectedPlayerType(PlayerPosition.bowler);
            }}
          >
            {"BOWLER"}
          </button>
          <button
            // style={{
            //   backgroundColor: setButtonBackgroundColor(
            //     PlayerPosition.defender
            //   ),
            // }}
            disabled={selectedPlayerType === PlayerPosition.allRounder}
            onClick={() => {
              setSelectedPlayerType(PlayerPosition.allRounder);
            }}
          >
            {"ALLROUNDER"}
          </button>
          <button
            // style={{
            //   backgroundColor: setButtonBackgroundColor(
            //     PlayerPosition.goalKeeper
            //   ),
            // }}
            disabled={selectedPlayerType === PlayerPosition.wicketKeeper}
            onClick={() => {
              setSelectedPlayerType(PlayerPosition.wicketKeeper);
            }}
          >
            {"WICKETKEEPER"}
          </button>
        </>
        {/* )} */}
      </div>
    </>
  );
};
