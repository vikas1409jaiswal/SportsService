// Shared types for MovingTrain components

export interface SpeedDataPoint {
  time: number;
  speed: number;
  position: number;
}

export interface MovingTrainDebugInfo {
  currentPosition: number;
  currentSpeed: number;
  timeProgress: number;
  frameIndex: number;
  totalFrames: number;
  isAnimating: boolean;
  animationDuration: number;
  cumulativeDistance: number;
}

export interface MovingTrainProps {
  bogies: JSX.Element[];
  trackLength: number;
  duration?: number;
  delay?: number;
  isColumn?: boolean;
  popUpIndex?: number;
  isMoving?: boolean;
  speedFunction?: (time: number) => number; // Speed as function of time (0-1)
  debug?: boolean; // Show debug information on screen
}

export interface BogieProps {
  bogie: JSX.Element;
  index: number;
  duration?: number;
  popUpIndex: number;
}

export interface MovingTrainDebugInfoProps {
  debugInfo: MovingTrainDebugInfo;
  trackLength: number;
  isColumn?: boolean;
  bogiesCount: number;
  speedFunction?: (time: number) => number;
  delay?: number;
  speedDataPoints?: SpeedDataPoint[];
}