using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.ImportExport.Framework.Etl.Engine.Config;

namespace Fred.ImportExport.Framework.Etl.Engine
{

    /// <summary>
    /// Moteur de l'ETL
    /// Il faut hériter de la classe abstraite
    /// </summary>
    /// <typeparam name="TI">Input Template</typeparam>
    /// <typeparam name="TR">Result Template</typeparam>
    public interface IEtlProcess<TI, TR>
    {

        /// <summary>
        /// Nom / description courte du processus
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Contient la liste des étapes du workflow de l'ETL
        /// </summary>
        IEtlConfig<TI, TR> Config { get; }


        /// <summary>
        /// Dans cette fonction, il faut construite le workflow
        /// </summary>
        void Build();


        /// <summary>
        /// Lance le processus
        /// </summary>
        Task ExecuteAsync();


        /// <summary>
        /// Set l'input et lance le processus
        /// </summary>
        /// <param name="input">liste de IEtlInput</param>
        Task ExecuteAsync(IList<TI> input);
    }
}