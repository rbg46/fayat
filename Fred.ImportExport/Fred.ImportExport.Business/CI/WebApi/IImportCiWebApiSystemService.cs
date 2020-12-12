using Fred.ImportExport.Business.CI.WebApi.Context.Inputs;

namespace Fred.ImportExport.Business.CI.WebApi
{
    /// <summary>
    /// Service qui fait l'import par api
    /// </summary>
    public interface IImportCiWebApiSystemService : IFredIEService
    {
        /// <summary>
        /// Fait l'import par api
        /// </summary>
        /// <param name="importCisByApiInput">input</param>
        void ImportCisByApi(ImportCisByApiInputs importCisByApiInput);
    }
}
