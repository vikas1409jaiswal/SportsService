import React from "react";
import { motion } from "framer-motion";

interface StaggeredTextProps {
  text: string;
  className?: string;
  style?: React.CSSProperties;
}

const StaggeredText: React.FC<StaggeredTextProps> = ({ text, className, style }) => {
  const letters = text.split("");
  return (
    <motion.div
      className={className}
      style={style}
      initial="hidden"
      animate="visible"
      variants={{
        visible: { transition: { staggerChildren: 0.045 } },
        hidden: {},
      }}
    >
      {letters.map((char, i) => (
        <motion.span
          key={i}
          style={{ display: "inline-block" }}
          variants={{
            hidden: { opacity: 0, y: 20 },
            visible: { opacity: 1, y: 0, transition: { type: "spring", stiffness: 400, damping: 24 } },
          }}
        >
          {char === " " ? "\u00A0" : char}
        </motion.span>
      ))}
    </motion.div>
  );
};

export default StaggeredText;
