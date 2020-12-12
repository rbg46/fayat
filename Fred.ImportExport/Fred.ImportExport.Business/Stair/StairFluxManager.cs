using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Flux;
using Fred.ImportExport.Database.ImportExport;
using Fred.ImportExport.Entities.Stair;
using Hangfire;
using Microsoft.VisualBasic.FileIO;

namespace Fred.ImportExport.Business.Stair
{
    public class StairFluxManager : AbstractFluxManager
    {
        private readonly string pathIn = ConfigurationManager.AppSettings["Stair:ImportIndicateurs:PathIn"];
        private readonly string pathErr = ConfigurationManager.AppSettings["Stair:ImportIndicateurs:PathErr"];
        private readonly string pathArchive = ConfigurationManager.AppSettings["Stair:ImportIndicateurs:PathArchive"];

        public string ImportJobId { get; } = ConfigurationManager.AppSettings["flux:stair"];

        public StairFluxManager(IFluxManager fluxManager)
            : base(fluxManager)
        {
            Flux = FluxManager.GetByCode(ImportJobId);
        }

        public void ExecuteImport()
        {
            BackgroundJob.Enqueue(() => Import());
        }

        public void ScheduleImport(string cron)
        {
            if (!string.IsNullOrEmpty(cron))
            {
                RecurringJob.AddOrUpdate(ImportJobId, () => Import(), cron);
            }
            else
            {
                string msg = $"Le CRON n'est pas paramétré pour le job {ImportJobId}";
                var exception = new FredBusinessException(msg);
                NLog.LogManager.GetCurrentClassLogger().Error(exception, msg);
                throw exception;
            }
        }

        public void Import()
        {
            ImportDesFormulaireSafe();
            ImportDesPlanActionSafe();

        }

        #region FormlaireSafe

        public void ImportDesFormulaireSafe()
        {

            DirectoryInfo directory = new DirectoryInfo(pathIn);

            FileInfo[] files = directory.GetFiles("FOR_*");


            foreach (FileInfo file in files)
            {

                List<StairSafeEnt> stairSafeEntities = new List<StairSafeEnt>();
                ExtraireSafeEntities(file, stairSafeEntities);

                if (!File.Exists(file.FullName.Replace(file.Extension, "log")))
                {
                    StairContext stairctx = new StairContext();
                    try
                    {
                        stairctx.StairIndicateurSafe.AddRange(stairSafeEntities);

                        stairctx.SaveChanges();


                    }
                    catch (Exception e)
                    {
                        e.ToString();
                    }

                    file.MoveTo(pathArchive + file.Name);
                }
                else
                {
                    File.Move(file.FullName.Replace(file.Extension, "log"), pathErr + file.Name.Replace(file.Extension, ".log"));
                    file.MoveTo(pathErr + file.Name);
                    //insert logs
                }


            }
        }

        private void ExtraireSafeEntities(FileInfo file, List<StairSafeEnt> stairSafeEntities)
        {
            using (TextFieldParser csvParser = new TextFieldParser(file.FullName, Encoding.UTF8))
            {

                csvParser.SetDelimiters(new string[] { ";" });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names


                string message;
                if (!File.Exists(pathArchive + file.Name))
                {
                    csvParser.ReadLine();
                    while (!csvParser.EndOfData)
                    {

                        // Read current line fields, pointer moves to the next line.
                        string[] champs = csvParser.ReadFields();



                        message = VerificationChampsFormulaireSafe(champs);

                        if (message == string.Empty)
                        {
                            stairSafeEntities.Add(new StairSafeEnt(champs));
                        }
                        else
                        {
                            File.AppendAllText(file.FullName.Replace(file.Extension, "log"), " un champ dans le fichier \" " + file.FullName + " \" est invalide. " + message + ". ( ligne : " + (csvParser.LineNumber - 1) + ")" + Environment.NewLine);
                        }
                    }
                }
                else
                {
                    File.AppendAllText(file.FullName.Replace(file.Extension, "log"), "Ce fichier ou un fichier du même nom a déjà été traité");
                }

            }
        }

