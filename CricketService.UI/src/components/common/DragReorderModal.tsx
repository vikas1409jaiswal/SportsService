import React, { useState, useEffect, useMemo, useCallback } from "react";
import { Modal } from "./Modal";
import "./DragReorderModal.scss";

export interface DragReorderItem {
  key: string;
  label?: string;
  isNonDraggable?: boolean;
}

interface DragReorderModalProps {
  isOpen: boolean;
  onClose: () => void;
  title?: string;
  items: DragReorderItem[];
  currentOrder?: string[];
  nonDraggableItems?: string[];
  onSave?: (reorderedItems: string[]) => void;
  onRemove?: (removedItems: string[]) => void;
  enabledBackgroundColor?: string;
  disabledBackgroundColor?: string;
  draggedBackgroundColor?: string;
  showRemoveButton?: boolean;
  removedItems?: string[];
}

export const DragReorderModal: React.FC<DragReorderModalProps> = ({
  isOpen,
  onClose,
  title = "Reorder Items",
  items,
  currentOrder,
  nonDraggableItems = [],
  onSave,
  onRemove,
  enabledBackgroundColor = "#e6ffe6",
  disabledBackgroundColor = "#f0f0f0",
  draggedBackgroundColor = "#e3f2fd",
  showRemoveButton = true,
  removedItems: initialRemovedItems = []
}) => {
  const [reorderedItems, setReorderedItems] = useState<string[]>([]);
  const [draggedIndex, setDraggedIndex] = useState<number | null>(null);
  const [removedItems, setRemovedItems] = useState<string[]>(initialRemovedItems);

  // Get item keys from items array
  const itemKeys = useMemo(() => {
    return items.map(item => item.key);
  }, [items]);

  // Initialize reordered items when modal opens
  useEffect(() => {
    let initialItems: string[];
    
    if (currentOrder && currentOrder.length > 0) {
      // Use the current saved order if available
      initialItems = [...currentOrder];
    } else if (itemKeys.length > 0) {
      // Fallback to original order
      initialItems = [...itemKeys];
    } else {
      initialItems = [];
    }
    
    // Filter out removed items
    const filteredItems = initialItems.filter(item => !removedItems.includes(item));
    setReorderedItems(filteredItems);
  }, [itemKeys, currentOrder, removedItems]);

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
    
    setReorderedItems(prevItems => {
      const newItems = [...prevItems];
      const [draggedItem] = newItems.splice(draggedIndex, 1);
      newItems.splice(dropIndex, 0, draggedItem);
      return newItems;
    });
    setDraggedIndex(null);
  }, [draggedIndex]);

  const handleDragEnd = useCallback(() => {
    setDraggedIndex(null);
  }, []);

  const handleRemove = useCallback((itemKey: string) => {
    if (isItemNonDraggable(itemKey)) {
      return; // Don't allow removing non-draggable items
    }
    
    setRemovedItems(prev => {
      if (prev.includes(itemKey)) {
        return prev; // Already removed
      }
      return [...prev, itemKey];
    });
    
    setReorderedItems(prev => prev.filter(item => item !== itemKey));
  }, []);

  const handleRestore = useCallback((itemKey: string) => {
    setRemovedItems(prev => prev.filter(item => item !== itemKey));
    
    // Add back to reordered items at the end
    setReorderedItems(prev => {
      if (prev.includes(itemKey)) {
        return prev; // Already in list
      }
      return [...prev, itemKey];
    });
  }, []);

  const handleSave = () => {
    console.log("Saving reordered items:", reorderedItems);
    console.log("Removed items:", removedItems);
    if (onSave) {
      onSave(reorderedItems);
    }
    if (onRemove && removedItems.length > 0) {
      onRemove(removedItems);
    }
    onClose();
  };

  const getItemLabel = (key: string) => {
    const item = items.find(item => item.key === key);
    return item?.label || key;
  };

  const isItemNonDraggable = (key: string) => {
    const item = items.find(item => item.key === key);
    return item?.isNonDraggable || nonDraggableItems.includes(key);
  };

  const footer = (
    <>
      <button className="save-button" onClick={handleSave}>Save Order</button>
      <button className="cancel-button" onClick={onClose}>Cancel</button>
    </>
  );

  return (
    <Modal
      isOpen={isOpen}
      onClose={onClose}
      title={title}
      footer={footer}
      className="drag-reorder-modal"
      maxHeight="80vh"
      width="600px"
    >
      <div className="reorder-content">
        <div className="active-items-section">
          <h3>Active Items</h3>
          <div className="table-container">
            {reorderedItems.length > 0 ? (
              <table className="reorder-table">
                <thead>
                  <tr>
                    <th>SN</th>
                    <th>Item Name</th>
                    {showRemoveButton && <th>Actions</th>}
                  </tr>
                </thead>
                <tbody>
                  {reorderedItems.map((itemKey, index) => {
                    const isNonDraggable = isItemNonDraggable(itemKey);
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
                        key={itemKey}
                        {...dragProps}
                        style={{
                          cursor: isNonDraggable ? "not-allowed" : "move",
                          backgroundColor: isNonDraggable
                            ? disabledBackgroundColor
                            : (draggedIndex === index ? draggedBackgroundColor : enabledBackgroundColor),
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
                            <span>{getItemLabel(itemKey)}</span>
                          </span>
                        </td>
                        {showRemoveButton && (
                          <td style={{ padding: "12px", textAlign: "center" }}>
                            {!isNonDraggable && (
                              <button
                                onClick={() => handleRemove(itemKey)}
                                style={{
                                  background: "#dc3545",
                                  color: "white",
                                  border: "none",
                                  borderRadius: "4px",
                                  padding: "4px 8px",
                                  cursor: "pointer",
                                  fontSize: "12px"
                                }}
                                title="Remove item"
                              >
                                ✕
                              </button>
                            )}
                          </td>
                        )}
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            ) : (
              <div style={{ padding: "32px", textAlign: "center", color: "#888" }}>
                No active items.
              </div>
            )}
          </div>
        </div>

        {removedItems.length > 0 && (
          <div className="removed-items-section" style={{ marginTop: "24px" }}>
            <h3>Removed Items</h3>
            <div className="removed-items-list">
              {removedItems.map(itemKey => (
                <div key={itemKey} className="removed-item">
                  <span>{getItemLabel(itemKey)}</span>
                  <button
                    onClick={() => handleRestore(itemKey)}
                    style={{
                      background: "#28a745",
                      color: "white",
                      border: "none",
                      borderRadius: "4px",
                      padding: "4px 8px",
                      cursor: "pointer",
                      fontSize: "12px",
                      marginLeft: "8px"
                    }}
                    title="Restore item"
                  >
                    ↩
                  </button>
                </div>
              ))}
            </div>
          </div>
        )}
      </div>
    </Modal>
  );
};