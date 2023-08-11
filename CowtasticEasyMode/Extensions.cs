namespace PrincessRTFM.CowtasticCafeEasyMode;

using System;
using System.Linq;
using System.Reflection;

public static class Extensions {
	public static T? GetCustomAttribute<T>(this MemberInfo member) where T : Attribute {
		return member is null
			? throw new ArgumentNullException(nameof(member))
			: member
			.GetCustomAttributes(typeof(T), true)
			.FirstOrDefault() as T;
	}

	public static bool IsAssignableTo(this Type subclass, Type superclass) => superclass.IsAssignableFrom(subclass);

	public static string Label(this bool value) => value ? "enabled" : "disabled";
}
