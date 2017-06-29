#pragma warning disable 0109

using UnityEngine;
using System.Collections.Generic;
using LinqTools;
using System;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif

public static class WadeUtils
{
	#region CONSTANTS
	public static float SMALLNUMBER = 0.00000001f;
	public static float EPSILON = 0.005f;

	public static float TAU = Mathf.PI * 2f;
	#endregion

	#region FLOATS
	public static bool AreEqual(float a, float b)
	{
		return Mathf.Abs(a - b) < SMALLNUMBER;
	}

	public static bool IsZero(float num)
	{
		return Mathf.Abs(num) < SMALLNUMBER;
	}

	public static bool IsPositive(float num)
	{
		return num > 0f;
	}

	public static float UnclampedInverseLerp(float a, float b, float value)
	{ return (value - a) / (b - a); }

	public static float RSign()
	{ return UnityEngine.Random.value > 0.5f ? 1 : -1; }
	#endregion

	#region VECTORS
	public static bool IsZero(this Vector2 vec)
	{
		return vec.sqrMagnitude < SMALLNUMBER;
	}

	public static bool IsZero(this Vector3 vec)
	{
		return vec.sqrMagnitude < SMALLNUMBER;
	}

	public static bool IsZero(this Vector4 vec)
	{
		return vec.sqrMagnitude < SMALLNUMBER;
	}

	public static bool AreEqual(Color32 a, Color32 b)
	{
		return Mathf.Abs(a.r - b.r) < SMALLNUMBER && Mathf.Abs(a.b - b.b) < SMALLNUMBER && Mathf.Abs(a.g - b.g) < SMALLNUMBER && Mathf.Abs( a.a - b.a) < SMALLNUMBER;
	}

	public static Vector2 SetX(this Vector2 vec, float x)
	{
		vec.x = x;
		return vec;
	}

	public static Vector2 SetY(this Vector2 vec, float y)
	{
		vec.y = y;
		return vec;
	}

	public static Vector3 SetX(this Vector3 vec, float x)
	{
		vec.x = x;
		return vec;
	}

	public static Vector3 SetY(this Vector3 vec, float y)
	{
		vec.y = y;
		return vec;
	}

	public static Vector3 SetXY(this Vector3 vec, Vector2 xy)
	{
		vec.x = xy.x;
		vec.y = xy.y;
		return vec;
	}

	public static Vector3 SetXZ(this Vector3 vec, Vector2 xz)
	{
		vec.x = xz.x;
		vec.z = xz.y;
		return vec;
	}

	public static Vector3 SetYZ(this Vector3 vec, Vector2 yz)
	{
		vec.y = yz.x;
		vec.z = yz.y;
		return vec;
	}

	public static Vector3 SetXY(this Vector3 vec, float x, float y)
	{
		vec.x = x;
		vec.y = y;
		return vec;
	}

	public static Vector3 SetXZ(this Vector3 vec, float x, float z)
	{
		vec.x = x;
		vec.z = z;
		return vec;
	}

	public static Vector3 SetYZ(this Vector3 vec, float y, float z)
	{
		vec.z = z;
		vec.y = y;
		return vec;
	}

	public static Vector3 SetZ(this Vector3 vec, float z)
	{
		vec.z = z;
		return vec;
	}

	public static Vector4 SetX(this Vector4 vec, float x)
	{
		vec.x = x;
		return vec;
	}

	public static Vector4 SetY(this Vector4 vec, float y)
	{
		vec.y = y;
		return vec;
	}

	public static Vector4 SetZ(this Vector4 vec, float z)
	{
		vec.z = z;
		return vec;
	}

	public static Vector4 SetW(this Vector4 vec, float w)
	{
		vec.w = w;
		return vec;
	}

	public static Vector4 SetXY(this Vector4 vec, Vector2 xy)
	{
		vec.x = xy.x;
		vec.y = xy.y;
		return vec;
	}

	public static Vector4 SetXZ(this Vector4 vec, Vector2 xz)
	{
		vec.x = xz.x;
		vec.z = xz.y;
		return vec;
	}

	public static Vector4 SetYZ(this Vector4 vec, Vector2 yz)
	{
		vec.y = yz.x;
		vec.z = yz.y;
		return vec;
	}

	public static Vector4 SetXW(this Vector4 vec, Vector2 xw)
	{
		vec.x = xw.x;
		vec.w = xw.y;
		return vec;
	}

	public static Vector4 SetYW(this Vector4 vec, Vector2 yw)
	{
		vec.y = yw.x;
		vec.w = yw.y;
		return vec;
	}

	public static Vector4 SetZW(this Vector4 vec, Vector2 zw)
	{
		vec.z = zw.x;
		vec.w = zw.y;
		return vec;
	}

	public static Vector4 SetXY(this Vector4 vec, float x, float y)
	{
		vec.x = x;
		vec.y = y;
		return vec;
	}

	public static Vector4 SetXZ(this Vector4 vec, float x, float z)
	{
		vec.x = x;
		vec.z = z;
		return vec;
	}

	public static Vector4 SetYZ(this Vector4 vec, float y, float z)
	{
		vec.z = z;
		vec.y = y;
		return vec;
	}

	public static Vector4 SetXW(this Vector4 vec, float x, float w)
	{
		vec.x = x;
		vec.w = w;
		return vec;
	}

	public static Vector4 SetYW(this Vector4 vec, float y, float w)
	{
		vec.w = w;
		vec.y = y;
		return vec;
	}

	public static Vector4 SetZW(this Vector4 vec, float z, float w)
	{
		vec.w = w;
		vec.z = z;
		return vec;
	}

	public static Vector4 SetXYZ(this Vector4 vec, Vector3 xyz)
	{
		vec.x = xyz.x;
		vec.y = xyz.y;
		vec.z = xyz.z;
		return vec;
	}

	public static Vector4 SetXYW(this Vector4 vec, Vector3 xyw)
	{
		vec.x = xyw.x;
		vec.y = xyw.y;
		vec.w = xyw.z;
		return vec;
	}

