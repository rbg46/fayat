using System;

namespace Fred.Framework.Tool
{
  /// <summary>
  ///   This attribute class is used to hide Personally Identifiable Information (PII) when converting a class to a string.
  ///   Typical usage is when logging a class aka Person, and you don't whant to log the bank number for exemple
  /// </summary>
  /// <seealso cref="System.Attribute" />
  /// <example>
  ///   Just add the attribute
  ///   [SensitiveInformationAttribute]
  ///   public string AccountNumber { get; set; }
  /// </example>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
  public sealed class SensitiveInformationAttribute : Attribute
  {
    /// <summary>
    ///   Default string top display when not hidden.
    /// </summary>
    private const string DefaultStringValue = "PII";

    /// <summary>
    ///   String value to display when not hidden.
    /// </summary>
    private readonly string displayOverride;

    /// <summary>
    ///   Initialise une nouvelle instance de PersonallyIdentifiableInformationAttribute class
    ///   <see cref="SensitiveInformationAttribute" />.
    /// </summary>
    public SensitiveInformationAttribute()
    {
      Hidden = true;
    }

    /// <summary>
    ///   Set le displayOverride.
    /// </summary>
    /// <param name="displayOverride">Value to display in ToString.</param>
    public SensitiveInformationAttribute(string displayOverride)
    {
      Hidden = false;
      this.displayOverride = displayOverride;
    }

    /// <summary>
    ///   Gets the string override value when marked as PII.
    /// </summary>
    /// <value>
    ///   The display override.
    /// </value>
    public string DisplayOverride => string.IsNullOrEmpty(this.displayOverride) ? DefaultStringValue : this.displayOverride;

    /// <summary>
    ///   Gets or sets a value indicating whether the value should be totally hidden.
    /// </summary>
    /// <value>
    ///   <c>true</c> if hidden; otherwise, <c>false</c>.
    /// </value>
    public bool Hidden { get; set; }
  }
}