#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;
using LinqTools;
using System.Reflection;
using System.Security;

public static class EditorUtils
{
	public static bool Toggle(this SerializedObject so, string propName, string labelName, params GUILayoutOption[] options)
	{
		var prop = so.FindProperty(propName);
		prop.boolValue = EditorGUILayout.Toggle(labelName, prop.boolValue, options);
		return prop.boolValue;
	}

	public static bool Toggle(this SerializedObject so, string propName, params GUILayoutOption[] options)
	{
		var prop = so.FindProperty(propName);
		prop.boolValue = EditorGUILayout.Toggle(prop.boolValue, options);
		return prop.boolValue;
	}

	public static string Text(this SerializedObject so, string propName, string labelName, params GUILayoutOption[] options)
	{
		var prop = so.FindProperty(propName);
		prop.stringValue = EditorGUILayout.TextField(labelName, prop.stringValue, options);
		return prop.stringValue;
	}

	public static string Text(this SerializedObject so, string propName, params GUILayoutOption[] options)
	{
		var prop = so.FindProperty(propName);
		prop.stringValue = EditorGUILayout.TextField(prop.stringValue, options);
		return prop.stringValue;
	}

	public static float FloatField(this SerializedObject so, string propName, string labelName, params GUILayoutOption[] options)
	{
		var prop = so.FindProperty(propName);
		prop.floatValue = EditorGUILayout.FloatField(labelName, prop.floatValue, options);
		return prop.floatValue;
	}

	public static float FloatField(this SerializedObject so, string propName, params GUILayoutOption[] options)
	{
		var prop = so.FindProperty(propName);
		prop.floatValue = EditorGUILayout.FloatField(prop.floatValue, options);
		return prop.floatValue;
	}

	public static float IntField(this SerializedObject so, string propName, string labelName, params GUILayoutOption[] options)
	{
		var prop = so.FindProperty(propName);
		prop.intValue = EditorGUILayout.IntField(labelName, prop.intValue, options);
		return prop.intValue;
	}

	public static float IntField(this SerializedObject so, string propName, params GUILayoutOption[] options)
	{
		var prop = so.FindProperty(propName);
		prop.intValue = EditorGUILayout.IntField(prop.intValue, options);
		return prop.intValue;
	}

	public static Vector2 Vector2Field(this SerializedObject so, string propName, string labelName, params GUILayoutOption[] options)
	{
		var prop = so.FindProperty(propName);
		prop.vector2Value = EditorGUILayout.Vector2Field(labelName, prop.vector2Value, options);
		return prop.vector2Value;
	}

	public static Vector3 Vector3Field(this SerializedObject so, string propName, string labelName, params GUILayoutOption[] options)
	{
		var prop = so.FindProperty(propName);
		prop.vector3Value = EditorGUILayout.Vector3Field(labelName, prop.vector3Value, options);
		return prop.vector3Value;
	}

	public static Vector4 Vector4Field(this SerializedObject so, string propName, string labelName, params GUILayoutOption[] options)
	{
		var prop = so.FindProperty(propName);
		prop.vector4Value = EditorGUILayout.Vector4Field(labelName, prop.vector4Value, options);
		return prop.vector4Value;
	}

	public static int IntSlider(this SerializedObject so, string propName, string labelName, int leftValue, int rightValue, params GUILayoutOption[] options)
	{
		var prop = so.FindProperty(propName);
		prop.intValue = EditorGUILayout.IntSlider(labelName, prop.intValue, leftValue, rightValue, options);
		return prop.intValue;
	}

	public static int IntSlider(this SerializedObject so, string propName, int leftValue, int rightValue, params GUILayoutOption[] options)
	{
		var prop = so.FindProperty(propName);
		prop.intValue = EditorGUILayout.IntSlider(prop.intValue, leftValue, rightValue, options);
		return prop.intValue;
	}

	public static int EnumPopup<T>(this SerializedObject so, string propName, string labelName, params GUILayoutOption[] options)
	{
		var prop = so.FindProperty(propName);
		var enumValue = (System.Enum)System.Enum.ToObject(typeof(T), prop.intValue);
		prop.intValue = (int)((object)EditorGUILayout.EnumPopup(labelName, enumValue, options));
		return prop.intValue;
	}

