
namespace PaintCraft.Tools
{
	[System.Flags]
	public enum PointStatus : int
	{
		NotSet = 0,
		PositionSet = 1 << 1,
		Temporary = 1 << 2,
		ReadyToApply = 1 << 3, //could be copied to canvas
		CopiedToCanvas = 1 << 4,
		NotCopiedToCanvas = PointStatus.PositionSet | PointStatus.Temporary | PointStatus.ReadyToApply
	}
}
