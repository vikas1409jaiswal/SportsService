import React from "react";
import { motion } from "framer-motion";
import { MdNotificationsActive } from "react-icons/md";

interface SubscribeButtonProps {
  isActive: boolean;
  isCompleted: boolean;
}

export const SubscribeButton: React.FC<SubscribeButtonProps> = ({ isActive, isCompleted }) => {
  const buttonVariants = {
    inactive: { 
      scale: 1, 
      opacity: 0.6,
      filter: "grayscale(100%)"
    },
    active: { 
      scale: [1, 1.1, 1.3, 1.1, 1.2, 1],
      opacity: 1,
      filter: "grayscale(0%)",
      transition: {
        duration: 1.5,
        ease: "easeInOut"
      }
    },
    completed: { 
      scale: 1, 
      opacity: 1,
      filter: "grayscale(0%)",
      boxShadow: "0 0 25px rgba(255, 59, 48, 0.8)"
    }
  };

  const iconVariants = {
    inactive: { 
      rotate: 0,
      color: "#666"
    },
    active: { 
      rotate: [0, -5, 5, -5, 5, 0],
      color: "#ff3b30",
      transition: {
        duration: 1.5,
        ease: "easeInOut"
      }
    },
    completed: { 
      rotate: 0,
      color: "#ff3b30"
    }
  };

  const getAnimationState = () => {
    if (isCompleted) return "completed";
    if (isActive) return "active";
    return "inactive";
  };

  return (
    <motion.div
      className="interaction-button subscribe-button"
      variants={buttonVariants}
      animate={getAnimationState()}
    >
      <motion.div
        className="button-content"
        variants={iconVariants}
        animate={getAnimationState()}
      >
        <MdNotificationsActive className="button-icon" />
        <span className="button-text">Subscribe</span>
      </motion.div>
      
      {isActive && (
        <motion.div
          className="pulse-effect"
          initial={{ scale: 0, opacity: 0.8 }}
          animate={{ 
            scale: [0, 1.8, 3], 
            opacity: [0.8, 0.4, 0] 
          }}
          transition={{ 
            duration: 1.5, 
            ease: "easeOut",
            repeat: 1
          }}
        />
      )}
      
      {isActive && (
        <>
          <motion.div
            className="notification-bell"
            initial={{ scale: 0, rotate: 0 }}
            animate={{ 
              scale: [0, 1.2, 1],
              rotate: [-10, 10, -5, 5, 0],
              y: [-20, -30, -20]
            }}
            transition={{ 
              duration: 1.2,
              ease: "easeOut"
            }}
          >
            🔔
          </motion.div>
          
          {[...Array(12)].map((_, i) => (
            <motion.div
              key={i}
              className="subscribe-particle"
              initial={{ 
                scale: 0, 
                x: 0, 
                y: 0, 
                opacity: 1
              }}
              animate={{ 
                scale: [0, 1, 0.5, 0],
                x: Math.cos((i * 30) * Math.PI / 180) * (40 + Math.random() * 30),
                y: Math.sin((i * 30) * Math.PI / 180) * (40 + Math.random() * 30),
                opacity: [1, 1, 0.5, 0]
              }}
              transition={{ 
                duration: 1.2, 
                delay: i * 0.04,
                ease: "easeOut"
              }}
            >
              ⭐
            </motion.div>
          ))}
        </>
      )}
    </motion.div>
  );
};