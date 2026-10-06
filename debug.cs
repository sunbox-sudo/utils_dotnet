namespace utils.debug;

/// <summary>
/// Severity levels, orderd from least to most sevrere.
/// Order maters beacuse levles are compared as numbers
/// </summary>
public enum LogLevel{
	/// <summary> Verbose logging (level 0) </summary>
	Trace,
	/// <summary> general logging (level 1) </summary>
	Info,
	/// <summary> Something unexpeted, but program cna carry on (level 2) </summary>
	Warn,
	/// <summary> Something faild (level 3) </summary>
	Error,
	/// <summary> Fatal error, process is terminated. (level 4) </summary>
	Critical
}

/// <summary>
/// Logging class that by defult prints to stderr
/// message less sevrere than <see cref="MinLevel"/> are ignored
/// </summary>
public static class Debug {
	//init
	static private StreamWriter? _writer;

	// skinks (destinations)
	static public bool ToStderr 	{ get; set; } = true;
	static public bool ToStdout	{ get; set; } = false;
	static public bool ToFile 	{ get; set; } = true;

	// File options
	static public string LogFileName 	{ get; set; } = $"utils_dotnet.log";
	static public string LogFileDir 	{ get; set; } = Path.Combine(Path.GetTempPath(), ".utils_dotnet");
	static public bool AppendToFile 	{ get; set; } = false;
	// add option to create with date + aomunt / time or something.

	// general
	static public LogLevel MinLevel { get; set; } = LogLevel.Info;
	static public bool LogTime 	{ get; set; } = true;
	static public bool UseUtc 	{ get; set; } = true;

	// lambda methods for each Level.
	static public void Trace(string msg)		=> write(LogLevel.Trace,	msg, ConsoleColor.White, ConsoleColor.Black);
	static public void Info(string msg)		=> write(LogLevel.Info, 	msg, ConsoleColor.Green, ConsoleColor.Black);
	static public void Warn(string msg)		=> write(LogLevel.Warn, 	msg, ConsoleColor.Yellow, ConsoleColor.Black);
	static public void Error(string msg)		=> write(LogLevel.Error,	msg, ConsoleColor.Red, ConsoleColor.Black);
	static public void Critical(string msg)		=> write(LogLevel.Critical, 	msg, ConsoleColor.Black, ConsoleColor.DarkRed);


	static public void Start(){
		if (!AppendToFile){
			string path = Path.Combine(LogFileDir, LogFileName);
			if (!File.Exists(path)) return;
			_writer?.Dispose();

			_writer = new StreamWriter(path, append: false) { AutoFlush = true };
		}

	}

	static private void write(LogLevel tag, string msg, ConsoleColor fg, ConsoleColor bg){
		if (!(tag >= MinLevel)) return;

		// AI helped and gave me hints!
		string sTime = "";
		if (LogTime){
			DateTime now = UseUtc ? DateTime.UtcNow : DateTime.Now;
			sTime = UseUtc
				? $"{now.ToString("dd'/'MM' 'HH':'mm':'ss'.'fff'z'")} "
				: $"{now.ToString("dd'/'MM' 'HH':'mm':'ss'.'fffzzz")} ";
		}

		// Linus kod igen
		string data = $"[{sTime}{tag.ToString().ToUpper()}] {msg}";

		if (ToStderr) writeConsole(Console.Error, data, fg, bg);
		if (ToStdout) writeConsole(Console.Out, data, fg, bg);
		if (ToFile){
			if (_writer == null) {
				Directory.CreateDirectory(LogFileDir);
				string path = Path.Combine(LogFileDir, LogFileName);
				_writer = new StreamWriter(path, AppendToFile) {AutoFlush = true};
			}
			_writer.WriteLine(data);
		}
	}


	static private void writeConsole(TextWriter writer, string data, ConsoleColor fg, ConsoleColor bg){
		// I have known what Stderr and stdout was before this but in C / BASH (POSIX)
		// I got a bit of help by AI to understand how I do it in C#;
		// BUT I was never that good with it in borh C and BASH I was familiar.
		// file descriptors
		// 0 - stdin
		// 1 - stdout
		// 2 - stderr
		// 2<n - isn't os reservd

		// Solves so ANSI escape code dosn't leak into file
		// (ANSI escape codes is used for colorize terminals or manuplate the consoles state)
		bool redirected = ReferenceEquals(writer, Console.Error)
			? Console.IsErrorRedirected
			: Console.IsOutputRedirected;

		// print with no color
		if (redirected){
			writer.WriteLine(data);
			return;
		}

		Console.ForegroundColor = fg;
		Console.BackgroundColor = bg;
		writer.WriteLine(data);
		Console.ResetColor();
	}

}

