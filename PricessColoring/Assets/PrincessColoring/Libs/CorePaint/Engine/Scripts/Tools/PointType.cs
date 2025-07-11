
namespace PaintCraft.Tools
{
	[System.Flags]
	public enum PointType : int
	{
		BasePoint = 1 << 0,
		InterpolatedPoint = 1 << 1
	}
}