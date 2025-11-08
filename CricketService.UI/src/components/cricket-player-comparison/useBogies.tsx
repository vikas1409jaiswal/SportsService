import { ESPNPlayerInfo } from "../../hooks/espn-cricinfo-hooks/usePlayerInfo";
import { ArrowDataComparer } from "./common/ArrowDataComparer";
import { ESPNPlayerAllMatchesInfo } from "./hook/useFetchPlayerAllMatches";

type FeedData = {
  dataName: string;
  data1Text: string;
  data2Text: string;
  speechText: string;
};

export const useProfileInfoJSX = (
  player1Data: ESPNPlayerInfo,
  player2Data: ESPNPlayerInfo
): JSX.Element[] => {
  const profileInfoBogies = [
    <ArrowDataComparer
      className="data age-container"
      dataName="Age"
      data1Text={player1Data.age}
      data2Text={player2Data.age}
      speechText={`${player1Data.name}, Age, ${
        player1Data.age?.split("y")[0]
      } years, ${player2Data.name}, Age ${
        player2Data.age?.split("y")[0]
      } years`}
    />,
    <ArrowDataComparer
      className="data batting-style-container"
      dataName="Batting Style"
      data1Text={player1Data.battingStyle}
      data2Text={player2Data.battingStyle}
      speechText={`Batting Style: ${player1Data.name}, ${player1Data.battingStyle}, ${player2Data.name}, ${player2Data.battingStyle}`}
    />,
    <ArrowDataComparer
      className="data bowling-style-container"
      dataName="Bowling Style"
      data1Text={player1Data.bowlingStyle}
      data2Text={player2Data.bowlingStyle}
      speechText={`Bowling Style: ${player1Data.name}, ${player1Data.bowlingStyle}, ${player2Data.name}, ${player2Data.bowlingStyle}`}
    />,
    <ArrowDataComparer
      className="data playing-role-container"
      dataName="Playing Role"
      data1Text={player1Data.playingRole}
      data2Text={player2Data.playingRole}
      speechText={`Playing Role: ${player1Data.name}, ${player1Data.playingRole}, ${player2Data.name}, ${player2Data.playingRole}`}
    />,
  ];
  return profileInfoBogies;
};

export const useBattingStatsJSX = (
  player1Name: string,
  player2Name: string,
  player1AddData: ESPNPlayerAllMatchesInfo,
  player2AddData: ESPNPlayerAllMatchesInfo
): JSX.Element[] => {
  const feedBattingStatsData: FeedData[] = [
    {
      dataName: "Matches",
      data1Text: player1AddData.battingCareer.matches,
      data2Text: player2AddData.battingCareer.matches,
      speechText: `let's see batting stats of both players. Matches: ${player1Name}, ${player1AddData.battingCareer.matches} matches, ${player2Name}, ${player2AddData.battingCareer.matches} matches`,
    },
    {
      dataName: "Runs",
      data1Text: player1AddData.battingCareer.runs,
      data2Text: player2AddData.battingCareer.runs,
      speechText: `Runs: ${player1Name}, ${player1AddData.battingCareer.runs} runs, ${player2Name}, ${player2AddData.battingCareer.runs} runs`,
    },
    {
      dataName: "Hundreds",
      data1Text: player1AddData.battingCareer.hundreds,
      data2Text: player2AddData.battingCareer.hundreds,
      speechText: `Hundreds: ${player1Name}, ${player1AddData.battingCareer.hundreds} hundreds, ${player2Name}, ${player2AddData.battingCareer.hundreds} hundreds`,
    },
    {
      dataName: "Fifties",
      data1Text: player1AddData.battingCareer.fifties,
      data2Text: player2AddData.battingCareer.fifties,
      speechText: `Fifties: ${player1Name}, ${player1AddData.battingCareer.fifties} fifties, ${player2Name}, ${player2AddData.battingCareer.fifties} fifties`,
    },
    {
      dataName: "Not Outs",
      data1Text: player1AddData.battingCareer.notOuts,
      data2Text: player2AddData.battingCareer.notOuts,
      speechText: `Not outs: ${player1Name}, ${player1AddData.battingCareer.notOuts}, ${player2Name}, ${player2AddData.battingCareer.notOuts}`,
    },
    {
      dataName: "Ducks",
      data1Text: player1AddData.battingCareer.ducks,
      data2Text: player2AddData.battingCareer.ducks,
      speechText: `Ducks: ${player1Name}, ${player1AddData.battingCareer.ducks}, ${player2Name}, ${player2AddData.battingCareer.ducks}`,
    },
    {
      dataName: "Fours",
      data1Text: player1AddData.battingCareer.fours,
      data2Text: player2AddData.battingCareer.fours,
      speechText: `Fours: ${player1Name}, ${player1AddData.battingCareer.fours} fours, ${player2Name}, ${player2AddData.battingCareer.fours} fours`,
    },
    {
      dataName: "Sixes",
      data1Text: player1AddData.battingCareer.sixes,
      data2Text: player2AddData.battingCareer.sixes,
      speechText: `Sixes: ${player1Name}, ${player1AddData.battingCareer.sixes} sixes, ${player2Name}, ${player2AddData.battingCareer.sixes} sixes`,
    },
    {
      dataName: "Average",
      data1Text: player1AddData.battingCareer.average,
      data2Text: player2AddData.battingCareer.average,
      speechText: `Average: ${player1Name}, ${player1AddData.battingCareer.average}, ${player2Name}, ${player2AddData.battingCareer.average}`,
    },
    {
      dataName: "Strike Rate",
      data1Text: player1AddData.battingCareer.strikeRate,
      data2Text: player2AddData.battingCareer.strikeRate,
      speechText: `Strike Rate: ${player1Name}, ${player1AddData.battingCareer.strikeRate}, ${player2Name}, ${player2AddData.battingCareer.strikeRate}`,
    },
  ];

  const battingStatsBogies = [
    <div className="batting-stats-header">Batting Statistics</div>,
  ];

  for (let i = 0; i < feedBattingStatsData.length; i = i + 2) {
    battingStatsBogies.push(
      <div className="batting-stats">
        <ArrowDataComparer
          className="stats-data"
          dataName={feedBattingStatsData[i].dataName}
          data1Text={feedBattingStatsData[i].data1Text}
          data2Text={feedBattingStatsData[i].data2Text}
          headWidth={75}
          tailWidth={215}
          speechText={feedBattingStatsData[i].speechText}
        />
        {feedBattingStatsData.length > i + 1 && (
          <ArrowDataComparer
            className="stats-data"
            dataName={feedBattingStatsData[i + 1].dataName}
            data1Text={feedBattingStatsData[i + 1].data1Text}
            data2Text={feedBattingStatsData[i + 1].data2Text}
            headWidth={75}
            tailWidth={215}
            speechText={feedBattingStatsData[i + 1].speechText}
          />
        )}
      </div>
    );
  }
  return battingStatsBogies;
};

