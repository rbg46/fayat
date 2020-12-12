using System.Data.Entity;
using Fred.ImportExport.Entities.Stair;

namespace Fred.ImportExport.Database.ImportExport
{
    public class StairContext : DbContext
  {


    public StairContext()
        : base("StairConnection") { }

    public DbSet<StairSafeEnt> StairIndicateurSafe { get; set; }

    public DbSet<StairPlanActionEnt> StairPlanActionSafe { get; set; }

    public DbSet<StairSphinxFormulaireEnt> SphinxFormulaire { get; set; }

    public DbSet<StairSphinxQuestionEnt> SphinxQuestion { get; set; }

    public DbSet<StairSphinxReponseEnt> SphinxReponse { get; set; }
    }
  
}
