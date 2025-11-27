import React from "react";
import { motion } from "framer-motion";
import { IoMdShare } from "react-icons/io";

interface ShareButtonProps {
  isActive: boolean;
  isCompleted: boolean;
}

export const ShareButton: React.FC<ShareButtonProps> = ({ isActive, isCompleted }) => {
  const buttonVariants = {
    inactive: { 
      scale: 1, 
      opacity: 0.6,
      filter: "grayscale(100%)"
    },
    active: { 
      scale: [1, 1.2, 1],
      opacity: 1,
      filter: "grayscale(0%)",
      transition: {
        duration: 1,
        ease: "easeInOut"
      }
    },
    completed: { 
      scale: 1, 
      opacity: 1,
      filter: "grayscale(0%)",
      boxShadow: "0 0 20px rgba(0, 122, 255, 0.6)"
    }
  };

  const iconVariants = {
    inactive: { 
      rotate: 0,
      color: "#666"
    },
    active: { 
      rotate: [0, 15, -10, 5, 0],
      color: "#007aff",
      transition: {
        duration: 1,
        ease: "easeInOut"
      }
    },
    completed: { 
      rotate: 0,
      color: "#007aff"
    }
  };

  const getAnimationState = () => {
    if (isCompleted) return "completed";
    if (isActive) return "active";
    return "inactive";
  };

  return (
    <motion.div
      className="interaction-button share-button"
      variants={buttonVariants}
      animate={getAnimationState()}
    >
      <motion.div
        className="button-content"
        variants={iconVariants}
        animate={getAnimationState()}
      >
        <IoMdShare className="button-icon" />
        <span className="button-text">Share</span>
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
            duration: 1, 
            ease: "easeOut",
            repeat: 1
          }}
        />
      )}
      
      {isActive && (
        <>
          {[...Array(8)].map((_, i) => (
            <motion.div
              key={i}
              className="share-ripple"
              initial={{ 
                scale: 0, 
                x: 0, 
                y: 0, 
                opacity: 0.8
              }}
              animate={{ 
                scale: [0, 1.5],
                x: Math.cos((i * 45) * Math.PI / 180) * 60,
                y: Math.sin((i * 45) * Math.PI / 180) * 60,
                opacity: [0.8, 0]
              }}
              transition={{ 
                duration: 0.8, 
                delay: i * 0.05,
                ease: "easeOut"
              }}
            >
              <div className="ripple-dot" />
            </motion.div>
          ))}
        </>
      )}
    </motion.div>
  );
};