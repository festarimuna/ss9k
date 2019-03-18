// Handles player interaction with game; all player input is routed to this class

// 1. bool overUI is checked by calling objects when mousing over gameWorld objects
//    if overUI == true, mouseOver and click routines are not called (e.g. don't handle clicking the player GO)
//    if overUI == false, mouseOver and click routines are called (e.g. continue on to handle clicking the player GO)
//2.  Detects player input in Update()
//3.  Determine what happens when different user input is read

// Attach to ManagersContainer GameObject

using System.Collections;
using UnityEngine;

public class ManagerControls : MonoBehaviour
{
    // private class references
    private ManagerCursor managerCursor;
    private ManagerGame managerGame;
    private ManagerPlayer managerPlayer;
    private ManagerTerrain managerTerrain;
    private ManagerUI managerUI;
    private ManagerUnits managerUnits;

    // public properties
    public bool OverUI
    {
        get
        {
            return overUI;
        }
        set
        {
            overUI = value;
        }
    }

    // private fields
    private bool overUI = false, helpPanelOpen = false;
    private bool menuOpen = false;
    private bool menuToggleCD = false; // elementUI toggle cooldown states for preventing rapid toggling of elementUI

    
    // public interface: setup
    public void Setup()
    {
        SetReferences();
    }

    // public interface: GO click
    public void LMBPlayer(GameObject player)
    {
    } // called by LMB click player from Player.cs
    public void RMBPlayer(GameObject player)
    {
    } // called by RMB click player from Player.cs
    public void LMBTile(GameObject tile)
    {
    } // called  by LMB click tile from TileTerrain.cs
    public void RMBTile(GameObject tile)
    {
        GameObject tileOld = managerPlayer.TileCurrent;
        if (managerUnits.MoveUnitPC(tile))
        {
            managerPlayer.CaptureTile(tileOld);
            tileOld.GetComponent<TileTerrain>().TileCaptured = true;

            //managerPlayer.CaptureTile(tile);
            //tile.GetComponent<TileTerrain>().TileCaptured = true;
        }

        if (managerPlayer.CheckIsEndOfTurn())
        {
            managerGame.EndTurn();
        }
    } // called  by RMB click tile from TileTerrain.cs: ends turn if managerPlayer.CheckIsEndOfTurn() satisfied

    // public interface: GO mouseOver
    public void MouseOverTile(GameObject tile)
    {
        if (managerPlayer.TileMousedOver != tile) 
        {
            managerPlayer.TileMousedOver = tile;
            if (managerPlayer.TileCurrent == tile ||
                    tile.GetComponent<TileTerrain>().TileCaptured == true)   
            {
                managerCursor.SetCursor(Settings.nameCursorRed);
            }
            else if (managerTerrain.ReturnDistanceTiles(managerPlayer.TileCurrent, tile) == Settings.movesMaxPCAtOnce)
            {
                managerCursor.SetCursor(Settings.nameCursorGreen);
            }
            else
            {
                managerCursor.SetCursor(Settings.nameCursorRed);
            }
        }
    } // called by mousing over tile from TileTerrain.cs
    public void MouseOffTile(GameObject canvasTile)
    {
        managerCursor.SetCursor(Settings.nameCursorDefault);
        managerUI.ToggleMaximiseElementUI(canvasTile.GetComponent<RectTransform>(), false);
        managerPlayer.TileMousedOver = null;
    } // called by mousing off tile from TileTerrain.cs; doesn't check if !overUI

    // public interface: UI interaction
    public void LMBUI() // called by LMB click on any UI object from LayerUI.cs
    {
        if (managerPlayer.Player != null)
        {
        }
    }
    public void MouseOverUI() // called by mousing over UI objects from LayerUI.cs
    {
        overUI = true;
        managerCursor.SetCursor(Settings.nameCursorDefault);
    }
    public void MouseoffUI() // called by mousing off UI objects from LayerUI.cs
    {
        overUI = false;
    }

    // public interface: Button interaction
    public void LMBHelp()
    {
        helpPanelOpen = true;
        managerUI.ToggleMaximiseElementUI(managerUI.PanelMenuHelp.GetComponent<RectTransform>(), true);
    }
    public void LBMNewGame()
    {
        managerUI.ToggleMaximiseElementUI(managerUI.PanelMenuMain.GetComponent<RectTransform>(), false);
        managerUI.ToggleMaximiseElementUI(managerUI.PanelMenuNewGame.GetComponent<RectTransform>(), true);
    } // called LMB click on menu button EndTurn routed through ManagerUI.HandleEntryPointButtonClick()
    public void LMBStartSmall()
    {
        managerGame.SetGridStartGame(Settings.countGridTilesXYSmall, Settings.countGridTilesXYSmall);
    } // called LMB click on menuNew button StartSmall routed through ManagerUI.HandleEntryPointButtonClick()
    public void LMBStartMedium()
    {
        managerGame.SetGridStartGame(Settings.countGridTilesXYMedium, Settings.countGridTilesXYMedium);
    } // called LMB click on menuNew button StartMedium routed through ManagerUI.HandleEntryPointButtonClick()
    public void LMBStartLarge()
    {
        managerGame.SetGridStartGame(Settings.countGridTilesXYLarge, Settings.countGridTilesXYLarge);
    } // called LMB click on menuNew button StartLarge routed through ManagerUI.HandleEntryPointButtonClick()
    public void LBMQuitGame()
    {
        managerGame.QuitGame();
    } // called LMB click on menu button QuitGame routed through ManagerUI.HandleEntryPointButtonClick()

    public void LMBEndTurn()
    {
        managerGame.EndTurn();
    } // called LMB click on panelBot button EndTurn routed through ManagerUI.HandleEntryPointButtonClick()


    // setup class
    private void SetReferences()
    {
        managerCursor = GetComponent<ManagerCursor>();
        managerGame = GetComponent<ManagerGame>();
        managerPlayer = GetComponent<ManagerPlayer>();
        managerTerrain = GetComponent<ManagerTerrain>();
        managerUI = GetComponent<ManagerUI>();
        managerUnits = GetComponent<ManagerUnits>();
    }


    // input check logic
    private void CheckMenuPress()
    {
        if (Input.GetKey(KeyCode.Escape) && managerGame.InGame == true && menuToggleCD == false)
        {
            menuOpen = !menuOpen;
            managerUI.ToggleMaximiseElementUI(managerUI.PanelMenuMain.GetComponent<RectTransform>(), menuOpen);
            menuToggleCD = true;
            StartCoroutine(UIElementCDTimer(KeyCode.Escape));
        }

        if (Input.GetKey(KeyCode.Escape) && helpPanelOpen)
        {
            managerUI.ToggleMaximiseElementUI(managerUI.PanelMenuHelp.GetComponent<RectTransform>(), false);
            helpPanelOpen = false;
        }

    }


    // uiElement toggle logic
    private IEnumerator UIElementCDTimer(KeyCode keyPressed)
    {
        yield return new WaitForSeconds(Settings.uiElementToggleTimer);

        switch (keyPressed)
        {
            case KeyCode.Escape:
                menuToggleCD = false;
                break;
        }
    } // sets bool [UIElement]CD to false after Settings.uiElementToggleTimer


    // unity game loop
    private void Update()
    {
        if (!managerGame.InputStopped)
        {
            CheckMenuPress();
        }
    }
}