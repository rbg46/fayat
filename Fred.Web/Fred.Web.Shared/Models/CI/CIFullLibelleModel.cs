namespace Fred.Web.Models.CI
{
    public class CIFullLibelleModel : CIModel
    {
        /// <summary>
        /// Obtient Le code Societe + Code CI
        /// </summary>
        public override string CodeSocieteCI
        {
            get
            {
                return EtablissementComptable == null ?
                    $"{Societe.Code} - {Code}"
                    : $"{Societe.Code} - {EtablissementComptable.Code} - {Code}";
            }
        }

        /// <summary>
        /// Obtient le libellé complet du CI
        /// </summary>
        public override string CodeLibelle
        {
            get
            {
                return $"{CodeSocieteCI} - {Libelle}";
            }
        }
    }
}