	public static Vector4 SetXZW(this Vector4 vec, Vector3 xzw)
	{
		vec.x = xzw.x;
		vec.z = xzw.y;
		vec.w = xzw.z;
		return vec;
	}

	public static Vector4 SetYZW(this Vector4 vec, Vector3 yzw)
	{
		vec.y = yzw.x;
		vec.z = yzw.y;
		vec.w = yzw.z;
		return vec;
	}

	public static Vector4 SetXYZ(this Vector4 vec, float x, float y, float z)
	{
		vec.x = x;
		vec.y = y;
		vec.z = z;
		return vec;
	}

	public static Vector4 SetXYW(this Vector4 vec, float x, float y, float w)
	{
		vec.x = x;
		vec.y = y;
		vec.w = w;
		return vec;
	}

	public static Vector4 SetXZW(this Vector4 vec, float x, float z, float w)
	{
		vec.x = x;
		vec.z = z;
		vec.w = w;
		return vec;
	}

	public static Vector4 SetYZW(this Vector4 vec, float y, float z, float w)
	{
		vec.y = y;
		vec.z = z;
		vec.w = w;
		return vec;
	}

	public static bool IsInRange(float orig, float min, float max)
	{
		return orig > min && orig < max;
	}

	public static bool IsInRange(Vector3 orig, Vector3 min, Vector3 max)
	{
		return IsInRange(orig.x, min.x, max.x) &&
				IsInRange(orig.y, min.y, max.y) &&
				IsInRange(orig.z, min.z, max.z);
	}

	public static bool IsInRange(Vector2 orig, Vector2 min, Vector2 max)
	{
		return IsInRange(orig.x, min.x, max.x) &&
				IsInRange(orig.y, min.y, max.y);
	}
	
	public static Vector2 Clamp(Vector2 orig, Vector2 min, Vector2 max)
	{
		return new Vector2(Mathf.Clamp(orig.x, min.x, max.x),
							Mathf.Clamp(orig.y, min.y, max.y));
	}

	public static Vector3 Clamp(Vector3 orig, Vector3 min, Vector3 max)
	{
		return new Vector3(Mathf.Clamp(orig.x, min.x, max.x),
							Mathf.Clamp(orig.y, min.y, max.y),
							Mathf.Clamp(orig.z, min.z, max.z));
	}

	public static Vector4 Clamp(Vector4 orig, Vector4 min, Vector4 max)
	{
		return new Vector4(Mathf.Clamp(orig.x, min.x, max.x),
						   Mathf.Clamp(orig.y, min.y, max.y));
	}

	public static Vector2 RandomBetween(Vector2 a, Vector2 b)
	{
		Vector2 retVal = Vector2.zero;
		retVal.x = UnityEngine.Random.Range(a.x, b.x);
		retVal.y = UnityEngine.Random.Range(a.y, b.y);
		return retVal;
	}

	public static Vector3 RandomBetween(Vector3 a, Vector3 b)
	{
		Vector3 retVal = Vector3.zero;
		retVal.x = UnityEngine.Random.Range(a.x, b.x);
		retVal.y = UnityEngine.Random.Range(a.y, b.y);
		retVal.z = UnityEngine.Random.Range(a.z, b.z);
		return retVal;
	}

	public static Vector4 RandomBetween(Vector4 a, Vector4 b)
	{
		Vector4 retVal = Vector4.zero;
		retVal.x = UnityEngine.Random.Range(a.x, b.x);
		retVal.y = UnityEngine.Random.Range(a.y, b.y);
		retVal.z = UnityEngine.Random.Range(a.z, b.z);
		retVal.w = UnityEngine.Random.Range(a.w, b.w);
		return retVal;
	}

	public static Vector2 Random(this Vector2 v)
	{
		v.x *= UnityEngine.Random.value;
		v.y *= UnityEngine.Random.value;
		return v;
	}

	public static Vector3 Random(this Vector3 v)
	{
		v.x *= UnityEngine.Random.value;
		v.y *= UnityEngine.Random.value;
		v.z *= UnityEngine.Random.value;
		return v;
	}

	public static Vector4 Random(this Vector4 v)
	{
		v.x *= UnityEngine.Random.value;
		v.y *= UnityEngine.Random.value;
		v.z *= UnityEngine.Random.value;
		v.w *= UnityEngine.Random.value;
		return v;
	}

	public static Vector3 Orthogonal(this Vector3 v)
	{ return new Vector3(v.y, -v.x, v.z); }

	public static Vector2 Orthogonal(this Vector2 v)
	{ return new Vector2(v.y, -v.x); }
	#endregion

	#region TRANSFORM
	public static void LookAt2D(this Transform transform, Transform target)
	{
		transform.LookAt2D(target.position);
	}

	public static void LookAt2D(this Transform transform, Vector3 position)
	{
		Vector3 directionToTarget = position - transform.position;
		float lookAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90f;
		transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
	}

	public static void LerpLookAt2D(this Transform transform, Vector3 position, float t)
	{
		Quaternion currentRot = transform.rotation;
		transform.LookAt2D(position);
		Quaternion lookRot = transform.rotation;

		transform.rotation = Quaternion.Lerp(currentRot, lookRot, t);
	}

	public static void LerpLookAt(this Transform transform, Transform target, Vector3 upDirection, float t)
	{
		Quaternion currentRot = transform.rotation;
		transform.LookAt(target, upDirection);
		Quaternion lookRot = transform.rotation;

		transform.rotation = Quaternion.Lerp(currentRot, lookRot, t);
	}

	public static void LerpLookAt(this Transform transform, Vector3 target, Vector3 upDirection, float t)
	{
		Quaternion currentRot = transform.rotation;
		transform.LookAt(target, upDirection);
		Quaternion lookRot = transform.rotation;

		transform.rotation = Quaternion.Lerp(currentRot, lookRot, t);
	}

	public enum ResetType
	{
		Position = 1 << 0,
		Rotation = 1 << 1,
		Scale = 1 << 2,
		All = Position | Rotation | Scale
	}

