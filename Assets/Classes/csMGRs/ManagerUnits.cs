// Contains public lists of enemy, player lists

// Manages moving unit GameObjects in-game and references to unit GameObjects for other classes
// (e.g. one list for unit player unit GameObjects and one list for enemy unit GameObjects)

// Attach to ManagersContainer GameObject

using System.Collections.Generic;
using UnityEngine; 

public class ManagerUnits : MonoBehaviour
{
    // private manager class references
    private ManagerPlayer managerPlayer;
    private ManagerTerrain managerTerrain;

    // public containers
    public List<GameObject> listUnitsPlayer;
    public List<GameObject> listUnitsEnemy;


    // setup
    private  void SetReferences()
    {
        managerPlayer = GetComponent<ManagerPlayer>();
        managerTerrain = GetComponent<ManagerTerrain>();
    }


    // public interface: setup
    public void Setup()
    {
        SetReferences();
    }

    // public interface: movement
    public void MoveUnit(GameObject unit, GameObject newTile)
    {
        UpdateUnitsCurrentTile(unit, newTile);
        unit.transform.parent = newTile.transform;
        unit.transform.localPosition = Settings.posZero;
    } // place unit at target tile
    public bool MoveUnitPC(GameObject tileTarget)
    {
        GameObject tile = tileTarget;
        int distance = managerTerrain.ReturnDistanceTiles(managerPlayer.TileCurrent, tile);
        if (distance > 0 &&
            distance <= Settings.movesMaxPCAtOnce &&
            !tile.GetComponent<TileTerrain>().TileCaptured)
        {
            MoveUnit(managerPlayer.Player, tile);
            managerPlayer.ReduceAPLeft(distance);
            return true;
        }
        return false;
    } // returns if player moved (==true) or didn't (==false)

    // public interface: containers
    public void AddUnitPlayerList(GameObject unitPlayer)
    {
        listUnitsPlayer.Add(unitPlayer);
    }
    public void AddUnitEnemyList(GameObject unitPlayer)
    {
        listUnitsEnemy.Add(unitPlayer);
    }
    public GameObject ReturnUnitPlayerList(string nameUnitPlayer)
    {
        foreach (GameObject unit in listUnitsPlayer)
        {
            if (unit.name == nameUnitPlayer)
            {
                return unit;
            }
        }
        return null;
    }
    public GameObject ReturnUnitEnemyList(string nameUnitEnemy)
    {
        foreach (GameObject unit in listUnitsEnemy)
        {
            if (unit.name == nameUnitEnemy)
            {
                return unit;
            }
        }
        return null;
    }
    public void ResetLists()
    {
        listUnitsEnemy.Clear();
        listUnitsPlayer.Clear();
    }


    // helpers
    private void UpdateUnitsCurrentTile(GameObject unit, GameObject newTile)
    {
        switch (unit.name)
        {
            case Settings.namePlayer:
                managerPlayer.TileCurrent = newTile;
                break;
            default:
                break;
        }
    } // update unit's current tile
}