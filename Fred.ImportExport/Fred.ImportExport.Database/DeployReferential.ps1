# Déploie les scripts de référentiel de données dans la base
# Utilise DbUp pour déterminer quels scripts exécuter.
# Attention, ce script est à l'image de ce qui est joué sur la PIC
# Si vous effectuez des modifications, il faut aussi modifier le package DbUp sur la PIC
# Voir http://dbup.readthedocs.io/en/latest/
# Voir https://github.com/DbUp/DbUp
# Voir https://github.com/johanclasson/vso-agent-tasks

# Historique des versions :
# Version 1.0, Jerome Neel, 09/10/2017 : Creation du script
# Version 1.1, Jerome Neel, 22/03/2018 : Déplacement des extensions DbUp dans Fred.Framework pour le partager avec Fred.IE


# Paramètres d'appel
Param(
    # ConnectionString : chaine de connection à la base de données
    [Parameter(Mandatory=$true, Position=1)]
    [string]
    $ConnectionString,

    # Filter : C# Regex pour filtrer les fichiers, exemple dans notre cas "Common|Dev" ou "Common|Prod"
    [Parameter(Mandatory=$false, Position=2)]
    [string]
    $Filter,

    # Test Mode : true = pas de modification en base, affiche seulement les fichiers qui seront traités ; False = modification en base
    [Parameter(Mandatory=$false, Position=3)]
    [string]
    $TestMode = $false,

    # BuildConfiguration : pour aller chercher les binaires au bon endroit
    [Parameter(Mandatory=$false, Position=4)]
    [string]
    $BuildConfiguration = "Release",

    # Schema : le schema n'est pas le même entre fred web (dbo) et fred ie (importexport)
    [Parameter(Mandatory=$false, Position=4)]
    [string]
    $Schema = "dbo"

)

# Paramètres
$DbUpCoreBin = "..\..\packages\dbup-core.4.2.0\lib\net35\"
$DbUpSqlServerBin = "..\..\packages\dbup-sqlserver.4.2.0\lib\net35\"
$DbUpFredFrameworkBin = "..\..\Fred.Web\Fred.Framework\Bin\" + $BuildConfiguration
$ScriptPath            = $PSScriptRoot + "\Referential"
$DbUpCoreLocation      = $PSScriptRoot + "\" + $DbUpCoreBin + "\dbup-core.dll"
$DbUpSqlServerLocation = $PSScriptRoot + "\" + $DbUpSqlServerBin + "\dbup-sqlserver.dll"
$DbUpFayatExention     = $PSScriptRoot + "\" + $DbUpFredFrameworkBin + "\Fred.Framework.dll"


try 
{

    # Print info
    Write-Host "Paramètres :" -ForegroundColor Green
    Write-Host Base de données         : $ConnectionString
    Write-Host Référentiel             : $ScriptPath
    Write-Host Filtre                  : $Filter
    Write-Host TestMode                : $TestMode
    Write-Host DbUpCoreBin             : $DbUpCoreBin
    Write-Host DbUpSqlServerBin        : $DbUpSqlServerBin
    Write-Host DbUpFredFrameworkBin    : $DbUpFredFrameworkBin
    Write-Host ScriptPath              : $ScriptPath



    # Chargement des Dll
    Add-Type -Path $DbUpCoreLocation
    Add-Type -Path $DbUpSqlServerLocation
    Add-Type -Path $DbUpFayatExention
   
    
    # Fonction pour filtrer un fichier
    $filterFunc = 
    {
        param([string]$file)
        return $file -match $Filter
    }


    # Création de l'objet Option (Version Fayat)
    $options = New-Object Fred.Framework.Extensions.DbUpExtension.FileSystemScriptOptions    # Ou aussi $options = [Fred.Framework.Extensions.DbUpExtension.FileSystemScriptOptions]::new()
    $options.IncludeSubDirectories=$true;
    $options.Filter = $filterFunc;
    $options.Order = 1                                                                       # = FileSearchOrder.FilePath => If set to File Path, scripts are strictly sorted on their complete path. 


    # Création de l'objet FileSystemScriptProvider                                           # Permet de rechercher les fichiers avec tri + chemin complet du fichier affiché dans la table DbUp
    $scriptProvider = New-Object Fred.Framework.Extensions.DbUpExtension.FileSystemScriptProvider -ArgumentList $ScriptPath, $options

    
    # Appel de DbUp...                                                                       # en c#, c'est un peu plus lisible, mais le code est le même...
    $dbUp = [DbUp.DeployChanges]::To
    $dbUp = [SqlServerExtensions]::SqlDatabase($dbUp, "$ConnectionString")
    $dbUp = [StandardExtensions]::WithTransaction($dbUp)                                     # Ca veut dire que l'ensemble des fichiers est traité dans une seule transaction, donc si un seul fichier ne passe pas, aucunes données est insérées en base
    $dbUp = [StandardExtensions]::WithScripts($dbUp, $scriptProvider)                        # Normalement : WithScriptsFromFileSystem($dbUp, $scriptPath, $options), mais là, on utilise notre propre extension
    $dbUp = [SqlServerExtensions]::JournalToSqlTable($dbUp, $Schema, '__ReferentialHistory')   # Table d'historique
    $dbUp = [StandardExtensions]::LogToConsole($dbUp)
    

    # Mode Test : Affiche seulement les fichiers qui seront traités
    if ($TestMode -eq $true)
    {
        Write-Host Mode test, aucune modification ne sera apportée en base. -ForegroundColor Green
        Write-Host Liste des fichiers qui devrait être jouée : -ForegroundColor Green
        $list= $dbUp.Build().GetScriptsToExecute()
        ForEach($file in $list)
        {
            Write-Host $file.Name
        } 
        
        Write-Host "Liste des fichiers terminée." -ForegroundColor Green
    }

    # Effectue l'insertion en base
    else
    {
        $upgradeResult = $dbUp.Build().PerformUpgrade()

        if(!$upgradeResult.Successful) 
        { 
            throw $upgradeResult.Error.Message 
        } 

        Write-Host "Déploiement terminé." -ForegroundColor Green
    }

}
catch 
{
    Write-Host "Erreur de déploiement :" -ForegroundColor Red
    Write-Error $_.Exception|format-list -force
}