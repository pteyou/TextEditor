namespace TextEditor.Cache
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
