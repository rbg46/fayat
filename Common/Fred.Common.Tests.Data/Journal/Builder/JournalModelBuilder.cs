using Fred.Common.Tests.EntityFramework;
using Fred.Web.Models.Journal;
using System;

namespace Fred.Common.Tests.Data.Journal.Builder
{
    public class JournalModelBuilder : ModelDataTestBuilder<JournalModel>
    {
        public JournalModel Prototype()
        {
            Model.JournalId = 0;
            Model.Code = "NDS";
            Model.Libelle = "Note débit RNF Matériel";
            Model.SocieteId = 1;
            Model.TypeJournal = "L";
            Model.DateCloture = null;

            return Model;
        }

        public JournalModelBuilder Default()
        {
            base.New();
            Model.JournalId = 0;
            Model.Code = "NDS";
            Model.Libelle = "Note débit RNF Matériel";
            Model.SocieteId = 1;
            Model.TypeJournal = "L";
            Model.DateCloture = null;

            return this;
        }

        public JournalModelBuilder JournalId(int id)
        {
            Model.JournalId = id;
            return this;
        }

        public JournalModelBuilder Code(string code)
        {
            Model.Code = code;
            return this;
        }

        public JournalModelBuilder Libelle(string libelle)
        {
            Model.Libelle = libelle;
            return this;
        }

        public JournalModelBuilder SocieteId(int id)
        {
            Model.SocieteId = id;
            return this;
        }

        public JournalModelBuilder TypeJournal(string typeJournal)
        {
            Model.TypeJournal = typeJournal;
            return this;
        }

        public JournalModelBuilder DateCloture(DateTime dateCloture)
        {
            Model.DateCloture = dateCloture;
            return this;
        }
    }
}
