export interface PlayerData {
    playerUuid: string,
    fullName: string,
    dateOfBirth: string,
    teamName: string,
    birthPlace: string,
    careerDetails: CareerDetails,
    imageSrc: string
}

interface CareerDetails {
    t20Career: CareerInfo | null,
    odiCareer: CareerInfo | null,
    testCareer: CareerInfo | null
}

interface CareerInfo {
    debutDetails: DebutDetails | null,
    battingStatistics: BattingStatistics | null,
    bowlingStatistics: BowlingStatistics | null
}

interface DebutDetails {
    matchUuid: string,
    date: string,
    opponent: string,
    venue: string,
    result: string,
    matchNo: string
}

interface BattingStatistics {
    matches: number,
    innings: number,
    runs: number,
    ballsFaced: number,
    centuries: number,
    halfCenturies: number,
    highestScore: number,
    fours: number,
    sixes: number,
    strikeRate: number
}

interface BowlingStatistics {
    matches: number,
    innings: number,
    wickets: number,
    overs: Overs,
    runsConceded: number,
    noBall: number,
    wideBall: number,
    maidens: number,
    dots: number,
    sixes: number,
    fours: number,
    economy: number
}

interface Overs {
    overs: number,
    balls: number
}