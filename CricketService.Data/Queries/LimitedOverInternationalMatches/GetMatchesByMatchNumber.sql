SELECT match_number, series, match_date
FROM public.limited_over_international_matches
WHERE series NOT IN ('Prudential World Cup',
'Benson & Hedges World Series Cup',
'Benson & Hedges World Cup',
'Benson & Hedges World Championship of Cricket',
'ICC Champions Trophy',
'ICC World Cup')
AND series NOT LIKE '% tour of %'
ORDER BY 
    CAST(
        SUBSTRING(match_number FROM 'ODI no\. ([0-9]+)') 
        AS INTEGER
    );