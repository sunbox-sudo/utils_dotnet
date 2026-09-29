using utils.debug;

namespace utils.menu;

/// <summary>
/// The <see langword="abstract"/> class for menus.
/// Only used for inheritance.
/// </summary>
/// <remarks>
/// Constructor is <see langword="internal"/>.
/// That means only code in the same assembly can inherit from it.
/// </remarks>
public abstract class Menu{
	string displayName;
	internal int index = 0;
	int maxIndex; // BUG !!! Used, but never assigned accurate data.
	bool isRunning = false; // Used to keep control of the menu so it doesn't loop indefinitely.

	internal Menu(string displayName){
		this.displayName = displayName;
	}

	/// <summary>
	/// Menu's main function. Used to get the user to select an option or exit.
	/// </summary>
	/// <remarks>
	/// Is <see langword="virtual"/>, so other menus can override it.
	/// </remarks>
	public virtual void Run(){
		// Enable isRunning. It gets disabled when the menu is done, so the menu can stop.
		isRunning = true;
		while (isRunning){
			draw();
			input();
		}
	}

	/// <summary>
	/// Gets and handles the input from the user.
	/// </summary>
	internal virtual void input(){
		ConsoleKey key = Console.ReadKey(intercept: true).Key;

		switch (key){
			case ConsoleKey.K:
			case ConsoleKey.UpArrow:
				index = Math.Max(0, index - 1);
				break;
			case ConsoleKey.J:
			case ConsoleKey.DownArrow:
				index = Math.Min(maxIndex, index + 1);
				break;
			case ConsoleKey.Enter:
				Debug.Log("menu.input. option selected " + index);
				isRunning = false;
				break;
			case ConsoleKey.Escape:
				isRunning = false;
				Debug.Log($"menu.input. User exited from menu.");
				break;
			default:
				Debug.Error("I DON'T EVEN KNOW HOW YOU ENDED UP HERE. " +
						"inform us about this, " +
						"and the steps you took to get here");
				break;
		}
	}

	/// <summary>
	/// Draws the menu.
	/// Other classes override this to draw their own menu.
	/// </summary>
	internal virtual void draw(){
		// Just used for other classes to override. Do not use on its own.
		Debug.Error("YOU SHOULDN'T BE HERE. Virtual menu.draw");
	}

	/// <summary>
	/// Draws the top part of the UI.
	/// </summary>
	internal void drawTop(){
		Console.Clear();
		Console.WriteLine($"===={displayName}====");
	}

	/// <summary>
	/// Draws the bottom part of the UI.
	/// </summary>
	internal void drawBottom(){
		Console.WriteLine("\n[↑↓] Navigate   [Enter] Select   [Esc] Quit");
	}
}

/// <summary>
/// Fixed menu is a menu that is fixed. Length is not changeable.
/// Data could be changed, but not added or removed.
/// </summary>
/// <remarks>
/// Fixed menus are built on arrays.
/// Perfect for e.g. main menu or other fixed menus.
/// </remarks>
public class FixedMenu : Menu{
	readonly MenuItem[] opts;
	int optsCount;

	/// <summary>
	/// Initializes a fixed menu with <paramref name="displayName"/>, and size <paramref name="optsCount"/>.
	/// </summary>
	/// <param name="displayName">The display name that shows for the user.</param>
	/// <param name="optsCount">The menu size (not changeable).</param>
	public FixedMenu(string displayName, int optsCount) : base(displayName){
		this.opts = new MenuItem[optsCount];
	}

	/// <summary>
	/// Initializes a new instance of the menu, with <paramref name="displayName"/>
	/// and the <paramref name="opts"/>.
	/// </summary>
	/// <param name="displayName">The display name that shows for the user.</param>
	/// <param name="opts">The menu item(s). Could change but not length.</param>
	public FixedMenu(string displayName, params MenuItem[] opts) : base(displayName){
		this.opts = opts;
	}

	/// <inheritdoc/>
	internal override void draw(){
		// draw top part
		drawTop();

		// main draw function
		for (int i = 0; i < opts.Length; i++){
			// skip draw if it's hidden
			if (opts[i].IsHidden) continue;

			// Changes the color if it's the selected index
			if (i == index){
				Console.BackgroundColor = ConsoleColor.Black;
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine($"{opts[i].Name}");
				Console.ResetColor();
				continue;
			}
			// Otherwise
			Console.WriteLine($"{opts[i].Name}");
		}
		// draw bottom
		drawBottom();
	}
}

/// <summary>
/// Dynamic menu is a menu that is dynamic.
/// Menu can change data, size, etc.
/// </summary>
/// <remarks>
/// Dynamic menus are built on List.
/// Perfect for e.g. users menu, or other dynamic menus.
/// </remarks>
public class DynamicMenu : Menu{
	List<MenuItem> opts = new();
	/// <summary>
	/// Initializes a dynamic menu with <paramref name="displayName"/>, with the menu items <paramref name="opts"/>.
	/// </summary>
	/// <param name="displayName">The display name that shows for the user.</param>
	/// <param name="opts">The menu item(s).</param>
	public DynamicMenu(string displayName, params List<MenuItem> opts) : base(displayName){
		this.opts = opts;
	}

	/// <inheritdoc/>
	internal override void draw(){
		// draw top part
		drawTop();

		// main draw function
		for (int i = 0; i < opts.Count; i++){
			// skip draw if it's hidden
			if (opts[i].IsHidden) continue;

			// Changes the color if it's the selected index
			if (i == index){
				Console.BackgroundColor = ConsoleColor.Black;
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine($"{opts[i].Name}");
				Console.ResetColor();
				continue;
			}
			// Otherwise
			Console.WriteLine($"{opts[i].Name}");
		}
		// draw bottom
		drawBottom();
	}
}


/// <summary>
/// Saves the menu item's name, what Action it has and its properties.
/// </summary>
public class MenuItem{
	/// <summary>
	/// The display name of the menu item.
	/// </summary>
	public readonly string Name;
	/// <summary>
	/// The action that is saved.
	/// </summary>
	public readonly Action Action;
	/// <summary>
	/// This boolean controls if it gets drawn in a menu.
	/// </summary>
	public bool IsHidden;

	internal MenuItem(string Name, Action Action){
		this.Name = Name;
		this.Action = Action;
	}

	internal MenuItem(string Name, Action Action, bool IsHidden){
		this.Name = Name;
		this.Action = Action;
		this.IsHidden = IsHidden;
	}
}
