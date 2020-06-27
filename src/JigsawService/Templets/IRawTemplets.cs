namespace JigsawService.Templets
{
    internal interface IRawTemplets
    {
        string Serialize();
        Maybe<Templet, string> Deserialize(string templet);
    }
}