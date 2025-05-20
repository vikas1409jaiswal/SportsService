export enum SpeechLanguage {
  EnglishIndian = "en-IN",
  HindiIndian = "hi-IN",
  EnglishUS = "en-US",
}

export const speakText = (
  text: string,
  lang: SpeechLanguage = SpeechLanguage.EnglishIndian,
  mute: boolean = false
) => {
  if ("speechSynthesis" in window) {
    const synthesis = window.speechSynthesis;
    console.log(synthesis);
    const utterance = new SpeechSynthesisUtterance(text);
    utterance.lang = lang;
    utterance.volume = mute ? 0 : 1;
    utterance.rate = 0.9;
    synthesis.speak(utterance);
  } else {
    alert("Text-to-speech is not supported in this browser");
  }
};
