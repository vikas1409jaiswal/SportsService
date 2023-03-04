SELECT match_no,
       bsc ->> 'PlayerName' AS player_names,
	   team1_details ->> 'TeamName' AS nationality,
       bsc ->> 'RunsScored' AS runs_scored,
       bsc ->> 'BallsFaced' AS balls_faced,
       bsc ->> 'Minutes' AS minutes,
       bsc ->> 'Fours' AS fours,
       bsc ->> 'Sixes' AS sixes,
       bsc ->> 'StrikeRate' AS strike_rate
FROM public.cricket_matches_info,
     jsonb_array_elements(team1_details -> 'BattingScoreCard') AS bsc
WHERE match_no = 'T20I no. 1';
