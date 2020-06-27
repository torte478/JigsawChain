namespace JigsawService
{
    internal interface IStored<TKey, TValue>
    {
        TKey Store(TValue value);
        bool IsStored(TKey id);
        TValue Extract(TKey id);
    }
}