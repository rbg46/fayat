using Newtonsoft.Json;

namespace Fred.Framework.Services
{
  /// <summary>
  ///   Classe Token
  /// </summary>
  public class Token
  {
    /// <summary>
    ///   Obtient ou définit le Token
    /// </summary>
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }

    /// <summary>
    ///   Obtient ou définit le type du Token
    /// </summary>
    [JsonProperty("token_type")]
    public string TokenType { get; set; }

    /// <summary>
    ///   Obtient ou définit le temps de validité du Token
    /// </summary>
    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }

  }
}
