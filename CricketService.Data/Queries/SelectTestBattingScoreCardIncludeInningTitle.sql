WITH h2hTable AS ( 
SELECT 
    uuid,
    (team1_details -> 'Team' ->> 'Name') AS team1_name,
    (team2_details -> 'Team' ->> 'Name') AS team2_name,

    -- Handle Team1 innings safely
    (
        COALESCE(
            (
                SELECT jsonb_agg(
                    jsonb_set(
                        player, 
                        '{Title}', 
                        to_jsonb(team1_details -> 'Inning1' ->> 'Title')
                    )
                )
                FROM jsonb_array_elements(team1_details -> 'Inning1' -> 'BattingScorecard') AS player
                WHERE team1_details -> 'Inning1' IS NOT NULL
            ),
            '[]'::jsonb
        )
    ) || COALESCE(
        (
            SELECT jsonb_agg(
                jsonb_set(
                    player, 
                    '{Title}', 
                    to_jsonb(team1_details -> 'Inning2' ->> 'Title')
                )
            )
            FROM jsonb_array_elements(team1_details -> 'Inning2' -> 'BattingScorecard') AS player
            WHERE team1_details -> 'Inning2' IS NOT NULL
        ),
        '[]'::jsonb
    ) AS team1_all_bs,

    -- Handle Team2 innings safely
    (
        COALESCE(
            (
                SELECT jsonb_agg(
                    jsonb_set(
                        player, 
                        '{Title}', 
                        to_jsonb(team2_details -> 'Inning1' ->> 'Title')
                    )
                )
                FROM jsonb_array_elements(team2_details -> 'Inning1' -> 'BattingScorecard') AS player
                WHERE team2_details -> 'Inning1' IS NOT NULL
            ),
            '[]'::jsonb
        )
    ) || COALESCE(
        (
            SELECT jsonb_agg(
                jsonb_set(
                    player, 
                    '{Title}', 
                    to_jsonb(team2_details -> 'Inning2' ->> 'Title')
                )
            )
            FROM jsonb_array_elements(team2_details -> 'Inning2' -> 'BattingScorecard') AS player
            WHERE team2_details -> 'Inning2' IS NOT NULL
        ),
        '[]'::jsonb
    ) AS team2_all_bs,

    match_number, match_type, match_date, season, series, series_result, match_title
FROM public.test_cricket_matches
),

-- (Rest of your query remains unchanged)
team1_batsmen AS (
  SELECT
    uuid,
    jsonb_array_elements(team1_all_bs) AS batting_scorecard,
    team1_name AS team_name,
	team2_name AS opp_team_name,
    match_number, match_type, match_date, season, series, series_result, match_title
  FROM h2hTable
),
team2_batsmen AS (
  SELECT
    uuid,
    jsonb_array_elements(team2_all_bs) AS batting_scorecard,
    team2_name AS team_name,
	team1_name AS opp_team_name,
    match_number, match_type, match_date, season, series, series_result, match_title
  FROM h2hTable
),
all_batsmen AS (
  SELECT * FROM team1_batsmen
  UNION ALL
  SELECT * FROM team2_batsmen
),
all_batting_scorecards_detail AS (
SELECT
  uuid,
  match_number AS MatchNumber,
  batting_scorecard -> 'PlayerName' ->> 'Href' AS player_href,
  batting_scorecard -> 'PlayerName' ->> 'Name' AS player_name,
  (batting_scorecard ->> 'RunsScored')::int AS run_scored,
  batting_scorecard ->> 'OutStatus' AS out_status,
  batting_scorecard ->> 'Title' AS inning_title,
  team_name AS team_name,
  opp_team_name AS opposition_team_name
FROM all_batsmen
ORDER BY to_date(match_date, 'Mon DD YYYY'), team_name
)
SELECT * FROM all_batting_scorecards_detail


