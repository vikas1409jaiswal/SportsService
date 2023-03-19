﻿export interface CricketMatch {
  matchUuid: string;
  season: number;
  series: string;
  playerOfTheMatch: string;
  matchNo: string;
  matchDays: string;
  matchTitle: string;
  venue: string;
  matchDate: string;
  tossWinner: string;
  tossDecision: string;
  result: string;
  team1: TeamScoreBoard;
  team2: TeamScoreBoard;
  tvUmpire: string;
  matchReferee: string;
  reserveUmpire: string;
  umpires: string[];
  internationalDebut: string[];
  formatDebut: string[];
}

export interface TestCricketMatch {
  matchUuid: string;
  season: number;
  series: string;
  playerOfTheMatch: string;
  matchNo: string;
  matchDays: string;
  matchTitle: string;
  venue: string;
  matchDate: string;
  tossWinner: string;
  tossDecision: string;
  result: string;
  team1: TestTeamScoreBoard;
  team2: TestTeamScoreBoard;
  tvUmpire: string;
  matchReferee: string;
  reserveUmpire: string;
  umpires: string[];
  internationalDebut: string[];
  formatDebut: string[];
}

export interface TestTeamScoreBoard {
  teamName: string;
  inning1: InningScoreBoard;
  inning2: InningScoreBoard;
}

export interface InningScoreBoard {
  battingScoreCard: BattingScoreCard[];
  bowlingScoreCard: BowlingScoreCard[];
  fallOfWickets: string[];
  extras: string;
  totalInningDetails: TotalInningDetails;
  didNotBat: Player[];
  playing11: Player[];
}

export interface TeamScoreBoard {
  teamName: string;
  battingScoreCard: BattingScoreCard[];
  bowlingScoreCard: BowlingScoreCard[];
  fallOfWickets: string[];
  extras: string;
  totalInningDetails: TotalInningDetails;
  didNotBat: Player[];
  playing11: Player[];
}

interface Player {
  name: string;
  href: string;
}

interface BattingScoreCard {
  playerName: Player;
  outStatus: string;
  runsScored: number;
  ballsFaced: number;
  minutes: number;
  fours: number;
  sixes: number;
  strikeRate: number;
}

interface BowlingScoreCard {
  playerName: Player;
  oversBowled: number;
  maidens: number;
  runsConceded: number;
  wickets: number;
  economy: number;
  dots: number;
  fours: number;
  sixes: number;
  wideBall: number;
  noBall: number;
}

interface TotalInningDetails {
  runs: number;
  wickets: number;
  overs: {
    overs: number;
    balls: number;
  };
  extras: {
    extraDetails: {
      w: number;
      nb: number;
      wb: number;
    };
    totalExtras: number;
  };
}

export enum CricketFormat {
  ODI = "One-day International",
  T20I = "T20 International",
  Test = "Test Cricket",
}

//For Team Details

export interface CricketTeam {
  uuid: string;
  teamName: string;
  flagUrl: string;
}
