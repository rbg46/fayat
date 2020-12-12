using System;
using System.Collections.Generic;
using FluentAssertions;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.ImportExport.Business.CI.WebApi.Context.Models;
using Fred.ImportExport.Business.CI.WebApi.Validator;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models.Ci;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fred.ImportExport.Business.Tests.CI.WebApi.Validator
{
    [TestClass]
    public class ImportByWebApiValidatorTests
    {

        [TestMethod]
        public void Verify_Si_la_societe_est_non_reconnue_alors_une_exception_doit_etre_soulevee()
        {
            var codeSocieteComptableNoExisting = "No_Exist_CodeSocieteComptable";

            var societeExistValidator = new SocieteValidator();

            ImportCiByWebApiContext context = new ImportCiByWebApiContext();

            context.SocietesContexts.Add(new ImportCiByWebApiSocieteContext()
            {
                Societe = null,
                CodeSocieteComptable = codeSocieteComptableNoExisting
            });

            Action act = () => societeExistValidator.VerifyAllSocietesFound(context);

            string errorMessage = string.Format(FredImportExportBusinessResources.Error_Import_Web_Api_Societe_Not_Found, codeSocieteComptableNoExisting);

            act.Should().Throw<FredIeBusinessException>().WithMessage(errorMessage);

        }


        [TestMethod]
        public void Verify_Si_la_societe_est_non_active_alors_une_exception_doit_etre_soulevee()
        {
            var codeSocieteComptable = "E001";

            var societeExistValidator = new SocieteValidator();

            ImportCiByWebApiContext context = new ImportCiByWebApiContext();

            context.SocietesContexts.Add(new ImportCiByWebApiSocieteContext()
            {
                Societe = new SocieteEnt
                {
                    Active = false
                },
                CodeSocieteComptable = codeSocieteComptable,

            });

            Action act = () => societeExistValidator.VerifyAllSocieteAreActives(context);

            string errorMessage = string.Format(FredImportExportBusinessResources.Error_Import_Web_Api_Societe_Not_Active, codeSocieteComptable);

            act.Should().Throw<FredIeBusinessException>().WithMessage(errorMessage);

        }

        [TestMethod]
        public void Verify_Si_l_etablissemnt_est_non_reconnu_alors_une_exception_doit_etre_soulevee()
        {
            var codeEtablissementComptable = "10";

            var etablissementValidator = new EtablissementValidator();

            ImportCiByWebApiContext context = new ImportCiByWebApiContext();

            context.SocietesContexts.Add(new ImportCiByWebApiSocieteContext()
            {
                EtablissementComptables = new List<EtablissementComptableEnt>(),

                CisToImport = new List<WebApiCiModel>()
                {
                    new WebApiCiModel()
                    {
                        CodeEtablissementComptable = codeEtablissementComptable
                    }
                }

            });

            Action act = () => etablissementValidator.VerifyAllEtablissementFoundForAllWebApiCis(context);

            string errorMessage = string.Format(FredImportExportBusinessResources.Error_Import_Web_Api_Etablissement_Not_Found, codeEtablissementComptable);

            act.Should().Throw<FredIeBusinessException>().WithMessage(errorMessage);

        }


        [TestMethod]
        public void Verify_Si_le_code_est_vide_alors_une_exception_doit_etre_soulevee()
        {
            var code = string.Empty;
            var libelle = "Ci witout code";
            var description = "TestImport";

            var consistencyValidator = new ConsistencyValidator();

            ImportCiByWebApiContext context = new ImportCiByWebApiContext();

            context.SocietesContexts.Add(new ImportCiByWebApiSocieteContext()
            {
                CisToImport = new List<WebApiCiModel>()
                {
                    new WebApiCiModel()
                    {
                        Code = string.Empty,
                        Libelle = libelle,
                        Description = description
                    }
                }

            });

            Action act = () => consistencyValidator.VerifyCodeAndLibelleForAllWebApiCis(context);

            string errorMessage = string.Format(FredImportExportBusinessResources.Error_Import_Web_Api_Code_Or_Libelle_Not_Found, code, libelle, description);

            act.Should().Throw<FredIeBusinessException>().WithMessage(errorMessage);

        }

        [TestMethod]
        public void Verify_Si_le_code_est_libelle_alors_une_exception_doit_etre_soulevee()
        {
            var code = "Ci witout Libelle";
            var libelle = string.Empty;
            var description = "TestImport";

            var consistencyValidator = new ConsistencyValidator();

            ImportCiByWebApiContext context = new ImportCiByWebApiContext();

            context.SocietesContexts.Add(new ImportCiByWebApiSocieteContext()
            {
                CisToImport = new List<WebApiCiModel>()
                {
                    new WebApiCiModel()
                    {
                        Code = code,
                        Libelle = libelle,
                        Description = description
                    }
                }

            });

            Action act = () => consistencyValidator.VerifyCodeAndLibelleForAllWebApiCis(context);

            string errorMessage = string.Format(FredImportExportBusinessResources.Error_Import_Web_Api_Code_Or_Libelle_Not_Found, code, libelle, description);

            act.Should().Throw<FredIeBusinessException>().WithMessage(errorMessage);

        }


    }
}
