import React from "react";
import { motion } from "framer-motion";

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

  const color1 = bgColors[0];
  const color2 = bgColors[1] || bgColors[0];
  const gradientId = `gradient-${color1.replace(/\W/g, "")}-${color2.replace(/\W/g, "")}`;

  return (
    <motion.svg
      width="100%"
      height={totalHeight}
      style={reversed ? { rotate: "180deg" } : {}}
      initial={{ opacity: 0, x: reversed ? 20 : -20 }}
      animate={{ opacity: 1, x: 0 }}
      transition={{ duration: 0.5, type: "spring", stiffness: 100 }}
      whileHover={{ scale: 1.02, filter: "brightness(1.1)" }}
    >
      <defs>
        <linearGradient id={gradientId} x1="0%" y1="0%" x2="100%" y2="0%">
          <stop offset="0%" stopColor={color1} />
          <stop offset="100%" stopColor={color2} />
        </linearGradient>
      </defs>
      <motion.polygon
        points={`0,${
          totalHeight / 2
        } ${arrowHeadWidth},0 ${arrowHeadWidth},${tailInHeight} ${totalWidth},${tailInHeight} ${totalWidth},${tailOutHeight} ${arrowHeadWidth},${tailOutHeight} ${arrowHeadWidth},${totalHeight} 0,${
          totalHeight / 2
        }`}
        fill={`url(#${gradientId})`}
        stroke="white"
        strokeWidth={5}
        strokeLinecap="round"
        initial={{ pathLength: 0 }}
        animate={{ pathLength: 1 }}
        transition={{ duration: 1, ease: "easeInOut" }}
      />
    </motion.svg>
  );
};
