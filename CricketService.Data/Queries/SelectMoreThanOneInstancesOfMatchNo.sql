SELECT match_no, COUNT(*) as count
FROM public.cricket_matches_info
GROUP BY match_no
HAVING COUNT(*) > 1;