	public static void ResetLocals(this Transform transform, ResetType inResetType = ResetType.All)
	{
		if (CheckBit((int)inResetType, (int)ResetType.Position))
			transform.localPosition = Vector3.zero;

		if (CheckBit((int)inResetType, (int)ResetType.Rotation))
			transform.localRotation = Quaternion.identity;

		if (CheckBit((int)inResetType, (int)ResetType.Scale))
			transform.localScale = Vector3.one;
	}
	#endregion

	#region RECTS
	public static Vec2Range GetBounds(this RectTransform rectTransform, ref Vector3[] rectCorners, float edgePercent = 0f)
	{
		rectTransform.GetWorldCorners(rectCorners);

		Vec2Range range;
		range.min = Vector2.one * Mathf.Infinity;
		range.max = Vector2.one * Mathf.NegativeInfinity;
		for (int i = 0; i < rectCorners.Length; i++)
		{
			Vector3 corner = rectCorners[i];
			range.min.x = Mathf.Min(range.min.x, corner.x);
			range.min.y = Mathf.Min(range.min.y, corner.y);
			range.max.x = Mathf.Max(range.max.x, corner.x);
			range.max.y = Mathf.Max(range.max.y, corner.y);
		}

		Vector2 toMax = range.max - range.min;
		range.min += toMax * edgePercent;
		range.max -= toMax * edgePercent;

		return range;
	}
	#endregion

	#region BITS
	public static bool CheckBit(int bit, int shouldContainBit)
	{
		return (bit & shouldContainBit) == shouldContainBit;
	}
	#endregion

	#region COLORS
	public static Color SetR(this Color c, float r)
	{
		c.r = r;
		return c;
	}

	public static Color SetG(this Color c, float g)
	{
		c.g = g;
		return c;
	}

	public static Color SetB(this Color c, float b)
	{
		c.b = b;
		return c;
	}

	public static Color SetA(this Color c, float a)
	{
		c.a = a;
		return c;
	}

	public static Color32 SetR(this Color32 c, byte r)
	{
		c.r = r;
		return c;
	}

	public static Color32 SetG(this Color32 c, byte g)
	{
		c.g = g;
		return c;
	}

	public static Color32 SetB(this Color32 c, byte b)
	{
		c.b = b;
		return c;
	}

	public static Color32 SetA(this Color32 c, byte a)
	{
		c.a = a;
		return c;
	}

	public static Gradient Lerp(Gradient a, Gradient b, float t)
	{
		Gradient c = new Gradient();
		List<GradientColorKey> colorKeys = new List<GradientColorKey>();
		List<GradientAlphaKey> alphaKeys = new List<GradientAlphaKey>();

		for (int i = 0; i < MaxGradientControls; i++)
		{
			float alpha = i / (MaxGradientControls - 1f);
			Color averageColor = Color.Lerp(a.Evaluate(alpha), b.Evaluate(alpha), t);
			colorKeys.Add(new GradientColorKey(averageColor, alpha));
			alphaKeys.Add(new GradientAlphaKey(averageColor.a, alpha));
		}

		c.SetKeys(colorKeys.ToArray(), alphaKeys.ToArray());
		return c;
	}

	// Gradients can have a max of 8 controls for each Color and Alpha
	public static int MaxGradientControls = 8;

	public static void CopyValue(Gradient source, ref Gradient destination)
	{
		GradientAlphaKey[] alphaKeys = new GradientAlphaKey[source.alphaKeys.Length];
		GradientColorKey[] colorKeys = new GradientColorKey[source.colorKeys.Length];

		for (int i = 0; i < alphaKeys.Length; i++)
			alphaKeys[i] = source.alphaKeys[i];

		for(int i = 0; i < colorKeys.Length; i++)
			colorKeys[i] = source.colorKeys[i];

		destination.SetKeys(colorKeys, alphaKeys);
	}

	public static HSVColor RGBToHSV(Color color)
	{
		HSVColor ret = new HSVColor(0f, 0f, 0f, color.a);

		float r = color.r;
		float g = color.g;
		float v = color.b;

		float max = Mathf.Max(r, Mathf.Max(g, v));

		if (max <= 0f)
			return ret;

		float min = Mathf.Min(r, Mathf.Min(g, v));
		float dif = max - min;

		if (max > min)
		{
			if (WadeUtils.AreEqual(g, max))
				ret.h = (v - r) / dif * 60f + 120f;
			else if (WadeUtils.AreEqual(v, max))
				ret.h = (r - g) / dif * 60f + 240f;
			else if (v > g)
				ret.h = (g - v) / dif * 60f + 360f;
			else
				ret.h = (g - v) / dif * 60f;

			if (!WadeUtils.IsPositive(ret.h))
				ret.h = ret.h + 360f;
		}
		else
			ret.h = 0f;

		ret.h *= 1f / 360f;
		ret.s = (dif / max) * 1f;
		ret.v = max;

		return ret;
	}

	public static Color HSVToRGB(HSVColor hsbColor)
	{
		float r = hsbColor.v;
		float g = hsbColor.v;
		float b = hsbColor.v;

		if (!WadeUtils.IsZero(hsbColor.s))
		{
			float max = hsbColor.v;
			float dif = hsbColor.v * hsbColor.s;
			float min = hsbColor.v - dif;

			float h = hsbColor.h * 360f;

			if (h < 60f)
			{
				r = max;
				g = h * dif / 60f + min;
				b = min;
			}
			else if (h < 120f)
			{
				r = (h - 120f) * dif / 60f + min;
				g = max;
				b = min;
			}
			else if (h < 180f)
			{
				r = min;
				g = max;
				b = (h - 120f) * dif / 60f + min;
			}
			else if (h < 240f)
			{
				r = min;
				g = (h - 240f) * dif / 60f + min;
				b = max;
			}
			else if (h < 300f)
			{
				r = (h - 240f) * dif / 60f + min;
				g = min;
				b = max;
			}
			else if (h <= 360f)
			{
				r = max;
				g = min;
				b = (h - 360f) * dif / 60f + min;
			}
			else
			{
				r = 0f;
				g = 0f;
				b = 0f;
			}
		}

		return new Color(Mathf.Clamp01(r), Mathf.Clamp01(g), Mathf.Clamp01(b), hsbColor.a);
	}

