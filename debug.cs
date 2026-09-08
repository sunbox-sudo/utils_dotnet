namespace utils.debug;

static class Debug
{
	static public void Clear(){
		Console.Error.Flush();
		File.WriteAllText("/tmp/debug.log", "");
	}

	static public void Log(string message){
		Console.ForegroundColor = ConsoleColor.White;
		Console.Error.WriteLine($"[DEBUG] {message}");
		Console.ResetColor();
	}
	static public void Warn(string message){
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.Error.WriteLine($"[WARN] {message}");
		Console.ResetColor();
	}
	static public void Error(string message){
		Console.ForegroundColor = ConsoleColor.Red;
		Console.Error.WriteLine($"[ERROR] {message}");
		Console.ResetColor();
	}
	static public void Critical(string message){
		Console.ForegroundColor = ConsoleColor.White;
		Console.BackgroundColor = ConsoleColor.Red;
		Console.Error.WriteLine($"[CRITICAL] {message}");
		Console.ResetColor();
		Environment.Exit(1);
	}
}
