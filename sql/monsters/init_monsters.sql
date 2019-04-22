INSERT INTO public.t_user_monsters
(
	monster_id,
	count,
	search,
	propose,
	user_id
)
SELECT
  	tm.id,
  	0,
  	true,
  	false,
  	1
FROM 
	T_monsters AS tm