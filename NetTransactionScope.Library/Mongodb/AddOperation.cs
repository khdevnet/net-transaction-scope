using System.Transactions;

namespace NetTransactionScope.Library.Mongodb
{
    public class AddOperation : IEnlistmentNotification
    {
        public AddOperation()
        {
            Transaction.Current.EnlistVolatile(this, EnlistmentOptions.None);
        }

        public void Commit(Enlistment enlistment)
        {
            throw new System.NotImplementedException();
        }

        public void InDoubt(Enlistment enlistment)
        {
            throw new System.NotImplementedException();
        }

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            throw new System.NotImplementedException();
        }

        public void Rollback(Enlistment enlistment)
        {
            throw new System.NotImplementedException();
        }
    }
}
