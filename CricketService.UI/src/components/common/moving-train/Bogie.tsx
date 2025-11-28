import React, { useEffect } from "react";
import { motion, useAnimation } from "framer-motion";
import { useInView } from "react-intersection-observer";
import { BogieProps } from "./types";

export const Bogie: React.FC<BogieProps> = ({
  bogie,
  index,
  duration,
  popUpIndex,
}) => {
  const { ref, inView } = useInView({ threshold: 0.5 });
  const control = useAnimation();

  useEffect(() => {
    if (inView && index > popUpIndex) {
      control.start({
        scale: [0, 1],
        transition: {
          duration: duration || 1.5,
        },
      });
    }
  }, [inView, control, duration, index, popUpIndex]);

  return (
    <motion.div
      className="bogie-container"
      initial={index > popUpIndex ? { scale: 0 } : { scale: 1 }}
      animate={control}
      ref={ref}
    >
      {bogie}
    </motion.div>
  );
};