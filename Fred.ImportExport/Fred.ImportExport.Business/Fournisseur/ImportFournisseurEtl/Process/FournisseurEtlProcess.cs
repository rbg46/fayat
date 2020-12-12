using AutoMapper;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Business.Fournisseur.Etl.Input;
using Fred.ImportExport.Business.Fournisseur.Etl.Output;
using Fred.ImportExport.Business.Fournisseur.Etl.Result;
using Fred.ImportExport.Business.Fournisseur.Etl.Transform;
using Fred.ImportExport.Entities.ImportExport;
using Fred.ImportExport.Framework.Etl.Engine;
using Fred.ImportExport.Framework.Etl.Engine.Builder;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models;
using System;
using System.Configuration;

namespace Fred.ImportExport.Business.Fournisseur.Etl.Process
{
  /// <summary>
  /// ETL d'import depuis Anael vers Fred des établissements comptables
  /// </summary>
  public class FournisseurEtlProcess : EtlProcessBase<FournisseurAnaelModel, FournisseurModel>
  {
    private readonly FluxManager fluxManager;
    private readonly IPaysManager paysManager;
    private readonly IFournisseurManager fournisseurManager;
    private readonly ISocieteManager societeManager;
    private readonly FluxEnt flux;
    private readonly string importJobId = ConfigurationManager.AppSettings["flux:fournisseur"];
    private readonly FournisseurInput input;


    /// <summary>
    /// Ctor
    /// N'est pas référencé par Unity, 
    /// Est instancié par un new dans le manager Fred.IE des Fournisseurs.
    /// </summary>
    /// <param name="fluxManager">Manager Fred.IE des Flux</param>
    /// <param name="paysManager">Manager Fred des Pays</param>
    /// <param name="fournisseurManager">Manager Fred des Fournisseurs</param>
    /// <param name="societeManager">Manager Fred des Sociétés</param>
    /// <param name="mapper">Le gestionnaire Automapper.</param>
    public FournisseurEtlProcess(FluxManager fluxManager, IPaysManager paysManager, IFournisseurManager fournisseurManager, ISocieteManager societeManager, IMapper mapper)
    {
      this.fluxManager = fluxManager;
      this.paysManager = paysManager;
      this.fournisseurManager = fournisseurManager;
      this.societeManager = societeManager;
      flux = fluxManager.GetByCode(importJobId);
      input = new FournisseurInput();
    }

    /// <inheritdoc/>
    public override void Build()
    {
      var builder = new EtlBuilder<FournisseurAnaelModel, FournisseurModel>(Config);
      builder
        .Input(input)
        .Transform(new FournisseurTransform(paysManager))
        .Result(new FournisseurResult())
        .Output(new FournisseurFredOuput(fournisseurManager, societeManager, flux))
        .Output(new FournisseurStormOuput(input.SocieteId));
    }

    /// <summary>
    ///   Renseigne les paramètres d'input
    /// </summary>
    /// <param name="bypassDate">Booléen permettant d'ignorer la condition de date de modif</param>
    /// <param name="codeSocieteComptable">Code société comptable ANAEL</param>
    /// <param name="typeSequences">Type de séquences TIERS, TIERS2, GROUPE, MAT</param>
    /// <param name="regleGestions">Regle de gestion (F,C,C1,I)</param>
    /// <param name="societeId">societeId</param>
    public void SetParameter(bool bypassDate, string codeSocieteComptable, string typeSequences, string regleGestions, int societeId)
    {
      input.ByPassDate = bypassDate;
      input.CodeSocieteComptables = codeSocieteComptable;
      input.Flux = flux;
      input.RegleGestions = regleGestions;
      input.TypeSequences = typeSequences;
      input.SocieteId = societeId;
    }


    /// <summary>
    /// Vérification du flux
    /// </summary>
    /// <returns>
    /// si la chaine est vide alors le flux peut être exécuté
    /// si la chaine n'est pas vide, retourne un msg d'erreur</returns>
    private string CanExecuteImport()
    {
      string msg = string.Empty;

      if (flux == null)
      {
        msg = string.Format(IEBusiness.FluxInconnu, importJobId);
      }
      else if (!flux.IsActif)
      {
        msg = string.Format(IEBusiness.FluxInactif, importJobId);
      }

      return msg;
    }

    /// <inheritdoc/>
    protected override void OnBegin()
    {
      // Si l'import ne peut pas être exécuté, alors arrêt de l'etl
      string error = CanExecuteImport();
      if (!string.IsNullOrEmpty(error))
      {
        throw new FredIeEtlStopException(error);
      }
    }

    /// <inheritdoc />
    protected override void OnSuccess()
    {
      flux.DateDerniereExecution = DateTime.UtcNow;
      fluxManager.Update(flux);
    }

    /// <inheritdoc />
    protected override void OnError(Exception ex)
    {
      // Qu'est ce que l'on fait ?
    }
  }
}
