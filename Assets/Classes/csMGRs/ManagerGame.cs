// Manages the main control flow for game

//  1. Sets up all other Manager classes, ultimately setting up the game (all GameObjects except for the "top-level Gameobjects [GOs]"     mentioned below are generated from script)
//  2. Provides public inteface for core functionality like managing end of turns
//  3. Calls methods across different Manager classes for core functionality like creating new levels
//  4. Keeps public static V3 instances for different presets

// Top-Level GOs: ManagersContainer, Camera, EventSystem Lights

// Attach to ManagersContainer GameObject

using System.Collections;
using UnityEngine;

public class ManagerGame : MonoBehaviour
{
    // singleton instance class reference
    public static ManagerGame managerGameIns;

    // private manager class references
    private ManagerAssets managerAssets;
    private ManagerBlight managerBlight;
    private ManagerCamera managerCamera;
    private ManagerControls managerControls;
    private ManagerCursor managerCursor;
    private ManagerTerrain managerTerrain;
    private ManagerPlayer managerPlayer;
    private ManagerUI managerUI;
    private ManagerUnits managerUnits;

    
    // properties
    public bool InGame
    {
        get
        {
            return inGame;
        }
    }
    public bool InputStopped
    {
        get
        {
            return inputStopped;
        }
    }
    public int GridTilesX
    {
        get
        {
            return gridTilesX;
        }
    }
    public int GridTilesZ
    {
        get
        {
            return gridTilesZ;
        }
    }
    public int GridTilesTotal
    {
        get
        {
            return gridTilesTotal;
        }
    }

    // fields
    private bool inGame, inputStopped;
    private int gridTilesX, gridTilesZ, gridTilesTotal;


    // public interface: setup
    public void SetGridStartGame(int sizeX, int sizeY)
    {
        gridTilesX = sizeX;
        gridTilesZ = sizeY;
        gridTilesTotal = sizeX * sizeY;
        managerUI.ToggleMaximiseElementUI(managerUI.PanelMenuNewGame.GetComponent<RectTransform>(), false);
        SetupLevelNewGame();
    }// set x,y size of the tile grid


    // public interface: game loop
    public void SetupLevelNewGame()
    {
        managerTerrain.CreateTerrain(gridTilesX, gridTilesZ, Settings.widthEdgeTilesGrid);
        managerUI.SetupGameScreenUI();
        managerPlayer.Setup();
        managerCamera.FaceUnit(managerPlayer.Player);
        managerBlight.SetupBlightNewGame();

        managerPlayer.SetupNewPlayerStats();
        managerPlayer.Points = 0;
        managerPlayer.Turn = 0;
        inGame = true;
        StartCoroutine(CheckGameOverMovement());
    }
    public void EndTurn()
    {
        StopAllCoroutines();
        managerPlayer.SetupNewTurn();
        StartCoroutine(CheckGameOverMovement());
        managerBlight.HandleBlightSpreadEndTurn();

        if (CheckIfEndGame())
        {
            EndGameStart();
        }
    }
    public void EndGameStart()
    {
        inputStopped = true;
        StopAllCoroutines();
        StartCoroutine(managerCamera.ZoomEndGame());

    }
    public void EndGameEnd()
    {
        managerUI.UpdateUIPanels();
        managerUI.ToggleMaximiseElementUI(managerUI.PanelMenuEndGame.GetComponent<RectTransform>(), true);
        managerUI.ToggleMaximiseElementUI(managerUI.PanelBot.GetComponent<RectTransform>(), false);

        managerCamera.FaceUnit(gameObject);
        managerUnits.ResetLists();
        managerTerrain.ResetLists();
        managerBlight.ResetLists();

        inputStopped = false;
    }
    public void ReloadMenu()
    {
        managerUI.ToggleMaximiseElementUI(managerUI.PanelMenuMain.GetComponent<RectTransform>(), true);
        managerUI.ToggleMaximiseElementUI(managerUI.PanelMenuEndGame.GetComponent<RectTransform>(), false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }


    // setup class
    private void SetCheckSingleton()
    {
        if (managerGameIns)
        {
            Destroy(transform.gameObject);
        }
        else
        {
            managerGameIns = this;
        }
        DontDestroyOnLoad(transform.gameObject);
    }
    private void SetReferences()
    {
        managerAssets = GetComponent<ManagerAssets>();
        managerBlight = GetComponent<ManagerBlight>();
        managerCamera = GetComponent<ManagerCamera>();
        managerControls = GetComponent<ManagerControls>();
        managerCursor = GetComponent<ManagerCursor>();
        managerTerrain = GetComponent<ManagerTerrain>();
        managerPlayer = GetComponent<ManagerPlayer>();
        managerUI = GetComponent<ManagerUI>();
        managerUnits = GetComponent<ManagerUnits>();
    }
    private void SetupManagers()
    {
        managerAssets.Setup();
        managerUI.Setup();
        managerTerrain.Setup();
        managerUnits.Setup();
        managerCamera.Setup();
        managerControls.Setup();
        managerCursor.Setup();
        managerBlight.Setup();
    }


    // Unity game loop
    private void Awake()
    {
        SetCheckSingleton();

        SetReferences();
        SetupManagers();
    }


    // game loop
    private bool CheckIfEndGame()
    {    
        int tilesTotal = 0;
        int tilesUnreachable = 0;
        // check if player can move to any new (uncaptured) tiles
        foreach (GameObject go in (managerBlight.ReturnListTilesNeighbours(managerPlayer.TileCurrent)))
        {
            if (managerTerrain.ReturnDistanceTiles(managerPlayer.TileCurrent, go) > Settings.movesMaxPCAtOnce ||
                go.GetComponent<TileTerrain>().TileCaptured)
            {
                tilesUnreachable++;
            }
            tilesTotal++;
        }
        if (tilesTotal == tilesUnreachable)
        {
            return true;
        }
        return false;
    } // end game if no uncaptured tile within maximum move range (check pc neighbours' locs) at end of turn

    private IEnumerator CheckGameOverMovement()
    {
        StartCoroutine(managerPlayer.TileCurrent.GetComponent<TileTerrain>().FadeTile());
        GameObject tileOld = managerPlayer.TileCurrent;
        while (inGame)
        {
            yield return new WaitForSeconds(Settings.loseNoMoveTime);
            
            if (tileOld = managerPlayer.TileCurrent)
            {
                EndGameStart();
            }
        }
    }
}