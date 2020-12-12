using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Fred.Framework.Tool
{
    /// <summary>
    ///   Classe gérant la sérialisation des objets
    /// </summary>
    public static class SerialisationTools
    {
        /// <summary>
        ///   Gère la sérialisation d'un objet sérialisable
        /// </summary>
        /// <param name="data">objet représentant une classe à enregistrer en bdd</param>
        /// <returns>Renvois un objet sérialisé</returns>
        public static byte[] Serialisation(object data)
        {
            byte[] buff = new byte[] { };
            // Sérialisation
            if (data?.GetType().IsSerializable != true)
            {
                return buff;
            }

            MemoryStream strm = new MemoryStream();
            BinaryFormatter frmt = new BinaryFormatter();

            frmt.Serialize(strm, data);
            int lng = (int)strm.Length;
            buff = strm.GetBuffer();
            strm.Close();

            // Limite le buffer aux données
            if (buff.Length <= lng)
            {
                return buff;
            }

            var buff2 = new byte[lng];

            for (int i = 0; i < lng; i++)
            {
                buff2[i] = buff[i];
            }

            return buff2;
        }

        /// <summary>
        ///   Gère la désérialisation d'un objet sérialisé
        /// </summary>
        /// <param name="data">objet sérialisé en tableau de byte</param>
        /// <returns>retourne un objet désérialisé</returns>
        public static object Deserialisation(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return null;
            }

            MemoryStream strm = new MemoryStream(data);
            BinaryFormatter frmt = new BinaryFormatter();
            strm.Position = 0;
            object resultat = frmt.Deserialize(strm);
            strm.Close();

            return resultat;
        }
    }
}
