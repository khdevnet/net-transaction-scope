using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using NetTransactionScope.Library.PostgreSql;

namespace NetTransactionScope.Tests
{
    public class PostgreSqlDbClassFixture : IDisposable
    {
        public PostgreSqlDbClassFixture()
        {
            RunCommand("docker stop postgresql-tests");
            RunCommand("docker rm postgresql-tests");
            RunCommand($"docker run --name postgresql-tests -p {BooksSqlDbContext.Port}:5432 -e POSTGRES_PASSWORD={BooksSqlDbContext.Password} -d postgres:9.6.2");
            Thread.Sleep(5000);
            InitSqlDbContext();
        }

        public void Dispose()
        {
            RunCommand("docker stop postgresql-tests");
            RunCommand("docker rm postgresql-tests");
        }

        private static void RunCommand(string command)
        {
            Process cmd = new Process();

            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;

            cmd.Start();

            /* execute "dir" */

            cmd.StandardInput.WriteLine(command);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            var txt = cmd.StandardOutput.ReadToEnd();
        }

        private static void InitSqlDbContext()
        {
            using (var sql = new BooksSqlDbContext())
            {
                sql.Database.EnsureDeleted();
                sql.Database.Migrate();
            }
        }
    }
}