	public static string ColorToRGBHex(Color color)
	{
		string rHex = ((int)(color.r * 255)).ToString("x2");
		string gHex = ((int)(color.g * 255)).ToString("x2");
		string bHex = ((int)(color.b * 255)).ToString("x2");
		return rHex + gHex + bHex;
	}

	public static bool ColorEqualityRGB(Color a, Color b)
	{
		return a.r == b.r
			&& a.g == b.g
			&& a.b == b.b;
	}

	public static int ColorEqualityRGBCount(Color a, Color b)
	{
		int count = 0;
		if (a.r == b.r) count++;
		if (a.g == b.g) count++;
		if (a.b == b.b) count++;

		return count;
	}

	public static int ColorNonBlackRGBCount(Color a)
	{
		int count = 0;
		if (a.r > 0.0f) count++;
		if (a.g > 0.0f) count++;
		if (a.b > 0.0f) count++;

		return count;
	}

	public static int ColorFullRGBCount(Color a)
	{
		int count = 0;
		if (a.r >= 1.0f) count++;
		if (a.g >= 1.0f) count++;
		if (a.b >= 1.0f) count++;

		return count;
	}

	public static Color InvertColor(Color c)
	{
		return new Color(1f - c.r, 1f - c.g, 1f - c.b, c.a);
	}

	//I don't know what to call this, but it acts like
	// ColorEqualityRGBCount except that Equality here
	// just means 'contains some' instead of "matches"
	public static int ColorPositiveMixRGBCount(Color a, Color b)
	{
		int count = 0;
		if (a.r == 0.0f || a.r > 0.0f == b.r > 0.0f)
			count++;

		if (a.g == 0.0f || a.g > 0.0f == b.g > 0.0f)
			count++;

		if (a.b == 0.0f || a.b > 0.0f == b.b > 0.0f)
			count++;

		return count;
	}

	//This may not actually be useful...
	public static Color ColorSubtract(Color a, Color b)
	{
		Color result = new Color(
			Mathf.Max(0.0f, a[0] - b[0]),
			Mathf.Max(0.0f, a[1] - b[1]),
			Mathf.Max(0.0f, a[2] - b[2]));

		return result;
	}

	public static Color AverageColorsLAB(Color a, Color b)
	{
		return LABColor.Lerp(a, b, 0.5f);
	}

	public static Color LerpColorsLAB(Color a, Color b, float t)
	{
		return LABColor.Lerp(a, b, t);
	}

	public static Color RandomColor()
	{
		return new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
	}

	public static Color32 RandomColor32()
	{
		return RandomColor();
	}

	public static int CompareColors(Color a, Color b) { return (a.r + a.g + a.b + a.a).CompareTo(b.r + b.g + b.b + b.a); }
	public static int CompareColors32(Color32 a, Color32 b) { return (a.r + a.g + a.b + a.a).CompareTo(b.r + b.g + b.b + b.a); }
	public static int CompareColorsComponentwise32(Color32 a, Color32 b) { return (a.r * 1000f + a.g * 100f + a.b * 10f + a.a).CompareTo(b.r * 1000f + b.g * 100f + b.b * 10f + b.a); }

	//public static float ColorDistance (Color a, Color b) { return Mathf.Abs((a.r+a.g+a.b+a.a) - (b.r+b.g+b.b+b.a)); }
	public static float ColorDistance(Color a, Color b) { return Mathf.Abs(a.r - b.r) + Mathf.Abs(a.g - b.g) + Mathf.Abs(a.b - b.b) + Mathf.Abs(a.a - b.a); }
	public static float ColorDistanceSqr(Color a, Color b) { return (a.r - b.r) * (a.r * b.r) + (a.g - b.g) * (a.g * b.g) + (a.b - b.b) * (a.b * b.b) + (a.a - b.a) * (a.a * b.a); }
	#endregion

	#region FIND OBJECTS
	public static T FindRandomObjectOfType<T>() where T : UnityEngine.Object
	{
		T[] tArray = GameObject.FindObjectsOfType<T>();
		return tArray[UnityEngine.Random.Range(0, tArray.Length)];
	}

	public static T FindNearestObjectOfType<T>(Transform transform) where T : MonoBehaviour
	{ return FindNearestObjectOfType<T>(transform.position); }

	public static T FindNearestObjectOfType<T>(Vector3 position) where T : MonoBehaviour
	{
		T[] tArray = GameObject.FindObjectsOfType<T>();
		T nearestOfType = null;

		if (tArray.Length > 0)
		{
			nearestOfType = tArray[0];
			float nearestDistance = Vector3.Distance(position, ((MonoBehaviour)tArray[0]).GetComponent<Transform>().position);
			for (int i = 1; i < tArray.Length; i++)
			{
				float distance = Vector3.Distance(position, ((MonoBehaviour)tArray[i]).GetComponent<Transform>().position);
				if (distance < nearestDistance)
				{
					nearestOfType = tArray[i];
					nearestDistance = distance;
				}
			}
		}

		return nearestOfType;
	}
	#endregion

	#region LISTS
	public static T First<T>(this List<T> list)
	{
		Debug.Assert(list.Count > 0, typeof(T).ToString() + " list is empty.");
		return list[0];
	}

	public static T Last<T>(this List<T> list)
	{
		Debug.Assert(list.Count > 0, typeof(T).ToString() + " list is empty.");
		return list[list.Count - 1];
	}

	public static T Random<T>(this IEnumerable<T> list)
	{
		Debug.Assert(list.Count() > 0, typeof(T).ToString() + " list is empty.");
		return list.OrderBy(x => UnityEngine.Random.value).First();
	}