	public static int EnumPopup<T>(this SerializedObject so, string propName, params GUILayoutOption[] options)
	{
		var prop = so.FindProperty(propName);
		var enumValue = (System.Enum)System.Enum.ToObject(typeof(T), prop.intValue);
		prop.intValue = (int)((object)EditorGUILayout.EnumPopup(enumValue, options));
		return prop.intValue;
	}

	public static bool PropertyField(this SerializedObject so, string propName, bool includeChildren = false, params GUILayoutOption[] options)
	{
		var prop = so.FindProperty(propName);
		return EditorGUILayout.PropertyField(prop, includeChildren, options);
	}

	public static T ObjectField<T>(this SerializedObject so, string propName, string labelName, bool allowSceneObjects, params GUILayoutOption[] layoutOptions) where T : Object
	{
		var prop = so.FindProperty(propName);
		prop.objectReferenceValue = EditorGUILayout.ObjectField(labelName,
			prop.objectReferenceValue, typeof(T), allowSceneObjects, layoutOptions);
		
		return (T)prop.objectReferenceValue;
	}

	public static T ObjectField<T>(this SerializedObject so, string propName, bool allowSceneObjects, params GUILayoutOption[] layoutOptions) where T : Object
	{
		var prop = so.FindProperty(propName);
		prop.objectReferenceValue = EditorGUILayout.ObjectField(prop.objectReferenceValue,
			typeof(T), allowSceneObjects, layoutOptions);

		return (T)prop.objectReferenceValue;
	}

	public static void SetValue(this SerializedProperty thisSP, SerializedProperty otherSP)
	{
		switch (thisSP.propertyType)
		{
			case SerializedPropertyType.Integer:
				thisSP.intValue = otherSP.intValue;
				break;
			case SerializedPropertyType.Boolean:
				thisSP.boolValue = otherSP.boolValue;
				break;
			case SerializedPropertyType.Float:
				thisSP.floatValue = otherSP.floatValue;
				break;
			case SerializedPropertyType.String:
				thisSP.stringValue = otherSP.stringValue;
				break;
			case SerializedPropertyType.Color:
				thisSP.colorValue = otherSP.colorValue;
				break;
			case SerializedPropertyType.ObjectReference:
				thisSP.objectReferenceValue = otherSP.objectReferenceValue;
				break;
			case SerializedPropertyType.LayerMask:
				thisSP.intValue = otherSP.intValue;
				break;
			case SerializedPropertyType.Enum:
				thisSP.enumValueIndex = otherSP.enumValueIndex;
				break;
			case SerializedPropertyType.Vector2:
				thisSP.vector2Value = otherSP.vector2Value;
				break;
			case SerializedPropertyType.Vector3:
				thisSP.vector3Value = otherSP.vector3Value;
				break;
			case SerializedPropertyType.Vector4:
				thisSP.vector4Value = otherSP.vector4Value;
				break;
			case SerializedPropertyType.Rect:
				thisSP.rectValue = otherSP.rectValue;
				break;
			case SerializedPropertyType.ArraySize:
				thisSP.intValue = otherSP.intValue;
				break;
			case SerializedPropertyType.Character:
				thisSP.intValue = otherSP.intValue;
				break;
			case SerializedPropertyType.AnimationCurve:
				thisSP.animationCurveValue = otherSP.animationCurveValue;
				break;
			case SerializedPropertyType.Bounds:
				thisSP.boundsValue = otherSP.boundsValue;
				break;
			default:
				SerializedProperty thisMin = thisSP.FindPropertyRelative("min");
				SerializedProperty thisMax = thisSP.FindPropertyRelative("max");
				SerializedProperty otherMin = otherSP.FindPropertyRelative("min");
				SerializedProperty otherMax = otherSP.FindPropertyRelative("max");
				SerializedProperty thisCanUse = thisSP.FindPropertyRelative("canUse");
				SerializedProperty thisValue = thisSP.FindPropertyRelative("value");
				SerializedProperty otherCanUse = otherSP.FindPropertyRelative("canUse");
				SerializedProperty otherValue = otherSP.FindPropertyRelative("value");
				switch (thisSP.type)
				{
					case "FloatRange":
						thisMin.floatValue = otherMin.floatValue;
						thisMax.floatValue = otherMax.floatValue;
						break;
					case "IntRange":
						thisMin.intValue = otherMin.intValue;
						thisMax.intValue = otherMax.intValue;
						break;
					case "Vec2Range":
						thisMin.vector2Value = otherMin.vector2Value;
						thisMax.vector2Value = otherMax.vector2Value;
						break;
					case "Vec3Range":
						thisMin.vector3Value = otherMin.vector3Value;
						thisMax.vector3Value = otherMax.vector3Value;
						break;
					case "Vec4Range":
						thisMin.vector4Value = otherMin.vector4Value;
						thisMax.vector4Value = otherMax.vector4Value;
						break;
					case "AllowInt":
						thisCanUse.boolValue = otherCanUse.boolValue;
						thisValue.intValue = otherValue.intValue;
						break;
					case "AllowFloat":
						thisCanUse.boolValue = otherCanUse.boolValue;
						thisValue.floatValue = otherValue.floatValue;
						break;
					case "AllowVector2":
						thisCanUse.boolValue = otherCanUse.boolValue;
						thisValue.vector2Value = otherValue.vector2Value;
						break;
					case "AllowVector3":
						thisCanUse.boolValue = otherCanUse.boolValue;
						thisValue.vector3Value = otherValue.vector3Value;
						break;
					case "AllowVector4":
						thisCanUse.boolValue = otherCanUse.boolValue;
						thisValue.vector4Value = otherValue.vector4Value;
						break;
					default:
						break;
				}
				break;
		}
	}

