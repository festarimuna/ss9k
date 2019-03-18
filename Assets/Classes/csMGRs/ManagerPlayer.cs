// Manages creation and state of player character

// 1. Create player GO
// 2. Maintain player state (stats) throughout sessions
// 3. Adjust player stats at start new turn
// 4. Provide public methods for adjusting stats

// Attach to ManagersContainer GameObject

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerPlayer : MonoBehaviour
{    
    // class properties
    public GameObject Player
    {
        get
        {
            return player;
        }
    }
    public GameObject TileCurrent
    {
        get
        {
            return tileCurrent;
        }
        set
        {
            managerUI.UpdateUIPanels();
            tileCurrent = value;
        }
    } 
    public GameObject TileMousedOver
    {
        get
        {
            return tileMousedOver;
        }
        set
        {
            tileMousedOver = value;
        }
    }

    // properties
    public bool IsLoadGame
    {
        get
        {
            return isLoadGame;
        }

        set
        {
            isLoadGame = value;
        }
    }
    public int APCurrent
    {
        get
        {
            return apCurrent;
        }

        set
        {
            if (value >= 0 && value <= APMaximum)
            {
                apCurrent = value;
                managerUI.UpdateUIPanels();
            }

            if (value < 0)
            {
                apCurrent = 0;
                managerUI.UpdateUIPanels();
            }
        }
    }
    public int APMaximum
    {
        get
        {
            return apTotal;
        }

        set
        {
            if (value >= 0 && value <= Settings.apMaxGame)
            {
                apTotal = value;
                managerUI.UpdateUIPanels();
            }
        }
    }
    public int Turn
    {
        get
        {
            return turn;
        }
        set
        {
            turn = value;
            managerUI.UpdateUIPanels();
        }
    }
    public int Points
    {
        get
        {
            return points;
        }
        set
        {
            points = value;
            managerUI.UpdateUIPanels();
        }
    }
    

    // class fields
    private GameObject tileCurrent;
    private GameObject tileMousedOver;
    private GameObject player;
    private GameObject selected;

    // private class references
    private GameObject modelPlayer;
    private ManagerAssets managerAssets;
    private ManagerTerrain managerTerrain;
    private ManagerUI managerUI;
    private ManagerUnits managerUnits;

    // private fields
    private bool isLoadGame;
    private int apCurrent, apTotal, turn, points; // action points are used to move


    // public interface: setup
    public void Setup()
    {
        SetReferences();
        SetupPlayerGO();
    }
    public void SetupNewPlayerStats()
    {
        APMaximum = Settings.apTotalNewGame;
        APCurrent = APMaximum;
    }
    public void SetupNewTurn()
    {
        APCurrent = APMaximum;
    }


    // public interface: player state 
    public void AddtoPlayerScore(GameObject tileCaptured)
    {
        if (!tileCaptured.GetComponent<TileTerrain>().TileCaptured)
        {
            Points += managerTerrain.ReturnTilePointsValue(tileCaptured);
            float points = Points;
        }
    }
    public void ReduceAPLeft(int distance)
    {
        if (APCurrent - distance >= 0)
        {
            APCurrent -= distance;
        }
        else
        {
            APCurrent = 0;
        }
    } // reduces APLeft by given distance
    public bool CheckIsEndOfTurn()
    {
        if (APCurrent == 0)
        {
            Turn++;
            return true;
        }
        return false;
    }
    public void CaptureTile(GameObject tile)
    {
        if (!tile.GetComponent<TileTerrain>().TileCaptured)
        {
            AddtoPlayerScore(tile);
            tile.GetComponent<TileTerrain>().TileCaptured = true;
            managerTerrain.TilesCaptured++;
            tile.transform.GetChild(1).GetComponent<Renderer>().material = managerAssets.ReturnMaterial(Settings.nameMatPlayer);
        }
    } 


    // setup
    private void SetReferences()
    {
        managerAssets = GetComponent<ManagerAssets>();
        managerTerrain = GetComponent<ManagerTerrain>();
        managerUI = GetComponent<ManagerUI>();
        managerUnits = GetComponent<ManagerUnits>();
    }


    // player creation functionality
    private void SetupPlayerGO()
    {
        if (player == null)
        {
            CreatePlayerParent();
            CreatePlayerModel();
        }
    } // Create Player container GameObject and modelPlayer GameObject prefab
    private void CreatePlayerParent() // Create Player container GameObject
    {
        player = new GameObject(Settings.namePlayer);
    }
    private void CreatePlayerModel() // Create Player container GameObject
    {
        // create modelPlayer GO
        modelPlayer = managerAssets.ReturnUnitGO(Settings.nameModelPlayer);
        Instantiate(modelPlayer, player.transform);

        // place modelPlayer in random tile
        managerUnits.MoveUnit(Player, managerTerrain.ReturnTileFromListByRandom());
        managerUnits.listUnitsPlayer.Add(player);
    }
}