	public static T Random<T>(this List<T> list)
	{
		Debug.Assert(list.Count > 0, typeof(T).ToString() + " list is empty.");
		return list.OrderBy(x => UnityEngine.Random.value).First();
	}

	public static void Add<T>(this List<T> inList, params T[] inAdds)
	{
		for(int i = 0; i < inAdds.Length; i++)
			inList.Add(inAdds[i]);
	}

	public static void Shuffle<T>(this IList<T> list)
	{
		var count = list.Count;
		var last = count - 1;
		for (var i = 0; i < last; ++i) {
			var r = UnityEngine.Random.Range(i, count);
			var tmp = list[i];
			list[i] = list[r];
			list[r] = tmp;
		}
	}
	#endregion

	public static void ClearAll<T>() where T : MonoBehaviour
	{
		foreach (var obj in MonoBehaviour.FindObjectsOfType<T>())
			MonoBehaviour.Destroy(obj.gameObject);
	}

	public static bool IsScene( this object o )
	{
		return !(o is TextAsset) && o.ToString().Contains( "UnityEngine.SceneAsset" );
	}

	#region STRINGS
	public static string SplitIntoWordsByCase(this string inString)
	{
		StringBuilder builder = new StringBuilder();
		for(int i = 0; i < inString.Length; i++)
		{
			if (i > 0 && !Char.IsUpper(inString[i-1]) && Char.IsUpper(inString[i]))
				builder.Append(' ');

			builder.Append(inString[i]);
		}
		return builder.ToString();
	}

	public static string UppercaseFirst(this string s)
	{
		// Check for empty string.
		if (string.IsNullOrEmpty(s))
			return string.Empty;

		// Return char and concat substring.
		return char.ToUpper(s[0]) + s.Substring(1);
	}
	#endregion

	public static bool IsInstanceOfGenericType(Type genericType, object instance)
	{
		Type type = instance.GetType();
		while (type != null)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
				return true;

			type = type.BaseType;
		}
		return false;
	}

#if UNITY_EDITOR
	[MenuItem("Utilities/Shortcuts/Clear Console %#c")] // CTRL/CMD + SHIFT + C
	public static void ClearConsole()
	{
		try
		{
			var logEntries = Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
			if(logEntries != null)
			{
				var method = logEntries.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
				if(method != null)
					method.Invoke(null, null);
			}
		}
		catch(Exception exception)
		{
			Debug.LogError("Failed to clear the console: " + exception.ToString());
		}
	}
	
	public delegate void ApplyOrRevertDelegate(GameObject inInstance, UnityEngine.Object inPrefab, ReplacePrefabOptions inReplaceOptions);
	
	[MenuItem("Utilities/Shortcuts/Apply all selected prefabs")] // CTRL/CMD + SHIFT + E
	static void ApplyPrefabs()
	{
		var count = SearchPrefabConnections((inInstance, inPrefab, inReplaceOptions) =>
		{
			PrefabUtility.ReplacePrefab(inInstance, inPrefab, inReplaceOptions);
		}, "apply");

		if (count > 0)
			AssetDatabase.SaveAssets();
	}
	
	[MenuItem("Utilities/Shortcuts/Revert all selected prefabs &#r")] // ALT + SHIFT + R
	static void RevertPrefabs()
	{
		SearchPrefabConnections((inInstance, inPrefab, inReplaceOptions) =>
		{
			PrefabUtility.ReconnectToLastPrefab(inInstance);
			PrefabUtility.RevertPrefabInstance(inInstance);
		}, "revert");
	}
	
	static int SearchPrefabConnections(ApplyOrRevertDelegate inDelegate, string inDescriptor)
	{
		var count = 0;
		if(inDelegate != null)
		{
			var selectedGameObjects = Selection.gameObjects;
			if(selectedGameObjects.Length > 0)
			{
				foreach(var gameObject in selectedGameObjects)
				{
					var prefabType = PrefabUtility.GetPrefabType(gameObject);
					
					// Is the selected GameObject a prefab?
					if(prefabType == PrefabType.PrefabInstance || prefabType == PrefabType.DisconnectedPrefabInstance)
					{
						// Get the prefab root.
						var prefabParent = ((GameObject)PrefabUtility.GetPrefabParent(gameObject));
						var prefabRoot = prefabParent.transform.root.gameObject;
						
						var currentGameObject = gameObject;
						var hasFoundTopOfHierarchy = false;
						var canApply = true;
						
						// We go up in the hierarchy until we locate a GameObject that doesn't have the same GetPrefabParent return value.
						while(currentGameObject.transform.parent && !hasFoundTopOfHierarchy)
						{
							// Same prefab?
							prefabParent = ((GameObject)PrefabUtility.GetPrefabParent(currentGameObject.transform.parent.gameObject));
							if(prefabParent && prefabRoot == prefabParent.transform.root.gameObject)
							{
								// Continue upwards.
								currentGameObject = currentGameObject.transform.parent.gameObject;
							}
							else
							{
								//The gameobject parent is another prefab, we stop here
								hasFoundTopOfHierarchy = true;
								if(prefabRoot != ((GameObject)PrefabUtility.GetPrefabParent(currentGameObject)))
								{
									//Gameobject is part of another prefab
									canApply = false;
								}
							}
						}
						
						if(canApply)
						{
							count++;
							var parent = PrefabUtility.GetPrefabParent(currentGameObject);
							inDelegate(currentGameObject, parent, ReplacePrefabOptions.ConnectToPrefab);
							var assetPath = AssetDatabase.GetAssetPath(parent);
							Debug.Log(assetPath + " " + inDescriptor, parent);
						}
					}
				}
				Debug.Log(count + " prefab" + (count > 1 ? "s" : "") + " updated");
			}
		}
		
		return count;
	}
#endif
}

#region STRUCTS
[System.Serializable]
public struct FloatRange
{
	public float min;
	public float max;
	
	public FloatRange(float inMin, float inMax)
	{
		min = inMin;
		max = inMax;
	}

