import React from "react";
import "./CenteredSpinner.scss";

const CenteredSpinner: React.FC = () => (
  <div className="centered-spinner-overlay">
    <div className="centered-spinner"></div>
  </div>
);

export default CenteredSpinner;
