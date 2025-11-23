import React, { useState, useEffect, useMemo, useCallback } from "react";
import "./ReorderModal.scss";
const NON_DRAGGABLE_STATS = ["Player Href", "Team Name", "Span"];
interface ReorderModalProps {
  isOpen: boolean;
  onClose: () => void;
  rows: any[];
  onSave?: (reorderedStats: string[]) => void;
  currentOrder?: string[];
}

export const ReorderModal: React.FC<ReorderModalProps> = ({ isOpen, onClose, rows, onSave, currentOrder }) => {
  const [reorderedStats, setReorderedStats] = useState<string[]>([]);
  const [draggedIndex, setDraggedIndex] = useState<number | null>(null);

  // Get stat names in order as in first row's data using useMemo for stable reference
  const statsNames = useMemo(() => {
    if (rows && rows.length > 0 && Array.isArray(rows[0].data)) {
      return rows[0].data.map((item: { key: string }) => item.key);
    }
    return [];
  }, [rows]);

  // Initialize reordered stats when modal opens
  useEffect(() => {
    if (currentOrder && currentOrder.length > 0) {
      // Use the current saved order if available
      setReorderedStats([...currentOrder]);
    } else if (statsNames.length > 0) {
      // Fallback to original order
      setReorderedStats([...statsNames]);
    }
  }, [statsNames, currentOrder]);

  const handleDragStart = useCallback((index: number) => {
    setDraggedIndex(index);
  }, []);

  const handleDragOver = useCallback((e: React.DragEvent) => {
    e.preventDefault();
    e.dataTransfer.dropEffect = "move";
  }, []);

  const handleDrop = useCallback((e: React.DragEvent, dropIndex: number) => {
    e.preventDefault();
    if (draggedIndex === null || draggedIndex === dropIndex) return;
    
    setReorderedStats(prevStats => {
      const newStats = [...prevStats];
      const [draggedItem] = newStats.splice(draggedIndex, 1);
      newStats.splice(dropIndex, 0, draggedItem);
      return newStats;
    });
    setDraggedIndex(null);
  }, [draggedIndex]);

  const handleDragEnd = useCallback(() => {
    setDraggedIndex(null);
  }, []);

  const handleSave = () => {
    console.log("Saving reordered stats:", reorderedStats);
    if (onSave) {
      onSave(reorderedStats);
    }
    onClose();
  };

  if (!isOpen) return null;

  return (
    <div className="reorder-modal-overlay" onClick={onClose}>
      <div className="reorder-modal" onClick={(e) => e.stopPropagation()}>
        <div className="reorder-modal-header">
          <h2>Reorder Statistics</h2>
          <button className="close-button" onClick={onClose}>×</button>
        </div>

        <div className="reorder-modal-content">
          <div className="table-container" style={{ maxHeight: "400px", overflowY: "auto" }}>
            {statsNames.length > 0 ? (
              <table className="reorder-table">
                <thead>
                  <tr>
                    <th>Current SN</th>
                    <th>Stats Name</th>
                  </tr>
                </thead>
                <tbody>
                  {reorderedStats.map((statName, index) => {
                    const isNonDraggable = NON_DRAGGABLE_STATS.includes(statName);
                    const dragProps = isNonDraggable
                      ? {}
                      : {
                          draggable: true,
                          onDragStart: () => handleDragStart(index),
                          onDragOver: handleDragOver,
                          onDrop: (e: React.DragEvent) => handleDrop(e, index),
                          onDragEnd: handleDragEnd
                        };
                    
                    return (
                      <tr
                        key={statName}
                        {...dragProps}
                        style={{
                          cursor: isNonDraggable ? "not-allowed" : "move",
                          backgroundColor: isNonDraggable
                            ? "#f0f0f0"
                            : (draggedIndex === index ? "#e3f2fd" : "#e6ffe6"),
                          userSelect: "none",
                          opacity: draggedIndex === index ? 0.5 : 1,
                          border: draggedIndex === index ? "2px dashed #1976d2" : "1px solid #eee"
                        }}
                      >
                        <td style={{ textAlign: "center", fontWeight: "bold", padding: "12px 8px" }}>{index + 1}</td>
                        <td style={{ padding: "12px" }}>
                          <span style={{ display: "flex", alignItems: "center" }}>
                            <span style={{
                              marginRight: "12px",
                              color: isNonDraggable ? "#bbb" : "#666",
                              fontSize: "16px",
                              cursor: isNonDraggable ? "not-allowed" : "grab"
                            }}>⋮⋮</span>
                            <span>{statName}</span>
                          </span>
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            ) : (
              <div style={{ padding: "32px", textAlign: "center", color: "#888" }}>
                No statistics available to reorder.
              </div>
            )}
          </div>
        </div>

        <div className="reorder-modal-footer">
          <button className="save-button" onClick={handleSave}>Save Order</button>
          <button className="cancel-button" onClick={onClose}>Cancel</button>
        </div>
      </div>
    </div>
  );
};