	public float Clamp( float v )
	{ return Mathf.Clamp( v, min, max ); }

	public int Clamp(int v)
	{ return Mathf.Clamp(v, (int)min, (int)max); }

	public float Lerp( float t )
	{ return Mathf.Lerp( min, max, t ); }

	public static FloatRange Lerp(FloatRange a, FloatRange b, float alpha)
	{
		a.min = Mathf.Lerp(a.min, b.min, alpha);
		a.max = Mathf.Lerp(a.max, b.max, alpha);
		return a;
	}

	public float UnclampedLerp( float t )
	{ return min + ( max - min ) * t; }

	public float InverseLerp( float value )
	{ return Mathf.InverseLerp(min, max, value);  }

	public float UnclampedInverseLerp( float value )
	{ return (value - min) / (max - min);  }

	public float Range
	{ get { return max - min; } }
	
	public float Random
	{ get { return UnityEngine.Random.Range( min, max ); } }
	
	public bool Contains( float v )
	{ return v > min && v < max; }

	public bool Contains(int v)
	{ return v > min && v < max; }

	public float Midpoint
	{ get { return ( min + max ) * 0.5f; } }

	public static FloatRange operator +(FloatRange a, FloatRange b)
	{
		a.min += b.min;
		a.max += b.max;
		return a;
	}

	public static FloatRange operator *(FloatRange floatRange, float x)
	{
		floatRange.min *= x;
		floatRange.max *= x;
		return floatRange;
	}
}

[System.Serializable]
public struct IntRange
{
	public int min;
	public int max;

	public IntRange(int inMin, int inMax)
	{
		min = inMin;
		max = inMax;
	}

	public float Clamp(float v)
	{ return Mathf.Clamp(v, min, max); }

	public int Clamp(int v)
	{ return Mathf.Clamp(v, min, max); }

	public float Lerp(float t)
	{ return Mathf.Lerp(min, max, t); }

	public float UnclampedLerp(float t)
	{ return min + (max - min) * t; }

	public float InverseLerp(float value)
	{ return Mathf.InverseLerp(min, max, value); }

	public float UnclampedInverseLerp(float value)
	{ return (value - min) / (max - min); }

	public int Range
	{ get { return max - min; } }

	public int Random
	{ get { return UnityEngine.Random.Range(min, max); } }

	public bool Contains(float v)
	{ return v > min && v < max; }

	public bool Contains(int v)
	{ return v > min && v < max; }

	public float Midpoint
	{ get { return (min + max) * 0.5f; } }

	public static IntRange operator +(IntRange a, IntRange b)
	{
		a.min += b.min;
		a.max += b.max;
		return a;
	}

	public static IntRange operator *(IntRange intRange, int x)
	{
		intRange.min *= x;
		intRange.max *= x;
		return intRange;
	}
}

[System.Serializable]
public struct Vec2Range
{
	public Vector2 min;
	public Vector2 max;

	public Vec2Range(Vector2 _min, Vector2 _max)
	{
		min = _min;
		max = _max;
	}

	public Vector2 Clamp(Vector2 v)
	{ return WadeUtils.Clamp(v, min, max); }

	public Vector2 Lerp(float t)
	{ return Vector2.Lerp(min, max, t); }

	public Vector2 UnclampedLerp(float t)
	{ return min + (max - min) * t; }

	public Vector2 Range
	{ get { return max - min; } }

	public Vector2 Random
	{
		get
		{
			return new Vector2(UnityEngine.Random.Range(min.x, max.x),
				UnityEngine.Random.Range(min.y, max.y));
		}
	}

	public bool Contains(Vector2 v)
	{
		return v.x > min.x && v.x < max.x &&
			v.y > min.y && v.y < max.y;
	}

	public Vector2 Midpoint
	{ get { return (min + max) * 0.5f; } }

	public static Vec2Range operator +(Vec2Range a, Vec2Range b)
	{
		a.min += b.min;
		a.max += b.max;
		return a;
	}

	public static Vec2Range operator *(Vec2Range floatRange, float x)
	{
		floatRange.min *= x;
		floatRange.max *= x;
		return floatRange;
	}

	public static Vec2Range Lerp(Vec2Range a, Vec2Range b, float alpha)
	{
		a.min = Vector2.Lerp(a.min, b.min, alpha);
		a.max = Vector2.Lerp(a.max, b.max, alpha);
		return a;
	}
}

[System.Serializable]
public struct Vec3Range
{
	public Vector3 min;
	public Vector3 max;

	public Vec3Range(Vector3 _min, Vector3 _max)
	{
		min = _min;
		max = _max;
	}

	public Vector3 Clamp(Vector3 v)
	{ return WadeUtils.Clamp(v, min, max); }

	public Vector3 Lerp(float t)
	{ return Vector3.Lerp(min, max, t); }

	public Vector3 UnclampedLerp(float t)
	{ return min + (max - min) * t; }

	public Vector3 Range
	{ get { return max - min; } }

	public Vector3 Random
	{
		get
		{
			return new Vector3(UnityEngine.Random.Range(min.x, max.x),
				UnityEngine.Random.Range(min.y, max.y),
				UnityEngine.Random.Range(min.z, max.z));
		}
	}

	public bool Contains(Vector3 v)
	{
		return v.x > min.x && v.x < max.x &&
			v.y > min.y && v.y < max.y &&
			v.z > min.z && v.z < max.z;
	}

	public Vector3 Midpoint
	{ get { return (min + max) * 0.5f; } }

	public static Vec3Range operator +(Vec3Range a, Vec3Range b)
	{
		a.min += b.min;
		a.max += b.max;
		return a;
	}

	public static Vec3Range operator *(Vec3Range floatRange, float x)
	{
		floatRange.min *= x;
		floatRange.max *= x;
		return floatRange;
	}

	public static Vec3Range Lerp(Vec3Range a, Vec3Range b, float alpha)
	{
		a.min = Vector3.Lerp(a.min, b.min, alpha);
		a.max = Vector3.Lerp(a.max, b.max, alpha);
		return a;
	}
}

