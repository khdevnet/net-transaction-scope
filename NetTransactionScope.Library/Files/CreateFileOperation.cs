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

        protected override void PrepareInternal(PreparingEnlistment preparingEnlistment)
        {
            _fileStorage.CreateFile(CurrentPath, _fileData);
        }

        public override void Commit(Enlistment enlistment)
        {
            this.CommitFile();
            base.Commit(enlistment);
        }

        public override void Rollback(Enlistment enlistment)
        {
            DeleteFile();
            enlistment.Done();
        }

        private void CommitFile()
        {
            if (File.Exists(CurrentPath))
            {
                File.SetAttributes(CurrentPath, FileAttributes.Normal);
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
