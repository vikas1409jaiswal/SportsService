# CricketService.UI - AI Coding Agent Instructions

## Project Overview
This is a React + TypeScript application for cricket statistics visualization and analysis. It fetches, displays, and analyzes cricket data from ESPN Cricinfo and ICC rankings, featuring player profiles, match records, team statistics, head-to-head comparisons, and interactive visualizations.

## Architecture

### Frontend Stack
- **Framework**: React 18.2 with TypeScript 4.9
- **Routing**: React Router v6 (client-side routing)
- **State Management**: React Query v3 for server state + React Context for local state
- **Styling**: SCSS/SASS + styled-components (hybrid approach)
- **UI Libraries**: Material-UI, React Table (legacy), Framer Motion, React Three Fiber
- **Build Tool**: Create React App (react-scripts 5.0)

### Backend Integration
The UI connects to a separate .NET backend API via proxy:http://localhost:5001/CricketMatch/internationalMatches?format=0
- **Development proxy**: Configured in `setupProxy.js` → proxies `/cricketplayer`, `/cricketmatch`, `/cricketteam` to `localhost:14834` (or env-specified port)
- **Direct API calls**: Some components directly call `localhost:5104` (hardcoded) for specific endpoints
- **External APIs**: ESPN Cricinfo web scraping via axios, ICC Rankings API

### Data Flow Pattern
1. **Custom hooks** (`src/hooks/espn-cricinfo-hooks/`, `src/hooks/icc-rankings-hooks/`) fetch data via React Query
2. **ESPN data is web-scraped**: Hooks use axios to fetch HTML, then parse with DOM manipulation (not cheerio despite dependency)
3. **Models** (`src/models/espn-cricinfo-models/`) define TypeScript interfaces for cricket data structures
4. **Components** consume hooks and display data with custom tables, charts, and animations

## Key Conventions

### File Organization
- **Feature-based structure**: Components grouped by cricket feature (players, matches, records, squads, rankings)
- **Shared components**: `src/components/common/` contains reusable UI (tables, loaders, dropdowns, charts)
- **Hooks separation**: ESPN hooks in `espn-cricinfo-hooks/`, ICC in `icc-rankings-hooks/`, component-specific in `src/components/CricketHooks/`
- **Naming**: PascalCase for React components, camelCase for utility functions and hooks

### Component Patterns
```typescript
// Typical component structure with React Query hook
export const CricketRecords: React.FC<Props> = ({ }) => {
  const data = useCustomESPNTable(); // Custom hook wraps useQuery
  return <IndividualPages rows={data} />;
};

// Custom hook pattern for ESPN data
export const useESPNPlayerInfo = (href: string): ESPNPlayerInfo => {
  const { data } = useQuery(
    ["player-data", href],
    () => fetchESPNPlayerInfo(href),
    {
      staleTime: 60 * 60 * 1000, // Long cache for static cricket data
      cacheTime: 60 * 60 * 1000,
      refetchOnWindowFocus: false,
    }
  );
  // Parse HTML response by creating DOM element
  const divElement = document.createElement("div");
  divElement.innerHTML = data?.data.toString();
  // Extract data via querySelector...
};
```

### Data Fetching Strategy
- **React Query** for all async data with aggressive caching (1 hour staleTime for historical cricket data)
- **Web scraping**: Fetch ESPN HTML → parse with `document.createElement()` → query with `querySelector`
- **No server-side rendering**: All data fetching happens client-side
- **Hardcoded URLs**: Backend URLs are hardcoded to `localhost:5104` throughout components (not configurable)

### Styling Approach
- **SCSS modules**: Most components have `.scss` companion files (e.g., `CricketHomePage.tsx` + `CricketHomePage.css`)
- **Styled-components**: Used in `ReactTable.tsx` and some common components
- **No unified design system**: Mix of Bootstrap, Material-UI, and custom styles
- **Common patterns**: Sticky columns, responsive tables, animated transitions with Framer Motion

## Development Workflows

### Running the Application
```bash
npm start           # Start dev server on localhost:3000 (hot reload enabled)
npm run build       # Production build to /build
npm run test        # Run Jest tests (currently CI=true)
npm run lint        # ESLint check
npm run lintfix     # Auto-format with Prettier
```

### Running with Backend Services
```bash
# Terminal 1: Start React dev server
npm start

# Terminal 2: Start local image/asset server
npm run pics        # Serves cricket images/videos from D:/CricketData on port 3013

# Terminal 3 (optional): Custom player server
npm run cricket-player-server  # Serves Players.html on port 3000

# Combined:
npm run start-both  # Runs React app + pics server concurrently
```

