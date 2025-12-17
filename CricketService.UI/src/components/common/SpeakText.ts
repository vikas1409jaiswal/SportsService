export enum SpeechLanguage {
  EnglishIndian = "en-IN",
  HindiIndian = "hi-IN",
  EnglishUS = "en-US",
}

export const speakText = (
  text: string,
  lang: SpeechLanguage = SpeechLanguage.EnglishIndian,
  mute: boolean = false,
  onStart?: () => void,
  onEnd?: () => void
) => {
  if ("speechSynthesis" in window) {
    const synthesis = window.speechSynthesis;
    console.log(synthesis);
    const utterance = new SpeechSynthesisUtterance(text);
    utterance.lang = SpeechLanguage.HindiIndian || lang;
    utterance.volume = mute ? 0 : 1;
    utterance.rate = 0.9;
    
    // Add event listeners if callbacks are provided
    if (onStart) {
      utterance.onstart = onStart;
    }
    if (onEnd) {
      utterance.onend = onEnd;
    }
    
    synthesis.speak(utterance);
    return utterance;
  } else {
    alert("Text-to-speech is not supported in this browser");
    return null;
  }
};
