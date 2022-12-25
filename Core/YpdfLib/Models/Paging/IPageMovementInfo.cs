namespace YpdfLib.Models.Paging
{
    public interface IPageMovementInfo : IDeepCloneable<IPageMovementInfo>
    {
        int? PageToMove { get; set; }
        int? NewPosition { get; set; }

        int GetPageToMoveOrDefault();
        int GetNewPositionOrDefault();
    }
}