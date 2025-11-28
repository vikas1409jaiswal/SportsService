import React from "react";
import { MovingTrainDebugInfoProps } from "./types";
import { DebugGraphs } from "./DebugGraphs";

export const MovingTrainDebugInfo: React.FC<MovingTrainDebugInfoProps> = ({
  debugInfo,
  trackLength,
  isColumn,
  bogiesCount,
  speedFunction,
  delay,
  speedDataPoints = []
}) => (
  <div
    style={{
      position: "fixed",
      top: "10px",
      right: "10px",
      background: "rgba(0, 0, 0, 0.9)",
      color: "white",
      padding: "15px",
      borderRadius: "8px",
      fontFamily: "monospace",
      fontSize: "12px",
      zIndex: 9999,
      minWidth: "350px",
      maxWidth: "400px",
      backdropFilter: "blur(8px)",
      maxHeight: "90vh",
      overflowY: "auto",
      border: "1px solid #333",
      boxShadow: "0 4px 20px rgba(0,0,0,0.5)"
    }}
  >
    <h4 style={{ margin: "0 0 12px 0", color: "#00ff00", borderBottom: "1px solid #333", paddingBottom: "8px" }}>🚂 MovingTrain Debug Info</h4>
    
    {/* Basic Stats Grid */}
    <div style={{ 
      display: "grid", 
      gridTemplateColumns: "1fr 1fr", 
      gap: "8px", 
      marginBottom: "15px",
      fontSize: "11px"
    }}>
      <div><strong>Status:</strong> {debugInfo.isAnimating ? "🟢 Running" : "🔴 Stopped"}</div>
      <div><strong>Position:</strong> {(debugInfo.currentPosition || 0).toFixed(1)}px</div>
      <div><strong>Speed:</strong> {(debugInfo.currentSpeed || 0).toFixed(2)}x</div>
      <div><strong>Progress:</strong> {((debugInfo.timeProgress || 0) * 100).toFixed(1)}%</div>
      <div><strong>Frame:</strong> {debugInfo.frameIndex || 0}/{debugInfo.totalFrames || 0}</div>
      <div><strong>Duration:</strong> {debugInfo.animationDuration || 0}s</div>
      <div><strong>Distance:</strong> {(debugInfo.cumulativeDistance || 0).toFixed(1)}px</div>
      <div><strong>Bogies:</strong> {bogiesCount}</div>
    </div>

    {/* Config Info */}
    <div style={{
      backgroundColor: "rgba(255,255,255,0.05)",
      padding: "8px",
      borderRadius: "4px",
      marginBottom: "10px",
      fontSize: "10px"
    }}>
      <div><strong>Track Length:</strong> {trackLength.toLocaleString()}px</div>
      <div><strong>Direction:</strong> {isColumn ? "Vertical ↕" : "Horizontal ↔"}</div>
      <div><strong>Speed Func:</strong> {speedFunction ? "✅ Custom" : "❌ Linear"}</div>
      <div><strong>Delay:</strong> {delay || 0}s</div>
    </div>
    
    {speedDataPoints.length > 0 && (
      <DebugGraphs
        dataPoints={speedDataPoints}
        currentTime={debugInfo.timeProgress * (debugInfo.animationDuration || 1)}
        currentSpeed={debugInfo.currentSpeed}
        trackLength={trackLength}
      />
    )}
  </div>
);