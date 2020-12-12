using System;

namespace Fred.Entities.Personnel
{
    public class PersonnelListResultViewModel
    {
        public int PersonnelId { get; set; }

        public int? UtilisateurId { get; set; }

        public int SocieteId { get; set; }

        public string Societe { get; set; }

        public string Matricule { get; set; }

        public string Nom { get; set; }

        public string Prenom { get; set; }

        public string EtablissementPaie { get; set; }

        public DateTime? DateEntree { get; set; }

        public DateTime? DateSortie { get; set; }

        public DateTime? DateSuppression { get; set; }

        public bool? IsActivedExtDirectory { get; set; }

        public bool IsInterne { get; set; }

        public bool IsInterimaire { get; set; }

        public bool IsActif { get; set; }

        public bool IsPersonnelNonPointable { get; set; }
    }
}
