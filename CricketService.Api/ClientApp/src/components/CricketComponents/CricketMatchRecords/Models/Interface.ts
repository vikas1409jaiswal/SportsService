export interface CricketMatch {
    matchUuid: string,
    season: number,
    series: string,
    playerOfTheMatch: string,
    matchNo: string,
    matchDays: string,
    matchTitle: string,
    venue: string,
    matchDate: string,
    tossWinner: string,
    tossDecision: string,
    result: string,
    team1: TeamScoreBoardDetails,
    team2: TeamScoreBoardDetails,
    tvUmpire: string,
    matchReferee: string,
    reserveUmpire: string,
    umpires: string[],
    internationalDebut: string[],
    formatDebut: string[]
}

export interface TeamScoreBoardDetails {
    teamName: string,
    battingScoreCard: BattingScoreCard[],
    bowlingScoreCard: BowlingScoreCard[],
    fallOfWickets: string[],
    extras: string,
    totalInningDetails: TotalInningDetails,
    didNotBat: string[],
    playing11: string[]
}

interface BattingScoreCard {
    playerName: string,
    outStatus: string,
    runsScored: number,
    ballsFaced: number,
    minutes: number,
    fours: number,
    sixes: number,
    strikeRate: number
}

interface BowlingScoreCard {
    playerName: string,
    oversBowled: number,
    maidens: number,
    runsConceded: number,
    wickets: number,
    economy: number,
    dots: number,
    fours: number,
    sixes: number,
    wideBall: number,
    noBall: number
}

interface TotalInningDetails {
    runs: number,
    wickets: number,
    overs: {
        overs: number,
        balls: number,
    },
    extras: {
        extraDetails: {
            w: number,
            nb: number,
            wb: number
        },
        totalExtras: number
    }
}

export enum CricketFormat {
    ODI = 'One-day International',
    T20I = 'T20 International',
    Test = 'Test Cricket'
}
