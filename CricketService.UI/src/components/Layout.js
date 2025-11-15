import React, { Component } from "react";

import "./Layout.scss";

export class Layout extends Component {
  static displayName = Layout.name;

  render() {
    return (
      <div className="layout-container">
         {this.props.children}
      </div>
    );
  }
}
