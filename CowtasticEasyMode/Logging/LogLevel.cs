namespace PrincessRTFM.CowtasticCafeEasyMode.Logging;

public enum LogLevel: byte {
	TRACE = 0,
	DEBUG = 1,
	INFO = 2,
	WARN = 3,
	ERROR = 4,
	FATAL = 5,
}

public static class LogLevelExtensions {
	public static string Abbreviate(this LogLevel level) {
		return (level switch {
			LogLevel.TRACE => "TRC",
			LogLevel.DEBUG => "DBG",
			LogLevel.INFO => "INF",
			LogLevel.WARN => "WRN",
			LogLevel.ERROR => "ERR",
			LogLevel.FATAL => "DIE",
			_ => level.ToString(),
		}).ToUpper();
	}
}
