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
  isMoving?: boolean;
  speedFunction?: (time: number) => number; // Speed as function of time (0-1)
}

export const MovingTrain: React.FC<MovingTrainProps> = ({
  bogies,
  trackLength,
  duration,
  delay,
  isColumn,
  popUpIndex,
  isMoving,
  speedFunction,
}) => {
  const control = useAnimation();

  useEffect(() => {
    if (!isMoving) return;

    if (speedFunction) {
      // Custom speed function animation
      const animationDuration = duration || 30;
      const frames: number[] = [];
      const frameCount = 60; // 60 frames for smooth animation
      
      // Generate keyframes based on speed function
      for (let i = 0; i <= frameCount; i++) {
        const timeProgress = i / frameCount;
        const speed = speedFunction(timeProgress);
        const position = -trackLength * timeProgress * speed;
        frames.push(position);
      }

      const transition = {
        duration: animationDuration,
        repeat: 0,
        delay: delay || 0,
        ease: "linear", // Linear between keyframes, but keyframes create custom curve
      };

      isColumn
        ? control.start({
            y: frames,
            transition,
          })
        : control.start({
            x: frames,
            transition,
          });
    } else {
      // Default linear animation
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
    }

    return () => {
      control.stop();
    };
  }, [isColumn, isMoving, control, delay, duration, trackLength, speedFunction]);

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
