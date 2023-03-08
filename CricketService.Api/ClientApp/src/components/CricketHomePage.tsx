import React, { useState, createContext, useEffect } from "react";
import { CricketPlayerInfoFetch } from "./CricketComponents/CricketPlayerInfoFetch/CricketPlayerInfoFetch";
import { teamData } from "./../data/TeamData";
import {
  CricketMatch,
  useCricketMatches,
} from "./CricketHooks/useCricketMatches";
import { playersData } from "./../components/CricketComponents/CricketPlayerInfoFetch/playersData";

import "./CricketHomePage.css";

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
    const [totalFetchPlayers, setTotalFetchPlayers] = useState<number>(
        10
    );

  const countriesList = teamData.filter((td) => td.cricketnationalteam);

  //const years = [1971, 1972, 1973, 1974, 1975, 1976, 1977, 1978, 1979, 1980];
  //const years = [1981, 1982, 1983, 1984, 1985, 1986, 1987, 1988, 1989, 1990, 1991, 1992, 1993, 1994, 1995];
  //const years = [1996, 1997, 1998, 1999, 2000, 2001, 2002, 2003, 2004];
  //const years = [2005, 2006, 2007, 2008, 2009, 2010, 2011, 2012, 2013, 2014, 2015, 2016];
  //const years = [2017, 2018, 2019, 2020, 2021, 2022, 2023];

  return (
    <>
      <CricketContext.Provider
        value={{
          currentMatchDetails,
          setCurrentMatchDetails,
        }}
      >
        <div className="cricket-home-page">
          {/* {years.map((y: any) => <CricketPlayerInfoFetch year={y} />)}*/}
                  <CricketPlayerInfoFetch
                      totalFetchPlayers={totalFetchPlayers}
                      setTotalFetchPlayers={setTotalFetchPlayers}
            players={playersData.slice(0, totalFetchPlayers).map((x) => [x.uuid, x.playerUrl])}
          />
        </div>
      </CricketContext.Provider>
    </>
  );
};
