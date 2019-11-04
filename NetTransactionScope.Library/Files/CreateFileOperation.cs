using System.IO;
using System.Transactions;

namespace NetTransactionScope.Library.Files
{
    public class CreateFileOperation : FileOperation
    {
        private readonly byte[] _fileData;

        public CreateFileOperation(string path, byte[] fileData)
        : base(path)
        {
            _fileData = fileData;
        }

        public override void Prepare(PreparingEnlistment preparingEnlistment)
        {
            try
            {

                EnsureTempFolderExists();
                BackupFile();

                //If work finished correctly, reply prepared
                preparingEnlistment.Prepared();
            }
            catch
            {
                // otherwise, do a ForceRollback
                preparingEnlistment.ForceRollback();
            }
        }

        public override void Commit(Enlistment enlistment)
        {
            //Do any work necessary when commit notification is received
            CreateFile();
            DeleteBackupFile();
            //Declare done on the enlistment
            enlistment.Done();
        }

        public override void Rollback(Enlistment enlistment)
        {
            //Do any work necessary when rollback notification is received

            DeleteFile();
            DeleteBackupFile();

            //Declare done on the enlistment
            enlistment.Done();
        }

        public override void InDoubt(Enlistment enlistment)
        {
            //Do any work necessary when indout notification is received
            //Declare done on the enlistment
            enlistment.Done();
        }

        private void BackupFile()
        {
            File.WriteAllBytes(BackupPath, _fileData);
        }

        private void CreateFile()
        {
            if (!File.Exists(CurrentPath))
            {
                File.WriteAllBytes(CurrentPath, _fileData);
            }
        }

        private void DeleteFile()
        {
            if (File.Exists(CurrentPath))
            {
                File.Delete(CurrentPath);
            }
        }
    }
}
