import React, { useEffect, useState } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { MovingCarouselProps } from "./types";

import "./MovingCarousel.scss";

export const MovingCarousel: React.FC<MovingCarouselProps> = ({
  bogies,
  autoAdvance = false,
  autoAdvanceInterval = 3000,
  showNavigation = false,
}) => {
  const [currentIndex, setCurrentIndex] = useState(0);

  const goToNext = React.useCallback(() => {
    if (bogies.length === 0) return;
    setCurrentIndex((prev) => (prev + 1) % bogies.length);
  }, [bogies.length]);

  const goToPrevious = React.useCallback(() => {
    if (bogies.length === 0) return;
    setCurrentIndex((prev) => (prev - 1 + bogies.length) % bogies.length);
  }, [bogies.length]);

  // Handle keyboard navigation
  useEffect(() => {
    const handleKeyDown = (event: KeyboardEvent) => {
      if (event.key === 'ArrowRight') {
        event.preventDefault();
        goToNext();
      } else if (event.key === 'ArrowLeft') {
        event.preventDefault();
        goToPrevious();
      }
    };

    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, [goToNext, goToPrevious]);

  // Auto-advance functionality
  useEffect(() => {
    if (!autoAdvance || bogies.length <= 1) return;

    const intervalId = setInterval(() => {
      goToNext();
    }, autoAdvanceInterval);

    return () => clearInterval(intervalId);
  }, [autoAdvance, autoAdvanceInterval, goToNext, bogies.length]);

  const goToIndex = (index: number) => {
    setCurrentIndex(index);
  };

  if (!bogies || bogies.length === 0) {
    return (
      <div className="moving-carousel-container">
        <div className="carousel-empty">
          <p>No items to display</p>
        </div>
      </div>
    );
  }

  const variants = {
    enter: {
      opacity: 0,
      scale: 1,
    },
    center: {
      opacity: 1,
      scale: 1,
    },
    exit: {
      opacity: 0,
      scale: 1,
    },
  };

  return (
    <div className="moving-carousel-container">
      {/* Navigation Hint */}
      {showNavigation && (
        <div className="carousel-hint">
          <span className="hint-text">
            <kbd>←</kbd> <kbd>→</kbd> to navigate
          </span>
        </div>
      )}

      {/* Main Carousel Display */}
      <div className="carousel-display">
        <AnimatePresence initial={false}>
          <motion.div
            key={currentIndex}
            variants={variants}
            initial="enter"
            animate="center"
            exit="exit"
            transition={{
              duration: 0.3,
              ease: "easeInOut",
            }}
            className="carousel-item"
          >
            {bogies[currentIndex]}
          </motion.div>
        </AnimatePresence>
      </div>

      {/* Navigation Buttons */}
      {showNavigation && (
        <div className="carousel-controls">
          <button
            className="carousel-btn carousel-btn-prev"
            onClick={goToPrevious}
            disabled={bogies.length <= 1}
            aria-label="Previous bogie"
          >
            <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor">
              <path d="M19 12H5M12 19l-7-7 7-7" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
            </svg>
          </button>

          {/* Progress Indicator */}
          <div className="carousel-indicators">
            {bogies.map((_, index) => (
              <button
                key={index}
                className={`indicator-dot ${index === currentIndex ? 'active' : ''}`}
                onClick={() => goToIndex(index)}
                aria-label={`Go to bogie ${index + 1}`}
              />
            ))}
          </div>

          <button
            className="carousel-btn carousel-btn-next"
            onClick={goToNext}
            disabled={bogies.length <= 1}
            aria-label="Next bogie"
          >
            <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor">
              <path d="M5 12h14M12 5l7 7-7 7" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/>
            </svg>
          </button>
        </div>
      )}

      {/* Current Position Display */}
      {showNavigation && (
        <div className="carousel-counter">
          <span>{currentIndex + 1} / {bogies.length}</span>
        </div>
      )}
    </div>
  );
};
