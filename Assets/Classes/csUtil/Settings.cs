using UnityEngine;

// Contains values for commonly-used variables

// names references: allows for changing of GO names here without having to change GameObject.Find("") string literals
// text: string literal values displayed to the player
// values new game defaults: values used in setting up a new games
// values game tools: values determining gameplay operation

public static class Settings
{


    // names references GOs
    public const string nameCursorDefault = "cursorDefault", nameCursorGreen = "cursorGreen", nameCursorRed = "cursorRed"; // cursors
    public const string nameGridTilesGO = "GridTiles", nameBGGridTilesGO = "GridTilesBG", nameGridTilesBGPrefabGO = "modelGridTiles"; // grid
    public const string nameModelPlayer = "modelPlayer", namePlayer = "Player"; // player
    public const string nameTilePrefix = "tile"; // tiles
    public const string nameManagersContainerGO = "ManagersContainer", nameCamGO = "Camera", nameDollyGO = "Dolly"; // top-level GOs
    public const string nameButton = "Button", nameCanvas = "Canvas", nameHeading = "Heading", nameHUD = "HUD", nameImage = "Image", nameLabel = "Label", nameText = "Text", nameTextBox = "TextBox", nameTextTurns = "TextTurns", nameTexture = "Texture"; // UI GOs

    // names references UI elements
    public const string nameButtonHelp = "ButtonHelp", nameButtonMenu = "ButtonMenu", nameButtonNewGame = "ButtonNewGame", nameButtonStartSmall = "ButtonStartSmall", nameButtonStartMedium = "ButtonStartMedium", nameButtonStartLarge = "ButtonStartLarge", nameButtonQuitGame = "ButtonQuitGame", nameButtonEndTurn = "ButtonEndTurn"; // buttons
    public const string nameMenuEndGame = "MenuEndGame", nameMenuHelp = "MenuHelp", nameMenuMain = "MenuMain", nameMenuNewGame = "MenuNewGame", namePanelBot = "UIPanelBottom"; // panels
    public const string nameTextAP = "UIPCAP", nameTextPCMoves = "UIPCMoves", nameTextPoints = "textPoints"; // text output

    // name references tools
    public const string colWhite = "colWhite", colBlack = "colBlack"; // colours
    public const string nameMatPlayer = "matPlayer"; // materials

    // text UI
    public const string textButtonHelp = "How to Play", textButtonNewGame = "Survey New Area", textButtonMenu = "To Menu", textButtonQuitGame = "Leave Survey Simulator",  textButtonStartSmall = "Small Map", textButtonStartMedium = "Medium Map", textButtonStartLarge = "Large map"; // buttons
    public const string headingMenuHelp = "How to Play", headingMenuMain = "Survey Simulator 9000", headingMenuNewGame = "Survey Size:", headingMenuEndGame = "Survey Complete"; // headings menu
    public const string textMessageEndGameTiles = "You surveyed a total of "; // message endGame
    public const string textMessageHelp = "\n\n- Capture tiles to protect them from the blight and earns points.\n\n- Capture tiles by moving on and then off tiles. \n\n- You must move within five seconds. \n\n- You must move onto a new (never visited) tile every turn.\n\n- You spend one Action Point (AP) to move to a new tile.\n\n- You earn different amounts of points from different colour tiles.\n\n- You complete a stage by earning a certain amount of points.\n\n- Each turn the blight may spread, turning tiles black.\n\n- Blighted  tiles cannot be captured or moved onto.\n\n- The game ends when you cannot move into a new (unvisited) tile.\n\n- Orange: 1, Yellow: 3, Green: 5, Blue: 9"; // message help
    public const string textSlash = "/", textAP = "Action Points: ", TextTurn = "Turn: ", textPoints = "Points: "; // panel bot

    // values new game defaults
    public const int apTotalNewGame = 1, loseNoMoveTime = 5;
    public const int countGridTilesXYSmall = 10, countGridTilesXYMedium = 20, countGridTilesXYLarge = 30;
    public const float uiElementToggleTimer = 0.2f;
    public const int pointsTileDirt = 1, pointsTileSand = 3, pointsTileGrass = 5, pointsTileDiamond = 9;
    public const float blightCoverageStartPrcnt = 0.1f, blightCoverageStartVariance = .5f, blightTimerMin = 2, blightTimerMax = 4, multiplierBlightTimer = 1f;
    public const int zoomCameraDefault = 5, zoomCameraEndGame = 2, zoomCameraWaitEnd = 2;
    public const float zoomCameraInterval = 0.5f;
    public const float incrementor = 0.1f;

    // values game tools
    public const float posDollyX = -18, posDollyY = 45.5f, posDollyZ = -10, rotationSpeedCam = 1f; // camera
    public static Vector3 colTileSizeDefault = new Vector3(1.1f, 0.1f, 1.1f); // colliders
    public const int widthEdgeTilesGrid = 80; // grid
    public const float mouseOverDelay = .33f; // mouse
    public const int tileEmptyAmount = 2; // tiles
    public const float widthTileButton = 1.5f, heightTileButton = 0.75f; // tiles
    public const int apMaxGame = 100, movesMaxPCAtOnce = 1; // player
    public static Vector3 posZero = new Vector3(0f, 0f, 0f), posOne = new Vector3(1f, 1f, 1f); // position values
}