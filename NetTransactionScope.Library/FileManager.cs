using System.Collections.Generic;
using System.IO;
using System.Transactions;

namespace NetTransactionScope.Library
{
    public class FileManager
    {
        public void Delete(string path)
        {
            if (IsInTransaction())
            {
                EnlistOperation(new DeleteFile(path));
            }
            else
            {
                File.Delete(path);
            }
        }

        private static bool IsInTransaction()
        {
            return Transaction.Current != null;
        }

        private static void EnlistOperation(IRollbackableOperation operation)
        {
            Transaction tx = Transaction.Current;
            TxEnlistment enlistment;

            if (_enlistments == null)
            {
                _enlistments = new Dictionary<string, TxEnlistment>();
            }

            if (!_enlistments.TryGetValue(tx.TransactionInformation.LocalIdentifier, out enlistment))
            {
                enlistment = new TxEnlistment(tx);
                _enlistments.Add(tx.TransactionInformation.LocalIdentifier, enlistment);
            }
            enlistment.EnlistOperation(operation);
        }
    }
}