        public string VerificationChampsFormulaireSafe(string[] champs)
        {
            string verification = string.Empty;

            //Test des champs

            verification = TestIntFormulaire(champs, verification);

            //Test du champ de date
            //expression réguliere datetime : ^[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1]) (0[1-9]|1[0-9]|2[0-3]):([0-5]|[0-9])
            Regex datetimeRegex = new Regex(@"^[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1]) (0[1-9]|1[0-9]|2[0-3]):([0-5]|[0-9])");
            if (!datetimeRegex.IsMatch(champs[3]))
            {
                verification = "[STAIR] Import Stair Indicateurs : le format de la date \"date\"n'est pas correcte.";
            }

            verification = TestAnswerData(champs, verification);

            return verification;

        }

        private string TestAnswerData(string[] champs, string verification)
        {
            // Test sur la cohérence entre le type et la valeur (TypeResponse et AnswerData)
            switch (champs[9])
            {
                case "bool":
                    if (champs[11] != "1" && champs[11] != "0")
                    {
                        verification = "[STAIR] Import Stair Indicateurs : le champ \"AnswerData\" n'est pas correct. Ce n'est pas un bool.";
                    }

                    break;
                case "int":
                    if (!IsInt(champs[11]))
                    {
                        verification = "[STAIR] Import Stair Indicateurs : le champ \"AnswerData\" n'est pas correct. Ce n'est pas un entier.";
                    }

                    break;
                case "datetime":
                    Regex dateRegex = new Regex(@"^[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])");
                    if (!dateRegex.IsMatch(champs[11]) && champs[11] != string.Empty)
                    {
                        verification = "[STAIR] Import Stair Indicateurs : le champ \"AnswerData\" n'est pas correct. Ce n'est pas une date et heure";
                    }

                    break;
                case "time":
                    Regex timeRegex = new Regex(@"^(0[0-9]|1[0-9]|2[0-3]):([0-5]|[0-9])");
                    if (!timeRegex.IsMatch(champs[11]) && champs[11] != string.Empty)
                    {
                        verification = "[STAIR] Import Stair Indicateurs : le champ \"AnswerData\" n'est pas correct. Ce n'est pas une heure";
                    }

                    break;

            }

            return verification;
        }

        private string TestIntFormulaire(string[] champs, string verification)
        {
            //Test si IDHistory, IDQuestion et IDResponse sont des entiers
            if (!IsInt(champs[1]))
            {
                verification = "[STAIR] Import Stair Indicateurs : le champ \"IDHistory\" n'est pas correct. Ce n'est pas un entier.";
            }

            if (!IsInt(champs[6]))
            {
                verification = "[STAIR] Import Stair Indicateurs : le champ \"IDQuestion\" n'est pas correct. Ce n'est pas un entier.";
            }

            if (!IsInt(champs[8]))
            {
                verification = "[STAIR] Import Stair Indicateurs : le champ \"IDResponse\" n'est pas correct. Ce n'est pas un entier.";
            }

            return verification;
        }

        #endregion FormulaireSafe

        #region Plan d'Action

