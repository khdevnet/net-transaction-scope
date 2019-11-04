using System;
using System.IO;
using System.Transactions;

namespace NetTransactionScope.Library.Files
{
    public abstract class FileOperation : IEnlistmentNotification
    {
        protected readonly string BackupPath;
        protected readonly string CurrentPath;
        private static readonly string TempFolder = Path.Combine(Path.GetTempPath(), "txOperations");

        protected FileOperation(string path)
        {
            BackupPath = GetTempFileName(Path.GetExtension(path));
            CurrentPath = path;
            //Enlist on the current transaction with the enlistment object
            Transaction.Current.EnlistVolatile(this, EnlistmentOptions.None);
        }

        protected static void EnsureTempFolderExists()
        {
            if (!Directory.Exists(TempFolder))
            {
                Directory.CreateDirectory(TempFolder);
            }
        }

        protected void RestoreFile()
        {
            if (File.Exists(BackupPath))
            {
                File.Copy(BackupPath, CurrentPath);
            }
        }

        protected void DeleteFile()
        {
            if (File.Exists(CurrentPath))
            {
                File.Delete(CurrentPath);
            }
        }

        protected void DeleteBackupFile()
        {
            if (File.Exists(BackupPath))
            {
                File.Delete(BackupPath);
            }
        }

        private static string GetTempFileName(string extension)
        {
            Guid g = Guid.NewGuid();
            string retVal = Path.Combine(TempFolder, g.ToString().Substring(0, 16)) + extension;

            return retVal;
        }

        public abstract void Commit(Enlistment enlistment);

        public abstract void InDoubt(Enlistment enlistment);

        public abstract void Prepare(PreparingEnlistment preparingEnlistment);

        public abstract void Rollback(Enlistment enlistment);
    }
}
