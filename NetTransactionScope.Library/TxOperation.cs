using System;
using System.Transactions;

namespace NetTransactionScope.Library
{
    public abstract class TxOperation : IEnlistmentNotification
    {
        private static object LockObj = new object();

        protected TxOperation()
        {
            if (Transaction.Current != null)
            {
                //Enlist on the current transaction with the enlistment object
                Transaction.Current.EnlistVolatile(this, EnlistmentOptions.None);
            }
            else
            {
                throw new TransactionException("Use inside transaction scope.");
            }

        }

        public virtual void Commit(Enlistment enlistment)
        {
            enlistment.Done();
        }

        public virtual void InDoubt(Enlistment enlistment)
        {
            enlistment.Done();
        }

        protected abstract void PrepareInternal(PreparingEnlistment preparingEnlistment);

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            lock (LockObj)
            {
                try
                {
                    PrepareInternal(preparingEnlistment);
                    preparingEnlistment.Prepared();
                }
                catch (Exception ex)
                {
                    preparingEnlistment.ForceRollback();
                }
            }

        }

        public abstract void Rollback(Enlistment enlistment);
    }
}
