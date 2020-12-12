using AutoMapper;
using Fred.Entities.Journal;
using Fred.Web.Models.Journal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootstrapper.AutoMapper
{
  public static class JournalMapper
  {
    public static void Map(IMapperConfiguration cfg)
    {
      cfg.CreateMap<JournalEnt, JournalModel>().ReverseMap()
        .AfterMap((jm, je) => { if (je.TypeJournal == null) je.TypeJournal = string.Empty; });
    }
  }
}
