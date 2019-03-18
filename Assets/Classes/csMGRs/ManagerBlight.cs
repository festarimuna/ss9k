// Manages blight functionality of Creep

// Attach to ManagersContainer GameObject

using System.Collections.Generic;
using UnityEngine;
using Utils;

public class ManagerBlight : MonoBehaviour
{
    // public containers
    public List<GameObject> listTilesBlighted; // list of blighted tiles

    // private containers
    private List<GameObject> listFrontier; // list of un/blighted neighbour tiles
    private List<GameObject> listTilesBlightedNew; // temp tile holder while listTilesBlighted enumerates for later blighting 

    //private manager class references
    private ManagerGame managerGame;
    private ManagerPlayer managerPlayer;
    private ManagerTerrain managerTerrain;


    // public interface: setup
    public void Setup()
    {
        SetReferences();
        SetupLists();
    }
    public void SetupBlightNewGame()
    {
        float tilesToBlight = ReturnCountTilesBlightStart();

        BlightManyTiles(tilesToBlight);
    } 

    // public interface: blight functionality
    public void HandleBlightSpreadEndTurn()
    {
        foreach (GameObject tile in listTilesBlighted)
        {
            // reduce all blighted tiles' blightTimers by 1 turn
            tile.GetComponent<TileTerrain>().BlightTimer--; 
            // if timer reached zero
            if (tile.GetComponent<TileTerrain>().BlightTimer <= 0) 
            {
                // blight a neighbouring tile
                SelectTileToBlight(tile);
                // generate new timer value
                tile.GetComponent<TileTerrain>().BlightTimer = ReturnBlightTime(); 
            }
        }
        foreach (GameObject tile in listTilesBlightedNew)
        {
            if (!listTilesBlighted.Contains(tile))
            {
                listTilesBlighted.Add(tile);
            }
        }
        listTilesBlightedNew.Clear();
    }
    
    // public interface: containers
    public void ResetLists()
    {
        listTilesBlighted.Clear();
        listFrontier.Clear();
        listTilesBlightedNew.Clear();
    }


    // setup
    private void SetReferences()
    {
        managerGame = GetComponent<ManagerGame>();
        managerPlayer = GetComponent<ManagerPlayer>();
        managerTerrain = GetComponent<ManagerTerrain>();
    }
    private void SetupLists()
    {
        listTilesBlighted = new List<GameObject>();
        listFrontier = new List<GameObject>();
        listTilesBlightedNew = new List<GameObject>();
    }

       
    // how many tiles are blighted on new game
    private float ReturnCountTilesBlightStart()
    {
        float countTilesBlightStart = managerGame.GridTilesTotal * Settings.blightCoverageStartPrcnt;
        float multiplierMin = 1 - Settings.blightCoverageStartVariance;
        float multiplierMax = 1 + Settings.blightCoverageStartVariance;
        float countTilesBlightStartModified = countTilesBlightStart * Random.Range(multiplierMin, multiplierMax);
        countTilesBlightStartModified = Mathf.Round(countTilesBlightStartModified);

        return countTilesBlightStartModified;
    } // return n in range of ManagerGame.Variance applied to ManagerGame.blightCoverageStartPrcnt
    private void BlightManyTiles(float tilesToBlight)
    {
        int tilesBlighted = 0;
        GameObject tileToBlight = new GameObject();
        List<GameObject> listDiscardedTiles = new List<GameObject>();

        while (tilesBlighted < tilesToBlight) // get tilesToBlight number of tiles 
        {
            tileToBlight = managerTerrain.ReturnTileFromListByRandom(); // select a tile at random
            while (tileToBlight.GetComponent<TileTerrain>().TileCaptured ||
                !managerTerrain.ReturnTileEmpty(tileToBlight.name) ||
                listDiscardedTiles.Contains(tileToBlight))
            {
                listDiscardedTiles.Add(tileToBlight);
                tileToBlight = managerTerrain.ReturnTileFromListByRandom(); // select another tile at random
            }

            tileToBlight.GetComponent<TileTerrain>().AddTileToList(tileToBlight, ListNames.tileListFrontier);
            AddTilesNeighboursToListFrontier(tileToBlight);
            BlightTile(tileToBlight);
            tilesBlighted++;
        }
    } // process for blighting multiple random tiles
    private void SelectTileToBlight(GameObject tileBlighting)
    {
        List<GameObject> tileBlightingNeighbours = ReturnListTilesNeighbours(tileBlighting);
        int maximumIndex = tileBlightingNeighbours.Count;
        int randomTileIndex = Random.Range(0, maximumIndex);

        if (tileBlightingNeighbours[randomTileIndex] != managerPlayer.TileCurrent &&
            !tileBlightingNeighbours[randomTileIndex].GetComponent<TileTerrain>().TileCaptured)
        {
            BlightTile(tileBlightingNeighbours[randomTileIndex]);
        }
    }

