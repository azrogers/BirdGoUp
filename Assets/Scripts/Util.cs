using DG.Tweening;
using UnityEngine;

public static class Util
{
	#region Lerp
	/// <summary>
	/// Lerps a value between a and b based on the value of t and the specified easing curve.
	/// </summary>
	public static float Lerp(float a, float b, float t, Ease easing = Ease.Linear)
	{
		t = Mathf.Clamp(t, 0, 1);
		return DOVirtual.EasedValue(a, b, t, easing);
	}

	/// <summary>
	/// Lerps two values between a and b based on the value of t and the specified easing curve.
	/// </summary>
	public static Color Lerp(Color a, Color b, float t, Ease easing = Ease.Linear) => new Color(
		Lerp(a.r, b.r, t, easing),
		Lerp(a.g, b.g, t, easing),
		Lerp(a.b, b.b, t, easing),
		Lerp(a.a, b.a, t, easing)
	);

	/// <summary>
	/// Lerps two Vector2s between a and b based on the value of t and the specified easing curve.
	/// </summary>
	public static Vector2 Lerp(Vector2 a, Vector2 b, float t, Ease easing = Ease.Linear) => new Vector2(
		Lerp(a.x, b.x, t, easing),
		Lerp(a.y, b.y, t, easing)
	);

	/// <summary>
	/// Lerps two float[]s between a and b based on the value of t and the specified easing curve.
	/// </summary>
	public static float[] Lerp(float[] a, float[] b, float t, Ease easing = Ease.Linear)
	{
		if(a.Length != b.Length)
		{
			throw new System.IndexOutOfRangeException("a and b arrays are of different lengths.");
		}

		var newArr = new float[a.Length];
		for(var i = 0; i < newArr.Length; i++)
		{
			newArr[i] = Lerp(a[i], b[i], t);
		}

		return newArr;
	}
	#endregion
}