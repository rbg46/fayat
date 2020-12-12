using System;
using System.Collections.Generic;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Affectation;
using Fred.Entities.Referential;

namespace Fred.Common.Tests.Data.Astreinte.Builder
{
    public class CodeAstreinteBuilder : ModelDataTestBuilder<CodeAstreinteEnt>
    {
        public CodeAstreinteBuilder()
        {
        }

        public CodeAstreinteEnt Prototype()
        {
            Model.Code = "Code1";
            Model.CodeAstreinteId = 1;
            Model.Description = "Description test";
            Model.GroupeId = 1; ;

            return Model;
        }
    }
}
