using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdMesh.Service.MobFox.Data;

namespace AdMesh.Service.MobFox
{
    public interface IMobfoxService
    {
        Task<MobfoxSessionData> LoginAsync(string email, string password);
        Task<IEnumerable<MobfoxApplication>> GetApplicationsAsync();
        Task ConnectAsync(MobfoxSessionData data);
        Task<MobfoxReport> GetReportAsync(MobfoxApplication application = null, DateTime? startDate = null, DateTime? endDate = null);
    }
}
