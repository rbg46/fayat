DECLARE @SOCIETEID int

select @SOCIETEID = SOCIETEID  from fred_societe where code = '0001'


Update fred_tache set ParentId = 5
where 1=1
and ciid in  (select ciid from fred_CI where societeid = @SOCIETEID) 
and ParentId = 4