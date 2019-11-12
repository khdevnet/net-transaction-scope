using System;
using System.IO;
using System.Transactions;

namespace NetTransactionScope.Library.Files
{
    public class CreateFileOperation : TxOperation
    {
        private readonly byte[] _fileData;
        private readonly FileStorage _fileStorage;
        private readonly string BackupPath;
        private readonly string DestFolderPath;
        private readonly string CurrentPath;

        public CreateFileOperation(string path, byte[] fileData)
        : this(path, fileData, new FileStorage())
        {
        }

        public CreateFileOperation(string path, byte[] fileData, FileStorage fileStorage)
        {
            CurrentPath = path;
            _fileData = fileData;
            _fileStorage = fileStorage;
            DestFolderPath = Path.GetDirectoryName(CurrentPath);
            BackupPath = GetTempFileName();
        }

        protected override void PrepareInternal(PreparingEnlistment preparingEnlistment)
        {
            if (_fileStorage.Exists(CurrentPath))
            {
                throw new Exception("File exist.");
            }

            _fileStorage.CreateFile(BackupPath, _fileData);
            File.SetAttributes(BackupPath, FileAttributes.Hidden);
        }

        public override void Commit(Enlistment enlistment)
        {
            CommitFile();
            enlistment.Done();
        }

        public override void Rollback(Enlistment enlistment)
        {
            DeleteBackupFile();
            enlistment.Done();
        }

        private void CommitFile()
        {
            if (_fileStorage.Exists(BackupPath) && !File.Exists(CurrentPath))
            {
                File.SetAttributes(BackupPath, FileAttributes.Normal);
                RenameFile();
            }
        }

        private void DeleteBackupFile()
        {
            if (_fileStorage.Exists(BackupPath))
            {
                _fileStorage.Delete(BackupPath);
            }
        }

        private string GetTempFileName()
        {
            string tempName = Guid.NewGuid().ToString();
            return Path.Combine(DestFolderPath, tempName) + ".tmp";
        }

        private void RenameFile()
        {
            _fileStorage.Move(BackupPath, CurrentPath);
        }
    }
}
