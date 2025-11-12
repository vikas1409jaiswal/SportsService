import { CricketMatchesTable } from "./feature-components/cricket-matches-analysis/CricketMatchesTable";
import { CricketHomePage } from "./components/CricketHomePage";
import { CricketPlayersProfile } from "./components/cricket-players/CricketPlayersProfile";
import { CricketRecords } from "./feature-components/cricket-records/CricketRecords";
import { CricketPlayerComparison } from "./components/cricket-player-comparison/CricketPlayerComparison";
import { CricketSquads } from "./components/cricket-squads/CricketSquads";
import { CricketMatchHomePage } from "./components/cricket-matches/CricketMatchHomePage";
import { TestCricketMatch } from "./components/cricket-matches/TestCricketMatch";
import { H2HRecords } from "./components/cricket-head-to-head/H2HRecords";
import { ThreeDGraphics } from "./3d-graphix/ThreeDGraphics";
import { SvgDemo } from "./components/SvgDemo";

const AppRoutes = [
  {
    index: true,
    element: <CricketMatchesTable />,
  },
  {
    path: "/cricket",
    element: <CricketHomePage />,
  },
  {
    path: "/players",
    element: <CricketPlayersProfile />,
  },
  {
    path: "/records",
    element: <CricketRecords />,
  },
  {
    path: "/comparison",
    element: <CricketPlayerComparison />,
  },
  {
    path: "/squads",
    element: <CricketSquads />,
  },
  {
    path: "/matches",
    element: <CricketMatchHomePage />,
  },
  {
    path: "/test-matches",
    element: <TestCricketMatch />,
  },
  {
    path: "/h2h",
    element: <H2HRecords />,
  },
  {
    path: "/3d",
    element: <ThreeDGraphics />,
  },
  {
    path: "/svg-demo",
    element: <SvgDemo />,
  },
];

export default AppRoutes;
