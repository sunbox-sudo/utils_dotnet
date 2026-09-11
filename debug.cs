namespace utils.debug;

/// <summary>
/// Debug class that print to scrren what error it gets
/// </summary>
public static class Debug
{
	/// <summary>
	/// Clears errors and log
	/// </summary>
	static public void Clear(){
		Console.Error.Flush();
		File.WriteAllText("/tmp/debug.log", "");
	}

	/// <summary>
	/// Print log <paramref name="message"/>
	/// </summary>
	/// <param name="message"> The log measage </param>
	static public void Log(string message){
		Console.ForegroundColor = ConsoleColor.White;
		Console.Error.WriteLine($"[DEBUG] {message}");
		Console.ResetColor();
	}

	/// <summary>
	/// Print Warning <paramref name="message"/>
	/// </summary>
	/// <param name="message"> The warninn message</param>
	static public void Warn(string message){
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.Error.WriteLine($"[WARN] {message}");
		Console.ResetColor();
	}

	/// <summary>
	/// Print error <paramref name="message"/>
	/// </summary>
	/// <param name="message"> The Error measage</param>
	static public void Error(string message){
		Console.ForegroundColor = ConsoleColor.Red;
		Console.Error.WriteLine($"[ERROR] {message}");
		Console.ResetColor();
	}

	/// <summary>
	/// Print crtical error and terminate procces <paramref name="message"/>
	/// </summary>
	/// <param name="message"> The Critalcal error mesage</param>
	static public void Critical(string message){
		Console.ForegroundColor = ConsoleColor.White;
		Console.BackgroundColor = ConsoleColor.Red;
		Console.Error.WriteLine($"[CRITICAL] {message}");
		Console.ResetColor();
		Environment.Exit(1);
	}
}
