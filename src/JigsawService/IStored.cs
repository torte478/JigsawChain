namespace JigsawService
{
    internal interface IStored<TKey, TValue>
    {
        TKey Store(TValue value);
        TKey Store(TKey key, TValue value);
        bool IsStored(TKey id);
        TValue Extract(TKey id);
        TValue Read(TKey id);
    }
}