﻿using System.IO;
using System.Transactions;

namespace NetTransactionScope.Library.Files
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
            DeleteFile();
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

        private void BackupFile()
        {
            if (File.Exists(CurrentPath))
            {
                File.Copy(CurrentPath, BackupPath);
            }
        }
    }
}
