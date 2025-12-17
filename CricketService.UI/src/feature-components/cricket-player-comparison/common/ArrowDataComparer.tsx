import React, { useEffect } from "react";
import { ArrowSvg } from "./ArrowSvg";
import { useInView } from "react-intersection-observer";
import { SpeechLanguage, speakText } from "../../../components/common/SpeakText";

import "./ArrowDataComparer.scss";

interface ArrowDataComparerProps {
  className?: string;
  dataName: string;
  data1Text: string;
  data2Text: string;
  headWidth?: number;
  tailWidth?: number;
  headHeight?: number;
  tailHeight?: number;
  speechText?: string;
}

export const ArrowDataComparer: React.FC<ArrowDataComparerProps> = ({
  className,
  dataName,
  data1Text,
  data2Text,
  headWidth,
  tailWidth,
  headHeight,
  tailHeight,
  speechText,
}) => {
  const [ref, inView] = useInView({
    triggerOnce: true, // Render the component only once
    threshold: 0.5, // Trigger when 50% of the component is in view
  });

  useEffect(() => {
    let utterance: SpeechSynthesisUtterance | null = null;
    if (inView) {
      utterance = speakText(speechText || "", SpeechLanguage.HindiIndian);
    }
    return () => {
      if (utterance && "speechSynthesis" in window) {
        window.speechSynthesis.cancel();
      }
    };
  }, [inView, speechText]);

  return (
    <div className={className || "default-data"} ref={ref}>
      <ArrowWithData
        bgColors={["cyan", "orange"]}
        text={data1Text}
        headWidth={headWidth}
        tailWidth={tailWidth}
        headHeight={headHeight}
        tailHeight={tailHeight}
        animationDelay={0.15}
      />
      <div className="arrow-data-name">{dataName}</div>
      <ArrowWithData
        bgColors={["violet", "darkblue"]}
        text={data2Text}
        reversed
        headWidth={headWidth}
        tailWidth={tailWidth}
        headHeight={headHeight}
        tailHeight={tailHeight}
        animationDelay={0.45}
      />
    </div>
  );
};

interface ArrowWithDataProps {
  bgColors: string[];
  reversed?: boolean;
  text: string;
  headWidth?: number;
  tailWidth?: number;
  headHeight?: number;
  tailHeight?: number;
  animationDelay?: number;
}

export const ArrowWithData: React.FC<ArrowWithDataProps> = ({
  bgColors,
  reversed,
  text,
  headWidth,
  tailWidth,
  headHeight,
  tailHeight,
  animationDelay = 0,
}) => {
  return (
    <div className="arrow-with-data-container">
      <ArrowSvg
        headWidth={headWidth || 150}
        headHeight={headHeight || 120}
        tailWidth={tailWidth || 475}
        tailHeight={tailHeight || 90}
        bgColors={bgColors}
        reversed={reversed}
        animationDelay={animationDelay}
      />
      <p className="data-text">{text}</p>
    </div>
  );
};
