# Survey Simulator 9000 (in development)

Survey Simulator is a small turn-based game where the player aims to score as many points as possible by capturing different tiles, worth different points values. As the player moves, the ominous "Creep" entity spreads across the map, capturing tiles. Tiles captured by the creep change colour to black, and cannot be moved to. The player cannot move back to tiles already captured by the player.
The player must plan ahead and calculate risks related to which tiles to capture, maximise the points earned, and avoid the Creep. The game ends when the player has no tiles left to capture in their immediate vicinity.

## Installation
This project was developed in Unity 2017.3(0f3), and works best with this version of the editor. Some issues have cropped up with TextMesh Pro, so I've omitted TextMesh Pro files from the project completely. To install the project on your local machine, do the following:
- Clone/download or extract the project onto your machine.
- download TextMesh Pro from the Asset Store and import it into the project.

**NB: For now, putting in the new TextMesh Pro folder causes some issues with text rendering. Not all text fits well into panels and other UI elements.**

## Project Philosophies
**1. Centralised control flow**: This project avoids having multiple classes use Unity's Awake() and Start() methods which often leads to increasingly complex flows of control. The flow of control is instead restricted to the ManagerGame.cs class which replicates the entry-point functionality of the C# Main() method. User input (mouse clicks) sees the flow of control re-directed to the ManagerGame.cs class. In this way, the program is intended to be fairly easy to understand and modify. The ManagerGame.cs class also handles setting up the game and the game loop.

**2. Flexibility**: This project tries to generate UI elements (ManagerUI.cs), allocate assets like models (ManagerAssets.cs), and handle references (Settings.cs) in a flexible manner. This allows for changing pretty much anything in the game down the line without incurring too many problems. For instance, the names of GameObjects are set in Settings.cs as refernces, not string literals, meaning that finding those objects with the GameObject.Find() method avoids string literals.

## Classes
Please find the classes in **Assets/Classes.**
Classes are organised into three sections:
- csGOs: 	GameObject classes
- csMGRs:	Manager classes
- csUtil:	Utility classes

## FAQ
- **Why this old version of Unity?**
I'm developing a full-length Adventure-RPG in this version, and so am still using it.
- **What's up with the lack of commit history?**
I've previously used Bitbucket/SourceTree for version control. I am slowly making the transition to GitHub, and so have just copied this project into a new repo here.
- **What's the point of this project?**
I'm doing this for fun, slowly building up a nice template project which I can extend with more functionality. I'm also trying to better manage Unity's idiosyncracies in a cleaner OOP way. This is my practice.


Email: jonathan.widdowson1@neetfreek.net
Web: https://neetfreek.net/
GitHub: https://github.com/festarimuna
