import React, { useState, useRef, useEffect } from "react";
import { PlayerImageContainer } from "../../common/PlayerImageContainer";
import { PlayerExtraInfo } from "../../common/PlayerExtraInfo";
import "./PlayerProfileContainer.scss";

interface PlayerProfileContainerProps {
  playerHref: string;
  teamName: string;
  customHeight?: number;
  scaleTeamCylinder?: number;
  className?: string;
}

export const PlayerProfileContainer: React.FC<PlayerProfileContainerProps> = ({
  playerHref,
  teamName,
  customHeight = 845,
  scaleTeamCylinder = 0.85,
  className = "",
}) => {
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  const [isSidePaneOpen, setIsSidePaneOpen] = useState(false);
  const [menuPosition, setMenuPosition] = useState({ top: 0, right: 0 });
  const dropdownRef = useRef<HTMLDivElement>(null);
  const menuButtonRef = useRef<HTMLButtonElement>(null);
  const dropdownMenuRef = useRef<HTMLDivElement>(null);

  // Close dropdown when clicking outside
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      const target = event.target as Node;
      if (
        dropdownRef.current &&
        !dropdownRef.current.contains(target) &&
        (!dropdownMenuRef.current || !dropdownMenuRef.current.contains(target))
      ) {
        setIsDropdownOpen(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

  const handleMenuClick = () => {
    if (menuButtonRef.current) {
      const rect = menuButtonRef.current.getBoundingClientRect();
      setMenuPosition({
        top: rect.bottom + 5,
        right: window.innerWidth - rect.right
      });
    }
    setIsDropdownOpen(!isDropdownOpen);
  };

  const handleBattingInningsClick = () => {
    setIsDropdownOpen(false);
    setIsSidePaneOpen(true);
  };

  const handleCloseSidePane = () => {
    setIsSidePaneOpen(false);
  };

  return (
    <>
      <div className={`player-container player-profile-container ${className}`.trim()}>
        <div className="three-dot-menu" ref={dropdownRef}>
          <button ref={menuButtonRef} className="menu-button" onClick={handleMenuClick} aria-label="Open menu">
            {/* Three-dot vertical icon using SVG for clarity and accessibility */}
            <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
              <circle cx="12" cy="5" r="2" fill="#333" />
              <circle cx="12" cy="12" r="2" fill="#333" />
              <circle cx="12" cy="19" r="2" fill="#333" />
            </svg>
          </button>
        </div>
        <PlayerImageContainer
          playerHref={playerHref}
          selectedRowIndex={0}
          teamName={teamName}
          hideRotatingCircle
          skipAnimation
          extraInfo={[
            <PlayerExtraInfo key="player-extra-info" playerHref={playerHref} />,
          ]}
          customHeight={customHeight}
          scaleTeamCylinder={scaleTeamCylinder}
        />
      </div>
      {isDropdownOpen && (
        <div
          ref={dropdownMenuRef}
          className="dropdown-menu-fixed"
          style={{
            position: 'fixed',
            top: `${menuPosition.top}px`,
            right: `${menuPosition.right}px`,
            background: '#fffbe6',
            border: '2px solid #ff9800',
            borderRadius: '8px',
            boxShadow: '0 4px 12px rgba(0, 0, 0, 0.15)',
            minWidth: '180px',
            zIndex: 9999,
            pointerEvents: 'auto'
          }}
        >
          <button
            className="dropdown-item"
            onClick={handleBattingInningsClick}
            style={{
              padding: '12px 16px',
              border: 'none',
              background: 'none',
              width: '100%',
              textAlign: 'left',
              cursor: 'pointer',
              fontSize: '14px'
            }}
          >
            All Batting Innings
          </button>
        </div>
      )}
      
      {isSidePaneOpen && (
        <div className="side-pane-overlay" onClick={handleCloseSidePane}>
          <div className={`side-pane ${isSidePaneOpen ? 'open' : ''}`} onClick={(e) => e.stopPropagation()}>
            <div className="side-pane-header">
              <h3 className="side-pane-title">All Batting Innings</h3>
              <button className="close-button" onClick={handleCloseSidePane}>
                ×
              </button>
            </div>
            <div className="side-pane-content">
              <p>Batting innings data will be displayed here...</p>
              {/* Add your batting innings content here */}
            </div>
          </div>
        </div>
      )}
    </>
  );
};