[System.Serializable]
public struct Vec4Range
{
	public Vector4 min;
	public Vector4 max;

	public Vec4Range(Vector4 _min, Vector4 _max)
	{
		min = _min;
		max = _max;
	}

	public Vector4 Clamp(Vector4 v)
	{ return WadeUtils.Clamp(v, min, max); }

	public Vector4 Lerp(float t)
	{ return Vector4.Lerp(min, max, t); }

	public Vector4 UnclampedLerp(float t)
	{ return min + (max - min) * t; }

	public Vector4 Range
	{ get { return max - min; } }

	public Vector4 Random
	{
		get
		{
			return new Vector4(UnityEngine.Random.Range(min.x, max.y),
				UnityEngine.Random.Range(min.y, max.y),
				UnityEngine.Random.Range(min.z, max.z),
				UnityEngine.Random.Range(min.w, max.w));
		}
	}

	public bool Contains(Vector4 v)
	{
		return v.x > min.x && v.x < max.x &&
			v.y > min.y && v.y < max.y &&
			v.z > min.z && v.z < max.z &&
			v.w > min.w && v.w < max.w;
	}

	public Vector4 Midpoint
	{ get { return (min + max) * 0.5f; } }

	public static Vec4Range operator +(Vec4Range a, Vec4Range b)
	{
		a.min += b.min;
		a.max += b.max;
		return a;
	}

	public static Vec4Range operator *(Vec4Range floatRange, float x)
	{
		floatRange.min *= x;
		floatRange.max *= x;
		return floatRange;
	}

	public static Vec4Range Lerp(Vec4Range a, Vec4Range b, float alpha)
	{
		a.min = Vector4.Lerp(a.min, b.min, alpha);
		a.max = Vector4.Lerp(a.max, b.max, alpha);
		return a;
	}
}

[System.Serializable]
public struct AllowInt
{
	public AllowInt(bool inCanUse, int inValue)
	{
		canUse = inCanUse;
		value = inValue;
	}

	public bool canUse;
	public int value;
}

[System.Serializable]
public struct AllowFloat
{
	public AllowFloat(bool inCanUse, float inValue)
	{
		canUse = inCanUse;
		value = inValue;
	}

	public bool canUse;
	public float value;
}

[System.Serializable]
public struct AllowVector2
{
	public AllowVector2(bool inCanUse, Vector2 inValue)
	{
		canUse = inCanUse;
		value = inValue;
	}

	public bool canUse;
	public Vector2 value;
}

[System.Serializable]
public struct AllowVector3
{
	public AllowVector3(bool inCanUse, Vector3 inValue)
	{
		canUse = inCanUse;
		value = inValue;
	}

	public bool canUse;
	public Vector3 value;
}

[System.Serializable]
public struct AllowVector4
{
	public AllowVector4(bool inCanUse, Vector4 inValue)
	{
		canUse = inCanUse;
		value = inValue;
	}

	public bool canUse;
	public Vector4 value;
}

#endregion

#region COLORS
[System.Serializable]
public struct HSVColor
{
	public float h;
	public float s;
	public float v;
	public float a;
	
	public HSVColor( float h, float s, float v, float a = 1f)
	{
		this.h = h;
		this.s = s;
		this.v = v;
		this.a = a;
	}
	
	public HSVColor( Color col )
	{
		HSVColor temp = WadeUtils.RGBToHSV( col );
		h = temp.h;
		s = temp.s;
		v = temp.v;
		a = temp.a;
	}
	
	public Color HSVToRGB()
	{
		return WadeUtils.HSVToRGB(this);
	}

	public override string ToString()
	{
		return "H:" + h + " S:" + s + " V:" + v;
	}
	
	public static HSVColor Lerp( HSVColor a, HSVColor b, float t )
	{
		float h,s;
		
		//check special case black (color.v==0): interpolate neither hue nor saturation!
		//check special case grey (color.s==0): don't interpolate hue!
		if( WadeUtils.AreEqual( a.v, 0f ) )
		{
			h = b.h;
			s = b.s;
		}
		else if( WadeUtils.AreEqual( b.v, 0f ))
		{
			h = a.h;
			s = a.s;
		}
		else
		{
			if( WadeUtils.AreEqual( a.s, 0f ) )
			{
				h = b.h;
			}
			else if( WadeUtils.AreEqual( b.s, 0f ) )
			{
				h = a.h;
			}
			else
			{
				// works around bug with LerpAngle
				float angle = Mathf.LerpAngle( a.h * 360f, b.h * 360f, t );
				while (angle < 0f)
				{
					angle += 360f;
				}
				while (angle > 360f)
				{
					angle = 360f;
				}
				
				h = angle/360f;
			}
			
			s = Mathf.Lerp( a.s, b.s, t );
		}
		
		return new HSVColor( h, s, Mathf.Lerp( a.v, b.v, t ), Mathf.Lerp( a.a, b.a, t ) );
	}
}

[System.Serializable]
public struct LABColor
{

	// This script provides a Lab color space in addition to Unity's built in Red/Green/Blue colors.
	// Lab is based on CIE XYZ and is a color-opponent space with L for lightness and a and b for the color-opponent dimensions.
	// Lab color is designed to approximate human vision and so it aspires to perceptual uniformity.
	// The L component closely matches human perception of lightness.
	// Put LABColor.cs in a 'Plugins' folder to ensure that it is accessible to other scripts.

	private float L;
	private float A;
	private float B;

	// lightness accessors
	public float l
	{
		get { return this.L; }
		set { this.L = value; }
	}

	// a color-opponent accessor
	public float a
	{
		get { return this.A; }
		set { this.A = value; }
	}

	// b color-opponent accessor
	public float b
	{
		get { return this.B; }
		set { this.B = value; }
	}

	// constructor - takes three floats for lightness and color-opponent dimensions
	public LABColor(float l, float a, float b)
	{
		this.L = l;
		this.A = a;
		this.B = b;
	}

