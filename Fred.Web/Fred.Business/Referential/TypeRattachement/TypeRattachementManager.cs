using System.Collections.Generic;

namespace Fred.Business.Referential
{
  /// <summary>
  ///   Gestionnaire des TypeRattachement.
  /// </summary>
  public class TypeRattachementManager : ITypeRattachementManager
  {
    private readonly List<TypeRattachement.TypeRattachement> lstTypeRattachement = new List<TypeRattachement.TypeRattachement>();

    /// <summary>
    ///   Initialise une nouvelle instance de la classe <see cref="TypeRattachementManager" />
    /// </summary>
    public TypeRattachementManager()
    {
      this.lstTypeRattachement.Add(new TypeRattachement.TypeRattachement(1, "A", "Agence"));
      this.lstTypeRattachement.Add(new TypeRattachement.TypeRattachement(2, "D", "Domicile"));
      this.lstTypeRattachement.Add(new TypeRattachement.TypeRattachement(3, "S", "Secteur"));
    }

    /// <summary>
    ///   Retourne le libelle du type de rattachement
    /// </summary>
    /// <param name="code">le code à rechercher</param>
    /// <returns>Le libelle du type de rattachement</returns>
    public static string GetLibelle(string code)
    {
      string resu = string.Empty;
      switch (code)
      {
        case TypeRattachement.TypeRattachement.Secteur:
          resu = TypeRattachement.TypeRattachement.SecteurLibelle;
          break;
        case TypeRattachement.TypeRattachement.Domicile:
          resu = TypeRattachement.TypeRattachement.DomicileLibelle;
          break;
        case TypeRattachement.TypeRattachement.Agence:
          resu = TypeRattachement.TypeRattachement.AgenceLibelle;
          break;
      }

      return resu;
    }

    /// <summary>
    ///   Retourne le code du type de rattachement
    /// </summary>
    /// <param name="id">id à rechercher</param>
    /// <returns>Le code du type de rattachement</returns>
    public static string GetCodeById(int id)
    {
      string resu = string.Empty;
      switch (id)
      {
        case 1:
          resu = TypeRattachement.TypeRattachement.Agence;
          break;
        case 2:
          resu = TypeRattachement.TypeRattachement.Domicile;
          break;
        case 3:
          resu = TypeRattachement.TypeRattachement.Secteur;
          break;
      }

      return resu;
    }

    /// <summary>
    ///   Retourne le libelle du type de rattachement
    /// </summary>
    /// <param name="code">le code à rechercher</param>
    /// <returns>Le libelle du type de rattachement</returns>
    public static int GetIdentifiant(string code)
    {
      int resu = 0;
      switch (code)
      {
        case TypeRattachement.TypeRattachement.Agence:
          resu = 1;
          break;
        case TypeRattachement.TypeRattachement.Domicile:
          resu = 2;
          break;
        case TypeRattachement.TypeRattachement.Secteur:
          resu = 3;
          break;
      }

      return resu;
    }

    /// <summary>
    ///   Retourne la liste des pays.
    /// </summary>
    /// <returns>Liste des types rattachement.</returns>
    public IEnumerable<TypeRattachement.TypeRattachement> GetList()
    {
      return this.lstTypeRattachement;
    }
  }
}