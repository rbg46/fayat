if not exists (select * from fred_type_participation_sep where Code like 'G')
begin
insert into fred_type_participation_sep (code, libelle) values ('G', 'Gérant')
end

if not exists (select * from fred_type_participation_sep where Code like 'M')
begin
insert into fred_type_participation_sep (code, libelle) values ('M', 'Mandataire')
end

if not exists (select * from fred_type_participation_sep where Code like 'A')
begin
insert into fred_type_participation_sep (code, libelle) values ('A', 'Associé')
end