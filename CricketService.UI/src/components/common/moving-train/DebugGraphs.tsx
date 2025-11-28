import React from "react";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
  ChartOptions,
} from 'chart.js';
import { Line } from 'react-chartjs-2';

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend
);

export interface SpeedDataPoint {
  time: number;
  speed: number;
  position: number;
}

interface DebugGraphsProps {
  dataPoints: SpeedDataPoint[];
  currentTime: number;
  currentSpeed: number;
  trackLength: number;
}

export const DebugGraphs: React.FC<DebugGraphsProps> = ({
  dataPoints,
  currentTime,
  currentSpeed,
  trackLength
}) => {
  // Prepare data for Speed vs Time graph
  const speedTimeData = {
    labels: dataPoints.map(point => point.time.toFixed(2)),
    datasets: [
      {
        label: 'Speed',
        data: dataPoints.map(point => point.speed),
        borderColor: '#00ff00',
        backgroundColor: 'rgba(0, 255, 0, 0.2)',
        borderWidth: 2,
        pointBackgroundColor: dataPoints.map((_, index) => 
          index === dataPoints.length - 1 ? '#ff0000' : 'rgba(0, 255, 0, 0.6)'
        ),
        pointBorderColor: dataPoints.map((_, index) => 
          index === dataPoints.length - 1 ? '#ffffff' : '#00ff00'
        ),
        pointRadius: dataPoints.map((_, index) => 
          index === dataPoints.length - 1 ? 6 : 3
        ),
        pointBorderWidth: dataPoints.map((_, index) => 
          index === dataPoints.length - 1 ? 2 : 1
        ),
        tension: 0.3,
      }
    ]
  };

  // Prepare data for Speed vs Position graph
  const speedPositionData = {
    labels: dataPoints.map(point => Math.abs(point.position).toFixed(0)),
    datasets: [
      {
        label: 'Speed',
        data: dataPoints.map(point => point.speed),
        borderColor: '#00ffff',
        backgroundColor: 'rgba(0, 255, 255, 0.2)',
        borderWidth: 2,
        pointBackgroundColor: dataPoints.map((_, index) => 
          index === dataPoints.length - 1 ? '#ff0000' : 'rgba(0, 255, 255, 0.6)'
        ),
        pointBorderColor: dataPoints.map((_, index) => 
          index === dataPoints.length - 1 ? '#ffffff' : '#00ffff'
        ),
        pointRadius: dataPoints.map((_, index) => 
          index === dataPoints.length - 1 ? 6 : 3
        ),
        pointBorderWidth: dataPoints.map((_, index) => 
          index === dataPoints.length - 1 ? 2 : 1
        ),
        tension: 0.3,
      }
    ]
  };

  const commonOptions: ChartOptions<'line'> = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        display: false,
      },
      tooltip: {
        backgroundColor: 'rgba(0, 0, 0, 0.8)',
        titleColor: '#ffffff',
        bodyColor: '#ffffff',
        borderColor: '#555',
        borderWidth: 1,
        callbacks: {
          label: function(context) {
            return `Speed: ${context.parsed.y.toFixed(3)}x`;
          }
        }
      }
    },
    scales: {
      x: {
        ticks: {
          color: '#888',
          font: {
            size: 9,
          },
          maxTicksLimit: 6,
        },
        grid: {
          color: '#333',
          drawBorder: false,
        },
      },
      y: {
        ticks: {
          color: '#888',
          font: {
            size: 9,
          },
        },
        grid: {
          color: '#333',
          drawBorder: false,
        },
      },
    },
    elements: {
      point: {
        hoverRadius: 6,
      },
      line: {
        tension: 0.3,
      },
    },
    interaction: {
      intersect: false,
      mode: 'index',
    },
  };

  const timeGraphOptions: ChartOptions<'line'> = {
    ...commonOptions,
    scales: {
      ...commonOptions.scales,
      x: {
        ...commonOptions.scales?.x,
        title: {
          display: false,
          text: 'Time (s)',
          color: '#888',
          font: {
            size: 10,
          },
        },
      },
      y: {
        ...commonOptions.scales?.y,
        title: {
          display: false,
          text: 'Speed',
          color: '#888',
          font: {
            size: 10,
          },
        },
      },
    },
  };

  const positionGraphOptions: ChartOptions<'line'> = {
    ...commonOptions,
    scales: {
      ...commonOptions.scales,
      x: {
        ...commonOptions.scales?.x,
        title: {
          display: false,
          text: 'Position (px)',
          color: '#888',
          font: {
            size: 10,
          },
        },
      },
      y: {
        ...commonOptions.scales?.y,
        title: {
          display: false,
          text: 'Speed',
          color: '#888',
          font: {
            size: 10,
          },
        },
      },
    },
  };

  const currentPoint = dataPoints[dataPoints.length - 1];

  return (
    <div style={{ marginTop: '15px', borderTop: '1px solid #444', paddingTop: '10px' }}>
      <h5 style={{ margin: '0 0 15px 0', color: '#00ffff', fontSize: '12px' }}>
        📊 Real-time Performance Graphs
      </h5>
      
      {/* Speed vs Time Chart */}
      <div style={{ marginBottom: '20px' }}>
        <div style={{ 
          fontSize: '11px', 
          marginBottom: '8px', 
          color: '#00ff00',
          fontWeight: 'bold'
        }}>
          🕒 Speed vs Time
        </div>
        <div style={{ width: '100%', height: '120px' }}>
          {dataPoints.length > 1 ? (
            <Line data={speedTimeData} options={timeGraphOptions} />
          ) : (
            <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', height: '100%', color: '#888', fontSize: '11px' }}>
              Collecting data...
            </div>
          )}
        </div>
      </div>

      {/* Speed vs Position Chart */}
      <div style={{ marginBottom: '15px' }}>
        <div style={{ 
          fontSize: '11px', 
          marginBottom: '8px', 
          color: '#00ffff',
          fontWeight: 'bold'
        }}>
          📍 Speed vs Position
        </div>
        <div style={{ width: '100%', height: '120px' }}>
          {dataPoints.length > 1 ? (
            <Line data={speedPositionData} options={positionGraphOptions} />
          ) : (
            <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', height: '100%', color: '#888', fontSize: '11px' }}>
              Collecting data...
            </div>
          )}
        </div>
      </div>

      {/* Legend and Stats */}
      <div style={{ 
        fontSize: '10px', 
        color: '#888', 
        borderTop: '1px solid #333',
        paddingTop: '8px',
        display: 'flex',
        justifyContent: 'space-between',
        flexWrap: 'wrap',
        gap: '8px'
      }}>
        <div>
          <span style={{ color: '#ff0000' }}>🔴</span> Current Position
        </div>
        <div>
          <span style={{ color: '#00ff00' }}>🟢</span> Speed-Time Curve
        </div>
        <div>
          <span style={{ color: '#00ffff' }}>🔵</span> Speed-Position Curve
        </div>
        <div style={{ color: '#fff', fontSize: '9px' }}>
          Data Points: {dataPoints.length}
        </div>
      </div>
      
      {/* Performance stats */}
      {currentPoint && (
        <div style={{
          marginTop: '8px',
          padding: '6px',
          backgroundColor: 'rgba(0,255,255,0.1)',
          borderRadius: '4px',
          fontSize: '9px',
          color: '#00ffff'
        }}>
          <strong>📈 Live Metrics:</strong> Speed: {currentSpeed.toFixed(3)}x | 
          Position: {Math.abs(currentPoint.position).toFixed(1)}px | 
          Progress: {((Math.abs(currentPoint.position) / trackLength) * 100).toFixed(1)}%
        </div>
      )}
    </div>
  );
};