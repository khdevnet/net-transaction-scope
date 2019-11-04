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
            try
            {

                _fileStorage.CreateFile(CurrentPath, _fileData);

                //If work finished correctly, reply prepared
                preparingEnlistment.Prepared();
            }
            catch (IOException ex)
            {
                preparingEnlistment.ForceRollback();
            }
        }

        public override void Commit(Enlistment enlistment)
        {
            enlistment.Done();
        }

        public override void Rollback(Enlistment enlistment)
        {
            //Do any work necessary when rollback notification is received

            DeleteFile();

            //Declare done on the enlistment
            enlistment.Done();
        }

        public override void InDoubt(Enlistment enlistment)
        {
            //Do any work necessary when indout notification is received
            //Declare done on the enlistment
            enlistment.Done();
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
