using System.IO;

namespace Fred.Framework.Tool
{
    /// <summary>
    /// Outils facilitant la gestion des fichiers
    /// </summary>
    public static class FileTool
    {
        /// <summary>
        /// Calculer la taille en Ko d'un fichier
        /// </summary>
        /// <param name="path">Path du fichier à calculer</param>
        /// <returns>Taille en Ko</returns>
        public static int GetFileSizeInKo(string path)
        {
            FileInfo temporaryFile = new FileInfo(path);

            return (int)(temporaryFile.Length / 1024);
        }
    }
}
