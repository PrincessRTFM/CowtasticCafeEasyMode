#if DEBUG
#if TRACE
#define TRACEDEBUG
#endif
#endif

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PrincessRTFM.CowtasticCafeEasyMode.Logging;

internal static class Log {
	private static readonly string hiddenNamespace = typeof(Log).Namespace.ToLowerInvariant();
	internal static TextWriter? output = null;

	public static StackFrame[] GetStackTrace(int maxFrames = 0) {
		StackTrace trace = new();
		IEnumerable<StackFrame> frames = trace
			.GetFrames()
			.SkipWhile(f => f.GetMethod().DeclaringType.Namespace.ToLowerInvariant() == hiddenNamespace);
		if (maxFrames > 0)
			frames = frames.Take(maxFrames);
		return frames.ToArray();
	}

	public static void Write(LogLevel level, string message) {
		DateTime now = DateTime.Now;
		string time = $"{now.Hour:D2}:{now.Minute:D2}:{now.Second:D2}";
		string content = $"[{time}|{level.Abbreviate()}] {message}";
		Console.WriteLine(content);
		if (output is not null) {
			output.WriteLine(content);
			output.Flush();
		}
	}

	[Conditional("TRACEDEBUG")]
	public static void Trace(string message) => Write(LogLevel.TRACE, message);
	[Conditional("DEBUG")]
	public static void Debug(string message) => Write(LogLevel.DEBUG, message);
	public static void Info(string message) => Write(LogLevel.INFO, message);
	public static void Warn(string message) => Write(LogLevel.WARN, message);
	public static void Error(string message) => Write(LogLevel.ERROR, message);
	public static void Fatal(string message) {
		Write(LogLevel.FATAL, message);
		Environment.FailFast(message);
	}

	private static void traceMethod(string prefix) {
		StackFrame tracing = GetStackTrace(1)[0];
		MethodBase call = tracing.GetMethod();
		Trace($"{prefix} {Core.DescribeMethod(call)}");
	}
	public static void EnterMethod() => traceMethod("=>");
	public static void ExitMethod() => traceMethod("<=");
}
