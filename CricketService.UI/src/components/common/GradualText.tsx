import { motion, useAnimation } from "framer-motion";
import React, { useEffect, useState } from "react";

interface GradualTextProps {
  id: string;
  duration: number;
  text: string;
}

export const GradualText: React.FC<GradualTextProps> = ({
  id,
  duration,
  text,
}) => {
  const [animatedText, setAnimatedText] = useState("");
  const animationControls = useAnimation();

  useEffect(() => {
    let currentChar = 0;
    const interval = setInterval(() => {
      setAnimatedText((prevText) => prevText + text[currentChar]);
      currentChar++;

      if (currentChar === text.length) {
        clearInterval(interval);
      }
    }, duration / text.length);

    return () => {
      clearInterval(interval);
      setAnimatedText("");
    };
  }, [duration, text]);

  useEffect(() => {
    animationControls.start({ opacity: 1 });
  }, [animationControls]);

  return (
    <motion.p
      id={id}
      initial={{ opacity: 0 }}
      animate={animationControls}
      exit={{ opacity: 0 }}
    >
      {animatedText.replace("undefined", "")}
    </motion.p>
  );
};
