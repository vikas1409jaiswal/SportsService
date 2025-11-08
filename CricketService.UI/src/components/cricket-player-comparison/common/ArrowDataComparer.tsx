import React, { useEffect } from "react";
import { ArrowSvg } from "./ArrowSvg";
import { useInView } from "react-intersection-observer";
import { SpeechLanguage, speakText } from "../../common/SpeakText";

import "./ArrowDataComparer.scss";

interface ArrowDataComparerProps {
  className?: string;
  dataName: string;
  data1Text: string;
  data2Text: string;
  headWidth?: number;
  tailWidth?: number;
  speechText?: string;
}

export const ArrowDataComparer: React.FC<ArrowDataComparerProps> = ({
  className,
  dataName,
  data1Text,
  data2Text,
  headWidth,
  tailWidth,
  speechText,
}) => {
  const [ref, inView] = useInView({
    triggerOnce: true, // Render the component only once
    threshold: 1, // Trigger when 50% of the component is in view
  });

  useEffect(() => {
    inView && speakText(speechText || "", SpeechLanguage.HindiIndian);
  }, [inView]);

  return (
    <div className={className || "default-data"} ref={ref}>
      <ArrowWithData
        bgColors={["cyan"]}
        text={data1Text}
        headWidth={headWidth}
        tailWidth={tailWidth}
      />
      <div className="arrow-data-name">{dataName}</div>
      <ArrowWithData
        bgColors={["violet"]}
        text={data2Text}
        reversed
        headWidth={headWidth}
        tailWidth={tailWidth}
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
}

export const ArrowWithData: React.FC<ArrowWithDataProps> = ({
  bgColors,
  reversed,
  text,
  headWidth,
  tailWidth,
}) => {
  return (
    <div className="arrow-with-data-container">
      <ArrowSvg
        headWidth={headWidth || 150}
        headHeight={100}
        tailWidth={tailWidth || 475}
        tailHeight={75}
        bgColors={bgColors}
        reversed={reversed}
      />
      <p className="data-text">{text}</p>
    </div>
  );
};
