import React from "react";

interface ArrowSvgProps {
  headWidth?: number;
  headHeight?: number;
  tailWidth?: number;
  tailHeight?: number;
  reversed?: boolean;
  bgColors: string[];
}

export const ArrowSvg: React.FC<ArrowSvgProps> = ({
  headHeight,
  headWidth,
  tailWidth,
  tailHeight,
  reversed,
  bgColors,
}) => {
  const totalHeight = headHeight || 100;
  const totalWidth = (headWidth || 90) + (tailWidth || 535);
  const tailInHeight = ((headHeight || 100) - (tailHeight || 50)) / 2;
  const tailOutHeight = tailInHeight + (tailHeight || 50);
  const arrowHeadWidth = headWidth || 90;

  return (
    <svg
      width="100%"
      height={totalHeight}
      style={reversed ? { rotate: "180deg" } : {}}
    >
      <polygon
        points={`0,${
          totalHeight / 2
        } ${arrowHeadWidth},0 ${arrowHeadWidth},${tailInHeight} ${totalWidth},${tailInHeight} ${totalWidth},${tailOutHeight} ${arrowHeadWidth},${tailOutHeight} ${arrowHeadWidth},${totalHeight} 0,${
          totalHeight / 2
        }`}
        fill={bgColors[0]}
        stroke="darkblue"
        strokeWidth={5}
        strokeLinecap="round"
      />
    </svg>
  );
};