export const useBowlingStatsJSX = (
  player1Name: string,
  player2Name: string,
  player1AddData: ESPNPlayerAllMatchesInfo,
  player2AddData: ESPNPlayerAllMatchesInfo
): JSX.Element[] => {
  const feedBowlingStatsData: FeedData[] = [
    {
      dataName: "Matches",
      data1Text: player1AddData.bowlingCareer.matches,
      data2Text: player2AddData.bowlingCareer.matches,
      speechText: `let's see bowling stats of both players. Matches: ${player1Name}, ${player1AddData.bowlingCareer.matches} matches, ${player2Name}, ${player2AddData.bowlingCareer.matches} matches`,
    },
    {
      dataName: "Wickets",
      data1Text: player1AddData.bowlingCareer.wickets,
      data2Text: player2AddData.bowlingCareer.wickets,
      speechText: `Wickets: ${player1Name}, ${player1AddData.bowlingCareer.wickets} wickets, ${player2Name}, ${player2AddData.bowlingCareer.wickets} wickets`,
    },
    {
      dataName: "4 Wickets",
      data1Text: player1AddData.bowlingCareer.fourWickets,
      data2Text: player2AddData.bowlingCareer.fourWickets,
      speechText: `4 Wicket Haul: ${player1Name}, ${player1AddData.bowlingCareer.fourWickets}, ${player2Name}, ${player2AddData.bowlingCareer.fourWickets}`,
    },
    {
      dataName: "5 Wickets",
      data1Text: player1AddData.bowlingCareer.fiveWickets,
      data2Text: player2AddData.bowlingCareer.fiveWickets,
      speechText: `5 Wicket Haul: ${player1Name}, ${player1AddData.bowlingCareer.fiveWickets}, ${player2Name}, ${player2AddData.bowlingCareer.fiveWickets}`,
    },
  ];

  const bowlingStatsBogies = [
    <div className="bowling-stats-header">Batting Statistics</div>,
  ];

  for (let i = 0; i < feedBowlingStatsData.length; i = i + 2) {
    bowlingStatsBogies.push(
      <div className="bowling-stats">
        <ArrowDataComparer
          className="stats-data"
          dataName={feedBowlingStatsData[i].dataName}
          data1Text={feedBowlingStatsData[i].data1Text}
          data2Text={feedBowlingStatsData[i].data2Text}
          headWidth={75}
          tailWidth={215}
          speechText={feedBowlingStatsData[i].speechText}
        />
        {feedBowlingStatsData.length > i + 1 && (
          <ArrowDataComparer
            className="stats-data"
            dataName={feedBowlingStatsData[i + 1].dataName}
            data1Text={feedBowlingStatsData[i + 1].data1Text}
            data2Text={feedBowlingStatsData[i + 1].data2Text}
            headWidth={75}
            tailWidth={215}
            speechText={feedBowlingStatsData[i + 1].speechText}
          />
        )}
      </div>
    );
  }
  return bowlingStatsBogies;
};
