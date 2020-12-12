namespace Fred.Business.ExplorateurDepense
{
    /// <summary>
    /// Explorateur de dépense pour Fayat TP
    /// </summary>
    public class ExplorateurDepenseFayatTPModel : ExplorateurDepenseGeneriqueModel
    {
        /// <summary>
        /// Conversion d'un ExplorateurDepenseFayatTP vers un ExplorateurDepenseGenerique
        /// </summary>
        /// <returns><see cref="ExplorateurDepenseGeneriqueModel"/></returns>
        public ExplorateurDepenseGeneriqueModel ConvertToExplorateurDepenseGenerique()
        {
            return new ExplorateurDepenseGeneriqueModel()
            {
                AgenceId = AgenceId,
                Ci = Ci,
                Code = Code,
                CommandeId = CommandeId,
                Commentaire = Commentaire,
                DateComptableRemplacement = DateComptableRemplacement,
                DateDepense = DateDepense,
                DateFacture = DateFacture,
                DateRapprochement = DateRapprochement,
                DepenseId = DepenseId,
                DepenseVisee = DepenseVisee,
                Devise = Devise,
                DeviseId = DeviseId,
                FournisseurId = FournisseurId,
                GroupeRemplacementTacheId = GroupeRemplacementTacheId,
                Id = Id,
                IsEnergie = IsEnergie,
                Libelle1 = Libelle1,
                Libelle2 = Libelle2,
                MontantFacture = MontantFacture,
                MontantHT = MontantHT,
                MontantHtInitial = MontantHtInitial,
                Nature = Nature,
                NatureId = NatureId,
                NumeroFacture = NumeroFacture,
                Periode = Periode,
                Personnel = Personnel,
                PUHT = PUHT,
                Quantite = Quantite,
                RemplacementTaches = RemplacementTaches,
                Ressource = Ressource,
                RessourceId = RessourceId,
                SoldeFar = SoldeFar,
                SousTypeDepense = SousTypeDepense,
                Tache = Tache,
                TacheId = TacheId,
                TacheOrigine = TacheOrigine,
                TacheOrigineCodeLibelle = TacheOrigineCodeLibelle,
                TacheOrigineId = TacheOrigineId,
                TacheRemplacable = TacheRemplacable,
                TypeDepense = TypeDepense,
                Unite = Unite,
                UniteId = UniteId
            };
        }
    }
}
