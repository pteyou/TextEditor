using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    interface ICache<KeyT, ValueT> where ValueT : class
    {
        bool Set(KeyT key, ValueT value);
        ValueT Get(KeyT key);
        int Size();
        bool IsEmpty();
        void Clear();
    }
}
