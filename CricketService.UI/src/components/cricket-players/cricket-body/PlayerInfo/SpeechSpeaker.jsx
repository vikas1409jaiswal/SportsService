import Speech from "react-speech";

export const SpeechSpeaker = ({ text }) => {
  return (
    <>
      <Speech
        text={text}
        pitch="0.5"
        rate="0.5"
        volume="1.0"
        lang="en-GB"
        voice="Harry"
        stop={true}
        pause={true}
        resume={true}
      />
    </>
  );
};
