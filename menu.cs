using utils.debug;
namespace utils.menu;

class Menu
{
	private string title;
	private List<string> options;
	private int selectedIndex;

	public Menu(string title){

		this.title = title;
		this.options = new List<string>();
		this.selectedIndex = 0;
	}

	public Menu(string title, List<string> options){

		this.title = title;
		this.options = new List<string>();
		this.selectedIndex = 0;
	}

	public void AddOption(string option){
		options.Add(option);
	}

	public bool RemoveOption(int index){
		if (index < 0 || index >= options.Count){
			Debug.Warn("Invalid index");
			return false;
		}
		options.RemoveAt(index);
		selectedIndex = Math.Min(selectedIndex, options.Count - 1);
		return true;
	}

	public void Draw(){
		Console.Clear();
		Console.WriteLine($"==={title}===\n");

		for (int i = 0; i < options.Count; i++){
			if (i == selectedIndex) {
				Console.BackgroundColor = ConsoleColor.Black;
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine($"  > {options[i]}");
				Console.ResetColor();
			}
			else {
				Console.WriteLine($"    {options[i]}");
			}
		}

		Console.WriteLine("\n[↑↓] Navigate   [Enter] Select   [Esc] Quit");
	}

	public int Run(){
		if (options.Count == 0){
			Console.WriteLine("Menu has no options");
			return -1;
		}

		while (true){
			Draw();
			ConsoleKey key = Console.ReadKey(intercept: true).Key;

			switch (key)
			{
				case ConsoleKey.K:
				case ConsoleKey.UpArrow:
					selectedIndex = Math.Max(0, selectedIndex - 1);
					break;
				case ConsoleKey.J:
				case ConsoleKey.DownArrow:
					selectedIndex = Math.Min(options.Count - 1, selectedIndex + 1);
					break;
				case ConsoleKey.Enter:
					return selectedIndex;
				case ConsoleKey.Escape:
					return -1;
			}
		}
	}
}
