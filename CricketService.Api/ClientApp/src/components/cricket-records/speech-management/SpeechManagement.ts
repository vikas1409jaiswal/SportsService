import { config } from "../../../configs";
import { SpeechLanguage, speakText } from "../../common/SpeakText";
import { ESPNTableRow } from "../hook/useCustomESPNTable";
import engToHinJson from "./../../../data/StaticData/englishToHindi.json";

const getValue = (row: ESPNTableRow, key: string) =>
  row?.data.find((x) => x.key === key)?.value || "";

export const speeches = {
  "top-10-players-intro": () => {
    config.language === "hindi"
      ? speakText(
          "इस वीडियो में हम शीर्ष 10 खिलाड़ियों को देखेंगे जो एकदिवसीय अंतराष्ट्रीय मैचों में सबसे ज्यादा बार शतक से चूके और नब्बे प्लस पर आउट हुए है",
          SpeechLanguage.HindiIndian
        )
      : speakText(
          "In this video we will see top 10 players who had taken most wickets in calendar year 2024." // who had scored most runs in big bash league"
        );
  },
  "video-end-message": () =>
    config.language === "hindi"
      ? speakText(
          "नंबर 1 जानने से पहले अगर आपको यह वीडियो पसंद आ रही हैं तो प्लीज हमारे चैनल को लाइक, शेयर और सब्सक्राइब करें",
          SpeechLanguage.HindiIndian
        )
      : speakText(
          "Before knowing who is number 1, if you are liking our content then Please like share and subscribe."
        ),
  "most-career-runs-speech": (
    index: number,
    playerName: string,
    row: ESPNTableRow
  ) => {
    config.language === "hindi"
      ? speakText(
          `नंबर ${index + 1}, ${
            (engToHinJson as any)["team-names"][getValue(row, "xyz")]
          } के ${(engToHinJson as any).players[playerName]}, इन्होने ${
            getValue(row, "Span").split("-")[0]
          } से ${getValue(row, "Span").split("-")[1]} तक ${getValue(
            row,
            "Matches"
          )} मैचों में ${getValue(row, "Runs")} रन बनाये हैं.`,
          SpeechLanguage.HindiIndian
        )
      : speakText(
          `Number ${index + 1}, ${playerName} from ${getValue(
            row,
            "xyz"
          )}, ${playerName} had scored ${getValue(
            row,
            "Runs"
          )} Runs in ${getValue(row, "Matches")?.replace(
            "*",
            ""
          )} matches between ${getValue(row, "Span").split("-")[0]} and ${
            getValue(row, "Span").split("-")[1]
          }.`
        );
  },
  "most-career-wickets-speech": (
    index: number,
    playerName: string,
    row: ESPNTableRow
  ) => {
    config.language === "hindi"
      ? speakText(
          `नंबर ${index + 1}, ${
            (engToHinJson as any)["team-names"][getValue(row, "xyz")]
          } के ${(engToHinJson as any).players[playerName]}, इन्होने ${
            getValue(row, "Span").split("-")[0]
          } से ${getValue(row, "Span").split("-")[1]} तक ${getValue(
            row,
            "Matches"
          )} मैचों में ${getValue(row, "Wickets")} विकेट लिए है.`,
          SpeechLanguage.HindiIndian
        )
      : speakText(
          // from ${getValue(row, "xyz")}
          `Number ${index + 1}, ${playerName} from ${getValue(
            row,
            "xyz"
          )}, ${playerName} had taken ${getValue(
            row,
            "Wickets"
          )} Wickets in ${getValue(row, "Matches")?.replace(
            "*",
            ""
          )} matches, between ${getValue(row, "Span").split("-")[0]} and ${
            getValue(row, "Span").split("-")[1]
          }.`
        );
  },
  "most-career-sixes-speech": (
    index: number,
    playerName: string,
    row: ESPNTableRow
  ) => {
    config.language === "hindi"
      ? speakText(
          `नंबर ${index + 1}, ${
            (engToHinJson as any)["team-names"][getValue(row, "xyz")]
          } के ${(engToHinJson as any).players[playerName]}, इन्होने ${
            getValue(row, "Span").split("-")[0]
          } से ${getValue(row, "Span").split("-")[1]} तक ${getValue(
            row,
            "Matches"
          )} मैचों में ${getValue(row, "Sixes")} छक्के मारे है.`,
          SpeechLanguage.HindiIndian
        )
      : speakText(
          `Number ${index + 1}, ${playerName} from ${getValue(
            row,
            "xyz"
          )}, had hit ${getValue(row, "Sixes")} Sixes in ${getValue(
            row,
            "Matches"
          )} matches between ${getValue(row, "Span").split("-")[0]} and ${
            getValue(row, "Span").split("-")[1]
          }.`
        );
  },
  "most-career-nervous-ninties-speech": (
    index: number,
    playerName: string,
    row: ESPNTableRow
  ) => {
    config.language === "hindi"
      ? speakText(
          `नंबर ${index + 1}, ${
            (engToHinJson as any)["team-names"][getValue(row, "xyz")]
          } के ${(engToHinJson as any).players[playerName]}, इन्होने ${
            getValue(row, "Span").split("-")[0]
          } से ${getValue(row, "Span").split("-")[1]} तक ${getValue(
            row,
            "Matches"
          )} मैचों में ${getValue(row, "Nineties")} बार नब्बे प्लस पर आउट हुए.`,
          SpeechLanguage.HindiIndian
        )
      : speakText(
          `Number ${index + 1}, ${playerName} from ${getValue(
            row,
            "xyz"
          )}, had scored ${getValue(
            row,
            "Nineties"
          )} nervous nineties in ${getValue(row, "Matches")} matches  between ${
            getValue(row, "Span").split("-")[0]
          } and ${getValue(row, "Span").split("-")[1]}.`
        );
  },
  "highest-individual-score-speech": (
    index: number,
    playerName: string,
    row: ESPNTableRow
  ) => {
    config.language === "hindi"
      ? speakText(
          `नंबर ${index + 1}, ${
            (engToHinJson as any)["team-names"][getValue(row, "xyz")]
          } के ${(engToHinJson as any).players[playerName]}, इन्होने ${getValue(
            row,
            "Against"
          )} के खिलाफ ${getValue(row, "Date")?.split(" ")[2]}  में  ${getValue(
            row,
            "H.Score"
          )?.replace("*", " नाबाद")} रन बनाये ${getValue(
            row,
            "Balls"
          )} गेंदों में.`,
          SpeechLanguage.HindiIndian
        )
      : speakText(
          `Number ${index + 1}, ${playerName} from ${getValue(
            row,
            "xyz"
          )}, ${playerName} had scored ${
            getValue(row, "H.Score")?.replace("*", " not out") ||
            getValue(row, "Runs")?.replace("*", " not out")
          } Runs in ${getValue(row, "Balls")} balls against ${getValue(
            row,
            "Against"
          )} in ${getValue(row, "Date")?.split(" ")[2]}.`
        );
  },
  "most-runs-calendar-speech": (
    index: number,
    playerName: string,
    row: ESPNTableRow,
    teamName: string
  ) => {
    config.language === "hindi"
      ? speakText(``, SpeechLanguage.HindiIndian)
      : speakText(
          `Number ${
            index + 1
          }, ${playerName} from ${teamName}, ${playerName} had scored ${getValue(
            row,
            "Runs"
          )} Runs in ${getValue(
            row,
            "Matches"
          )} Matches. His highest score was ${getValue(row, "H.Score")?.replace(
            "*",
            "not out"
          )} .`
        );
  },
  "most-sixes-calendar-speech": (
    index: number,
    playerName: string,
    row: ESPNTableRow,
    teamName: string
  ) => {
    config.language === "hindi"
      ? speakText(``, SpeechLanguage.HindiIndian)
      : speakText(
          `Number ${
            index + 1
          }, ${playerName} from ${teamName}, ${playerName} had hit ${getValue(
            row,
            "Sixes"
          )} Sixes in ${getValue(
            row,
            "Matches"
          )} Matches. He had scored ${getValue(row, "Runs")} Runs.`
        );
  },
  "most-hundreds-calendar-speech": (
    index: number,
    playerName: string,
    row: ESPNTableRow,
    teamName: string
  ) => {
    config.language === "hindi"
      ? speakText(``, SpeechLanguage.HindiIndian)
      : speakText(
          `Number ${
            index + 1
          }, ${playerName} from ${teamName}, ${playerName} had scored ${getValue(
            row,
            "Hundred"
          )} centuries in ${getValue(
            row,
            "Matches"
          )} Matches. He had also scored ${getValue(row, "Runs")} Runs.`
        );
  },
  "best-wicketkeeper-calendar-speech": (
    index: number,
    playerName: string,
    row: ESPNTableRow,
    teamName: string
  ) => {
    config.language === "hindi"
      ? speakText(``, SpeechLanguage.HindiIndian)
      : speakText(
          `Number ${
            index + 1
          }, ${playerName} from ${teamName}, ${playerName} had taken ${getValue(
            row,
            "Dismissal"
          )} Dismissals in ${getValue(
            row,
            "Matches"
          )} Matches comprising ${getValue(
            row,
            "Catches"
          )} catches and ${getValue(row, "Stumping")} stumping.`
        );
  },
  "most-wickets-calendar-speech": (
    index: number,
    playerName: string,
    row: ESPNTableRow,
    teamName: string
  ) => {
    config.language === "hindi"
      ? speakText(``, SpeechLanguage.HindiIndian)
      : speakText(
          `Number ${
            index + 1
          }, ${playerName} from ${teamName}, ${playerName} had taken ${getValue(
            row,
            "Wickets"
          )} wickets in ${getValue(
            row,
            "Matches"
          )} Matches. His best performance is ${getValue(row, "BBI")} .`
        );
  },
  "best-striker-rate-speech": (
    index: number,
    playerName: string,
    row: ESPNTableRow,
    teamName: string
  ) => {
    config.language === "hindi"
      ? speakText(``, SpeechLanguage.HindiIndian)
      : speakText(
          `Number ${
            index + 1
          }, ${playerName} from ${teamName}, ${playerName} had an impressive strike rate of ${getValue(
            row,
            "S.Rate"
          )}, in ${getValue(row, "Matches")} Matches, comprising ${getValue(
            row,
            "Runs"
          )} runs in ${getValue(row, "Balls")} balls.`
        );
  },
};
