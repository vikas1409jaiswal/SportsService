import React from "react";
import "./SidePane.scss";

interface SidePaneProps {
  open: boolean;
  title: string;
  subtitle?: string;
  onClose: () => void;
  children?: React.ReactNode;
}

export const SidePane: React.FC<SidePaneProps> = ({ open, title, subtitle, onClose, children }) => {
  if (!open) return null;
  return (
    <div className="side-pane-overlay" onClick={onClose}>
      <div className={`side-pane${open ? " open" : ""}`} onClick={e => e.stopPropagation()}>
        <div className="side-pane-header">
          <div>
            <h3 className="side-pane-title">{title}</h3>
            {subtitle && <div className="side-pane-subtitle">{subtitle}</div>}
          </div>
          <button className="close-button" onClick={onClose}>
            ×
          </button>
        </div>
        <div className="side-pane-content">{children}</div>
      </div>
    </div>
  );
};
