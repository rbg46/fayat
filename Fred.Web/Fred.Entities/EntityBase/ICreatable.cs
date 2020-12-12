using System;

namespace Fred.Entities.EntityBase
{
  /// <summary>
  ///  Represente une entité ou il y aurra un auteur et une date de création
  /// </summary>
  public interface ICreatable
  {
    /// <summary>
    /// AuteurCreationId
    /// </summary>
    int AuteurCreationId { get; set; }

    /// <summary>
    /// DateCreation
    /// </summary>
    DateTime DateCreation { get; set; }
  }
}
