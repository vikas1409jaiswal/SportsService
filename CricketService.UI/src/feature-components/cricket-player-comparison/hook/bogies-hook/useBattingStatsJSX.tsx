import { ArrowDataComparer } from "../../common/ArrowDataComparer";
import { BattingStatistics } from "../../../../components/CricketComponents/CricketPlayerInfoFetch/useCustomPlayerInfo";

type FeedData = {
  dataName: string;
  data1Text: string;
  data2Text: string;
  speechText: string;
};

const useBattingStatsJSX = (
  player1Name: string,
  player2Name: string,
  player1AddData: BattingStatistics,
  player2AddData: BattingStatistics
): JSX.Element[] => {
  const feedBattingStatsData: FeedData[] = [
    {
      dataName: "Matches",
      data1Text: player1AddData?.matches.toString() ?? "",
      data2Text: player2AddData?.matches.toString() ?? "",
      speechText: `let's see batting stats of both players. Matches: ${player1Name}, ${player1AddData?.matches} matches, ${player2Name}, ${player2AddData?.matches} matches`,
    },
    {
      dataName: "Runs",
      data1Text: player1AddData?.runs.toString() ?? "",
      data2Text: player2AddData?.runs.toString() ?? "",
      speechText: `Runs: ${player1Name}, ${player1AddData?.runs} runs, ${player2Name}, ${player2AddData?.runs} runs`,
    },
    {
      dataName: "Hundreds",
      data1Text: player1AddData?.centuries.toString() ?? "",
      data2Text: player2AddData?.centuries.toString() ?? "",
      speechText: `Hundreds: ${player1Name}, ${player1AddData?.centuries} hundreds, ${player2Name}, ${player2AddData?.centuries} hundreds`,
    },
    {
      dataName: "Fifties",
      data1Text: player1AddData?.halfCenturies.toString() ?? "",
      data2Text: player2AddData?.halfCenturies.toString() ?? "",
      speechText: `Fifties: ${player1Name}, ${player1AddData?.halfCenturies} fifties, ${player2Name}, ${player2AddData?.halfCenturies} fifties`,
    },
    {
      dataName: "Not Outs",
      data1Text: player1AddData?.notOut?.toString() ?? "",
      data2Text: player2AddData?.notOut?.toString() ?? "",
      speechText: `Not outs: ${player1Name}, ${player1AddData?.notOut}, ${player2Name}, ${player2AddData?.notOut}`,
    },
    {
      dataName: "Ducks",
      data1Text: player1AddData?.ducks?.toString() ?? "",
      data2Text: player2AddData?.ducks?.toString() ?? "",
      speechText: `Ducks: ${player1Name}, ${player1AddData?.ducks}, ${player2Name}, ${player2AddData?.ducks}`,
    },
    {
      dataName: "Fours",
      data1Text: player1AddData?.fours.toString() ?? "",
      data2Text: player2AddData?.fours.toString() ?? "",
      speechText: `Fours: ${player1Name}, ${player1AddData?.fours} fours, ${player2Name}, ${player2AddData?.fours} fours`,
    },
    {
      dataName: "Sixes",
      data1Text: player1AddData?.sixes.toString() ?? "",
      data2Text: player2AddData?.sixes.toString() ?? "",
      speechText: `Sixes: ${player1Name}, ${player1AddData?.sixes} sixes, ${player2Name}, ${player2AddData?.sixes} sixes`,
    },
    {
      dataName: "Average",
      data1Text: player1AddData?.average?.toString() ?? "",
      data2Text: player2AddData?.average?.toString() ?? "",
      speechText: `Average: ${player1Name}, ${player1AddData?.average}, ${player2Name}, ${player2AddData?.average}`,
    },
    {
      dataName: "Strike Rate",
      data1Text: player1AddData?.strikeRate?.toString() ?? "",
      data2Text: player2AddData?.strikeRate?.toString() ?? "",
      speechText: `Strike Rate: ${player1Name}, ${player1AddData?.strikeRate}, ${player2Name}, ${player2AddData?.strikeRate}`,
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

export default useBattingStatsJSX;
