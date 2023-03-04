SELECT match_no, 
       team1_details -> 'BattingScoreCard' AS team1_batting_scorecard,
       team2_details -> 'BattingScoreCard' AS team2_batting_scorecard
FROM public.cricket_matches_info
