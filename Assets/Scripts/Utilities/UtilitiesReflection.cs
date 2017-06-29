#if NETFX_CORE
    #define USE_WP_REFLECTION
#endif

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using LinqTools;
using System.ComponentModel;
using UObject = UnityEngine.Object;

public static class UtilitiesReflection
{
    public static System.Reflection.Assembly GetAssembly(System.Type inType)
    {
#if USE_WP_REFLECTION
        return inType.GetTypeInfo().Assembly;
#else
        return inType.Assembly;
#endif
    }

    public static bool IsSubclassOf(System.Type inTargetReceiver, System.Type inTargetAttempt)
    {
#if USE_WP_REFLECTION
        return inTargetReceiver.GetTypeInfo().IsSubclassOf(inTargetAttempt);
#else
        return inTargetReceiver.IsSubclassOf(inTargetAttempt);
#endif
    }

    public static bool IsAssignableFrom(System.Type inAssignReceiver, System.Type inAssignAttempt)
    {
#if USE_WP_REFLECTION
        return inAssignReceiver.GetTypeInfo().IsAssignableFrom(inAssignAttempt.GetTypeInfo());
#else
        return inAssignReceiver.IsAssignableFrom(inAssignAttempt);
#endif
    }

    public static MethodInfo GetMethod(System.Type inType, string inMethodName)
    {
#if USE_WP_REFLECTION
        return inType.GetTypeInfo().GetDeclaredMethod(inMethodName);
#else
        return inType.GetMethod(inMethodName);
#endif
    }

    public static Type GetBaseType(Type inType)
    {
#if USE_WP_REFLECTION
        return inType.GetTypeInfo().BaseType;
#else
        return inType.BaseType;
#endif
    }

    public static IEnumerable<FieldInfo> GetFields(Type inType, BindingFlags inBindingFlags)
    {
#if USE_WP_REFLECTION
        return inType.GetTypeInfo().DeclaredFields;
#else
        return inType.GetFields(inBindingFlags);
#endif
    }

	public static IEnumerable<FieldInfo> GetAllFields(Type inType, BindingFlags inFlags)
	{
		if(inType != null)
		{
			inFlags |= BindingFlags.DeclaredOnly;
			return inType.GetFields(inFlags).Union(GetAllFields(inType.BaseType, inFlags));
		}
		else
		{
			return Enumerable.Empty<FieldInfo>();
		}
	}

    public static MemberInfo GetMember(Type inType, string inMethodName)
    {
#if USE_WP_REFLECTION
        var declaredMembers = inType.GetTypeInfo().DeclaredMembers;
        foreach(var member in declaredMembers)
        {
            if(member.Name == inMethodName)
                return member;
        }
        return default(MemberInfo);
#else
        return inType.GetMember(inMethodName).FirstOrDefault();
#endif
    }

    public static MemberInfo GetMember(Type inType, string inMethodName, BindingFlags inBindingFlags)
    {
#if USE_WP_REFLECTION
        var declaredMembers = inType.GetTypeInfo().DeclaredMembers;
        foreach(var member in declaredMembers)
        {
            if(member.Name == inMethodName)
                return member;
        }
        return default(MemberInfo);
#else
        return inType.GetMember(inMethodName, inBindingFlags).FirstOrDefault();
#endif
    }

    public static bool IsEnum(Type enumType)
    {
#if USE_WP_REFLECTION
        return enumType.GetTypeInfo().IsEnum;
#else
        return enumType.IsEnum;
#endif
    }

    public static bool IsInterface(Type type)
    {
#if USE_WP_REFLECTION
        return type.GetTypeInfo().IsInterface;
#else
        return type.IsInterface;
#endif
    }

    public static IEnumerable<Type> GetInterfaces(Type type)
    {
#if USE_WP_REFLECTION
        return type.GetTypeInfo().ImplementedInterfaces;
#else
        return type.GetInterfaces();
#endif
    }

    public static IEnumerable<PropertyInfo> GetProperties(Type type, BindingFlags flags)
    {
#if USE_WP_REFLECTION
        return type.GetTypeInfo().DeclaredProperties;
#else
        return type.GetProperties(flags);
#endif
    }

    public static IEnumerable<Type> GetTypesFromAssemblyWithType(Type type)
    {
#if USE_WP_REFLECTION
        return GetAssembly(type).DefinedTypes.Select(typeInfo => typeInfo.DeclaringType);
#else
        return GetAssembly(type).GetTypes();
#endif
    }

	public static IEnumerable<Type> GetTypesFromAssembly(Assembly inAssembly)
	{
		if (inAssembly == null)
			return null;
		return inAssembly.GetTypes();
	}

    public static bool HasCustomAttributes<T>(Type inQueryType, bool inInherit) where T : Attribute
    {
        return GetCustomAttributes<T>(inQueryType, inInherit).Count() > 0;
    }

     public static object[] GetCustomAttributes<T>(FieldInfo inFieldInfo, bool inInherit) where T : Attribute
    {
#if USE_WP_REFLECTION
        return new object[] { inFieldInfo.DeclaringType.GetTypeInfo().GetCustomAttribute<T>(inInherit) };
#else
        return inFieldInfo.GetCustomAttributes(typeof(T), inInherit);
#endif
    }

    public static object[] GetCustomAttributes<T>(MemberInfo inMemberInfo, bool inInherit) where T : Attribute
    {
#if USE_WP_REFLECTION
        return new object[] { inMemberInfo.DeclaringType.GetTypeInfo().GetCustomAttribute<T>(inInherit) };
#else
        return inMemberInfo.GetCustomAttributes(typeof(T), inInherit);
#endif
    }

    public static object[] GetCustomAttributes<T>(Type inQueryType, bool inInherit) where T : Attribute
    {
#if USE_WP_REFLECTION
        return new object[] { inQueryType.GetTypeInfo().GetCustomAttribute<T>(inInherit) };
#else
        return inQueryType.GetCustomAttributes(typeof(T), inInherit);
#endif
    }

    public static void CloneFields(object dest, object source)
	{
		if (dest.GetType()!= source.GetType())
			return;
		
		foreach(FieldInfo field in UtilitiesReflection.GetFields(dest.GetType(), BindingFlags.Public))
		{
			field.SetValue(dest, field.GetValue(source));
		}
	}

	public static IEnumerable<Assembly> GetAppDomainAssemblies()
	{
		return AppDomain.CurrentDomain.GetAssemblies();
	}
}