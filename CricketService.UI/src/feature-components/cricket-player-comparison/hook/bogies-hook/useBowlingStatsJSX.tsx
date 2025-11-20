import { ArrowDataComparer } from "../../common/ArrowDataComparer";
import { ESPNPlayerAllMatchesInfo } from "../useFetchPlayerAllMatchesV2";

type FeedData = {
  dataName: string;
  data1Text: string;
  data2Text: string;
  speechText: string;
};

const useBowlingStatsJSX = (
  player1Name: string,
  player2Name: string,
  player1AddData: ESPNPlayerAllMatchesInfo,
  player2AddData: ESPNPlayerAllMatchesInfo
): JSX.Element[] => {
  const feedBowlingStatsData: FeedData[] = [
    {
      dataName: "Matches",
      data1Text: String(player1AddData.bowlingCareer.matches),
      data2Text: String(player2AddData.bowlingCareer.matches),
      speechText: `let's see bowling stats of both players. Matches: ${player1Name}, ${player1AddData.bowlingCareer.matches} matches, ${player2Name}, ${player2AddData.bowlingCareer.matches} matches`,
    },
    {
      dataName: "Wickets",
      data1Text: String(player1AddData.bowlingCareer.wickets),
      data2Text: String(player2AddData.bowlingCareer.wickets),
      speechText: `Wickets: ${player1Name}, ${player1AddData.bowlingCareer.wickets} wickets, ${player2Name}, ${player2AddData.bowlingCareer.wickets} wickets`,
    },
    {
      dataName: "4 Wickets",
      data1Text: String(player1AddData.bowlingCareer.fourWickets),
      data2Text: String(player2AddData.bowlingCareer.fourWickets),
      speechText: `4 Wicket Haul: ${player1Name}, ${player1AddData.bowlingCareer.fourWickets}, ${player2Name}, ${player2AddData.bowlingCareer.fourWickets}`,
    },
    {
      dataName: "5 Wickets",
      data1Text: String(player1AddData.bowlingCareer.fiveWickets),
      data2Text: String(player2AddData.bowlingCareer.fiveWickets),
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

export default useBowlingStatsJSX;
