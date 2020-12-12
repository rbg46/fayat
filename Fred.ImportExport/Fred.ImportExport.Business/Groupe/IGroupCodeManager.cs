namespace Fred.ImportExport.Business.Groupe
{
    public interface IGroupCodeManager
    {
        string GetGroupCodeByCodeFlux(string codeFlux);
        string GetGroupCodeByCompanyId(int companyId);
        string GetGroupCodeByUserId(int userId);
    }
}
