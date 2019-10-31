using System.Transactions;

namespace NetTransactionScope.Library
{
    public class DeleteFileOperation : FileOperation
    {
        public DeleteFileOperation(string path)
        : base(path)
        {
        }

        public override void Prepare(PreparingEnlistment preparingEnlistment)
        {
            try
            {

                EnsureTempFolderExists();
                BackupFile();
                DeleteFile();

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
            DeleteBackupFile();
            //Declare done on the enlistment
            enlistment.Done();
        }

        public override void Rollback(Enlistment enlistment)
        {
            //Do any work necessary when rollback notification is received

            RestoreFile();
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
    }
}
