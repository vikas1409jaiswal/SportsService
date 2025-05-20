SELECT uuid, team_name, formats, logo_url, flag_url, test_records, odi_records, t20i_records
	FROM public.cricket_teams_info WHERE formats = 'Twenty20';