// Types for MovingCarousel component

export interface AnimationConfig {
  enterY?: number;
  exitY?: number;
  enterScale?: number;
  exitScale?: number;
  enterOpacity?: number;
  exitOpacity?: number;
  springStiffness?: number;
  springDamping?: number;
  springMass?: number;
  opacityDuration?: number;
  scaleDuration?: number;
  easingFunction?: string;
}

export interface MovingCarouselProps {
  bogies: JSX.Element[];
  autoAdvance?: boolean;
  autoAdvanceInterval?: number; // milliseconds
  animationConfig?: AnimationConfig;
  showNavigation?: boolean; // Controls visibility of navigation controls, indicators, and counter (default: false)
}
