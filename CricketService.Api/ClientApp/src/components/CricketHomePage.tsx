import React, { useState, createContext, useEffect } from 'react';
import { CricketHeader } from './CricketComponents/CricketHeader/CricketHeader';
import { teamData } from './../data/TeamData';

import './CricketHomePage.css';
import { CricketMatch, useCricketMatches } from './CricketHooks/useCricketMatches';

export interface CricketHomePageProps {
}

export enum CricketFormats {
    TestCricket = 'Test Cricket',
    ODICricket = 'One-day International',
    T20ICricket = 'T20 International'
}

interface CricketContextValue {
  countryName: string;
  setCountryName: (countryName: string) => void;
  selectedPlayerId: string;
  setSelectedPlayerId: (id: string) => void;
  selectedPlayerPosition: PlayerPosition;
  setSelectedPlayerPosition: (position: PlayerPosition) => void;
  currentMatchDetails: CricketMatch;
  setCurrentMatchDetails: (match: CricketMatch) => void;
}

export enum PlayerPosition {
  none = 'None',
  batter = 'Batter',
  bowler = 'Bowler',
  wicketKeeper = 'WicketKeeper',
  allRounder = 'AllRounder'
}

const initialCricketContextValue: CricketContextValue = {
  countryName: '',
  setCountryName: () => { },
  selectedPlayerId: '',
  setSelectedPlayerId: () => { },
  selectedPlayerPosition: PlayerPosition.none,
  setSelectedPlayerPosition: () => { },
  currentMatchDetails: {
        season: '',
        series: '',
        playerOfTheMatch: '',
        matchNo: '',
        matchDays: '',
        matchTitle: '',
        venue: '',
        matchDate: '',
        tossWinner: '',
        tossDecision: '',
        result: '',
        team1: {
            teamName: '',
            battingScorecard: [],
            bowlingScorecard: [],
            extras: '',
            fallOfWickets: [],
            didNotBat: []
        },
        team2: {
            teamName: '',
            battingScorecard: [],
            bowlingScorecard: [],
            extras: '',
            fallOfWickets: [],
            didNotBat: []
        },
        tvUmpire: '',
        matchReferee: '',
        reserveUmpire: '',
        umpires: [],
        formatDebut: [],
        internationalDebut: []
    },
    setCurrentMatchDetails: () => { }
};
export const CricketContext = createContext<CricketContextValue>(initialCricketContextValue);

export const CricketHomePage: React.FunctionComponent<CricketHomePageProps> = (props) => {

  const [countryName, setCountryName] = useState('India');
  const [selectedPlayerId, setSelectedPlayerId] = useState('');
  const [selectedPlayerPosition, setSelectedPlayerPosition] = useState<PlayerPosition>(PlayerPosition.batter);
  const [currentMatchDetails, setCurrentMatchDetails] = useState<CricketMatch>(initialCricketContextValue.currentMatchDetails);

    const countriesList = teamData.filter(td => td.cricketnationalteam);

    //const years = [1971, 1972, 1973, 1974, 1975, 1976, 1977, 1978, 1979, 1980];
    //const years = [1981, 1982, 1983, 1984, 1985, 1986, 1987, 1988, 1989, 1990, 1991, 1992, 1993, 1994, 1995];
     //const years = [1996, 1997, 1998, 1999, 2000, 2001, 2002, 2003, 2004];
    //const years = [2005, 2006, 2007, 2008, 2009, 2010, 2011, 2012, 2013, 2014, 2015, 2016];
    //const years = [2017, 2018, 2019, 2020, 2021, 2022, 2023];
    const years = [2004];

  return (
    <>
      <CricketContext.Provider value={{
        countryName,
        setCountryName,
        selectedPlayerPosition,
        setSelectedPlayerPosition,
        selectedPlayerId,
        setSelectedPlayerId,
        currentMatchDetails,
        setCurrentMatchDetails
      }}>
              <div className='cricket-home-page'>
                  {years.map((y: any) => <CricketHeader year={y} />)}
          <div className='select-country'>
            <select name="countries" onChange={(e: any) => setCountryName(e.target.value)}>
              {
                countriesList.map(c =>
                  <option value={c.cricketnationalteam}>{c.cricketnationalteam}</option>
                )
              }
            </select>
                  </div>
        </div>
          </CricketContext.Provider>
    </>
  );
};