**Important**: Asset servers expect files at `D:/CricketData/CricketersPhotos/` (Windows path, not portable)

### Adding New Cricket Features

1. **Create models** in `src/models/espn-cricinfo-models/` with TypeScript interfaces
2. **Build custom hook** in `src/hooks/espn-cricinfo-hooks/`:
   - Use `useQuery` with descriptive key array
   - Parse ESPN HTML or call backend API
   - Set long `staleTime` for historical data
3. **Create component** in feature folder (e.g., `src/components/cricket-records/`)
4. **Add route** in `AppRoutes.js` if needed
5. **Import and render** in `App.js` (currently directly renders components, routing commented out)

### Working with Cricket Data Types

Key interfaces (see `src/models/espn-cricinfo-models/`):
- `CricketMatch`: Match details for ODI/T20I
- `CricketMatchTest`: Test match with multiple innings
- `Team`: Team scorecard with batting/bowling details
- `Batsman`, `Bowler`: Player performance in innings
- `MatchSquad`: Playing XI for both teams
- `PointsTableRow`: Tournament standings

## Configuration

### Key Config Files
- **`src/configs.ts`**: Global config for language and animation toggles
- **`src/setupProxy.js`**: Backend proxy configuration (only applies in development)
- **`tsconfig.json`**: Custom type roots include `src/@types` for `.d.ts` files

### Environment Variables
- `REACT_APP_*` for build-time vars (standard CRA convention)
- Backend URLs are **not** environment-based (hardcoded in components)

## Known Patterns & Gotchas

### Cricket-Specific Logic
- **Player name extraction**: `getNameFromHref()` in `ReusableFunctions.ts` parses ESPN URLs, supports English/Hindi translation
- **Name abbreviation**: Auto-shortens first names to initials if full name exceeds character limit
- **Format enums**: Cricket formats defined in `src/models/enums/CricketFormat` (Test=1, ODI=2, T20I=3)

### Component State
- **Global match state**: `CricketContext` in `CricketHomePage.tsx` provides `currentMatchDetails` to children
- **React Query as cache**: Used for both fetching and caching, rarely need useState for server data
- **Commented code**: `App.js` has multiple commented components (project actively being developed)

### Testing
- Jest configured but minimal test coverage
- Test files should use `.test.js` or `.spec.js` suffix
- Run in CI mode by default (`cross-env CI=true`)

## External Dependencies to Know

### Data Fetching
- `axios`: HTTP client for all API calls
- `react-query`: Server state management (v3, not latest)

### UI & Visualization
- `@tanstack/react-table`: Modern table library (v8, replacing old react-table)
- `react-table`: Legacy v7 table (still used in `ReactTable.tsx`)
- `chart.js` + `react-chartjs-2`: Charts
- `@react-three/fiber` + `@react-three/drei`: 3D graphics (ThreeDGraphics.tsx)
- `framer-motion`: Animations

### Utilities
- `html-to-image`: Screenshot generation
- `xml2js`: Parse XML responses
- `cheerio`: Server-side HTML parsing (dependency present, not used in frontend)

## TypeScript Notes
- **Strict mode enabled** in tsconfig
- **Custom type definitions**: `src/@types/XML.d.ts` for XML modules
- **Interface over type**: Prefer `interface` for data models
- **Any usage**: Acceptable for ESPN scraped data due to unpredictable HTML structure

## Development Tips

- When adding ESPN data parsing, inspect ESPN's HTML structure in DevTools first
- Use long cache times (1+ hour) for historical cricket data to reduce API load
- Test with different cricket formats (Test/ODI/T20I) as data structures vary
- Player/team UUIDs from ESPN are stable and can be used as React keys
- Check `src/data/StaticData/` for hardcoded reference data (e.g., English-Hindi translations)

## Backend API Endpoints (when available)

```
GET /cricketplayer/team/{team}/player/{player}  # Player details
GET /cricketplayer/team/{team}/players?format=1  # Team players by format
GET /cricketteam/teams/all?format={id}           # All teams for format
GET /cricketteam/team/{uuid}                     # Team details by UUID
GET /cricketteam/teams/{uuid}/records/against/all # H2H records
GET /cricketmatch/internationalMatches?format={id} # Matches by format
POST /cricketmatch/internationalMatches/GeneratePdf # Match PDF export
```

Format IDs: 1=Test, 2=ODI, 3=T20I

---

*Note: This is a personal project in active development. Some routes and features are commented out in App.js as the developer switches between different features.*