        public void ImportDesPlanActionSafe()
        {
            DirectoryInfo directory = new DirectoryInfo(pathIn);

            FileInfo[] files = directory.GetFiles("PLA_*");


            foreach (FileInfo file in files)
            {

                List<StairPlanActionEnt> stairPlanActionEntities = new List<StairPlanActionEnt>();
                using (TextFieldParser csvParser = new TextFieldParser(file.FullName, Encoding.UTF8))
                {

                    csvParser.SetDelimiters(new string[] { ";" });
                    csvParser.HasFieldsEnclosedInQuotes = true;

                    // Skip the row with the column names
                    csvParser.ReadLine();

                    string message;

                    while (!csvParser.EndOfData)
                    {

                        // Read current line fields, pointer moves to the next line.
                        string[] champs = csvParser.ReadFields();



                        message = VerfificationChampsPlanActionSafe(champs);

                        if (message == string.Empty)
                        {
                            stairPlanActionEntities.Add(new StairPlanActionEnt(champs));
                        }
                        else
                        {
                            File.AppendAllText(file.FullName.Replace(file.Extension, "log"), " un champ dans le fichier \" " + file.FullName + " \" est invalide. " + message + ". ( ligne : " + (csvParser.LineNumber - 1) + ")" + Environment.NewLine);
                        }
                    }
                }

                if (!File.Exists(file.FullName.Replace(file.Extension, "log")))
                {
                    StairContext stairctx = new StairContext();
                    try
                    {
                        file.MoveTo(pathArchive + file.Name);

                        stairctx.StairPlanActionSafe.AddRange(stairPlanActionEntities);

                        stairctx.SaveChanges();


                    }
                    catch
                    {

                        //Erreur de connexion avec la base
                        //Le fichier a déjà été traité une fois

                    }
                }
                else
                {
                    File.Move(file.FullName.Replace(file.Extension, "log"), pathErr + file.Name.Replace(file.Extension, ".log"));
                    file.MoveTo(pathErr + file.Name);
                    //insert logs
                }


            }
        }


        public string VerfificationChampsPlanActionSafe(string[] champs)
        {
            string verification = string.Empty;

            //Test des champs

            verification = TestIntPlanAction(champs, verification);
            verification = TestDatePlanAction(champs, verification);

            return verification;
        }

        private static string TestDatePlanAction(string[] champs, string verification)
        {
            //Test du champ de date
            //expression réguliere datetime : ^[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])
            Regex datetimeRegex = new Regex(@"^[0-9]{4}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])");
            if (!datetimeRegex.IsMatch(champs[7]) && champs[7] != string.Empty)
            {
                verification = "[STAIR] Import Stair Indicateurs : le format de la date \"CreatedDate\"n'est pas correcte.";
            }

            if (!datetimeRegex.IsMatch(champs[8]) && champs[8] != string.Empty)
            {
                verification = "[STAIR] Import Stair Indicateurs : le format de la date \"DueDate\"n'est pas correcte.";
            }

            if (!datetimeRegex.IsMatch(champs[9]) && champs[9] != string.Empty)
            {
                verification = "[STAIR] Import Stair Indicateurs : le format de la date \"ResolutionDate\"n'est pas correcte.";
            }

            return verification;
        }

        private string TestIntPlanAction(string[] champs, string verification)
        {
            //Test si IDHistory, IDQuestion et IDResponse sont des entiers
            if (!IsInt(champs[2]))
            {
                verification = "[STAIR] Import Stair Plan d'action : le champ \"IDHistory\" n'est pas correct. Ce n'est pas un entier.";
            }

            if (!IsInt(champs[3]))
            {
                verification = "[STAIR] Import Stair Plan d'action : le champ \"IDQuestion\" n'est pas correct. Ce n'est pas un entier.";
            }

            if (!IsInt(champs[4]))
            {
                verification = "[STAIR] Import Stair Plan d'action : le champ \"IDResponse\" n'est pas correct. Ce n'est pas un entier.";
            }

            if (!IsInt(champs[13]))
            {
                verification = "[STAIR] Import Stair Plan d'action : le champ \"Priority\" n'est pas correct. Ce n'est pas un entier.";
            }

            return verification;
        }


        #endregion Plan d'Actioon

        public bool IsInt(string chaine)
        {
            var valeur = true;
            char[] tab = chaine.ToCharArray();

            foreach (char carac in tab)
            {
                if (!char.IsDigit(carac) && valeur)
                {
                    valeur = false;
                }
            }

            return valeur;
        }
    }
}