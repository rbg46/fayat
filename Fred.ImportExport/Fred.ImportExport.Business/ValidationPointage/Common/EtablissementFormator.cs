using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fred.Business.Referential;

namespace Fred.ImportExport.Business.ValidationPointage.Common
{
    public class EtablissementFormator : IEtablissementFormator
    {
        private readonly IEtablissementPaieManager etsPaieManager;

        public EtablissementFormator(IEtablissementPaieManager etsPaieManager)
        {
            this.etsPaieManager = etsPaieManager;
        }

        /// <summary>
        ///   Concaténation des codes établissements de paie
        /// </summary>
        /// <param name="etablissementPaieIdList">Liste d'identifiant des établissements de paie</param>
        /// <returns>Concaténation des codes sous forme de chaine de caractères</returns>
        public string ConcatEtablissementPaieCode(IEnumerable<int> etablissementPaieIdList)
        {
            var etsCodeList = new StringBuilder();

            if (etablissementPaieIdList?.Any() == true)
            {
                foreach (var etsPaieId in etablissementPaieIdList.ToList())
                {
                    var ets = this.etsPaieManager.GetEtablissementPaieById(etsPaieId);
                    if (ets != null)
                    {
                        etsCodeList.Append(ets.Code.Trim());
                    }
                }
            }
            return etsCodeList.ToString();
        }
    }
}
