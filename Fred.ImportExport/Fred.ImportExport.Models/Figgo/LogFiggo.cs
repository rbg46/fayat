using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.ImportExport.Models.Figgo
{
    public class LogFiggo
    {
        /// <summary>
        /// liste des erreur de la RG09
        /// </summary>
        public List<string> ErrorListRG09 { get; set; }

        public List<string> ErrorListFonctionnel { get; set; }
        /// <summary>
        /// nombre de ligne reçu
        /// </summary>
        public int NombreLigneReceived  { get; set; }
        /// <summary>
        ///  nombre de ligne erroné
        /// </summary>
        public int NombreLigneError { get; set; }
        /// <summary>
        ///  nombre de ligne Ok
        /// </summary>
        public int NombreLigneOk { get; set; }
    }
}