	public static bool Compare(this SerializedProperty thisSP, SerializedProperty otherSP)
	{
		switch (thisSP.propertyType)
		{
			case SerializedPropertyType.Integer:
				return thisSP.intValue == otherSP.intValue;
			case SerializedPropertyType.Boolean:
				return thisSP.boolValue == otherSP.boolValue;
			case SerializedPropertyType.Float:
				return Mathf.Abs(thisSP.floatValue - otherSP.floatValue) < Mathf.Epsilon;
			case SerializedPropertyType.String:
				return thisSP.stringValue == otherSP.stringValue;
			case SerializedPropertyType.Color:
				return thisSP.colorValue.Equals(otherSP.colorValue);
			case SerializedPropertyType.ObjectReference:
				return thisSP.objectReferenceValue.Equals(otherSP.objectReferenceValue);
			case SerializedPropertyType.LayerMask:
				return thisSP.intValue == otherSP.intValue;
			case SerializedPropertyType.Enum:
				return thisSP.enumValueIndex == otherSP.enumValueIndex;
			case SerializedPropertyType.Vector2:
				return (thisSP.vector2Value - otherSP.vector2Value).magnitude < Mathf.Epsilon;
			case SerializedPropertyType.Vector3:
				return (thisSP.vector3Value - otherSP.vector3Value).magnitude < Mathf.Epsilon;
			case SerializedPropertyType.Vector4:
				return (thisSP.vector4Value - otherSP.vector4Value).magnitude < Mathf.Epsilon;
			case SerializedPropertyType.Rect:
				return thisSP.rectValue == otherSP.rectValue;
			case SerializedPropertyType.ArraySize:
				return thisSP.intValue == otherSP.intValue;
			case SerializedPropertyType.Character:
				return (char)thisSP.intValue == (char)otherSP.intValue;
			case SerializedPropertyType.AnimationCurve:
				AnimationCurve compareCurve = (AnimationCurve)otherSP.animationCurveValue;
				if (thisSP.animationCurveValue.keys.Length != compareCurve.keys.Length)
					return false;
				else
				{
					for (int i = 0; i < compareCurve.keys.Length; i++)
					{
						if (!thisSP.animationCurveValue.keys[i].Equals(compareCurve.keys[i]))
							return false;
					}
				}
				return true;
			case SerializedPropertyType.Bounds:
				return thisSP.boundsValue == otherSP.boundsValue;
			case SerializedPropertyType.Gradient:
				return SafeGradientValue(thisSP) == SafeGradientValue(otherSP);
			case SerializedPropertyType.Quaternion:
				return thisSP.quaternionValue == otherSP.quaternionValue;
			default:
				SerializedProperty thisMin = thisSP.FindPropertyRelative("min");
				SerializedProperty thisMax = thisSP.FindPropertyRelative("max");
				SerializedProperty otherMin = otherSP.FindPropertyRelative("min");
				SerializedProperty otherMax = otherSP.FindPropertyRelative("max");
				SerializedProperty thisCanUse = thisSP.FindPropertyRelative("canUse");
				SerializedProperty thisValue = thisSP.FindPropertyRelative("value");
				SerializedProperty otherCanUse = otherSP.FindPropertyRelative("canUse");
				SerializedProperty otherValue = otherSP.FindPropertyRelative("value");
				switch (thisSP.type)
				{
					case "IntRange":
						return thisMin.intValue == otherMin.intValue &&
							thisMax.intValue == otherMax.intValue;
					case "FloatRange":
						return Mathf.Abs(thisMin.floatValue - otherMin.floatValue) < Mathf.Epsilon &&
							 Mathf.Abs(thisMax.floatValue - otherMax.floatValue) < Mathf.Epsilon;
					case "Vec2Range":
						return (thisMin.vector2Value - otherMin.vector2Value).magnitude < Mathf.Epsilon &&
							(thisMax.vector2Value - otherMax.vector2Value).magnitude < Mathf.Epsilon;
					case "Vec3Range":
						return (thisMin.vector3Value - otherMin.vector3Value).magnitude < Mathf.Epsilon &&
							(thisMax.vector3Value - otherMax.vector3Value).magnitude < Mathf.Epsilon;
					case "Vec4Range":
						return (thisMin.vector4Value - otherMin.vector4Value).magnitude < Mathf.Epsilon &&
							(thisMax.vector4Value - otherMax.vector4Value).magnitude < Mathf.Epsilon;
					case "AllowInt":
						return thisCanUse.boolValue == otherCanUse.boolValue &&
							thisValue.intValue == otherValue.intValue;
					case "AllowFloat":
						return thisCanUse.boolValue == otherCanUse.boolValue &&
							Mathf.Abs(thisValue.floatValue - otherValue.floatValue) < Mathf.Epsilon;
					case "AllowVector2":
						return thisCanUse.boolValue == otherCanUse.boolValue &&
							(thisValue.vector2Value - otherValue.vector2Value).magnitude < Mathf.Epsilon;
					case "AllowVector3":
						return thisCanUse.boolValue == otherCanUse.boolValue &&
							(thisValue.vector3Value - otherValue.vector3Value).magnitude < Mathf.Epsilon;
					case "AllowVector4":
						return thisCanUse.boolValue == otherCanUse.boolValue &&
							(thisValue.vector4Value - otherValue.vector4Value).magnitude < Mathf.Epsilon;
					default:
						return false;
				}
		}
	}

			/// Access to SerializedProperty's internal gradientValue property getter, in a manner that'll only soft break (returning null) if the property changes or disappears in future Unity revs.
	static Gradient SafeGradientValue(SerializedProperty sp)
	{
		BindingFlags instanceAnyPrivacyBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
		PropertyInfo propertyInfo = typeof(SerializedProperty).GetProperty(
			"gradientValue",
			instanceAnyPrivacyBindingFlags,
			null,
			typeof(Gradient),
			new System.Type[0],
			null
		);
		if (propertyInfo == null)
			return null;

		Gradient gradientValue = propertyInfo.GetValue(sp, null) as Gradient;
		return gradientValue;
	}

	public static Texture2D GetTexture()
	{
		Texture2D bgTexture = new Texture2D(1, 1);
		bgTexture.SetPixel(0, 0, Color.white);
		bgTexture.Apply();

		return bgTexture;
	}

	public static Texture2D GetColoredTexture(Color color)
	{
		Texture2D bgTexture = new Texture2D(1, 1);
		bgTexture.SetPixel(0, 0, color);
		bgTexture.Apply();

		return bgTexture;
	}
}
#endif