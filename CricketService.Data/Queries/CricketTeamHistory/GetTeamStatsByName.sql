SELECT  
    match_number,
    -- Full India team record (optional)
    -- jsonb_path_query_first(instant_teams_records, '$[*] ? (@.TeamName == "India")') AS india_team_record,
    
    -- Just the nested stats you want
    jsonb_build_object(
        'Matches', jsonb_path_query_first(
            instant_teams_records, 
            '$[*] ? (@.TeamName == "India").TeamFormatRecordDetails.Matches'
        ),
        'Won', jsonb_path_query_first(
            instant_teams_records, 
            '$[*] ? (@.TeamName == "India").TeamFormatRecordDetails.Won'
        ),
        'Lost', jsonb_path_query_first(
            instant_teams_records, 
            '$[*] ? (@.TeamName == "India").TeamFormatRecordDetails.Lost'
        )
    ) AS india_format_stats
FROM public.cricket_teams_history 
ORDER BY match_number DESC;