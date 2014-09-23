using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AdMesh.Service.MobFox.Data;
using AdMesh.Service.MobFox.Error;
using IOToolkit.Helpers;
using IOToolkit.Services;

namespace AdMesh.Service.MobFox
{
    public class MobfoxService : IMobfoxService
    {
        private const string LoginServiceBaseUrl = " http://api.mobfox.com/getApiCredentials/{0}/{1}";
        private const string PublisherServiceBaseUrl = "http://api.mobfox.com/api/{0}/{1}/{2}";

        private const string ListPublicationsMethod = "listPublications";
        private const string PublisherReportMethod = "generatePublisherReport";

        private const string AppIdParameter = "publication_id";
        private const string StartDateParameter = "start_date";
        private const string EndDateParameter = "end_date";

        private const string DateFormat = "yyyy-MM-dd";

        private string _baseUrl;

        private readonly ICryptoService _cryptoService;

        private MobfoxSessionData _currentSession;

        public MobfoxService(ICryptoService cryptoService)
        {
            _cryptoService = cryptoService;
        }

        public Task<MobfoxSessionData> LoginAsync(string email, string password)
        {
            return Task.Run(async () =>
            {
                var pwdMd5 = _cryptoService.GetMd5Hash(password, lowerCase: true);
                var path = string.Format(LoginServiceBaseUrl, email, pwdMd5);
                var session = await GetMobfoxAsync<MobfoxSessionData>(path);
                await ConnectAsync(session);
                return session;
            });
        }

        public Task<IEnumerable<MobfoxApplication>> GetApplicationsAsync()
        {
            return Task.Run(async () =>
            {
                var apps = await GetMobfoxAsync<MobfoxApplicationResponse>(BuildAuthPathFor(ListPublicationsMethod));
                IEnumerable<MobfoxApplication> ret = apps.Applications;
                return ret;
            });
        }

        public Task<MobfoxReport> GetReportAsync(MobfoxApplication application = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            return Task.Run(async () =>
            {
                var @params = new List<KeyValuePair<string, string>>();

                if (application != null)
                {
                    @params.Add(new KeyValuePair<string, string>(AppIdParameter, application.Id));
                }

                if (startDate != null)
                    @params.Add(new KeyValuePair<string, string>(StartDateParameter, startDate.Value.ToString(DateFormat)));

                if (endDate != null)
                    @params.Add(new KeyValuePair<string, string>(EndDateParameter, endDate.Value.ToString(DateFormat)));


                var uri = QueryStringHelpers.Build(@params, BuildAuthPathFor(PublisherReportMethod));

                //  Mobfox dont recognize parameters if they start with '?'...
                uri = uri.Replace('?', '&');
                try
                {
                    var report = (await GetMobfoxAsync<MobfoxReportResponse>(uri)).ReportInternal.Statistics;
                    report.StartTime = startDate.HasValue ? startDate.Value.Date : startDate;
                    report.EndTime = endDate.HasValue ? endDate.Value.Date : endDate;

                    return report;
                }
                catch (FormatException e)
                {

                    throw new MobfoxNoReportException(string.Empty, e);
                }


            });
        }

        public async Task ConnectAsync(MobfoxSessionData data)
        {
            _currentSession = data;
        }



        private string BuildAuthPathFor(string method)
        {
#if DEBUG
            Debug.Assert(_currentSession != null);
#endif
            return string.Format(PublisherServiceBaseUrl, _currentSession.ApiAccountId, _currentSession.ApiKey, method);
        }

        private static async Task<T> GetMobfoxAsync<T>(string path) where T : MobfoxDataBase
        {
            using (var cli = new HttpClient())
            {
                var resp = await cli.GetAsync(path);
                resp.EnsureSuccessStatusCode();
                var dataStream = await resp.Content.ReadAsStreamAsync();
                var serializer = new XmlSerializer(typeof(T));
                var data = (T)serializer.Deserialize(dataStream);
                ThrowOnError(data);
                return data;
            }
        }

        private static void ThrowOnError(MobfoxDataBase data)
        {
            if (string.IsNullOrEmpty(data.Error) == false)
                throw new Exception(data.Error);
        }


    }
}
