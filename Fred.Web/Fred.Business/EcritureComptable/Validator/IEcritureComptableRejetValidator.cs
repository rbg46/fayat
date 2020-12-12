﻿using FluentValidation;
using Fred.Entities.EcritureComptable;

namespace Fred.Business.EcritureComptable.Validator
{
    /// <summary>
    /// Interface du valideur IEcritureComptableRejetValidator
    /// </summary>
    public interface IEcritureComptableRejetValidator : IValidator<EcritureComptableRejetEnt>
    { }
}
