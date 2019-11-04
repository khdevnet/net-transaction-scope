using System;
using System.Diagnostics;
using System.Threading;
using NetTransactionScope.Library.Mongodb;

namespace NetTransactionScope.Tests
{
    public class MongoDbClassFixture : IDisposable
    {
        public MongoDbClassFixture()
        {
            BooksNoSqlInitialzer.Init();
            RunCommand("docker rm mongo-tests");
            RunCommand("docker run --name mongo-tests -p 33381:27017 -d mongo:4");
            Thread.Sleep(1000);
        }

        public void Dispose()
        {
            RunCommand("docker stop mongo-tests");
            RunCommand("docker rm mongo-tests");
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
    }
}
