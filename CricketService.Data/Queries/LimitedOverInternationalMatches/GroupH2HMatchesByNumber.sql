WITH h2hTable AS(
SELECT format, team1_name, team2_name, COUNT(match_uuid) AS matches
	FROM public.cricket_teams_history_h2h
	GROUP BY format, team1_name, team2_name
	ORDER BY matches DESC)
SELECT * FROM h2hTable
WHERE team1_name = 'England' OR team2_name = 'England'