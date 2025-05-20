import React, { Component } from "react";
import { Route, Routes } from "react-router-dom";
import AppRoutes from "./AppRoutes";
import { Layout } from "./components/Layout";
import { QueryClient, QueryClientProvider } from "react-query";
import { CricketHomePage } from "./components/CricketHomePage";

import "./App.scss";
import { CricketFormat } from "./models/enums/CricketFormat";
import { CricketPlayersProfile } from "./components/cricket-players/CricketPlayersProfile";
import { CricketRecords } from "./components/cricket-records/CricketRecords";
import { CricketSquads } from "./components/cricket-squads/CricketSquads";
import { CricketSchedule } from "./components/matches-schedule/CricketSchedule";
import { CricketersBirthDays } from "./components/cricket-records/CricketersBirthDays";
import { SvgDemo } from "./components/SvgDemo";
import { ThreeDGraphics } from "./3d-graphix/ThreeDGraphics";
import { CricketPlayerComparison } from "./components/cricket-player-comparison/CricketPlayerComparison";

const client = new QueryClient();

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <QueryClientProvider client={client}>
        {/* <CricketPlayerComparison /> */}
        {/* <CricketSquads /> */}
        <CricketHomePage />
        {/* <CricketRecords /> */}
        {/* <CricketersBirthDays /> */}
        {/* <CricketSchedule /> */}
        {/* <Layout>
          <Routes>
            {AppRoutes.map((route, index) => {
              const { element, ...rest } = route;
              return <Route key={index} {...rest} element={element} />;
            })}
          </Routes>
        </Layout>  */}
        {/* <SvgDemo /> */}
        {/* <ThreeDGraphics /> */}
      </QueryClientProvider>
    );
  }
}
