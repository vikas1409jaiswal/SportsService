import { ArrowDataComparer } from "../../common/ArrowDataComparer";
import { BattingStatistics } from "../../../../components/CricketComponents/CricketPlayerInfoFetch/useCustomPlayerInfo";
import React, { useEffect } from "react";
import { speakText, SpeechLanguage } from "../../../../components/common/SpeakText";
import StaggeredText from "../../../../components/common/StaggeredText";
import { battingComparisonSpeech } from "./speechBattingComparison";

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
  player2AddData: BattingStatistics,
  BattingStatsHeader: React.FC
): JSX.Element[] => {
  const feedBattingStatsData: FeedData[] = [
    {
      dataName: "Matches",
      data1Text: player1AddData?.matches.toString() ?? "",
      data2Text: player2AddData?.matches.toString() ?? "",
      speechText: battingComparisonSpeech["matches-comparison-speech"](
        { playerName: player1Name, value: player1AddData?.matches ?? 0 },
        { playerName: player2Name, value: player2AddData?.matches ?? 0 }
      ),
    },
    {
      dataName: "Runs",
      data1Text: player1AddData?.runs.toString() ?? "",
      data2Text: player2AddData?.runs.toString() ?? "",
      speechText: battingComparisonSpeech["runs-comparison-speech"](
        { playerName: player1Name, value: player1AddData?.runs ?? 0 },
        { playerName: player2Name, value: player2AddData?.runs ?? 0 }
      ),
    },
    {
      dataName: "Hundreds",
      data1Text: player1AddData?.centuries.toString() ?? "",
      data2Text: player2AddData?.centuries.toString() ?? "",
      speechText: battingComparisonSpeech["centuries-comparison-speech"](
        { playerName: player1Name, value: player1AddData?.centuries ?? 0 },
        { playerName: player2Name, value: player2AddData?.centuries ?? 0 }
      ),
    },
    {
      dataName: "Fifties",
      data1Text: player1AddData?.halfCenturies.toString() ?? "",
      data2Text: player2AddData?.halfCenturies.toString() ?? "",
      speechText: battingComparisonSpeech["fifties-comparison-speech"](
        { playerName: player1Name, value: player1AddData?.halfCenturies ?? 0 },
        { playerName: player2Name, value: player2AddData?.halfCenturies ?? 0 }
      ),
    },
    {
      dataName: "Not Outs",
      data1Text: player1AddData?.notOut?.toString() ?? "",
      data2Text: player2AddData?.notOut?.toString() ?? "",
      speechText: battingComparisonSpeech["not-outs-comparison-speech"](
        { playerName: player1Name, value: player1AddData?.notOut ?? 0 },
        { playerName: player2Name, value: player2AddData?.notOut ?? 0 }
      ),
    },
    {
      dataName: "Ducks",
      data1Text: player1AddData?.ducks?.toString() ?? "",
      data2Text: player2AddData?.ducks?.toString() ?? "",
      speechText: battingComparisonSpeech["ducks-comparison-speech"](
        { playerName: player1Name, value: player1AddData?.ducks ?? 0 },
        { playerName: player2Name, value: player2AddData?.ducks ?? 0 }
      ),
    },
    {
      dataName: "Fours",
      data1Text: player1AddData?.fours.toString() ?? "",
      data2Text: player2AddData?.fours.toString() ?? "",
      speechText: battingComparisonSpeech["fours-comparison-speech"](
        { playerName: player1Name, value: player1AddData?.fours ?? 0 },
        { playerName: player2Name, value: player2AddData?.fours ?? 0 }
      ),
    },
    {
      dataName: "Sixes",
      data1Text: player1AddData?.sixes.toString() ?? "",
      data2Text: player2AddData?.sixes.toString() ?? "",
      speechText: battingComparisonSpeech["sixes-comparison-speech"](
        { playerName: player1Name, value: player1AddData?.sixes ?? 0 },
        { playerName: player2Name, value: player2AddData?.sixes ?? 0 }
      ),
    },
    {
      dataName: "Average",
      data1Text: player1AddData?.average?.toString() ?? "",
      data2Text: player2AddData?.average?.toString() ?? "",
      speechText: battingComparisonSpeech["average-comparison-speech"](
        { playerName: player1Name, value: player1AddData?.average ?? 0 },
        { playerName: player2Name, value: player2AddData?.average ?? 0 }
      ),
    },
    {
      dataName: "Strike Rate",
      data1Text: player1AddData?.strikeRate?.toString() ?? "",
      data2Text: player2AddData?.strikeRate?.toString() ?? "",
      speechText: battingComparisonSpeech["strike-rate-comparison-speech"](
        { playerName: player1Name, value: player1AddData?.strikeRate ?? 0 },
        { playerName: player2Name, value: player2AddData?.strikeRate ?? 0 }
      ),
    },
  ];

  const battingStatsBogies = [
    <BattingStatsHeader key="batting-header" />,
  ];

  for (let i = 0; i < feedBattingStatsData.length; i++) {
    battingStatsBogies.push(
      <div className="batting-stats">
        <ArrowDataComparer
          className="stats-data"
          dataName={feedBattingStatsData[i].dataName}
          data1Text={feedBattingStatsData[i].data1Text}
          data2Text={feedBattingStatsData[i].data2Text}
          headWidth={90}
          tailWidth={300}
          speechText={feedBattingStatsData[i].speechText}
        />
      </div>
    );
  }
  return battingStatsBogies;
};

interface BattingStatsHeaderProps {
  selectedFormat: string;
}

export const BattingStatsHeader: React.FC<BattingStatsHeaderProps> = ({ selectedFormat }) => {
  // Persist animation state across unmounts/remounts
  const hasAnimatedRef = React.useRef(false);
  const [shouldAnimate, setShouldAnimate] = React.useState(false);

  useEffect(() => {
    if (!hasAnimatedRef.current) {
      setShouldAnimate(true);
      hasAnimatedRef.current = true;
    }
  }, []);

  useEffect(() => {
    let utterance: SpeechSynthesisUtterance | null | undefined;
    if (shouldAnimate) {
      utterance = speakText(`Let's compare batting stats of both players in ${formatLabel}.`, SpeechLanguage.EnglishIndian);
    }
    return () => {
      if (utterance && "speechSynthesis" in window) {
        window.speechSynthesis.cancel();
      }
    };
  }, [shouldAnimate]);

  // Format mapping
  let formatLabel = '';
  if (selectedFormat === 'T20I') formatLabel = 'T20 International';
  else if (selectedFormat === 'ODI') formatLabel = 'One Day International';
  else if (selectedFormat === 'Test') formatLabel = 'Test Match';

  // Only animate on first mount, otherwise show instantly
  if (shouldAnimate) {
    return (
      <div style={{ textAlign: "center" }}>
        <StaggeredText
          text="Batting Statistics"
          className="batting-stats-header"
          style={{ display: "inline-block" }}
        />
        {formatLabel && (
          <StaggeredText
        text={formatLabel}
        className="batting-stats-format-subheader"
        style={{ display: "inline-block" }}
          />
        )}
      </div>
    );
  }
  // After first animation, just show the text statically
  return (
    <div style={{ textAlign: "center" }}>
      <div className="batting-stats-header">Batting Statistics</div>
      {formatLabel && (
        <div className="batting-stats-format-subheader">{formatLabel}</div>
      )}
    </div>
  );
};

export default useBattingStatsJSX;
