using utils.debug;
namespace utils.menu;

public class Menu
{
	private string title;
	private List<string> options;
	private int selectedIndex;

	/// <summary>
	/// Initializes a new instance of the <see cref="Menu"/> calss.
	/// </summary>
	/// <param name="title">The title of menu that user should see</param>
	public Menu(string title){

		this.title = title;
		this.options = new List<string>();
		this.selectedIndex = 0;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Menu"/> calss.
	/// </summary>
	/// <param name="title">The title of menu that user should see</param>
	/// <param name="options">The menu options list</param>
	public Menu(string title, List<string> options){

		this.title = title;
		this.options = new List<string>();
		this.selectedIndex = 0;
	}

	/// <summary>
	/// Adds string to menu
	/// </summary>
	/// <param name="option">The string added to menu</param>
	public void AddOption(string option){
		options.Add(option);
	}

	/// <summary>
	/// Removes a option at spesific index
	/// </summary>
	/// <param name="index"> The index that should be removed</param>
	public bool RemoveOption(int index){
		if (index < 0 || index >= options.Count){
			Debug.Warn("Invalid index");
			return false;
		}
		options.RemoveAt(index);
		selectedIndex = Math.Min(selectedIndex, options.Count - 1);
		return true;
	}

	void Draw(){
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

	/// <summary>
	/// Used to draw and get input from menu
	/// </summary>
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
