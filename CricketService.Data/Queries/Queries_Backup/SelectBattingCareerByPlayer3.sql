-- create a prepared statement with placeholders
PREPARE player_stats (text, text) AS
SELECT 
    SUM(runs_scored) AS total_runs_scored,
    SUM(balls_faced) AS total_balls_faced,
    SUM(minutes) AS total_minutes,
    SUM(fours) AS total_fours,
    SUM(sixes) AS total_sixes,
    CAST(SUM(runs_scored) AS FLOAT)/ CAST(SUM(balls_faced) AS FLOAT) * 100 AS total_strike_rate,
    MAX(runs_scored) AS highest_score,
    COUNT(CASE WHEN runs_scored >= 100 THEN 1 END) AS centuries,
    COUNT(CASE WHEN runs_scored >= 50 AND runs_scored < 100 THEN 1 END) AS half_centuries
FROM (
    SELECT 
        CAST(bsc ->> 'RunsScored' AS integer) AS runs_scored,
        CAST(bsc ->> 'BallsFaced' AS integer) AS balls_faced,
        CAST(bsc ->> 'Minutes' AS integer) AS minutes,
        CAST(bsc ->> 'Fours' AS integer) AS fours,
        CAST(bsc ->> 'Sixes' AS integer) AS sixes
    FROM 
        public.one_day_international_matches, 
        jsonb_array_elements(team1_details -> 'BattingScoreCard') AS bsc
    WHERE 
        (team1_details ->> 'TeamName' = $2)
        AND (bsc ->> 'PlayerName' LIKE $1)
    
    UNION ALL
    
    SELECT 
        CAST(bsc ->> 'RunsScored' AS integer) AS runs_scored,
        CAST(bsc ->> 'BallsFaced' AS integer) AS balls_faced,
        CAST(bsc ->> 'Minutes' AS integer) AS minutes,
        CAST(bsc ->> 'Fours' AS integer) AS fours,
        CAST(bsc ->> 'Sixes' AS integer) AS sixes
    FROM 
        public.one_day_international_matches, 
        jsonb_array_elements(team2_details -> 'BattingScoreCard') AS bsc
    WHERE 
        (team2_details ->> 'TeamName' = $2)
        AND (bsc ->> 'PlayerName' LIKE $1)
) AS batting_stats;

-- execute the prepared statement with actual values
EXECUTE player_stats ('%Matthew Hayden%', 'Australia');
