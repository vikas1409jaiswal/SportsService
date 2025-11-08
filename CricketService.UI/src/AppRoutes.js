import { Home } from "./components/Home";
import { CricketHomePage } from "./components/CricketHomePage";
import { CricketTeamRecords } from "./components/CricketComponents/CricketTeamRecords/CricketTeamRecords";
import { CricketPlayerRecords } from "./components/CricketComponents/CricketPlayerRecords/CricketPlayerRecords";
import { CricketAnimations } from "./components/CricketComponents/CricketMatchRecords/CricketAnimations";

const AppRoutes = [
  {
    index: true,
    element: <Home />,
  },
  {
    path: "/cricket",
    element: <CricketHomePage />,
  },
  {
    path: "/cricket-team-records",
    element: <CricketTeamRecords />,
  },
  {
    path: "/cricket-player-records",
    element: <CricketPlayerRecords />,
  },
  {
    path: "/cricket-match-records",
    element: <CricketAnimations />,
  },
];

export default AppRoutes;
