using Fred.Entities;

namespace Fred.ImportExport.Business.ApplicationSap
{
  public static class SapParameterHelper
  {

    public const string UrlPrefix = "Storm:WebApiUrl:";
    public const string LoginPrefix = "Storm:Login:";
    public const string PasswordPrefix = "Storm:Password:";

    public const string SocieteKeyPartAppSettings = "Societe:";
    public const string GroupeKeyPartAppSettings = "Groupe:";

    public static string BuildApplicationsSapKey(string prefix, OrganisationType organisationKeyType, string code)
    {
      var organisationKeyPartAppSettings = organisationKeyType == OrganisationType.Societe ? SapParameterHelper.SocieteKeyPartAppSettings : SapParameterHelper.GroupeKeyPartAppSettings;

      var organisationKeyName = prefix + organisationKeyPartAppSettings + code;

      return organisationKeyName.ToLower();
    }
    public static string BuildPrefixApplicationsSapKey(string prefix, OrganisationType organisationKeyType)
    {
      var organisationKeyPartAppSettings = organisationKeyType == OrganisationType.Societe ? SapParameterHelper.SocieteKeyPartAppSettings : SapParameterHelper.GroupeKeyPartAppSettings;

      var organisationKeyName = prefix + organisationKeyPartAppSettings;

      return organisationKeyName.ToLower();
    }

  }
}
