using System.IO;
using System.Transactions;

namespace NetTransactionScope.Library.Files
{
    public class CreateFileOperation : FileOperation
    {
        private readonly byte[] _fileData;
        private readonly FileStorage _fileStorage;

        public CreateFileOperation(string path, byte[] fileData)
        : this(path, fileData, new FileStorage())
        {
            _fileData = fileData;
        }

        public CreateFileOperation(string path, byte[] fileData, FileStorage fileStorage)
            : base(path)
        {
            _fileData = fileData;
            _fileStorage = fileStorage;
        }

        public override void Prepare(PreparingEnlistment preparingEnlistment)
        {
            //try
            //{

            EnsureTempFolderExists();
            BackupFile();

            // If work finished correctly, reply prepared
            preparingEnlistment.Prepared();
            //}
            //catch (IOException ex)
            //{
            //    // otherwise, do a ForceRollback
            //    preparingEnlistment.ForceRollback();
            //}
        }

        public override void Commit(Enlistment enlistment)
        {
            //Do any work necessary when commit notification is received
            _fileStorage.CreateFile(CurrentPath, _fileData);
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


        private void DeleteFile()
        {
            if (File.Exists(CurrentPath))
            {
                File.Delete(CurrentPath);
            }
        }
    }
}
