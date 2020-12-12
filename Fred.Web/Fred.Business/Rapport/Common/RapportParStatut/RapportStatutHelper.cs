using Fred.Entities;
using Fred.Entities.Rapport;

namespace Fred.Business.Rapport.Common.RapportParStatut
{
    public static class RapportStatutHelper
    {
        public static RapportEnt CheckPersonnelStatut(RapportEnt rapport, string statutPersonnel)
        {
            if (rapport != null)
            {
                rapport.TypeStatutRapportEnum = (TypeStatutRapport)GetTypePersonnel(statutPersonnel);
                return rapport;
            }
            return rapport;
        }

        public static int GetTypePersonnel(string statut)
        {
            if (statut == Constantes.PersonnelStatutValue.Ouvrier || statut == Constantes.TypePersonnel.Ouvrier)
            {
                return Constantes.TypeRapportStatut.Ouvrier;
            }
            else if (statut == Constantes.PersonnelStatutValue.Cadre || statut == Constantes.TypePersonnel.Cadre)
            {
                return Constantes.TypeRapportStatut.Cadre;
            }
            else if (statut == Constantes.PersonnelStatutValue.ETAM || statut == Constantes.TypePersonnel.ETAM)
            {
                return Constantes.TypeRapportStatut.ETAM;
            }
            return Constantes.TypeRapportStatut.None;
        }
    }
}
