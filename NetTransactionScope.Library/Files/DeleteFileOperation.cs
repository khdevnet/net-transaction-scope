using System.IO;
using System.Transactions;

namespace NetTransactionScope.Library.Files
{
    public class DeleteFileOperation : FileOperation
    {
        public DeleteFileOperation(string path)
        : base(path)
        {
        }

        public override void PrepareInternal(PreparingEnlistment preparingEnlistment)
        {
            EnsureTempFolderExists();
            BackupFile();
            DeleteFile();
        }

        public override void Rollback(Enlistment enlistment)
        {
            //Do any work necessary when rollback notification is received
            RestoreFile();
            DeleteBackupFile();

            //Declare done on the enlistment
            enlistment.Done();
        }

        private void BackupFile()
        {
            if (File.Exists(CurrentPath))
            {
                File.Copy(CurrentPath, BackupPath);
            }
        }
    }
}
