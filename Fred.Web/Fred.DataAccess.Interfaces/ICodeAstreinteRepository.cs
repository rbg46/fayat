
using Fred.Entities.Referential;
namespace Fred.DataAccess.Interfaces

{
  /// <summary>
  /// Code Astreinte Repository Interface 
  /// </summary>
  public interface ICodeAstreinteRepository : IRepository<CodeAstreinteEnt>
  {
    /// <summary>
    /// GetCodeAstreintes
    /// </summary>
    /// <param name="code">code</param>
    /// <returns>Une ligne de code Astreintes</returns>
    CodeAstreinteEnt GetCodeAstreintes(string code);
  }
}
