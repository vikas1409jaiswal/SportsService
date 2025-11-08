import React, { useEffect, useState } from "react";
import { PlayerInfo } from "../../../CricketComponents/CricketPlayerInfoFetch/useCustomPlayerInfo";

interface AnimatedValueContentProps {
  value: number;
  duration: number;
  player?: PlayerInfo | null;
  showStar?: boolean;
  className?: string;
}

export const AnimatedValueContent: React.FC<AnimatedValueContentProps> = ({
  value,
  duration,
  player,
  showStar,
  className,
}) => {
  const [animatedValue, setAnimatedValue] = useState(0);

  useEffect(() => {
    let start = 0;
    const increment = Math.ceil(value / (duration / 100));

    const interval = setInterval(() => {
      start += increment;
      if (start >= value) {
        setAnimatedValue(value);
        clearInterval(interval);
      } else {
        setAnimatedValue(start);
      }
    }, 100);

    return () => clearInterval(interval);
  }, [value, duration, player]);

  return (
    <span className={className || "default"}>
      {animatedValue}
      {showStar && <sup>*</sup>}
    </span>
  );
};
