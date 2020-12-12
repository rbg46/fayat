if not exists (select * from fred_type_societe where Code like 'INT')
begin
insert into fred_type_societe (code, libelle) values ('INT', 'Interne')
end

if not exists (select * from fred_type_societe where Code like 'PAR')
begin
insert into fred_type_societe (code, libelle) values ('PAR', 'Partenaire')
end

if not exists (select * from fred_type_societe where Code like 'SEP')
begin
insert into fred_type_societe (code, libelle) values ('SEP','SEP')
end
