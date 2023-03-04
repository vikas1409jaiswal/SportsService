export interface CricketTeamData {
    teamUuid: string,
    teamName: string,
    teamRecordDetails: FormatWiseRecords,
    flagUri: string
}


interface FormatWiseRecords {
    t20IResults: RecordDetails,
    odiResults: RecordDetails,
}

interface RecordDetails {
    debut: DebutDetails,
    matches: number,
    won: number,
    lost: number,
    tiedAndWon: number,
    tiedAndLost: number,
    noResult: number,
    winPercentage: number,
    teamMileStones: TeamMileStones
}

interface TeamMileStones {
    careerRuns: number,
    careerCenturies: number,
    careerHalfCenturies: number,
    careerOneAndHalfCenturies: number,
    careerDoubleCenturies: number,
    inningRecordDetails: InningRecordDetails,
    mostRuns: KeyValuePair,
    mostWickets: KeyValuePair,
    mostSixes: KeyValuePair,
    mostFours: KeyValuePair,
    most50s: KeyValuePair,
    most100s: KeyValuePair,
    most150s: KeyValuePair,
    most200s: KeyValuePair,
    highestIndividualScore: KeyValuePair
}

interface KeyValuePair {
    key: string,
    value: number
}

interface DebutDetails {
    matchUuid: string,
    date: string
    opponent: string,
    venue: string,
    result: string,
    matchNo: string
}

interface InningRecordDetails {
    highestTotal: TotalDetails,
    lowestTotal: TotalDetails,
}


interface TotalDetails {
    runs: number,
    wickets: number,
    overs: {
        overs: number,
        balls: number,
    }
}


