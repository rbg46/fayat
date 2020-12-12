using Fred.Entities.Utilisateur;
using Fred.ImportExport.Business.ValidationPointage.Factory;
using Fred.ImportExport.Entities.ImportExport;

namespace Fred.ImportExport.Business.ValidationPointage.Common
{
    /// <summary>
    /// Contient les données contextuelles au controle pointage
    /// </summary>
    public class ValidationPointageContextData
    {
        public ValidationPointageFactorySetting FluxSetting { get; internal set; }
        public string JobId { get; internal set; }
        public string NomUtilisateur { get; internal set; }
        public UtilisateurEnt Utilisateur { get; internal set; }
        public string ConnexionChaineSource { get; internal set; }
        public FluxEnt Flux { get; internal set; }
        public string CodeSocietePaye { get; internal set; }
        public int SocieteId { get; internal set; }
        public string CodeSocieteComptable { get; internal set; }
    }
}
