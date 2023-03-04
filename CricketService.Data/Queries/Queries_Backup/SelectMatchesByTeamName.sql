SELECT match_no, team1_details -> 'TeamName' AS team1, team2_details -> 'TeamName' AS team2
FROM public.cricket_matches_info
WHERE team1_details ->> 'TeamName' = 'Australia' OR team2_details ->> 'TeamName' = 'Australia'