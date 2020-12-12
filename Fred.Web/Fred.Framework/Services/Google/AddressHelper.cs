using System.Collections.Generic;
using System.Text;

namespace Fred.Framework.Services.Google
{

  /// <summary>
  /// Helper pour gerer les adresses
  /// </summary>
  public static class AddressHelper
  {

    /// <summary>
    /// Converti une classe Adresse en chaine de caractere.
    /// </summary>
    /// <param name="adresse">adresse</param>
    /// <param name="withoutAddresses">fait la recherche avec ou sans les adresses</param>
    /// <returns>Chaine de caractere  representant l'adresse</returns>
    public static string ConvertAddressToString(Address adresse, bool withoutAddresses = false)
    {
      var result = new List<string>();
      if (!string.IsNullOrEmpty(adresse.Adresse1) && !withoutAddresses)
      {
        result.Add(adresse.Adresse1);
      }
      if (!string.IsNullOrEmpty(adresse.Adresse2) && !withoutAddresses)
      {
        result.Add(adresse.Adresse2);
      }
      if (!string.IsNullOrEmpty(adresse.Adresse3) && !withoutAddresses)
      {
        result.Add(adresse.Adresse3);
      }
      if (!string.IsNullOrEmpty(adresse.CodePostal))
      {
        result.Add(adresse.CodePostal);
      }
      if (!string.IsNullOrEmpty(adresse.Ville))
      {
        result.Add(adresse.Ville);
      }
      if (!string.IsNullOrEmpty(adresse.Pays))
      {
        result.Add(adresse.Pays);
      }
      else
      {
        result.Add("France");
      }

      return string.Join(",", result);
    }

    /// <summary>
    /// Formate l'adresse reçue des services Google
    /// </summary>
    /// <param name="addressComponents">Composant</param>
    /// <returns>Address formatée</returns>
    public static Address ConvertAddressComponentsToAddress(List<AddressComponent> addressComponents)
    {
      Address result = new Address();

      result.Adresse1 = GetAddressComponentData(addressComponents, "street_number");

      result.Adresse1 += " ";

      result.Adresse1 += GetAddressComponentData(addressComponents, "route");

      result.Adresse1 = result.Adresse1.Trim();

      if(result.Adresse1== "Unnamed Road")
      {
        result.Adresse1 = string.Empty;
      }

      result.Ville = GetAddressComponentData(addressComponents, "locality");

      result.CodePostal = GetAddressComponentData(addressComponents, "postal_code");

      result.CodePostal += " ";

      result.CodePostal += GetAddressComponentData(addressComponents, "postal_code_suffix");

      result.CodePostal = result.CodePostal.Trim();

      result.Pays += GetAddressComponentData(addressComponents, "country");

      return result;
    }

    private static string GetAddressComponentData(List<AddressComponent> addressComponents, string property)
    {
      var sb = new StringBuilder();
      foreach (AddressComponent addressComponent in addressComponents)
      {
        if (addressComponent.Types[0].Equals(property))
        {
          sb.Append(addressComponent.Long_name);         
        }
      }
      var result = sb.ToString();
      return result.Trim();
    }

  }
}
