WITH playerScoreTable AS (
WITH h2hTable AS (
  SELECT 
    uuid,
    (team1_details -> 'Team' ->> 'Name') AS team1_name,
    (team2_details -> 'Team' ->> 'Name') AS team2_name,
    (team1_details -> 'Inning1' -> 'BattingScorecard') || 
    COALESCE(team1_details -> 'Inning2' -> 'BattingScorecard', '[]'::jsonb) AS team1_all_bs,
    (team2_details -> 'Inning1' -> 'BattingScorecard') || 
    COALESCE(team2_details -> 'Inning2' -> 'BattingScorecard', '[]'::jsonb) AS team2_all_bs,
    match_number, match_type, match_date, season, series, series_result, match_title
  FROM public.test_cricket_matches
  WHERE (team1_details -> 'Team' ->> 'Name' = 'England' AND team2_details -> 'Team' ->> 'Name' = 'India')
    OR (team2_details -> 'Team' ->> 'Name' = 'England' AND team1_details -> 'Team' ->> 'Name' = 'India')
),
-- Unnest team1 batting data
team1_batsmen AS (
  SELECT
    uuid,
    jsonb_array_elements(team1_all_bs) AS batting_scorecard,
    team1_name AS team_name,
    match_number, match_type, match_date, season, series, series_result, match_title
  FROM h2hTable
),
-- Unnest team2 batting data
team2_batsmen AS (
  SELECT
    uuid,
    jsonb_array_elements(team2_all_bs) AS batting_scorecard,
    team2_name AS team_name,
    match_number, match_type, match_date, season, series, series_result, match_title
  FROM h2hTable
),
-- Combine both teams' data
all_batsmen AS (
  SELECT * FROM team1_batsmen
  UNION ALL
  SELECT * FROM team2_batsmen
)
-- Final result with extracted fields
SELECT
  uuid,
  batting_scorecard -> 'PlayerName' ->> 'Href' AS player_name,
  (batting_scorecard ->> 'RunsScored')::int AS runs_scored,
  team_name,
  match_number,
  match_date
FROM all_batsmen
WHERE team_name = 'India' AND (batting_scorecard -> 'PlayerName' ->> 'Name') LIKE '%Sachin%'
ORDER BY to_date(match_date, 'Mon DD YYYY'), team_name
)
SELECT SUM(runs_scored)
FROM playerScoreTable
