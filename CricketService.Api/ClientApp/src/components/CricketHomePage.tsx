import React, { useState, createContext, useEffect } from "react";
import { CricketPlayerInfoFetch } from "./CricketComponents/CricketPlayerInfoFetch/CricketPlayerInfoFetch";
import {
  CricketMatch,
  useCricketMatch,
} from "./CricketComponents/CricketMatchesInfoFetch/useCricketMatches";
import { playersData } from "./../components/CricketComponents/CricketPlayerInfoFetch/playersData";

import "./CricketHomePage.css";
import {
  CricketMatchesInfoFetch,
  getRangeYearsArray,
} from "./CricketComponents/CricketMatchesInfoFetch/CricketMatchesInfoFetch";
import Chart from "react-google-charts";

export interface CricketHomePageProps {}

export enum CricketFormats {
  TestCricket = "Test Cricket",
  ODICricket = "One-day International",
  T20ICricket = "T20 International",
}

interface CricketContextValue {
  currentMatchDetails: CricketMatch;
  setCurrentMatchDetails: (match: CricketMatch) => void;
}

export enum PlayerPosition {
  none = "None",
  batter = "Batter",
  bowler = "Bowler",
  wicketKeeper = "WicketKeeper",
  allRounder = "AllRounder",
}

const initialCricketContextValue: CricketContextValue = {
  currentMatchDetails: {
    matchUuid: "",
    season: "",
    series: "",
    playerOfTheMatch: "",
    matchNo: "",
    matchDays: "",
    matchTitle: "",
    venue: "",
    matchDate: "",
    tossWinner: "",
    tossDecision: "",
    result: "",
    team1: {
      teamName: "",
      battingScorecard: [],
      bowlingScorecard: [],
      extras: "",
      fallOfWickets: [],
      didNotBat: [],
    },
    team2: {
      teamName: "",
      battingScorecard: [],
      bowlingScorecard: [],
      extras: "",
      fallOfWickets: [],
      didNotBat: [],
    },
    tvUmpire: "",
    matchReferee: "",
    reserveUmpire: "",
    umpires: [],
    formatDebut: [],
    internationalDebut: [],
  },
  setCurrentMatchDetails: () => {},
};
export const CricketContext = createContext<CricketContextValue>(
  initialCricketContextValue
);

export const CricketHomePage: React.FunctionComponent<CricketHomePageProps> = (
  props
) => {
  const [currentMatchDetails, setCurrentMatchDetails] = useState<CricketMatch>(
    initialCricketContextValue.currentMatchDetails
  );
  const [totalFetchPlayers, setTotalFetchPlayers] = useState<number>(10);

  const years = getRangeYearsArray(1877, 2023).map((x) => parseInt(x));

  return (
    <>
      <CricketContext.Provider
        value={{
          currentMatchDetails,
          setCurrentMatchDetails,
        }}
      >
        <div className="cricket-home-page">
          {/*<CricketMatchesInfoFetch years={years} />)*/}
          {/*<CricketPlayerInfoFetch*/}
          {/*  totalFetchPlayers={totalFetchPlayers}*/}
          {/*  setTotalFetchPlayers={setTotalFetchPlayers}*/}
          {/*  players={playersData*/}
          {/*    .slice(500)*/}
          {/*    //.slice(0, totalFetchPlayers)*/}
          {/*    .map((x) => [x.uuid, x.href])}*/}
          {/*/>*/}
        </div>
      </CricketContext.Provider>
    </>
  );
};
