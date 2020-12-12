using System.Collections.Generic;
using System.Linq;
using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.Entities.Societe;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Etablissement
{

    /// <summary>
    /// RG_5423_003 : Lien du CI avec un établissement fictif
    ///    Il faut gérer le cas où ANAEL ne fournit pas d’établissement pour un CI mais on doit en définir un dans FRED malgré tout
    /// (NB – cela signifie qu’il y aura des écarts entre les établissements définis dans FRED et ceux définis dans ANAEL).
    /// Rajouter un champ[EtablissementParDefaut] (O/N) au niveau de la table[FRED_SOCIETE] pour marquer les sociétés concernées.
    /// Lors de l’import d’un CI d’une société marquée[EtablissementParDefaut]= Oui,
    /// ignorer les informations d’établissement éventuellement fournies par ANAEL,
    /// et à la place lier le CI à l’établissement de code ‘01’ de la société (cet établissement devra avoir été créé avant l’exécution de l’import).
    /// Si cet établissement de code ‘01’ n’est pas trouvé pour la société, 
    /// ne pas importer le CI concerné et passer au CI suivant(ne pas interrompre le flux).
    /// Lors de l’import d’un CI d’une société marquée[EtablissementParDefaut]=Non,
    /// exécuter l’import comme actuellement en liant le CI à l’établissement fourni par ANAEL
    /// (ou directement à la société si aucun établissement n’est fourni).
    /// </summary>
    public class DefaultEtablissementProvider
    {

        /// <summary>
        /// Permet de savoir si on peut surcharger l'etablissement comptable du ci.
        /// Il faut que la societe ai un etablissement comptable avec un code '01' et que societe.EtablissementParDefaut = vrai,
        /// sinon societe.EtablissementParDefaut = faux
        /// </summary>
        /// <param name="societe">societe</param>
        /// <param name="etablissementComptables">Liste des etablissementComptable de la societes</param>
        /// <returns>Vria si on peut surcharger</returns>
        public bool CanOverrideEtablissementComptableIfNecessary(SocieteEnt societe, List<EtablissementComptableEnt> etablissementComptables)
        {
            var result = true;
            if (societe.EtablissementParDefaut)
            {
                var defaultEtablissement = etablissementComptables.FirstOrDefault(x => x.Code == "01");

                if (defaultEtablissement == null)
                {
                    result = false;
                }
            }
            return result;
        }

        /// <summary>
        /// Met le bon etablissement comptable sur le ci.
        /// </summary>
        /// <param name="societe">societe</param>
        /// <param name="etablissementComptables">Liste des etablissementComptable de la societes</param>
        /// <param name="anaelCisConvertedToCiEnts">Liste de cient dont on doit surcharger l'etablissement comptable</param>
        public void MapDefaultEtablissementIfNecessary(SocieteEnt societe, List<EtablissementComptableEnt> etablissementComptables, List<CIEnt> anaelCisConvertedToCiEnts)
        {
            if (societe.EtablissementParDefaut)
            {
                var defaultEtablissement = etablissementComptables.FirstOrDefault(x => x.Code == "01");

                foreach (var cIEnt in anaelCisConvertedToCiEnts)
                {
                    cIEnt.EtablissementComptableId = defaultEtablissement.EtablissementComptableId; // ici je sais que j'ai un etablissement par defaut
                }
            }
        }
    }
}
