using System;
using System.Text.RegularExpressions;

namespace Fred.Framework.Tool
{
  /// <summary>
  ///   Classe INSEE Number
  /// </summary>
  public static class InseeNumber
  {
    /// <summary>
    ///   Constante pour le calcul de la clé
    /// </summary>
    private const short Key = 97;

    /// <summary>
    ///   Nombre de caractère du numéro INSEE
    /// </summary>
    private const short Maxlength = 13;

    #region "Méthodes publiques"

    /// <summary>
    ///   Verifie le numéro INSEE passé en paramètre (numero + clé)
    /// </summary>
    /// <param name="strNumero">Numéro INSEE</param>
    /// <param name="strCle">Clé de verification du numéro INSEE</param>
    /// <returns>Renvoie un tuple (bool,string): (True,"") si le numéro est correcte, sinon (False,[Clé proposée])</returns>
    public static Tuple<bool, string> CheckInseeNumber(string strNumero, string strCle)
    {
      string key = KeyProcessInsee(strNumero).ToString("D2");
      if (key.Equals(strCle))
      {
        return Tuple.Create(true, string.Empty);
      }
      return Tuple.Create(false, key);
    }

    /// <summary>
    ///   Verifie le numéro INSEE passé en paramètre
    /// </summary>
    /// <param name="strNumero">Numéro INSEE avec la clé</param>
    /// <returns>True si le numéro et la clé sont cohérent, sinon false</returns>
    public static Tuple<bool, string> CheckInseeNumber(string strNumero)
    {
      string strCle;
      strNumero = CleanString(strNumero);

      try
      {
        strCle = strNumero.Remove(0, Maxlength);
        strNumero = strNumero.Remove(Maxlength);
      }
      catch
      {
        // il manque des caractères
        return Tuple.Create(true, string.Empty);
      }

      return CheckInseeNumber(strNumero, strCle);
    }

    /// <summary>
    ///   Calcul la clé correspondante au numéro INSEE passé en paramètre
    /// </summary>
    /// <param name="strNumero">Numero INSEE</param>
    /// <returns>Clé du numéro INSEE passé en paramètre, 0 si numéro invalide</returns>
    public static short KeyProcessInsee(string strNumero)
    {
      // clé retournée
      short cle = 0;

      // numéro apres convertion
      long numero = InseeToInteger(strNumero);

      if (numero != 0)
      {
        cle = (short)(Key - (numero % Key));
      }

      return cle;
    }

    #endregion

    #region "Méthodes privées"

    /// <summary>
    ///   Enlève les caractères ne pouvant faire partie du numéro
    ///   A-Z0-9 uniquement
    /// </summary>
    /// <param name="strNumero">Numéro INSEE</param>
    /// <returns>Retourne la chaîne épurée</returns>
    private static string CleanString(string strNumero)
    {
      strNumero = strNumero.ToUpper();
      Regex regInsee = new Regex("[^A-Z0-9_]");
      strNumero = regInsee.Replace(strNumero, string.Empty);

      return strNumero;
    }

    /// <summary>
    ///   Convertion du numéro (string) en entier
    /// </summary>
    /// <param name="strNumero">Numéro INSEE</param>
    /// <returns>Retourne le numéro INSEE sous forme d'un entier, 0 si numéro invalide</returns>
    private static long InseeToInteger(string strNumero)
    {
      // le numero apres convertion
      long numero = 0;

      // Pour les Corses !
      // Emplacement de la lettre pour les corses
      const short indiceLettreCorse = 6;

      // Constante pour calcul Corse 2A
      const int corsea = 1000000;

      // Constante pour calcul Corse 2B
      const int corseb = 2000000;

      strNumero = CleanString(strNumero);

      // le numero doit faire NB_CARACTERES sinon c'est pas
      // la peine d'aller plus loin
      if (strNumero.Length != Maxlength)
      {
        return numero;
      }

      // convertion en entier, si la chaîne ne peut etre convertie
      // soit une erreur, soit un Corse...
      if (!long.TryParse(strNumero, out numero))
      {
        // verification du 7eme caractère
        if (strNumero[indiceLettreCorse] == 'A')
        {
          // un Corse du Sud
          strNumero = strNumero.Replace('A', '0');
          if (long.TryParse(strNumero, out numero))
          {
            numero -= corsea;
          }
        }
        else if (strNumero[indiceLettreCorse] == 'B')
        {
          // Haute Corse
          strNumero = strNumero.Replace('B', '0');
          if (long.TryParse(strNumero, out numero))
          {
            numero -= corseb;
          }
        }
      }

      return numero;
    }

    #endregion
  }
}