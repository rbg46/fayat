using Fred.Entities.Referential;
using Fred.ImportExport.Business.Etablissement.Etl.Result;
using Fred.ImportExport.Framework.Etl.Input;
using Fred.ImportExport.Framework.Etl.Result;
using Fred.ImportExport.Framework.Etl.Transform;
using Fred.ImportExport.Models.Etablissement;

namespace Fred.ImportExport.Business.Etablissement.Etl.Transform
{

  /// <summary>
  /// Processus etl : Transformation du resultat de la requête Anael
  /// </summary>
  public class EtablissementComptableTransform : IEtlTransform<EtablissementComptableAnaelModel, EtablissementComptableEnt>
  {

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="societeId">Société à qui attribuer les établissements comptables</param>
    public EtablissementComptableTransform(int? societeId)
    {
      SocieteId = societeId;
    }


    /// <summary>
    /// Société à qui attribuer les établissements comptables
    /// </summary>
    private int? SocieteId { get; }



    /// <summary>
    /// Appelé par l'ETL
    /// Execute le process de transformation 
    /// </summary>
    /// <param name="input">données d'entrée de l'etl</param>
    /// <param name="result">données de sortie de l'etl</param>
    public void Execute(IEtlInput<EtablissementComptableAnaelModel> input, ref IEtlResult<EtablissementComptableEnt> result)
    {
      if (result == null)
      {
        result = new EtablissementComptableResult();
      }

      foreach (var modelAnael in input.Items)
      {
        var entity = ConvertToEntity(modelAnael, SocieteId);
        result.Items.Add(entity);
      }
    }

    private EtablissementComptableEnt ConvertToEntity(EtablissementComptableAnaelModel anael, int? societeId)
    {
      return new EtablissementComptableEnt()
      {
        Code = anael.CodeEtablissement,
        Libelle = anael.Libelle,
        SocieteId = societeId
      };

    }
  }
}