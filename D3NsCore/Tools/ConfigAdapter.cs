using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace D3NsCore.Tools
{
    internal class ConfigAdapter
    {
        private readonly Dictionary<string, string> _dicConf;

        public ConfigAdapter(Dictionary<string, string> dicConf)
        {
            _dicConf = dicConf;
        }

        private string GetValue([CallerMemberName] string confKey = "")
        {
            return _dicConf.ContainsKey(confKey)
                ? _dicConf[confKey]
                : null;
        }

        private string[] GetValues([CallerMemberName] string confKey = "")
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            return (GetValue(confKey) ?? "").Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        private int GetInt32Value([CallerMemberName] string confKey = "")
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            var value = GetValue(confKey);

            if (int.TryParse(value, out var int32Value))
            {
                return int32Value;
            }

            throw new ConfigurationErrorsException($"Bad config {confKey}");
        }

        public string Key => GetValue();
        public string Secret => GetValue();
        public string GetMyIp => GetValue();
        public string Domain => GetValue();
        public string DnsRecordName => GetValue();
    }
}