using UnityEngine;
using System.Collections;

public enum CompareType
{
	Greater,
	GEqual,
	Less,
	LEqual,
	Equal,
	NotEqual
}

public enum CompareDataType
{
	Int,
	Float,
	Bool
}

public static class CompareUtils
{
	public static bool CompareTo(this int compareTo, CompareType compareType, int value)
	{
		switch (compareType)
		{
			case CompareType.Greater:
				return value > compareTo;
			case CompareType.GEqual:
				return value >= compareTo;
			case CompareType.Less:
				return value < compareTo;
			case CompareType.LEqual:
				return value <= compareTo;
			case CompareType.Equal:
				return value == compareTo;
			case CompareType.NotEqual:
				return value != compareTo;
			default:
				return false;
		}
	}

	public static bool CompareTo(this float compareTo, CompareType compareType, float value)
	{
		switch (compareType)
		{
			case CompareType.Greater:
				return value > compareTo;
			case CompareType.GEqual:
				return value >= compareTo;
			case CompareType.Less:
				return value < compareTo;
			case CompareType.LEqual:
				return value <= compareTo;
			case CompareType.Equal:
				return Mathf.Abs(compareTo - value) < Mathf.Epsilon;
			case CompareType.NotEqual:
				return Mathf.Abs(compareTo - value) > Mathf.Epsilon;
			default:
				return false;
		}
	}

	public static bool CompareTo(this bool compareTo, CompareType compareType, bool value)
	{
		switch (compareType)
		{
			case CompareType.Equal:
				return value == compareTo;
			case CompareType.NotEqual:
				return value != compareTo;
			default:
				return false;
		}
	}
}
