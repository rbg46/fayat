using Fred.Entities;
using System.Diagnostics;

namespace Fred.ImportExport.Business.ApplicationSap
{
    [DebuggerDisplay("SocieteOrganisationId={SocieteOrganisationId} SocieteCode={SocieteCode} GroupeOrganisationId={GroupeOrganisationId} GroupeCode={GroupeCode}")]
    public class GeneratedApplicationSapParameter
    {
        public GeneratedApplicationSapParameter(int groupeOrganisationId, string groupeCode, int societeOrganisationId, string societeCode)
        {
            GroupeCode = groupeCode;
            SocieteCode = societeCode;
            SocieteApplicationSapParameter = CreateApplicationSapParameter(OrganisationType.Societe, societeCode);
            GroupeApplicationSapParameter = CreateApplicationSapParameter(OrganisationType.Groupe, groupeCode);
            GroupeOrganisationId = groupeOrganisationId;
            SocieteOrganisationId = societeOrganisationId;
        }

        public string GroupeCode { get; set; }
        public string SocieteCode { get; set; }

        public ApplicationSapParameter SocieteApplicationSapParameter { get; set; }
        public ApplicationSapParameter GroupeApplicationSapParameter { get; set; }
        public int GroupeOrganisationId { get; }
        public int SocieteOrganisationId { get; }
        public int SocieteId { get; internal set; }

        private ApplicationSapParameter CreateApplicationSapParameter(OrganisationType organisationKeyType, string code)
        {
            var result = new ApplicationSapParameter();

            var urlKey = SapParameterHelper.BuildApplicationsSapKey(SapParameterHelper.UrlPrefix, organisationKeyType, code);
            var loginKey = SapParameterHelper.BuildApplicationsSapKey(SapParameterHelper.LoginPrefix, organisationKeyType, code);
            var passwordKey = SapParameterHelper.BuildApplicationsSapKey(SapParameterHelper.PasswordPrefix, organisationKeyType, code);

            result.Url = urlKey;
            result.Login = loginKey;
            result.Password = passwordKey;
            return result;
        }
    }
}
