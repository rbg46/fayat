using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Fred.Web.Shared.Models.Budget.BibliothequePrix;

namespace Fred.Business.Budget.BibliothequePrix.Validator
{
    public interface IBibliothequePrixModelLoaderValidator : IValidator<BibliothequePrixSave.Model>        
    {
    }
}
