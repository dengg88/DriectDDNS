using D3NsCore.Entities;
using Dapper;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;

namespace D3NsCore.Tools
{
    internal class DataAccess
    {
        private readonly string _filename;

        public DataAccess(string filename)
        {
            _filename = filename;

            CreateDatabaseIfNotExist();
        }

        private void CreateDatabaseIfNotExist()
        {
            if (File.Exists(_filename)) return;

            using (var conn = GetConnection())
            {
                conn.Execute(
                    //Create table:Config
                    "CREATE TABLE Configs("
                    + $"{nameof(ConfigEntry.Key)} VARCHAR(32) PRIMARY KEY,"
                    + $"{nameof(ConfigEntry.Value)} VARCHAR(512)"
                    + ");" +

                    //Init Data
                    $"INSERT INTO Configs ({nameof(ConfigEntry.Key)}) VALUES ('{nameof(ConfigAdapter.Key)}');"
                    + $"INSERT INTO Configs ({nameof(ConfigEntry.Key)}) VALUES ('{nameof(ConfigAdapter.Secret)}');"
                    + $"INSERT INTO Configs ({nameof(ConfigEntry.Key)}) VALUES ('{nameof(ConfigAdapter.GetMyIp)}');"
                    + $"INSERT INTO Configs ({nameof(ConfigEntry.Key)}) VALUES ('{nameof(ConfigAdapter.Domain)}');"
                    + $"INSERT INTO Configs ({nameof(ConfigEntry.Key)}) VALUES ('{nameof(ConfigAdapter.DnsRecordName)}');"
                );
            }
        }

        private DbConnection GetConnection()
        {
            var conn = new SqliteConnection("Data Source=" + _filename);
            conn.Open();
            return conn;
        }

        // --- common ---
        public int BulkOperation<T>(IEnumerable<T> items, Func<T, DbConnection, int> proc)
        {
            var count = 0;
            using (var conn = GetConnection())
            {
                using (var t = conn.BeginTransaction())
                {
                    count += items.Sum(p => proc(p, conn));

                    t.Commit();
                }
            }

            return count;
        }

        public Dictionary<string, string> GetConfigs()
        {
            using (var conn = GetConnection())
                return conn.Query<ConfigEntry>("SELECT * FROM Configs")
                    .ToDictionary(p => p.Key, p => p.Value);
        }
    }
}