using System;
using System.IO;
using System.Runtime.Caching;

namespace Fred.Framework.Reporting
{
    /// <summary>
    /// Méthodes d'extension pour Reporting />
    /// </summary>
    public static class ReportingExtension
    {
        /// <summary>
        /// Transform MemoryStream Object To Pdf Or Excel
        /// </summary>
        /// <param name="stream">stream</param>
        /// <param name="isPdf">is Pdf</param>
        /// <returns>object</returns>
        public static object TransformMemoryStreamToPdfOrExcel(this MemoryStream stream, bool isPdf)
        {
            byte[] bytes = stream.GetBuffer();
            string typeCache;
            string cacheId = Guid.NewGuid().ToString();
            ExcelFormat excelFormat = new ExcelFormat();

            if (isPdf)
            {
                typeCache = "pdfBytes_";
            }
            else
            {
                typeCache = "excelBytes_";
                // --> Comme ça, ça marche pour l'excel mais pas pour le pdf!        
                stream.Position = 0;
                bytes = new byte[stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);
            }
            excelFormat.Dispose();
            stream.Dispose();
            CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(300), Priority = CacheItemPriority.NotRemovable };
            MemoryCache.Default.Add(typeCache + cacheId, bytes, policy);

            return new { id = cacheId };
        }

    }
}
