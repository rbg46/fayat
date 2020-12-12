namespace Fred.EntityFramework
{
    public class FilteredIndexSqlGenerator // : SqlServerMigrationSqlGenerator
    {
        //protected override void Generate(CreateIndexOperation createIndexOperation)
        //{
        //    using (var writer = Writer())
        //    {
        //        writer.Write("CREATE ");
        //        if (createIndexOperation.IsUnique)
        //        {
        //            writer.Write("UNIQUE ");
        //        }
        //        if (createIndexOperation.IsClustered)
        //        {
        //            writer.Write("CLUSTERED ");
        //        }
        //        writer.Write("INDEX ");
        //        writer.Write(Quote(createIndexOperation.Name));
        //        writer.Write(" ON ");
        //        writer.Write(Name(createIndexOperation.Table));
        //        writer.Write("(");
        //        writer.Write(string.Join(",", createIndexOperation.Columns.Select(c => Quote(c))));
        //        writer.Write(")");

        //        // This condition applies only if the anonymousArguments are specified
        //        if (createIndexOperation.AnonymousArguments.ContainsKey("Where"))
        //        {
        //            writer.Write(" WHERE " + createIndexOperation.AnonymousArguments["Where"]);
        //        }
        //        Statement(writer);
        //    }
        //}
    }
}
