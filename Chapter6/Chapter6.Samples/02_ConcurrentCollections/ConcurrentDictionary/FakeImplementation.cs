using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace Chapter6.Samples._02_ConcurrentCollections.ConcurrentDictionary
{
    /// <summary>
    /// This is fake implementation that is used only for copying sources to word document!
    /// </summary>
    public class FakeImplementationConcurrentDictionary<TKey, TValue>
    {
        private Tables m_tables;
        private bool s_isValueWriteAtomic;
        private int m_budget;
        private int DEFAULT_CAPACITY;

        private bool TryAddInternal(TKey key, TValue value, out TValue resultingValue)
        {
            while (true)
            {
                bool resizeDesired = false;
                var tables = m_tables;
                int bucketNo, lockNo;
                int hashcode = tables.m_comparer.GetHashCode(key);

                GetBucketAndLockNo(hashcode, out bucketNo, out lockNo);

                try
                {
                    Monitor.Enter(tables.m_locks[lockNo]);

                    // If the table just got resized, we may not be holding the right lock, and must retry.
                    // This should be a rare occurence.
                    if (tables != m_tables)
                    {
                        continue;
                    }

                    // Looping through Nodes in the bucket. If existing Node was found
                    // the method returns false, otherwise new Node would be added
                    for (Node node = tables.m_buckets[bucketNo]; node != null; node = node.m_next)
                    {
                        // ...
                    }

                    //
                    // If the number of elements guarded by this lock has exceeded the budget, resize the bucket table.
                    // It is also possible that GrowTable will increase the budget but won't resize the bucket table.
                    // That happens if the bucket table is found to be poorly utilized due to a bad hash function.
                    //
                    if (tables.m_countPerLock[lockNo] > m_budget)
                    {
                        resizeDesired = true;
                    }
                }
                finally
                {
                    Monitor.Exit(tables.m_locks[lockNo]);
                }

                // Resize table if needed.
                // This method should be called outside the lock to prevent a deadlocks.
                if (resizeDesired)
                {
                    GrowTable(tables, tables.m_comparer);
                }

                resultingValue = value;
                return true;
            }
        }

public int Count
{
    get
    {
        int count = 0;

        try
        {
            // Acquire all locks
            AcquireAllLocks();

            // Compute the count, we allow overflow
            for (int i = 0; i < m_tables.m_countPerLock.Length; i++)
            {
                count += m_tables.m_countPerLock[i];
            }

        }
        finally
        {
            // Release locks that have been acquired earlier
            ReleaseLocks();
        }

        return count;
    }
}

        public void Clear()
        {
            int locksAcquired = 0;
            try
            {
                AcquireAllLocks(ref locksAcquired);

                // Recreate new Tables object with default capacity
                // and existing locks
                Tables newTables = new Tables(
                    new Node[DEFAULT_CAPACITY], m_tables.m_locks, 
                    new int[m_tables.m_countPerLock.Length], 
                    m_tables.m_comparer);
                m_tables = newTables;
                m_budget = Math.Max(1, newTables.m_buckets.Length / newTables.m_locks.Length);
            }
            finally
            {
                ReleaseLocks(0, locksAcquired);
            }
        }

        private void ReleaseLocks(int i, int locksAcquired)
        {
            throw new NotImplementedException();
        }
        private void ReleaseLocks()
        {
            throw new NotImplementedException();
        }

        private void AcquireAllLocks(ref int locksAcquired)
        {
            throw new NotImplementedException();
        }
        private void AcquireAllLocks()
        {
            throw new NotImplementedException();
        }

        private void GetBucketAndLockNo(int hashcode, out int bucketNo, out int lockNo)
        {
            throw new System.NotImplementedException();
        }

        private void GrowTable(Tables tables, EqualityComparer<TKey> mComparer)
        {
            throw new System.NotImplementedException();
        }

        internal class Node
        {
            public Node m_next;
            public TKey m_key;
            public TValue m_value;

            public Node(object key, object value, int hashcode, Node mBucket)
            {
                throw new System.NotImplementedException();
            }
        }

        internal class Tables
        {
            public object[] m_locks;
            public Node[] m_buckets;
            public EqualityComparer<TKey> m_comparer;
            public int[] m_countPerLock;

            public Tables(Node[] nodes, object[] mLocks, int[] ints, EqualityComparer<TKey> mComparer)
            {
                throw new NotImplementedException();
            }
        }

    }

}