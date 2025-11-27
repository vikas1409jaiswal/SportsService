import React, { useEffect, useState } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { LikeButton } from "./LikeButton";
import { ShareButton } from "./ShareButton";
import { SubscribeButton } from "./SubscribeButton";

import "./YouTubeInteraction.scss";

interface YouTubeInteractionProps {
  show: boolean;
  duration?: number;
  onComplete?: () => void;
}

export const YouTubeInteraction: React.FC<YouTubeInteractionProps> = ({
  show,
  duration = 6000,
  onComplete,
}) => {
  const [currentStep, setCurrentStep] = useState<'like' | 'share' | 'subscribe' | 'complete'>('like');
  const [isVisible, setIsVisible] = useState(false);

  useEffect(() => {
    if (show) {
      setIsVisible(true);
      setCurrentStep('like');
      
      const timeouts = [
        // Like animation
        setTimeout(() => setCurrentStep('share'), 1500),
        // Share animation  
        setTimeout(() => setCurrentStep('subscribe'), 3000),
        // Subscribe animation
        setTimeout(() => setCurrentStep('complete'), 4500),
        // Hide animation
        setTimeout(() => {
          setIsVisible(false);
          onComplete?.();
        }, duration),
      ];

      return () => timeouts.forEach(clearTimeout);
    }
  }, [show, duration, onComplete]);

  const containerVariants = {
    hidden: { 
      opacity: 0, 
      scale: 0.5,
      y: 100 
    },
    visible: { 
      opacity: 1, 
      scale: 1,
      y: 0,
      transition: {
        duration: 0.6,
        ease: "easeOut",
        staggerChildren: 0.2
      }
    },
    exit: {
      opacity: 0,
      scale: 0.5,
      y: -100,
      transition: {
        duration: 0.4,
        ease: "easeIn"
      }
    }
  };

  const backgroundVariants = {
    hidden: { opacity: 0 },
    visible: { 
      opacity: 1,
      transition: { duration: 0.3 }
    },
    exit: { 
      opacity: 0,
      transition: { duration: 0.3 }
    }
  };

  if (!show && !isVisible) return null;

  return (
    <AnimatePresence>
      {isVisible && (
        <motion.div
          className="youtube-interaction-overlay"
          variants={backgroundVariants}
          initial="hidden"
          animate="visible"
          exit="exit"
        >
          <motion.div
            className="youtube-interaction-container"
            variants={containerVariants}
            initial="hidden"
            animate="visible"
            exit="exit"
          >
            <motion.div 
              className="interaction-header"
              initial={{ opacity: 0, y: -20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ delay: 0.3, duration: 0.5 }}
            >
              <h2>Enjoying the content?</h2>
              <p>Support the channel with your engagement!</p>
            </motion.div>

            <div className="interaction-buttons">
              <LikeButton 
                isActive={currentStep === 'like'} 
                isCompleted={['share', 'subscribe', 'complete'].includes(currentStep)}
              />
              <ShareButton 
                isActive={currentStep === 'share'} 
                isCompleted={['subscribe', 'complete'].includes(currentStep)}
              />
              <SubscribeButton 
                isActive={currentStep === 'subscribe'} 
                isCompleted={currentStep === 'complete'}
              />
            </div>

            <motion.div 
              className="interaction-footer"
              initial={{ opacity: 0 }}
              animate={{ opacity: 1 }}
              transition={{ delay: 1, duration: 0.5 }}
            >
              <p>Your support helps us create more amazing cricket content! 🏏</p>
            </motion.div>
          </motion.div>
        </motion.div>
      )}
    </AnimatePresence>
  );
};