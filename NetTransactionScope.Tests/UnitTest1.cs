using System;
using System.Transactions;
using Xunit;

namespace NetTransactionScope.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void CompleteTransaction()
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    //Create an enlistment object
                    myEnlistmentClass myElistment = new myEnlistmentClass();


                    //Perform transactional work here.

                    scope.Complete();
                }
            }
            catch (System.Transactions.TransactionException ex)
            {
                Console.WriteLine(ex);
            }
            catch
            {
                Console.WriteLine("Cannot complete transaction");
                throw;
            }
        }

        [Fact]
        public void NotCompleteTransaction()
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    //Create an enlistment object
                    myEnlistmentClass myElistment = new myEnlistmentClass();

                    //Perform transactional work here.
                }
            }
            catch (System.Transactions.TransactionException ex)
            {
                Console.WriteLine(ex);
            }
            catch
            {
                Console.WriteLine("Cannot complete transaction");
                throw;
            }
        }
    }

    class myEnlistmentClass : IEnlistmentNotification
    {
        public myEnlistmentClass()
        {
            //Enlist on the current transaction with the enlistment object
            Transaction.Current.EnlistVolatile(this, EnlistmentOptions.None);
        }

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            Console.WriteLine("Prepare notification received");

            //Perform transactional work

            //If work finished correctly, reply prepared
            preparingEnlistment.Prepared();

            // otherwise, do a ForceRollback
            // preparingEnlistment.ForceRollback();
        }

        public void Execute()
        {
        }

        public void Commit(Enlistment enlistment)
        {
            Console.WriteLine("Commit notification received");

            //Do any work necessary when commit notification is received

            //Declare done on the enlistment
            enlistment.Done();
        }

        public void Rollback(Enlistment enlistment)
        {
            Console.WriteLine("Rollback notification received");

            //Do any work necessary when rollback notification is received

            //Declare done on the enlistment
            enlistment.Done();
        }

        public void InDoubt(Enlistment enlistment)
        {
            Console.WriteLine("In doubt notification received");

            //Do any work necessary when indout notification is received

            //Declare done on the enlistment
            enlistment.Done();
        }
    }
}