	// constructor - takes a Color
	public LABColor(Color col)
	{
		LABColor temp = FromColor(col);
		L = temp.l;
		A = temp.a;
		B = temp.b;
	}

	// static function for linear interpolation between two LABColors
	public static LABColor Lerp(LABColor a, LABColor b, float t)
	{
		return new LABColor(Mathf.Lerp(a.l, b.l, t), Mathf.Lerp(a.a, b.a, t), Mathf.Lerp(a.b, b.b, t));
	}

	// static function for interpolation between two Unity Colors through normalized colorspace
	public static Color Lerp(Color a, Color b, float t)
	{
		return (LABColor.Lerp(LABColor.FromColor(a), LABColor.FromColor(b), t)).ToColor();
	}

	// static function for returning the color difference in a normalized colorspace (Delta-E)
	public static float Distance(LABColor a, LABColor b)
	{
		return Mathf.Sqrt(Mathf.Pow((a.l - b.l), 2f) + Mathf.Pow((a.a - b.a), 2f) + Mathf.Pow((a.b - b.b), 2f));
	}

	// static function for converting from Color to LABColor
	public static LABColor FromColor(Color c)
	{
		float D65x = 0.9505f;
		float D65y = 1.0f;
		float D65z = 1.0890f;
		float rLinear = c.r;
		float gLinear = c.g;
		float bLinear = c.b;
		float r = (rLinear > 0.04045f) ? Mathf.Pow((rLinear + 0.055f) / (1f + 0.055f), 2.2f) : (rLinear / 12.92f);
		float g = (gLinear > 0.04045f) ? Mathf.Pow((gLinear + 0.055f) / (1f + 0.055f), 2.2f) : (gLinear / 12.92f);
		float b = (bLinear > 0.04045f) ? Mathf.Pow((bLinear + 0.055f) / (1f + 0.055f), 2.2f) : (bLinear / 12.92f);
		float x = (r * 0.4124f + g * 0.3576f + b * 0.1805f);
		float y = (r * 0.2126f + g * 0.7152f + b * 0.0722f);
		float z = (r * 0.0193f + g * 0.1192f + b * 0.9505f);
		x = (x > 0.9505f) ? 0.9505f : ((x < 0f) ? 0f : x);
		y = (y > 1.0f) ? 1.0f : ((y < 0f) ? 0f : y);
		z = (z > 1.089f) ? 1.089f : ((z < 0f) ? 0f : z);
		LABColor lab = new LABColor(0f, 0f, 0f);
		float fx = x / D65x;
		float fy = y / D65y;
		float fz = z / D65z;
		fx = ((fx > 0.008856f) ? Mathf.Pow(fx, (1.0f / 3.0f)) : (7.787f * fx + 16.0f / 116.0f));
		fy = ((fy > 0.008856f) ? Mathf.Pow(fy, (1.0f / 3.0f)) : (7.787f * fy + 16.0f / 116.0f));
		fz = ((fz > 0.008856f) ? Mathf.Pow(fz, (1.0f / 3.0f)) : (7.787f * fz + 16.0f / 116.0f));
		lab.l = 116.0f * fy - 16f;
		lab.a = 500.0f * (fx - fy);
		lab.b = 200.0f * (fy - fz);
		return lab;
	}

	// static function for converting from LABColor to Color
	public static Color ToColor(LABColor lab)
	{
		float D65x = 0.9505f;
		float D65y = 1.0f;
		float D65z = 1.0890f;
		float delta = 6.0f / 29.0f;
		float fy = (lab.l + 16f) / 116.0f;
		float fx = fy + (lab.a / 500.0f);
		float fz = fy - (lab.b / 200.0f);
		float x = (fx > delta) ? D65x * (fx * fx * fx) : (fx - 16.0f / 116.0f) * 3f * (delta * delta) * D65x;
		float y = (fy > delta) ? D65y * (fy * fy * fy) : (fy - 16.0f / 116.0f) * 3f * (delta * delta) * D65y;
		float z = (fz > delta) ? D65z * (fz * fz * fz) : (fz - 16.0f / 116.0f) * 3f * (delta * delta) * D65z;
		float r = x * 3.2410f - y * 1.5374f - z * 0.4986f;
		float g = -x * 0.9692f + y * 1.8760f - z * 0.0416f;
		float b = x * 0.0556f - y * 0.2040f + z * 1.0570f;
		r = (r <= 0.0031308f) ? 12.92f * r : (1f + 0.055f) * Mathf.Pow(r, (1.0f / 2.4f)) - 0.055f;
		g = (g <= 0.0031308f) ? 12.92f * g : (1f + 0.055f) * Mathf.Pow(g, (1.0f / 2.4f)) - 0.055f;
		b = (b <= 0.0031308f) ? 12.92f * b : (1f + 0.055f) * Mathf.Pow(b, (1.0f / 2.4f)) - 0.055f;
		r = (r < 0) ? 0 : r;
		g = (g < 0) ? 0 : g;
		b = (b < 0) ? 0 : b;
		return new Color(r, g, b);
	}

	// function for converting an instance of LABColor to Color
	public Color ToColor()
	{
		return LABColor.ToColor(this);
	}

	// override for string
	public override string ToString()
	{
		return "L:" + l + " A:" + a + " B:" + b;
	}

	// are two LABColors the same?
	public override bool Equals(System.Object obj)
	{
		if (obj == null || GetType() != obj.GetType()) return false;
		return (this == (LABColor)obj);
	}

	// override hashcode for a LABColor
	public override int GetHashCode()
	{
		return l.GetHashCode() ^ a.GetHashCode() ^ b.GetHashCode();
	}

	// Equality operator
	public static bool operator ==(LABColor item1, LABColor item2)
	{
		return (item1.l == item2.l && item1.a == item2.a && item1.b == item2.b);
	}

	// Inequality operator
	public static bool operator !=(LABColor item1, LABColor item2)
	{
		return (item1.l != item2.l || item1.a != item2.a || item1.b != item2.b);
	}
}
#endregion