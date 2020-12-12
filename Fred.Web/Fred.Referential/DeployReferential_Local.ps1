# Déploie les scripts de référentiel de données dans la base
# Utilise DbUp pour déterminer quels scripts exécuter.
# Voir http://dbup.readthedocs.io/en/latest/
# Voir https://github.com/DbUp/DbUp


Write-Host -ForegroundColor Yellow "La 1ere fois, ça ne marchera que si tu pars d'une base vide de toutes données"
Write-Host -ForegroundColor Yellow "Donc :"
Write-Host -ForegroundColor Yellow "1 : soit tu drops ta base, tu relances la migration code first, et après tu joues ce script"
Write-Host -ForegroundColor Yellow "2 : soit tu joues manuellement les fichiers InCaseOfNotEmptyDatabase \ *.sql"


$DeployReferentialScript = $PSScriptRoot + "\DeployReferential.ps1"

# SQL Server Developper
$Connection="Data Source=localhost\FredInstanceDev;Initial Catalog=FredWeb_Dev;Integrated Security=True;"


& $DeployReferentialScript -ConnectionString $Connection -Filter "Common|Developpement|Recette" -TestMode $false -BuildConfiguration "SpeedDebug" -Schema 'dbo'
