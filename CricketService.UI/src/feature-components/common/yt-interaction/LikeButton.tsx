import React from "react";
import { motion } from "framer-motion";
import { AiFillHeart } from "react-icons/ai";

interface LikeButtonProps {
  isActive: boolean;
  isCompleted: boolean;
}

export const LikeButton: React.FC<LikeButtonProps> = ({ isActive, isCompleted }) => {
  const buttonVariants = {
    inactive: { 
      scale: 1, 
      opacity: 0.6,
      filter: "grayscale(100%)"
    },
    active: { 
      scale: [1, 1.3, 1.1, 1.3, 1],
      opacity: 1,
      filter: "grayscale(0%)",
      transition: {
        duration: 1.2,
        ease: "easeInOut",
        times: [0, 0.3, 0.5, 0.7, 1]
      }
    },
    completed: { 
      scale: 1, 
      opacity: 1,
      filter: "grayscale(0%)",
      boxShadow: "0 0 20px rgba(255, 69, 58, 0.6)"
    }
  };

  const iconVariants = {
    inactive: { 
      rotate: 0,
      color: "#666"
    },
    active: { 
      rotate: [0, -10, 10, -5, 0],
      color: "#ff453a",
      transition: {
        duration: 1.2,
        ease: "easeInOut"
      }
    },
    completed: { 
      rotate: 0,
      color: "#ff453a"
    }
  };

  const getAnimationState = () => {
    if (isCompleted) return "completed";
    if (isActive) return "active";
    return "inactive";
  };

  return (
    <motion.div
      className="interaction-button like-button"
      variants={buttonVariants}
      animate={getAnimationState()}
    >
      <motion.div
        className="button-content"
        variants={iconVariants}
        animate={getAnimationState()}
      >
        <AiFillHeart className="button-icon" />
        <span className="button-text">Like</span>
      </motion.div>
      
      {isActive && (
        <motion.div
          className="pulse-effect"
          initial={{ scale: 0, opacity: 0.8 }}
          animate={{ 
            scale: [0, 1.5, 2.5], 
            opacity: [0.8, 0.3, 0] 
          }}
          transition={{ 
            duration: 1.2, 
            ease: "easeOut",
            repeat: 1
          }}
        />
      )}
      
      {isActive && (
        <>
          {[...Array(6)].map((_, i) => (
            <motion.div
              key={i}
              className="floating-heart"
              initial={{ 
                scale: 0, 
                x: 0, 
                y: 0, 
                opacity: 1,
                rotate: Math.random() * 360
              }}
              animate={{ 
                scale: [0, 1, 0.8, 0], 
                x: (Math.random() - 0.5) * 100,
                y: -80 + Math.random() * -40,
                opacity: [1, 1, 0.5, 0],
                rotate: Math.random() * 720
              }}
              transition={{ 
                duration: 1.5, 
                delay: i * 0.1,
                ease: "easeOut"
              }}
            >
              ❤️
            </motion.div>
          ))}
        </>
      )}
    </motion.div>
  );
};