    // Breadth-Depth Search blight functionality
    public List<GameObject> ReturnListTilesNeighbours(GameObject tile)
    {
        List<GameObject> tilesNeighbour = new List<GameObject>();


        while (tilesNeighbour.Count < 4)
        {
            foreach (GameObject tileConsidered in managerTerrain.listTilesGrid)
            {
                Vector3 tilePos = tile.GetComponent<TileTerrain>().PosXZ;
                Vector3 tileConsideredPos = tileConsidered.GetComponent<TileTerrain>().PosXZ;
                int xAxisShift = 0; // increases (shifts) xAxis by value

                if (tileConsideredPos.x == tilePos.x + xAxisShift)
                {
                    if (tileConsideredPos.z == tilePos.z + 1
                        || tileConsideredPos.z == tilePos.z - 1)
                    {
                        tilesNeighbour.Add(tileConsidered);
                    }
                }
                if (tileConsideredPos.z == tilePos.z + xAxisShift)
                {
                    if (tileConsideredPos.x == (tilePos.x + 1)
                        || tileConsideredPos.x == tilePos.x - 1)
                    {
                        tilesNeighbour.Add(tileConsidered);
                    }
                }
            }
        }


        if (tilesNeighbour.Count > 0)
        {
            return tilesNeighbour;
        }
        return null;
    } // return method-scope <GO>List of tile's up, left, down, right 

    private void AddTilesNeighboursToListFrontier(GameObject tileToBlight)
    {
        if (ReturnListTilesNeighbours(tileToBlight) != null)
        {
            List<GameObject> tilesNeighbours = ReturnListTilesNeighbours(tileToBlight);

            foreach (GameObject tile in tilesNeighbours)
            {
                if (!listFrontier.Contains(tile))
                {
                    listFrontier.Add(tile);
                }
            }
        }
    }   // add ReturnListTilesNeighbours(tileToBlight) tiles to listFrontier
          
    private void BlightTile(GameObject tileToBlight) // blight single uncaputured tile
    {
        TileTerrain tile = tileToBlight.GetComponent<TileTerrain>();
        tile.TileCaptured = true;
        tile.BlightTimer = ReturnBlightTime();
        tileToBlight.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;

        tile.enabled = false;
        managerTerrain.TilesBlighted++;
        if (managerGame.InGame)
        {
            listTilesBlightedNew.Add(tileToBlight);
        }
        else
        {
            listTilesBlighted.Add(tileToBlight);
        }
    }

    private int ReturnBlightTime()
    {
        float multiplier = (Random.Range((Settings.multiplierBlightTimer * -1), Settings.multiplierBlightTimer) + 1); // +1 to make decimal into multiplyable percentage 
        float multiplierFixed = multiplier;

        if (multiplier <= 0)
        {
            multiplierFixed = 1f;
        }

        float blightTimer = Random.Range(Settings.blightTimerMin, Settings.blightTimerMax);
        float timeBlight = blightTimer * multiplierFixed;
        int timeBlightInt = (int)timeBlight;

        return timeBlightInt;
    }   // generate, return a time based on Settings.blightTimerMin,Max adjusted by ManagerGame.multiplierBlightTimer
}