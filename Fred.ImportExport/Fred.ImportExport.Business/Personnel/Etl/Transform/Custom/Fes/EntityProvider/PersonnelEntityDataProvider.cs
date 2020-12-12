using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Entities;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Imports;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;

namespace Fred.ImportExport.Business.Personnel.Etl.Transform.Custom.Fes
{
    public class PersonnelEntityDataProvider
    {


        public EtablissementPaieEnt GetEtablissementPaie(List<EtablissementPaieEnt> etablissementPaiesForSociete, PersonnelModel anaelModel)
        {
            var result = etablissementPaiesForSociete.FirstOrDefault(x => x.Code.Trim() == anaelModel.CodeEtablissement.Trim());
            return result;
        }

        public string GetEmail(string email)
        {
            return TruncateLongString(email, 50);
        }

        private string TruncateLongString(string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            return str.Substring(0, Math.Min(str.Length, maxLength)).Trim();
        }

        public int? GetRessourceId(List<RessourceEnt> ressourcesForSociete, PersonnelModel anaelModel)
        {
            RessourceEnt ressource = ressourcesForSociete.FirstOrDefault(x => x.Code.Trim() == anaelModel.CodeEmploi.Trim());


            int? ressourceId = ressource?.RessourceId;

            return ressourceId;
        }

        internal int? GetManagerId(ImportPersonnelsGlobalData importPersonnelsGlobalData, PersonnelModel anaelModel)
        {
            if (string.IsNullOrEmpty(anaelModel.MatriculeManager) || string.IsNullOrEmpty(anaelModel.SocieteManager))
            {
                return null;
            }

            PersonnelEnt fredManager = null;
            var societeDataOfManager = importPersonnelsGlobalData.GetSocieteData(anaelModel.SocieteManager);

            fredManager = GetFredManager(societeDataOfManager.FredPersonnelsForSociete, anaelModel.MatriculeManager);

            if (fredManager != null)
            {
                return fredManager.PersonnelId;
            }
            else
            {
                // il n'est pas possible d'avoir cette valeur en base.
                // Je considere alors que -1 signifie que le manager n'est pas encore créer dans fred.
                // Cela permettra de savoir si le personnel a changer.
                // -1 signifie que le manager est, pour l'instant, non connu de fred.
                return -1;
            }
        }

        private PersonnelEnt GetFredManager(List<PersonnelEnt> personnelsOfSocieteOfManager, string matriculeManager)
        {
            return personnelsOfSocieteOfManager.FirstOrDefault(p => p.Matricule == matriculeManager);
        }

        public int? GetEtablissementRattachementId(EtablissementPaieEnt etablissementPaie, PersonnelModel anaelModel)
        {
            int? result = null;

            if (etablissementPaie != null)
            {
                result = etablissementPaie.EtablissementPaieId;
            }
            else
            {
                result = null;
            }
            return result;
        }

        public static PersonnelEnt GetPersonnelFromDataBase(PersonnelModel modelAnael, List<PersonnelEnt> existingPersonnelForSociete, int societeId)
        {
            PersonnelEnt persoInterneFred = existingPersonnelForSociete.FirstOrDefault(p => p.Matricule == modelAnael.Matricule && p.SocieteId == societeId);
            return persoInterneFred;
        }

        public string GetStatus(PersonnelModel anaelModel)
        {
            var result = string.Empty;
            switch (anaelModel.Statut)
            {
                case "1":
                    result = Constantes.TypePersonnel.Ouvrier; // 1
                    break;
                case "2":
                    result = Constantes.TypePersonnel.ETAMChantier; //ETAMChantier = 2
                    break;
                case "3":
                    result = Constantes.TypePersonnel.ETAMChantier; //ETAMChantier = 2
                    break;
                case "4":
                    result = Constantes.TypePersonnel.Cadre; //Cadre = 3
                    break;
                case "5":
                    result = Constantes.TypePersonnel.Cadre; //Cadre = 3
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
