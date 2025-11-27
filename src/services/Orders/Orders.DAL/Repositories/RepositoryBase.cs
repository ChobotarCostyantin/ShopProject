using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.DAL.Repositories
{
    public abstract class RepositoryBase
    {
        protected DbConnection Connection = null!;
        protected DbTransaction Transaction = null!;

        public void SetTransaction(DbConnection connection, DbTransaction transaction)
        {
            Connection = connection;
            Transaction = transaction;
        }
        
        public void ThrowIfConnectionOrTransactionIsUninitialized()
        {
            if (Connection == null || Transaction == null)
                throw new InvalidOperationException("Repository is not initialized with connection or transaction");
        }
    }
}