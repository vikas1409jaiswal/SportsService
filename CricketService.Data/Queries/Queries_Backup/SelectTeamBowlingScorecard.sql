SELECT match_no,
       bsc ->> 'PlayerName' AS player_names,
	   team1_details ->> 'TeamName' AS nationality,
       bsc ->> 'OversBowled' AS overs_bowled,
       bsc ->> 'Wickets' AS wickets,
       bsc ->> 'Maidens' AS maidens,
       bsc ->> 'Economy' AS economy,
       bsc ->> 'Sixes' AS sixes,
       bsc ->> 'Fours' AS fours,
	   bsc ->> 'Dots' AS dots,
       bsc ->> 'RunsConceded' AS runs_conceded,
       bsc ->> 'NoBall' AS no_ball,
       bsc ->> 'WideBall' AS wide_ball
FROM public.cricket_matches_info,
     jsonb_array_elements(team1_details -> 'BowlingScoreCard') AS bsc
WHERE match_no = 'T20I no. 1';
