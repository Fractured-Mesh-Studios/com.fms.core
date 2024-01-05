
namespace CoreEngine
{
	public enum CoordinateType
	{
		None,
		LocalSpace,
		WorldSpace,
		ObjectSpace,
		BoneSpace
	}

	public enum UpdateMethod
	{
		Update,
		FixedUpdate,
		LateUpdate
	}

	public enum InterpolationType
	{
		None,
		Linear,
		Spherical
	}

	public enum KeyModifier
	{
		None,
		Ctrl,
		Shift,
		Alt,
		Cmd
	}

}