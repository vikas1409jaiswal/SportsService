import React, { ReactNode } from "react";
import "./Modal.scss";

interface ModalProps {
  isOpen: boolean;
  onClose: () => void;
  title?: string;
  children: ReactNode;
  footer?: ReactNode;
  className?: string;
  maxHeight?: string;
  width?: string;
  showCloseButton?: boolean;
}

export const Modal: React.FC<ModalProps> = ({
  isOpen,
  onClose,
  title,
  children,
  footer,
  className = "",
  maxHeight = "80vh",
  width = "auto",
  showCloseButton = true
}) => {
  if (!isOpen) return null;

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div
        className={`modal ${className}`}
        onClick={(e) => e.stopPropagation()}
        style={{
          maxHeight,
          width,
          maxWidth: "90vw"
        }}
      >
        {(title || showCloseButton) && (
          <div className="modal-header">
            {title && <h2 className="modal-title">{title}</h2>}
            {showCloseButton && (
              <button className="modal-close-button" onClick={onClose} aria-label="Close modal">
                ×
              </button>
            )}
          </div>
        )}

        <div className="modal-content" style={{ maxHeight: maxHeight ? `calc(${maxHeight} - 120px)` : "auto" }}>
          {children}
        </div>

        {footer && (
          <div className="modal-footer">
            {footer}
          </div>
        )}
      </div>
    </div>
  );
};