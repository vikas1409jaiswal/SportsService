import React, { useState, useEffect, useMemo, useRef } from 'react';
import { useFetchAllPlayersInfo, PlayerInfo } from './hook/useFetchAllPlayersInfo';
import { Container, Row, Col, Alert, Spinner, FormGroup, Label, Input, Button } from 'reactstrap';
import { Modal } from '../../components/common/Modal';

import './PlayerH2HSelection.scss';

interface PlayerH2HSelectionProps {
  onPlayersSelected: (player1Uuid: string, player2Uuid: string) => void;
  isMenuOpened: boolean;
}

export const PlayerH2HSelection: React.FC<PlayerH2HSelectionProps> = ({
  onPlayersSelected,
  isMenuOpened
}) => {
  const [isModalOpen, setIsModalOpen] = useState<boolean>(false);
  const [selectedPlayer1, setSelectedPlayer1] = useState<string>('');
  const [selectedPlayer2, setSelectedPlayer2] = useState<string>('');
  const [searchTerm1, setSearchTerm1] = useState<string>('');
  const [searchTerm2, setSearchTerm2] = useState<string>('');
  const [showDropdown1, setShowDropdown1] = useState<boolean>(false);
  const [showDropdown2, setShowDropdown2] = useState<boolean>(false);
  
  const dropdown1Ref = useRef<HTMLDivElement>(null);
  const dropdown2Ref = useRef<HTMLDivElement>(null);
  
  const { data: allPlayers, isLoading, error } = useFetchAllPlayersInfo();

  // Sort players alphabetically and create filtered lists
  const sortedPlayers = useMemo(() => {
    return allPlayers?.sort((a, b) => a.fullName.localeCompare(b.fullName)) || [];
  }, [allPlayers]);

  // Filter players for dropdown 1 based on search term
  const filteredPlayers1 = useMemo(() => {
    if (!searchTerm1) return sortedPlayers;
    return sortedPlayers.filter(player => 
      player.fullName.toLowerCase().includes(searchTerm1.toLowerCase())
    );
  }, [sortedPlayers, searchTerm1]);

  // Filter players for dropdown 2 (exclude selected player 1 and filter by search term)
  const filteredPlayers2 = useMemo(() => {
    const availablePlayers = sortedPlayers.filter(player => player.uuid !== selectedPlayer1);
    if (!searchTerm2) return availablePlayers;
    return availablePlayers.filter(player => 
      player.fullName.toLowerCase().includes(searchTerm2.toLowerCase())
    );
  }, [sortedPlayers, selectedPlayer1, searchTerm2]);

  // Handle click outside to close dropdowns
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (dropdown1Ref.current && !dropdown1Ref.current.contains(event.target as Node)) {
        setShowDropdown1(false);
      }
      if (dropdown2Ref.current && !dropdown2Ref.current.contains(event.target as Node)) {
        setShowDropdown2(false);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  // Handle modal close
  const handleModalClose = () => {
    setIsModalOpen(false);
  };

  // Handle successful player selection
  const handleConfirmSelection = () => {
    if (selectedPlayer1 && selectedPlayer2) {
      onPlayersSelected(selectedPlayer1, selectedPlayer2);
      setIsModalOpen(false);
    }
  };

  const handlePlayer1Select = (player: PlayerInfo) => {
    setSelectedPlayer1(player.uuid);
    setSearchTerm1(player.fullName);
    setShowDropdown1(false);
    
    // Reset player 2 if it's the same as player 1
    if (selectedPlayer2 === player.uuid) {
      setSelectedPlayer2('');
      setSearchTerm2('');
    }
  };

  const handlePlayer2Select = (player: PlayerInfo) => {
    setSelectedPlayer2(player.uuid);
    setSearchTerm2(player.fullName);
    setShowDropdown2(false);
  };

  const handleSearchTerm1Change = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSearchTerm1(event.target.value);
    setShowDropdown1(true);
    if (event.target.value === '') {
      setSelectedPlayer1('');
    }
  };

  const handleSearchTerm2Change = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSearchTerm2(event.target.value);
    setShowDropdown2(true);
    if (event.target.value === '') {
      setSelectedPlayer2('');
    }
  };

  if (!isMenuOpened) return null;

  return (
    <>
      {/* Compact trigger button */}
      <div className="player-selection-trigger">
        <Button 
          color="primary" 
          size="lg"
          onClick={() => setIsModalOpen(true)}
          className="mb-3"
        >
          🏏 Select Players for Comparison
        </Button>
      </div>

      {/* Modal for player selection */}
      <Modal
        isOpen={isModalOpen}
        onClose={handleModalClose}
        title="Select Players for Head-to-Head Comparison"
        width="600px"
        maxHeight="80vh"
        className="player-selection-modal"
        footer={
          <div className="d-flex justify-content-between align-items-center w-100">
            <Button color="secondary" onClick={handleModalClose}>
              Cancel
            </Button>
            <Button 
              color="primary" 
              onClick={handleConfirmSelection}
              disabled={!selectedPlayer1 || !selectedPlayer2}
            >
              Compare Players
            </Button>
          </div>
        }
      >
        {isLoading ? (
          <div className="text-center p-4">
            <Spinner color="primary" /> Loading players...
          </div>
        ) : error ? (
          <Alert color="danger">
            Error loading players: {error.message}
          </Alert>
        ) : (
          <Container fluid>
            <Row>
              <Col md={6}>
                <FormGroup>
                  <Label>Player 1:</Label>
                  <div className="search-dropdown-container" ref={dropdown1Ref}>
                    <Input
                      type="text"
                      placeholder="Search and select first player..."
                      value={searchTerm1}
                      onChange={handleSearchTerm1Change}
                      onFocus={() => setShowDropdown1(true)}
                      className="search-input"
                    />
                    {showDropdown1 && filteredPlayers1.length > 0 && (
                      <div className="dropdown-menu show search-results">
                        {filteredPlayers1.slice(0, 10).map((player: PlayerInfo) => (
                          <button
                            key={player.uuid}
                            className="dropdown-item"
                            type="button"
                            onClick={() => handlePlayer1Select(player)}
                          >
                            {player.fullName}
                          </button>
                        ))}
                        {filteredPlayers1.length > 10 && (
                          <div className="dropdown-item-text text-muted small">
                            ... and {filteredPlayers1.length - 10} more. Keep typing to narrow results.
                          </div>
                        )}
                      </div>
                    )}
                  </div>
                </FormGroup>
              </Col>

              <Col md={6}>
                <FormGroup>
                  <Label>Player 2:</Label>
                  <div className="search-dropdown-container" ref={dropdown2Ref}>
                    <Input
                      type="text"
                      placeholder="Search and select second player..."
                      value={searchTerm2}
                      onChange={handleSearchTerm2Change}
                      onFocus={() => setShowDropdown2(true)}
                      disabled={!selectedPlayer1}
                      className="search-input"
                    />
                    {showDropdown2 && filteredPlayers2.length > 0 && selectedPlayer1 && (
                      <div className="dropdown-menu show search-results">
                        {filteredPlayers2.slice(0, 10).map((player: PlayerInfo) => (
                          <button
                            key={player.uuid}
                            className="dropdown-item"
                            type="button"
                            onClick={() => handlePlayer2Select(player)}
                          >
                            {player.fullName}
                          </button>
                        ))}
                        {filteredPlayers2.length > 10 && (
                          <div className="dropdown-item-text text-muted small">
                            ... and {filteredPlayers2.length - 10} more. Keep typing to narrow results.
                          </div>
                        )}
                      </div>
                    )}
                  </div>
                </FormGroup>
              </Col>
            </Row>

            {selectedPlayer1 && selectedPlayer2 && (
              <Alert color="success" className="mt-3">
                <strong>Ready to Compare:</strong>{' '}
                <span className="text-primary font-weight-bold">
                  {sortedPlayers.find(p => p.uuid === selectedPlayer1)?.fullName}
                </span>{' '}
                vs{' '}
                <span className="text-info font-weight-bold">
                  {sortedPlayers.find(p => p.uuid === selectedPlayer2)?.fullName}
                </span>
              </Alert>
            )}
          </Container>
        )}
      </Modal>
    </>
  );
};