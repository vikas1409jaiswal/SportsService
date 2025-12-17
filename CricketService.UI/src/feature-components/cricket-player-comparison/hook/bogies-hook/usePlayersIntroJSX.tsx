import React from "react";
import { motion } from "framer-motion";
import "./PlayersIntroJSX.scss";

// 3D animated intro for player1 vs player2
const usePlayersIntroJSX = (player1Name: string, player2Name: string): JSX.Element => {
  return (
    <motion.div 
      className="players-intro-3d-container"
      initial={{ opacity: 0 }}
      animate={{ opacity: 1 }}
      transition={{ duration: 0.8 }}
    >
      {/* Background animated elements */}
      <div className="background-elements">
        {[...Array(20)].map((_, i) => (
          <motion.div
            key={i}
            className="floating-element"
            initial={{ 
              x: Math.random() * 800,
              y: Math.random() * 800,
              scale: 0,
              rotate: 0
            }}
            animate={{ 
              x: Math.random() * 800,
              y: Math.random() * 800,
              scale: [0, 1, 0],
              rotate: 360
            }}
            transition={{
              duration: 3 + Math.random() * 4,
              repeat: Infinity,
              repeatType: "reverse",
              delay: Math.random() * 2
            }}
          />
        ))}
      </div>

      {/* Main content */}
      <div className="players-intro-content">
        <motion.div 
          className="title-section"
          initial={{ y: -100, opacity: 0 }}
          animate={{ y: 0, opacity: 1 }}
          transition={{ duration: 1, delay: 0.3 }}
        >
          <motion.h1
            className="comparison-title"
            animate={{
              backgroundPosition: ["0% 50%", "100% 50%", "0% 50%"]
            }}
            transition={{ duration: 3, repeat: Infinity }}
          >
            FACE-OFF
          </motion.h1>
          <motion.div 
            className="subtitle"
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            transition={{ delay: 1, duration: 0.8 }}
          >
            Battle of the Champions
          </motion.div>
        </motion.div>

        <div className="players-intro-3d vertical-layout">
          <motion.div
            className="player-container left"
            initial={{ x: -300, rotateY: -90, opacity: 0 }}
            animate={{ 
              x: 0, 
              rotateY: 0, 
              opacity: 1,
              z: [0, 50, 0]
            }}
            transition={{ 
              duration: 1.5, 
              delay: 0.5,
              type: "spring",
              stiffness: 100
            }}
          >
            <motion.span 
              className="player-name left"
              whileHover={{ 
                scale: 1.1, 
                rotateY: 10,
                boxShadow: "0 20px 60px rgba(25, 118, 210, 0.6)"
              }}
              animate={{
                rotateY: [0, 5, -5, 0],
                scale: [1, 1.05, 1]
              }}
              transition={{ 
                duration: 2,
                repeat: Infinity,
                repeatType: "reverse"
              }}
            >
              {player1Name}
            </motion.span>
            <motion.div 
              className="player-glow left"
              animate={{
                scale: [1, 1.2, 1],
                opacity: [0.3, 0.7, 0.3]
              }}
              transition={{
                duration: 2,
                repeat: Infinity,
                repeatType: "reverse"
              }}
            />
          </motion.div>

          <motion.div
            className="vs-container"
            initial={{ scale: 0, rotateX: 180 }}
            animate={{ 
              scale: 1, 
              rotateX: 0,
              rotateY: [0, 360]
            }}
            transition={{ 
              duration: 2, 
              delay: 1,
              rotateY: { duration: 4, repeat: Infinity, ease: "linear" }
            }}
          >
            <motion.span 
              className="vs-text"
              whileHover={{ 
                scale: 1.3,
                textShadow: "0 0 30px #ff1744"
              }}
            >
              VS
            </motion.span>
            <motion.div 
              className="vs-lightning"
              animate={{
                opacity: [0, 1, 0],
                scale: [0.8, 1.2, 0.8]
              }}
              transition={{
                duration: 0.5,
                repeat: Infinity,
                repeatType: "reverse"
              }}
            />
          </motion.div>

          <motion.div
            className="player-container right"
            initial={{ x: 300, rotateY: 90, opacity: 0 }}
            animate={{ 
              x: 0, 
              rotateY: 0, 
              opacity: 1,
              z: [0, 50, 0]
            }}
            transition={{ 
              duration: 1.5, 
              delay: 0.7,
              type: "spring",
              stiffness: 100
            }}
          >
            <motion.span 
              className="player-name right"
              whileHover={{ 
                scale: 1.1, 
                rotateY: -10,
                boxShadow: "0 20px 60px rgba(255, 23, 68, 0.6)"
              }}
              animate={{
                rotateY: [0, -5, 5, 0],
                scale: [1, 1.05, 1]
              }}
              transition={{ 
                duration: 2,
                repeat: Infinity,
                repeatType: "reverse",
                delay: 0.5
              }}
            >
              {player2Name}
            </motion.span>
            <motion.div 
              className="player-glow right"
              animate={{
                scale: [1, 1.2, 1],
                opacity: [0.3, 0.7, 0.3]
              }}
              transition={{
                duration: 2,
                repeat: Infinity,
                repeatType: "reverse",
                delay: 0.5
              }}
            />
          </motion.div>
        </div>

      </div>

    </motion.div>
  );
};

export default usePlayersIntroJSX;
