import React, { useEffect } from "react";
import { motion, useAnimation } from "framer-motion";
import { MovingTrainProps, MovingTrainDebugInfo as DebugInfoType, SpeedDataPoint } from "./types";
import { MovingTrainDebugInfo } from "./MovingTrainDebugInfo";
import { Bogie } from "./Bogie";

import "./MovingTrain.scss";

export const MovingTrain: React.FC<MovingTrainProps> = ({
  bogies,
  trackLength,
  duration,
  delay,
  isColumn,
  popUpIndex,
  isMoving,
  speedFunction,
  debug = false,
}) => {
  const control = useAnimation();
  const [debugInfo, setDebugInfo] = React.useState<DebugInfoType>({
    currentPosition: 0,
    currentSpeed: 0,
    timeProgress: 0,
    frameIndex: 0,
    totalFrames: 0,
    isAnimating: false,
    animationDuration: 0,
    cumulativeDistance: 0
  });
  const [speedDataPoints, setSpeedDataPoints] = React.useState<SpeedDataPoint[]>([]);

  useEffect(() => {
    if (!isMoving) {
      if (debug) {
        setDebugInfo({
          currentPosition: 0,
          currentSpeed: 0,
          timeProgress: 0,
          frameIndex: 0,
          totalFrames: 0,
          isAnimating: false,
          animationDuration: 0,
          cumulativeDistance: 0
        });
        setSpeedDataPoints([]);
      }
      return;
    }

    if (speedFunction) {
      // Custom speed function animation
      const animationDuration = duration || 30;
      const frames: number[] = [];
      const frameCount = 100; // More frames for smoother animation
      
      // Generate keyframes based on speed function with cumulative distance
      // First pass: calculate total "speed-time" area to normalize
      let totalSpeedTimeArea = 0;
      const speedValues = [];
      
      for (let i = 0; i < frameCount; i++) {
        const timeProgress = i / (frameCount - 1);
        const speed = Math.max(0, speedFunction(timeProgress)); // Ensure non-negative speed
        speedValues.push(speed);
        if (i > 0) {
          // Add to total area (trapezoidal rule approximation)
          totalSpeedTimeArea += (speedValues[i] + speedValues[i-1]) / 2;
        }
      }
      
      // Normalize so that the train covers the full track length
      const normalizationFactor = totalSpeedTimeArea > 0 ? (frameCount - 1) / totalSpeedTimeArea : 1;
      
      let cumulativeDistance = 0;
      frames.push(0); // Start position
      
      // Store debug info if debug mode is enabled
      if (debug) {
        setDebugInfo(prev => ({
          ...prev,
          totalFrames: frameCount,
          animationDuration,
          isAnimating: true
        }));
        setSpeedDataPoints([]);
      }
      
      for (let i = 1; i <= frameCount; i++) {
        const timeProgress = (i - 1) / (frameCount - 1);
        const speed = Math.max(0, speedFunction(timeProgress));
        
        // Calculate distance moved in this time step
        // Normalize speed so total distance equals trackLength
        const normalizedSpeed = speed * normalizationFactor;
        const stepDistance = (trackLength / (frameCount - 1)) * normalizedSpeed;
        
        cumulativeDistance += stepDistance;
        frames.push(-Math.min(cumulativeDistance, trackLength));
      }

      const transition = {
        duration: animationDuration,
        repeat: 0,
        delay: delay || 0,
        ease: "linear", // Linear between keyframes, but keyframes create custom curve
      };

      if (debug) {
        // Add animation monitoring using a timer for speedFunction
        const startTime = Date.now() + (delay || 0) * 1000; // Account for delay
        const monitorInterval = setInterval(() => {
          const now = Date.now();
          if (now < startTime) {
            // Still in delay phase
            setDebugInfo(prev => ({
              ...prev,
              currentPosition: 0,
              currentSpeed: 0,
              timeProgress: 0,
              frameIndex: 0,
              cumulativeDistance: 0
            }));
            return;
          }
          
          const elapsedTime = (now - startTime) / 1000;
          const timeProgress = Math.min(elapsedTime / animationDuration, 1);
          const currentFrame = Math.floor(timeProgress * frameCount);
          const currentSpeed = speedFunction(timeProgress);
          
          // Calculate estimated position based on frames
          let estimatedPosition = 0;
          if (timeProgress > 0 && frames.length > currentFrame) {
            estimatedPosition = frames[currentFrame] || 0;
          }
          
          setDebugInfo(prev => ({
            ...prev,
            currentPosition: estimatedPosition,
            currentSpeed,
            timeProgress,
            frameIndex: currentFrame,
            cumulativeDistance: Math.abs(estimatedPosition)
          }));
          
          // Add data point for graphing
          setSpeedDataPoints(prev => {
            const newPoint: SpeedDataPoint = {
              time: elapsedTime,
              speed: currentSpeed,
              position: estimatedPosition
            };
            // Keep only last 50 points for performance
            const newPoints = [...prev, newPoint];
            return newPoints.length > 50 ? newPoints.slice(-50) : newPoints;
          });
          
          if (timeProgress >= 1) {
            clearInterval(monitorInterval);
            setDebugInfo(prev => ({ ...prev, isAnimating: false }));
          }
        }, 100); // Update every 100ms
        
        // Store cleanup function
        setTimeout(() => clearInterval(monitorInterval), (animationDuration + (delay || 0)) * 1000 + 1000);
      }

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
      if (debug) {
        setDebugInfo(prev => ({
          ...prev,
          totalFrames: 60, // Default frame estimation
          animationDuration: duration || 30,
          isAnimating: true,
          currentSpeed: 1.0
        }));
        setSpeedDataPoints([]);
      }
      
      const transition = {
        duration: duration || 30,
        repeat: 0,
        delay: delay || 0,
        ease: "linear",
      };
      
      if (debug) {
        // Add animation monitoring using a timer for linear animation
        const startTime = Date.now();
        const animationDuration = duration || 30;
        const monitorInterval = setInterval(() => {
          const elapsedTime = (Date.now() - startTime) / 1000;
          const timeProgress = Math.min(elapsedTime / animationDuration, 1);
          const estimatedPosition = -trackLength * timeProgress;
          
          setDebugInfo(prev => ({
            ...prev,
            currentPosition: estimatedPosition,
            timeProgress,
            frameIndex: Math.floor(timeProgress * 60), // Estimate frame for linear
            cumulativeDistance: Math.abs(estimatedPosition)
          }));
          
          // Add data point for graphing
          setSpeedDataPoints(prev => {
            const newPoint: SpeedDataPoint = {
              time: elapsedTime,
              speed: 1.0, // Linear animation has constant speed
              position: estimatedPosition
            };
            // Keep only last 50 points for performance
            const newPoints = [...prev, newPoint];
            return newPoints.length > 50 ? newPoints.slice(-50) : newPoints;
          });
          
          if (timeProgress >= 1) {
            clearInterval(monitorInterval);
            setDebugInfo(prev => ({ ...prev, isAnimating: false }));
          }
        }, 100); // Update every 100ms
        
        // Store cleanup function
        setTimeout(() => clearInterval(monitorInterval), (animationDuration + (delay || 0)) * 1000 + 1000);
      }

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
  }, [isColumn, isMoving, control, delay, duration, trackLength, speedFunction, debug]);

  return (
    <>
      {debug && (
        <MovingTrainDebugInfo
          debugInfo={debugInfo}
          trackLength={trackLength}
          isColumn={isColumn}
          bogiesCount={bogies?.length || 0}
          speedFunction={speedFunction}
          delay={delay}
          speedDataPoints={speedDataPoints}
        />
      )}
      <motion.div
        className="moving-train-container"
        animate={control}
        style={isColumn ? { flexDirection: "column" } : {}}
      >
        {bogies?.map((bogie, i) => (
          <Bogie bogie={bogie} index={i} popUpIndex={popUpIndex || 2} key={`bogie-${i}`} />
        ))}
      </motion.div>
    </>
  );
};