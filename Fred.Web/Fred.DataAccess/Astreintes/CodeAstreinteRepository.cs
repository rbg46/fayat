using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.EntityFramework;

namespace Fred.DataAccess.Astreintes
{
    /// <summary>
    /// Code Astreinte Repository Class 
    /// </summary>
    public class CodeAstreinteRepository : FredRepository<CodeAstreinteEnt>, ICodeAstreinteRepository
    {
        public CodeAstreinteRepository(FredDbContext context)
             : base(context)
        {
        }

        /// <summary>
        /// GetCodeAstreintes
        /// </summary>
        /// <param name="code">code</param>
        /// <returns>Une ligne de code Astreintes</returns>
        public CodeAstreinteEnt GetCodeAstreintes(string code)
        {
            return this.Query().Get().FirstOrDefault(x => x.Code.Equals(code));
        }
    }
}
