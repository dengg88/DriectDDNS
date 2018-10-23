using D3NsCore.Models;
using D3NsCore.Tools;
using System.Linq;
using System.Net;
using System.Threading;

namespace D3NsCore
{
    public class D3NsClient : BasicServ
    {
        private ConfigAdapter _conf;
        private HttpAccess _http;
        private bool _isRunning;
        private Thread _thread;

        public string Domain => _conf.DnsRecordName + "." + _conf.Domain;

        public D3NsClient(string dbFile)
        {
            _conf = new ConfigAdapter(new DataAccess(dbFile).GetConfigs());
            _http = new HttpAccess();
        }

        public override void Start()
        {
            _isRunning = true;
            _thread = new Thread(Run) { IsBackground = true };
            _thread.Start();
        }

        public override void Stop()
        {
            _isRunning = false;
            _thread.Join();
        }

        private void Run()
        {
            var key = _conf.Key;
            var sec = _conf.Secret;

            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(sec))
            {
                LogFatal("Please fill configs.");
                return;
            }

            string myIp, dnsIp;

            while (_isRunning) //TODO: Error handle
            {
                //Get my ip
                //Get dns ip
                //cmp
                // + put my ip to dns
                // - do nothing
                //sleep

                try
                {
                    LogTrace("Getting my ip...");
                    myIp = GetMyIp();
                    LogInfo($"My ip is: {myIp}");
                    LogTrace("Getting dns ip...");
                    dnsIp = GetDnsIp();
                    LogInfo($"Dns ip is: {dnsIp}");

                    if (myIp != dnsIp)
                    {
                        LogInfo("Updating dns record...");
                        PutDnsIp(myIp);
                    }
                    else
                    {
                        LogTrace("Nothing to do.");
                    }
                }
                catch (WebException e)
                {
                    LogError(e.Message);
                }

                LogTrace("Sleeping...");
                for (var i = 0; i < 600; i++)
                {
                    if (_isRunning == false) break;
                    Thread.Sleep(1000);
                }
                LogTrace("Wake up.");
            }

            LogInfo("Exit.");
        }

        // ---- utilitys ----

        private string GetMyIp() => _http.GetJsonAnon(new { REMOTE_ADDR = "" }, _conf.GetMyIp).REMOTE_ADDR;

        private string GetDnsIp() => _http.GetJsonAnon(new[] { new { data = "" } }, $"https://api.godaddy.com/v1/domains/{_conf.Domain}/records/A/{_conf.DnsRecordName}", new HttpHeader("Accept", "application/json"), new HttpHeader("Authorization", $"sso-key {_conf.Key}:{_conf.Secret}")).Select(p => p.data).Single();

        private void PutDnsIp(string ip) => _http.PutString($"https://api.godaddy.com/v1/domains/{_conf.Domain}/records/A/{_conf.DnsRecordName}", $"[{{\"data\":\"{ip}\",\"ttl\":600}}]", "application/json", new HttpHeader("Authorization", $"sso-key {_conf.Key}:{_conf.Secret}"));
    }
}