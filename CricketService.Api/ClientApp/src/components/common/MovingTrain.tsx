import React, { useEffect } from "react";
import { motion, useAnimation } from "framer-motion";
import { useInView } from "react-intersection-observer";

import "./MovingTrain.scss";

interface MovingTrainProps {
  bogies: JSX.Element[];
  trackLength: number;
  duration?: number;
  delay?: number;
  isColumn?: boolean;
  popUpIndex?: number;
}

export const MovingTrain: React.FC<MovingTrainProps> = ({
  bogies,
  trackLength,
  duration,
  delay,
  isColumn,
  popUpIndex,
}) => {
  const control = useAnimation();

  useEffect(() => {
    const transition = {
      duration: duration || 30,
      repeat: 0,
      delay: delay || 0,
      ease: "linear",
    };

    isColumn
      ? control.start({
          y: [0, -trackLength],
          transition,
        })
      : control.start({
          x: [0, -trackLength],
          transition,
        });

    return () => {
      control.stop();
    };
  }, [isColumn]);

  return (
    <motion.div
      className="moving-train-container"
      animate={control}
      style={isColumn ? { flexDirection: "column" } : {}}
    >
      {bogies?.map((bogie, i) => (
        <Bogie bogie={bogie} index={i} popUpIndex={popUpIndex || 2} />
      ))}
    </motion.div>
  );
};

interface BogieProps {
  bogie: JSX.Element;
  index: number;
  duration?: number;
  popUpIndex: number;
}

const Bogie: React.FC<BogieProps> = ({
  bogie,
  index,
  duration,
  popUpIndex,
}) => {
  const { ref, inView, entry } = useInView({ threshold: 0.5 });
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
  }, [inView, control, duration]);

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
