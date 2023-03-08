SELECT uuid, season, series, player_of_the_match, match_no, match_days, match_title, venue, match_date, toss_winner, toss_decision, result, team1_details, team2_details, tv_umpire, match_referee, reserve_umpire, umpires, format_debut, international_debut
	FROM public.one_day_international_matches;
	
SELECT uuid, season, series, player_of_the_match, match_no, match_days, match_title, venue, match_date, toss_winner, toss_decision, result, team1_details, team2_details, tv_umpire, match_referee, reserve_umpire, umpires, format_debut, international_debut
	FROM public.t20_international_matches;
	
SELECT uuid, team_name, formats, logo_url, flag_url
	FROM public.cricket_teams_info;
	
SELECT uuid, name, full_name, href, international_team_names, team_names, date_of_birth, birth_place, formats, batting_style, bowling_style, playing_role, height, image_src, contents
	FROM public.cricket_players_info;