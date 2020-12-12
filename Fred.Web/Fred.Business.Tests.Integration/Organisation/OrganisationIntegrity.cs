using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Fred.Business.Organisation.Tree;
using Fred.Business.Tests.Integration.Organisation.Extensions;
using Fred.Business.Tests.Integration.Organisation.OutputProcessor;
using Fred.DataAccess.Organisation.Tree.Repository;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Framework.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.Business.Tests.Integration.Organisation
{
    [TestClass]
    [Ignore]
    public class OrganisationIntegrity : FredBaseTest
    {
        private readonly OutputProcessorFactory _outputProcessorFactory = new OutputProcessorFactory();

        /// <summary>
        /// Afficher toutes les erreurs de l'arbre des organisations 
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void verify()
        {
            var organisationTreeRepository = new OrganisationTreeRepository(FredContext);
            var organisationTreeSvc = new OrganisationTreeService(organisationTreeRepository, CacheMgr);


            var fredCis = this.FredContext.CIs.Include(x => x.Organisation).ToList();

            var repoCi = CiRepository;
            var repoEtab = EtablissementComptableRepository;
            var repoSociete = SocieteRepository;

            var etabs = repoEtab.Query().Get().Include(x => x.Organisation).AsNoTracking().ToList();
            var societes = repoSociete.Query().Get().Include(x => x.Organisation).AsNoTracking().ToList();

            // Récupérer l'arbre
            OrganisationTree tree = organisationTreeSvc.GetOrganisationTree();

            var ciSelection = fredCis.ToList();

            var ciToReImport = new List<CIEnt>();
            var sousCiToReImport = new List<CIEnt>();
            var cisWithoutSolution = new List<CIEnt>();

            foreach (var input in ciSelection)
            {
                //La recherche : 
                var ci = input;
                var societe = societes.First(x => x.SocieteId == input.SocieteId);
                var etablissement = etabs.FirstOrDefault(x => x.EtablissementComptableId == ci.EtablissementComptableId);

                // Les tables             
                var tableInfo = GetTableInfos(ci, societe, etablissement);

                // L'arbre 
                var arbreInfo = GetArbreInfo(tree, ci);

                if (HasErroOnEtablissement(etablissement, arbreInfo.etabParent) || HasErrorOnSociete(societe, arbreInfo.societeParent))
                {
                    var cisWithSameCode = ciSelection.Where(x => x.Code == ci.Code).ToList();
                    var hasAnyWithSameCodeMoreOnce = cisWithSameCode.Any() ? "O" : "N";

                    var cisWithSameCodeParent = new List<CIEnt>();
                    var hasCiParentWithSameCodeMoreOnce = "N";
                    var parentIsEnfant = false;
                    var parentIsEnfantMessage = "N";
                    if (arbreInfo.ciParent != null)
                    {
                        cisWithSameCodeParent = ciSelection.Where(x => x.Code == arbreInfo.ciParent.Code).ToList();
                        hasCiParentWithSameCodeMoreOnce = cisWithSameCodeParent.ToList().Count > 1 ? "O" : "N";
                        parentIsEnfant = arbreInfo.ciParent.OrganisationId == arbreInfo.ciArbre.OrganisationId;
                        parentIsEnfantMessage = parentIsEnfant ? "0" : "N";
                    }

                    var c1Error = tableInfo.ciTableSocieteCode != arbreInfo.ciArbreSocieteCode;
                    var c2Error = tableInfo.ciTableCodeEtablissementComptable != arbreInfo.ciArbreCodeEtablissementComptable;
                    var c3Error = tableInfo.ciTableCode != arbreInfo.ciArbreCode;
                    var c4Error = tableInfo.ciTableSocieteOrganisationId != arbreInfo.societeParentOrganisationId;
                    var c5Error = tableInfo.ciTableCodeEtablissementComptableOrganisationId != arbreInfo.etabParentOrganisationId;
                    var c6Error = ci.Organisation.OrganisationId.ToString() != arbreInfo.ciArbre.OrganisationId.ToString();
                    var c7Error = ci.Organisation.TypeOrganisationId != arbreInfo.ciArbre.TypeOrganisationId;

                    var c1ErrorCross = c1Error ? "X" : "";
                    var c2ErrorCross = c2Error ? "X" : "";
                    var c3ErrorCross = c3Error ? "X" : "";
                    var c4ErrorCross = c4Error ? "X" : "";
                    var c5ErrorCross = c5Error ? "X" : "";
                    var c6ErrorCross = c6Error ? "X" : "";
                    var c7ErrorCross = c7Error ? "X" : "";
                    string type = GetTypeError(c1Error, c2Error);
                    var titreErreur = $"{ci.Organisation.TypeOrganisationId}{type}{hasAnyWithSameCodeMoreOnce}{hasCiParentWithSameCodeMoreOnce}{parentIsEnfantMessage}";
                    var numeroErreur = $"TYPE CITYPE({ci.Organisation.TypeOrganisationId})-{type}-PLUSIEURS_CI_AVEC_MEME_CODE({hasAnyWithSameCodeMoreOnce})-PLUSIEURS_CI_AVEC_MEME_CODE_PARENT({hasCiParentWithSameCodeMoreOnce})-PARENT IS ENFANT({parentIsEnfantMessage})";
                    Print($"------------------------------------------------------------------------------------------------");
                    Print($"------------------------------------------------------------------------------------------------");
                    Print($"ERREUR NUMERO {titreErreur}");
                    Print($"ERREUR SOCIETE {societe.SocieteId} - {societe.Code}");
                    Print($"ERREUR {numeroErreur}");
                    if (parentIsEnfant)
                    {
                        Print($"CODE CI == CODE CI PARENT ");
                    }

                    Print($"");
                    Print($"ANALYSE DU CI : OrganisationId : {ci.Organisation.OrganisationId} CiId : {ci.CiId} - SocieteId : {ci.SocieteId} - EtablissementComptableId : {ci.EtablissementComptableId} - CODE : {ci.Code} ");

                    Print($"CODE                         TABLE                        -  ARBRE  ");
                    Print($"SOCIETE                      : {tableInfo.ciTableSocieteCode?.PadLeft(10)}                 -  {arbreInfo.ciArbreSocieteCode.PadLeft(10)}  {c1ErrorCross}");
                    Print($"ETABLISSEMENT                : {tableInfo.ciTableCodeEtablissementComptable.PadLeft(10)}                 -  {arbreInfo.ciArbreCodeEtablissementComptable.PadLeft(10)}  {c2ErrorCross}");
                    Print($"CI PARENT                    : {"".PadLeft(10)}                 -  {arbreInfo.ciParent?.Code.PadLeft(10)}  ");
                    Print($"CI                           : {tableInfo.ciTableCode.PadLeft(10)}                 -  {arbreInfo.ciArbreCode.PadLeft(10)}  {c3ErrorCross}");

                    Print($"");
                    Print($"ORGANISATION                 TABLE                        -  ARBRE  ");
                    Print($"SOCIETE                      : {tableInfo.ciTableSocieteOrganisationId.PadLeft(10)}                 -  {arbreInfo.societeParentOrganisationId.PadLeft(10)}  {c4ErrorCross}");
                    Print($"ETABLISSEMENT                : {tableInfo.ciTableCodeEtablissementComptableOrganisationId.PadLeft(10)}                 -  {arbreInfo.etabParentOrganisationId.PadLeft(10)}  {c5ErrorCross}");
                    Print($"CI PARENT                    : {"".PadLeft(10)}                 -  {arbreInfo.ciParent?.OrganisationId.ToString().PadLeft(10)}  ");
                    Print($"CI                           : {ci.Organisation.OrganisationId.ToString().PadLeft(10)}                 -  {arbreInfo.ciArbre.OrganisationId.ToString().PadLeft(10)}  {c6ErrorCross}");
                    Print($"");

                    Print($"TYPE-CI                      : {ci.Organisation.TypeOrganisationId.ToString().PadLeft(10)}                 -  {arbreInfo.ciArbre.TypeOrganisationId.ToString().PadLeft(10)}  {c7ErrorCross}");
                    Print($"");

                    AffichageDesCiParentAvecLeMemeCode(cisWithSameCodeParent);

                    AffichageDesCisAvecLeMemeCode(cisWithSameCode);

                    var sousCiError = AffichageSolutionPourSousCiDontLeParentAMalEteAffecteDansLarbre(ci, arbreInfo.ciParent, cisWithSameCodeParent, c2Error);
                    sousCiToReImport.Add(sousCiError);

                    var ciError = AffichageSolutionPourCiDontLorganisationAMalEteAffecteDansLarbre(ci, etabs, societes, c2Error);
                    ciToReImport.Add(ciError);
                    if (ciError == null && sousCiError == null)
                    {
                        cisWithoutSolution.Add(ci);
                    }
                }
            }
            Print($"CI EN ERREURS");
            AffichageCi(ciToReImport.Where(x => x != null).ToList(), etabs, societes);

            Print($"SOUS CI EN ERREURS");
            AffichageCi(sousCiToReImport.Where(x => x != null).ToList(), etabs, societes);

            Print($"CI without solution");
            AffichageCi(cisWithoutSolution.Where(x => x != null).ToList(), etabs, societes);

            using (var sw = new StreamWriter("Errors_P1.txt", false))
            {
                sw.Write(stringBuilder.ToString());
            }
        }

        private void AffichageCi(List<CIEnt> cis, List<EtablissementComptableEnt> etabliisements, List<SocieteEnt> societes)
        {
            Print($"");
            Print($"");
            Print($"");
            Print($"");
            Print($"");
            Print($"societeCode-codeEtablissementComptable-code");
            foreach (var ci in cis)
            {
                Print($"{societes.FirstOrDefault(x => x.SocieteId == ci.SocieteId)?.CodeSocieteComptable}-{etabliisements.FirstOrDefault(x => x.EtablissementComptableId == ci.EtablissementComptableId)?.Code}-{ci.Code} ");
            }
        }

        private static StringBuilder stringBuilder = new StringBuilder();
        private static void Print(string v)
        {
            stringBuilder.AppendLine(v);
            Debug.WriteLine(v);
        }

        private static string GetTypeError(bool c1Error, bool c2Error)
        {
            var type = "TYPEZ";
            if (c1Error && c2Error)
            {
                type = "TYPE3";
            }
            if (c1Error && !c2Error)
            {
                type = "TYPE1";
            }
            if (!c1Error && c2Error)
            {
                type = "TYPE2";
            }

            return type;
        }

        private ArbreInfo GetArbreInfo(OrganisationTree tree, CIEnt ci)
        {
            var result = new ArbreInfo();
            var parents = tree.GetAllParentsOfCi(ci.CiId);
            result.ciArbre = tree.GetCi(ci.CiId);
            var ciArbreIsSousCi = result.ciArbre.IsSousCi();
            result.ciParent = parents.FirstOrDefault(x => x.IsCi() && ci.Organisation.OrganisationId != x.OrganisationId);
            result.etabParent = parents.FirstOrDefault(x => x.IsEtablissement());
            result.etabParentOrganisationId = result.etabParent != null ? result.etabParent.OrganisationId.ToString() : "";
            result.societeParent = parents.FirstOrDefault(x => x.IsSociete());
            result.societeParentOrganisationId = result.societeParent.OrganisationId.ToString();
            var societeParenId = result.societeParent.Id.ToString();

            result.ciArbreSocieteCode = result.societeParent.Code;
            result.ciArbreCodeEtablissementComptable = result.etabParent != null ? result.etabParent.Code : "";
            result.ciArbreCode = result.ciArbre.Code;
            return result;
        }

        private TableInfo GetTableInfos(CIEnt ci, SocieteEnt societe, EtablissementComptableEnt etablissement)
        {
            var result = new TableInfo();

            result.ciTableSocieteCode = societe.CodeSocieteComptable;
            //result.var ciTableIsSousCi = ci.Organisation.TypeOrganisationId == 9;
            result.ciTableSocieteOrganisationId = societe.Organisation.OrganisationId.ToString();
            //result.var ciTableSocieteId = societe.SocieteId.ToString();
            result.ciTableCodeEtablissementComptable = etablissement != null ? etablissement.Code : "";
            result.ciTableCodeEtablissementComptableOrganisationId = etablissement != null ? etablissement.Organisation.OrganisationId.ToString() : "";
            result.ciTableCode = ci.Code;

            return result;
        }

        private void AffichageDesCisAvecLeMemeCode(List<CIEnt> cisWithSameCode)
        {
            Print($"CI WITH SAME CODE :");
            foreach (var item in cisWithSameCode)
            {
                Print($"CI : OrganisationId : {item.Organisation.OrganisationId} CiId : {item.CiId} - SocieteId : {item.SocieteId} - EtablissementComptableId : {item.EtablissementComptableId} - CODE : {item.Code} ");
            }
        }

        private void AffichageDesCiParentAvecLeMemeCode(List<CIEnt> cisWithSameCodeParent)
        {
            Print($"CI WITH SAME CODE PARENT :");
            foreach (var item in cisWithSameCodeParent)
            {
                Print($"CI : OrganisationId : {item.Organisation.OrganisationId} CiId : {item.CiId} - SocieteId : {item.SocieteId} - EtablissementComptableId : {item.EtablissementComptableId} - CODE : {item.Code} ");
            }
        }

        private CIEnt AffichageSolutionPourSousCiDontLeParentAMalEteAffecteDansLarbre(CIEnt ci, OrganisationBase ciParent, List<CIEnt> cisWithSameCodeParent, bool c2Error)
        {
            if (ciParent != null && c2Error && ci.Organisation.OrganisationId != ciParent.OrganisationId && ci.Organisation.TypeOrganisationId == 9)
            {
                Print($"POSSIBLE CORRECTION 1");
                Print($"RECHERCHE CI AVEC : SocieteId : {ci.SocieteId} - EtablissementComptableId : {ci.EtablissementComptableId } - ciParent.Code : {ciParent.Code}");
                var possibleMatch = cisWithSameCodeParent.FirstOrDefault(x => x.SocieteId == ci.SocieteId && x.EtablissementComptableId == ci.EtablissementComptableId && x.Code == ciParent.Code);
                if (possibleMatch != null)
                {
                    if (possibleMatch.Organisation.TypeOrganisationId == 8)
                    {
                        if (ciParent.OrganisationId != possibleMatch.Organisation.OrganisationId)
                        {
                            Print($"POSSIBLE CORRECTION 1 : OrganisationId : {possibleMatch.Organisation.OrganisationId} CiId : {possibleMatch.CiId} - SocieteId : {possibleMatch.SocieteId} - EtablissementComptableId : {possibleMatch.EtablissementComptableId} - CODE : {possibleMatch.Code} ");
                            return ci;
                        }
                        else
                        {
                            Print($"POSSIBLE CORRECTION 1 : en echec 1A : C'est le meme ci parent qu'avant. voir si le ci parent n'a pas lui aussi un probleme : {ciParent.Code}");
                        }

                    }
                    else
                    {
                        Print($"POSSIBLE CORRECTION 1 : en echec 1B : C'est le meme ci qui sera le ci parent.");
                    }
                }
                else
                {
                    Print($"POSSIBLE CORRECTION 1 : en echec 1C : Pas de ci qui match avec la recherche.");
                }
            }
            return null;
        }

        private CIEnt AffichageSolutionPourCiDontLorganisationAMalEteAffecteDansLarbre(CIEnt ci, List<EtablissementComptableEnt> etabliisements, List<SocieteEnt> societes, bool c2Error)
        {
            if (c2Error && ci.Organisation.TypeOrganisationId == 8)
            {
                Print($"POSSIBLE CORRECTION 2");
                Print($"RECHERCHE CI AVEC : SocieteId : {ci.SocieteId} - EtablissementComptableId : {ci.EtablissementComptableId } ");
                var possibleMatch = etabliisements.FirstOrDefault(x => x.SocieteId == ci.SocieteId && x.EtablissementComptableId == ci.EtablissementComptableId);
                if (possibleMatch != null)
                {
                    Print($"POSSIBLE CORRECTION 2 : INFO ETABLISSEMENT : OrganisationId : {possibleMatch.Organisation.OrganisationId} - CODE : {possibleMatch.Code} - EtablissementComptableId : {possibleMatch.EtablissementComptableId} - SocieteId : {possibleMatch.SocieteId} ");
                    var societe = societes.First(x => x.SocieteId == possibleMatch.SocieteId);
                    Print($"POSSIBLE CORRECTION 2 : INFO SOCIETE       : OrganisationId : {societe.Organisation.OrganisationId} - CODE : {possibleMatch.Code} - SocieteId : {possibleMatch.SocieteId}  ");
                    return ci;
                }
                else
                {
                    Print($"POSSIBLE CORRECTION 2 : en echec 3 : Pas de ci qui match avec la recherche.");
                }
            }
            return null;
        }

        private static bool HasErrorOnSociete(SocieteEnt societe, OrganisationBase societeParent)
        {
            return societeParent.OrganisationId != societe.Organisation.OrganisationId;
        }

        private static bool HasErroOnEtablissement(EtablissementComptableEnt etablissement, OrganisationBase etabParent)
        {
            return etabParent?.OrganisationId != etablissement?.Organisation.OrganisationId;
        }

        /// <summary>
        /// Afficher toutes les erreurs de l'arbre des organisations 
        /// </summary>
        [TestMethod]
        [TestCategory("DBDepend")]
        public void LaunchOgranisationTreeTest()
        {
            var organisationTreeRepository = new OrganisationTreeRepository(FredContext);
            var organisationTreeSvc = new OrganisationTreeService(organisationTreeRepository, CacheMgr);


            // Si un user est défini alors récupérer son arbre 
            // Sinon Traiter tout l'arbre
            //int? idUser = 5625;
            //int? idUser = 11169;
            int? idUser = null;

            // Récupérer l'arbre
            Entities.Organisation.Tree.OrganisationTree tree = organisationTreeSvc.GetOrganisationTree();

            // Si un utilisateur
            if (idUser != null)
            {
                tree = organisationTreeSvc.GetOrganisationTree().GetOrganisationTreeForUser((int)idUser);
            }

            // Traiter tous les noeuds
            var stringBuilderErrors = ProcessTree(tree);

            //Initialize mon type d'output et lui envoie le string builder contenant les erreurs.
            _outputProcessorFactory.UseFileOutputProcessor().Process(stringBuilderErrors);
        }

        /// <summary>
        /// Afficher toutes les erreurs de l'arbre
        /// </summary>
        private StringBuilder ProcessTree(Entities.Organisation.Tree.OrganisationTree tree)
        {
            var nbEntitesEnErreur = 0;

            var nbEntitesEnErreurs = new Dictionary<OrganisationType, int>()
            {
                { OrganisationType.Societe, 0 },
                { OrganisationType.Etablissement, 0 },
                { OrganisationType.Ci, 0 },
                { OrganisationType.SousCi, 0 }
            };

            var repoCi = CiRepository;
            var repoEtab = EtablissementComptableRepository;
            var repoSociete = SocieteRepository;

            var cis = repoCi.Get().Include(x => x.Organisation).Where(x => x.Organisation.TypeOrganisationId == 8).ToList();
            var cisCount = cis.Count;
            var sousCis = repoCi.Get().Include(x => x.Organisation).Where(x => x.Organisation.TypeOrganisationId == 9).ToList();
            var sousCisCount = sousCis.Count;
            var etabs = repoEtab.Query().Get().AsNoTracking().ToList();
            var societes = repoSociete.Query().Get().AsNoTracking().ToList();

            // Get root node
            Node<OrganisationBase> rootNode = tree.Nodes.FirstOrDefault();

            // Ordonner les noeuds
            var orderedNodes = rootNode.PreOrder();

            List<string> messages = new List<string>();
            var strBuilder = new StringBuilder();

            // Afficher les noeuds
            foreach (var nodeTmp in orderedNodes)
            {
                var orgaID = (OrganisationType)nodeTmp.TypeOrganisationId;

                // Clear anciens messages
                messages.Clear();

                strBuilder.AppendLine(orgaID.ToFriendlyString().PadRight(30) + "|" + new string('-', nodeTmp.TypeOrganisationId * 2) + nodeTmp.ToStringTest());

                // Vérifier les incohérences selon le type
                switch (orgaID)
                {
                    case OrganisationType.Holding:
                        break;
                    case OrganisationType.Pole:
                        break;
                    case OrganisationType.Groupe:
                        break;
                    case OrganisationType.Societe:
                        messages.AddRange(ProcessSociete(tree, societes.Where(a => a.SocieteId == nodeTmp.Id).FirstOrDefault()));
                        break;
                    case OrganisationType.Puo:
                        break;
                    case OrganisationType.Uo:
                        break;
                    case OrganisationType.Etablissement:
                        messages.AddRange(ProcessEtablissement(tree, etabs.Where(a => a.EtablissementComptableId == nodeTmp.Id).FirstOrDefault()));
                        break;
                    case OrganisationType.Ci:
                        messages.AddRange(ProcessCi(tree, cis.Where(a => a.CiId == nodeTmp.Id).FirstOrDefault()));
                        break;
                    case OrganisationType.SousCi:
                        messages.AddRange(ProcessSousCi(tree, sousCis.Where(a => a.CiId == nodeTmp.Id).FirstOrDefault()));
                        break;
                    default:
                        break;
                }

                // Display messages
                if ((messages != null) && (messages.Count > 0))
                {
                    strBuilder.AppendLine("".PadRight(30) + "########################################################################@" + orgaID);
                    foreach (var messageTmp in messages)
                    {
                        strBuilder.AppendLine("".PadRight(30) + messageTmp);
                    }
                    strBuilder.AppendLine("".PadRight(30) + "########################################################################");

                    // Incrémenter le nb erreurs général
                    nbEntitesEnErreur++;

                    // Incrementer par type d'erreur
                    nbEntitesEnErreurs[(OrganisationType)nodeTmp.TypeOrganisationId]++;
                }
            }


            strBuilder.AppendLine($"########################################################################");
            strBuilder.AppendLine($"########  Nb total d'erreurs  :       {nbEntitesEnErreur}             ");
            strBuilder.AppendLine($"########################################################################");

            //Affichage par type d'erreur
            foreach (var item in nbEntitesEnErreurs)
            {
                strBuilder.AppendLine($"########  Nb d'erreurs sur {item.Key.ToFriendlyString()}  :       {item.Value}        (recherche: @{item.Key}) ");
            }

            strBuilder.AppendLine($"########################################################################");

            return strBuilder;
        }

        /// <summary>
        /// Trouver les erreurs liés à un CI
        /// </summary>
        /// <param name="tree">Arbre</param>
        /// <param name="ci">CI concerné</param>
        /// <returns>Liste des messages d'erreurs</returns>
        private List<string> ProcessCi(Entities.Organisation.Tree.OrganisationTree tree, CIEnt ci)
        {
            // Liste des messages
            List<string> messages = new List<string>();
            OrganisationBase societeOrganisationBase = null;
            OrganisationBase etablissementComptableOrganisationBase = null;

            if (ci == null)
            {
                messages.Add($"---Aucun CI trouvé dans la table CI pour ce noeud dans l'arbre !");
            }
            else
            {
                // Récupérer l'organisation correspondante
                OrganisationBase ciOrganisationBase = tree.GetCiStrict(ci.CiId);

                if (ciOrganisationBase == null)
                {
                    messages.Add($"---Aucune organisation trouvée dans l'arbre pour ce CI !");
                }
                else
                {
                    //etablissementComptableOrganisationBase = organisationTree.GetEtablissementComptable((ciOrganisationBase.OrganisationId);
                    etablissementComptableOrganisationBase = tree.GetEtablissementComptableParent(ciOrganisationBase.OrganisationId);
                    bool isEtabDefiniDansArbre = (etablissementComptableOrganisationBase != null);
                    bool isEtabDefiniDansTable = (ci.EtablissementComptableId != null);
                    if (isEtabDefiniDansArbre && !isEtabDefiniDansTable)
                    {
                        messages.Add($"---Non correspondance de l'établissement comptable : ARBRE(EtablissementId: {etablissementComptableOrganisationBase.Id}) / TABLE_CI(EtablissementId: null)");
                    }
                    else
                    {
                        if (!isEtabDefiniDansArbre && isEtabDefiniDansTable)
                        {
                            messages.Add($"---Non correspondance de l'établissement comptable : ARBRE(EtablissementId: null) / TABLE_CI(EtablissementId: {ci.EtablissementComptableId})");
                        }
                        else
                        {
                            if ((isEtabDefiniDansArbre && isEtabDefiniDansTable) && (etablissementComptableOrganisationBase.Id != ci.EtablissementComptableId))
                            {
                                messages.Add($"---Non correspondance de l'établissement comptable : ARBRE(EtablissementId: {etablissementComptableOrganisationBase.Id}) / TABLE_CI(EtablissementId: {ci.EtablissementComptableId})");
                            }
                        }
                    }

                    //societeOrganisationBase = organisationTree.GetSocieteParentOfCi(ci.CiId);
                    societeOrganisationBase = tree.GetSocieteParent(ciOrganisationBase.OrganisationId);
                    bool isSocieteDefinieDansArbre = (societeOrganisationBase != null);
                    bool isSocieteDefinieDansTable = (ci.SocieteId != null);
                    if (!isSocieteDefinieDansArbre && isSocieteDefinieDansTable)
                    {
                        messages.Add($"---Non correspondance de la société : ARBRE(SocieteId: null) / TABLE_CI(SocieteId: {ci.SocieteId})");
                    }
                    else
                    {
                        if (isSocieteDefinieDansArbre && !isSocieteDefinieDansTable)
                        {
                            messages.Add($"---Non correspondance de la société : ARBRE(SocieteId: {societeOrganisationBase.Id}) / TABLE_CI(SocieteId: null)");
                        }
                        else
                        {
                            if (isSocieteDefinieDansArbre && isSocieteDefinieDansTable && (ci.SocieteId != societeOrganisationBase.Id))
                            {
                                messages.Add($"---Non correspondance de la société : ARBRE(SocieteId: {societeOrganisationBase.Id}) / TABLE_CI(SocieteId: {ci.SocieteId})");
                            }
                        }
                    }
                }
            }

            if ((messages != null) && (messages.Count > 0))
            {
                messages.Insert(0, "------------------------------------------------------------------------");
                if (ci != null)
                {
                    messages.Insert(0, $"Erreurs sur le CI {{{ci.ToStringTest()}}}");
                }
                else
                {
                    messages.Insert(0, $"Erreurs sur le CI");
                }
            }

            return messages;
        }

        /// <summary>
        /// Trouver les erreurs liés à un CI
        /// </summary>
        /// <param name="tree">Arbre</param>
        /// <param name="ci">CI concerné</param>
        /// <returns>Liste des messages d'erreurs</returns>
        private List<string> ProcessSousCi(Entities.Organisation.Tree.OrganisationTree tree, CIEnt ci)
        {
            // Liste des messages
            List<string> messages = new List<string>();
            OrganisationBase societeOrganisationBase = null;
            OrganisationBase etablissementComptableOrganisationBase = null;

            if (ci == null)
            {
                messages.Add($"---Aucun Sous CI trouvé dans la table CI pour ce noeud dans l'arbre !");
            }
            else
            {
                // Récupérer l'organisation correspondante
                OrganisationBase ciOrganisationBase = tree.GetSousCi(ci.CiId);

                if (ciOrganisationBase == null)
                {
                    messages.Add($"---Aucune organisation trouvée dans l'arbre pour ce sous CI !");
                }
                else
                {
                    //etablissementComptableOrganisationBase = organisationTree.GetEtablissementComptable((ciOrganisationBase.OrganisationId);
                    etablissementComptableOrganisationBase = tree.GetEtablissementComptableParent(ciOrganisationBase.OrganisationId);
                    bool isEtabDefiniDansArbre = (etablissementComptableOrganisationBase != null);
                    bool isEtabDefiniDansTable = (ci.EtablissementComptableId != null);
                    if (isEtabDefiniDansArbre && !isEtabDefiniDansTable)
                    {
                        messages.Add($"---Non correspondance de l'établissement comptable : ARBRE(EtablissementId: {etablissementComptableOrganisationBase.Id}) / TABLE_CI(EtablissementId: null)");
                    }
                    else
                    {
                        if (!isEtabDefiniDansArbre && isEtabDefiniDansTable)
                        {
                            messages.Add($"---Non correspondance de l'établissement comptable : ARBRE(EtablissementId: null) / TABLE_CI(EtablissementId: {ci.EtablissementComptableId})");
                        }
                        else
                        {
                            if ((isEtabDefiniDansArbre && isEtabDefiniDansTable) && (etablissementComptableOrganisationBase.Id != ci.EtablissementComptableId))
                            {
                                messages.Add($"---Non correspondance de l'établissement comptable : ARBRE(EtablissementId: {etablissementComptableOrganisationBase.Id}) / TABLE_CI(EtablissementId: {ci.EtablissementComptableId})");
                            }
                        }
                    }

                    //societeOrganisationBase = organisationTree.GetSocieteParentOfCi(ci.CiId);
                    societeOrganisationBase = tree.GetSocieteParent(ciOrganisationBase.OrganisationId);
                    bool isSocieteDefinieDansArbre = (societeOrganisationBase != null);
                    bool isSocieteDefinieDansTable = (ci.SocieteId != null);
                    if (!isSocieteDefinieDansArbre && isSocieteDefinieDansTable)
                    {
                        messages.Add($"---Non correspondance de la société : ARBRE(SocieteId: null) / TABLE_CI(SocieteId: {ci.SocieteId})");
                    }
                    else
                    {
                        if (isSocieteDefinieDansArbre && !isSocieteDefinieDansTable)
                        {
                            messages.Add($"---Non correspondance de la société : ARBRE(SocieteId: {societeOrganisationBase.Id}) / TABLE_CI(SocieteId: null)");
                        }
                        else
                        {
                            if (isSocieteDefinieDansArbre && isSocieteDefinieDansTable && (ci.SocieteId != societeOrganisationBase.Id))
                            {
                                messages.Add($"---Non correspondance de la société : ARBRE(SocieteId: {societeOrganisationBase.Id}) / TABLE_CI(SocieteId: {ci.SocieteId})");
                            }
                        }
                    }
                }
            }

            if ((messages != null) && (messages.Count > 0))
            {
                messages.Insert(0, "------------------------------------------------------------------------");
                if (ci != null)
                {
                    messages.Insert(0, $"Erreurs sur le Sous CI {{{ci.ToStringTest()}}}");
                }
                else
                {
                    messages.Insert(0, $"Erreurs sur le Sous CI");
                }
            }

            return messages;
        }

        /// <summary>
        /// Trouver les erreurs liés à un établissement comptable
        /// </summary>
        /// <param name="tree">Arbre</param>
        /// <param name="etab">Établissement comptable concerné</param>
        /// <returns>Liste des messages d'erreurs</returns>
        private List<string> ProcessEtablissement(Entities.Organisation.Tree.OrganisationTree tree, EtablissementComptableEnt etab)
        {
            // Liste des messages
            List<string> messages = new List<string>();
            OrganisationBase societeOrganisationBase = null;

            if (etab == null)
            {
                messages.Add($"---Aucun établissement comptable trouvé dans la table CI pour ce noeud dans l'arbre !");
            }
            else
            {
                // Récupérer l'organisation correspondante
                OrganisationBase ciOrganisationBase = tree.GetEtablissementComptable(etab.EtablissementComptableId);

                if (ciOrganisationBase == null)
                {
                    messages.Add($"---Aucune organisation trouvée dans l'arbre pour cet établissement comptable !");
                }
                else
                {
                    //societeOrganisationBase = organisationTree.GetSocieteParentOfCi(ci.CiId);
                    societeOrganisationBase = tree.GetSocieteParent(ciOrganisationBase.OrganisationId);
                    bool isSocieteDefinieDansArbre = (societeOrganisationBase != null);
                    bool isSocieteDefinieDansTable = (etab.SocieteId != null);
                    if (!isSocieteDefinieDansArbre && isSocieteDefinieDansTable)
                    {
                        messages.Add($"---Non correspondance de la société : ARBRE(SocieteId: null) / TABLE_CI(SocieteId: {etab.SocieteId})");
                    }
                    else
                    {
                        if (isSocieteDefinieDansArbre && !isSocieteDefinieDansTable)
                        {
                            messages.Add($"---Non correspondance de la société : ARBRE(SocieteId: {societeOrganisationBase.Id}) / TABLE_CI(SocieteId: null)");
                        }
                        else
                        {
                            if (isSocieteDefinieDansArbre && isSocieteDefinieDansTable && (etab.SocieteId != societeOrganisationBase.Id))
                            {
                                messages.Add($"---Non correspondance de la société : ARBRE(SocieteId: {societeOrganisationBase.Id}) / TABLE_CI(SocieteId: {etab.SocieteId})");
                            }
                        }
                    }
                }
            }

            if ((messages != null) && (messages.Count > 0))
            {
                messages.Insert(0, "------------------------------------------------------------------------");
                if (etab != null)
                {
                    messages.Insert(0, $"Erreurs sur l'établissement comptable {{{etab.ToStringTest()}}}");
                }
                else
                {
                    messages.Insert(0, $"Erreurs sur l'établissement comptable");
                }
            }

            return messages;
        }

        /// <summary>
        /// Trouver les erreurs liés à une société
        /// </summary>
        /// <param name="tree">Arbre</param>
        /// <param name="societeEnt">Établissement comptable concerné</param>
        /// <returns>Liste des messages d'erreurs</returns>
        private List<string> ProcessSociete(Entities.Organisation.Tree.OrganisationTree tree, SocieteEnt societeEnt)
        {
            // Liste des messages
            List<string> messages = new List<string>();
            OrganisationBase groupeBase = null;
            OrganisationBase societeBase = null;

            if (societeEnt == null)
            {
                messages.Add($"---Aucune société trouvée dans la table Société pour ce noeud dans l'arbre !");
            }
            else
            {
                // Récupérer l'organisation correspondante
                societeBase = tree.GetSociete(societeEnt.SocieteId);

                if (societeBase == null)
                {
                    messages.Add($"---Aucune organisation trouvée dans l'arbre pour cette société !");
                }
                else
                {
                    groupeBase = tree.GetGroupeParent(societeBase.OrganisationId);
                    bool isGroupeDefinieDansArbre = (groupeBase != null);
                    if (!isGroupeDefinieDansArbre)
                    {
                        messages.Add($"---Non correspondance du groupe : ARBRE(GroupeId: null) / TABLE_CI(GroupeId: {societeEnt.GroupeId})");
                    }
                    else
                    {
                        if (isGroupeDefinieDansArbre && (societeEnt.GroupeId != groupeBase.Id))
                        {
                            messages.Add($"---Non correspondance du groupe : ARBRE(GroupeId: {groupeBase.Id}) / TABLE_CI(GroupeId: {societeEnt.SocieteId})");
                        }
                    }
                }
            }

            if ((messages != null) && (messages.Count > 0))
            {
                messages.Insert(0, "------------------------------------------------------------------------");
                if (societeEnt != null)
                {
                    messages.Insert(0, $"Erreurs sur la société {{{societeEnt.ToStringTest()}}}");
                }
                else
                {
                    messages.Insert(0, $"Erreur sur la société");
                }

            }

            return messages;
        }
    }
}
