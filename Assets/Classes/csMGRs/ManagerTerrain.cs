// Manages creation of tiles, grid, listTilesGrid

// Creates terrain GO comprised of tile GOs along with background GO
// Returns references to tiles in listTilesGrid
// General Tile functionality like measuring distance between two tiles

// Attach to ManagersContainer GameObject

using System.Collections.Generic;
using UnityEngine;
using Utils;

public class ManagerTerrain : MonoBehaviour
{
    // in-game grid GO class references
    private GameObject GridTiles;
    private GameObject bgGridTiles;

    // private class references
    private ManagerAssets managerAssets;

    // private containers
    public List<GameObject> listTilesGrid = new List<GameObject>();

    // public properties
    public float OffsetHeightTile
    {
        get
        {
            return offsetHeightTile;
        }

        set
        {
            offsetHeightTile = value;
        }
    }
    public int CountTilesX
    {
        get
        {
            return countTilesX;
        }

        set
        {
            if (value > 0 && value < 1501)
            {
                countTilesX = value;
            }
        }
    }
    public int CountTilesZ
    {
        get
        {
            return countTilesZ;
        }

        set
        {
            if (value > 0 && value < 1501)
            {
                countTilesZ = value;
            }
        }
    }
    public int TilesBlighted
    {
        get
        {
            return tilesCorrupted;
        }
        set
        {
            tilesCorrupted = value;
        }
    }
    public int TilesCaptured
    {
        get
        {
            return tilesCaptured;
        }
        set
        {
            tilesCaptured = value;
        }
    }
    public int TilesTotal
    {
        get
        {
            return tilesTotal;
        }
    }
    public int WidthGridTilesEdge
    {
        get
        {
            return widthgridTilesEdge;
        }
        set
        {
            if (value > 0 && value < 500)
            {
                widthgridTilesEdge = value;
            }
        }
    }

    // private fields
    private float offsetHeightTile;
    private int tilesCorrupted = 0;
    private int tilesCaptured;
    private int countTilesX, countTilesZ, tilesTotal;
    private int widthgridTilesEdge;

    // private enums
    private TerrainType terrainType;


    // public interface : setup
    public void Setup()
    {
        SetReferences();
        SetFields();
    }
    public void CreateTerrain(int countTilesX, int countTilesZ, int widthGridTilesEdge)
    {
        AssignTerrainParameters(countTilesX, countTilesZ, widthGridTilesEdge);
        CreateGridBG();
        CreateGridTiles();
        CreateTileTerrainQuad();
    } // generate grid, tiles, background

    // public interface: containers
    public GameObject ReturnTileFromListByName(string nameTile)
    {
        foreach (GameObject go in listTilesGrid)
        {
            if (go.name == nameTile)
            {
                return go;
            }
        }
        return null;
    } // return tile from ListTiles using name
    public GameObject ReturnTileFromListByPos(Vector3 posTile)
    {
        foreach (GameObject go in listTilesGrid)
        {
            if (go.GetComponent<TileTerrain>().PosXZ == posTile)
            {
                return go;
            }
        }
        return null;
    } // return tile from ListTiles using name
    public GameObject ReturnTileFromListByRandom()
    {
        int indexTileRandom = Random.Range(1, listTilesGrid.Count);
        return listTilesGrid[indexTileRandom];
    } // return a random tile from ListTiles 
    public GameObject ReturnTileContainsGO(string nameGO)
    {
        foreach (GameObject go in listTilesGrid)
        {
            if (go.transform.Find(nameGO))
            {
                return go;
            }
        }
        return null;
    } // return the tile (parent) the game object is on (childed-to)
    public void ResetLists()
    {        
        Destroy(GridTiles);
        Destroy(bgGridTiles);
        listTilesGrid.Clear();
    }

    // public inteface: tiles
    public int ReturnTilePointsValue(GameObject tileCaptured)
    {
        int points = 0;
        TerrainType typeTile = tileCaptured.GetComponent<TileTerrain>().terrainType;

        switch (typeTile)
        {
            case TerrainType.Dirt:
                points = Settings.pointsTileDirt;
                break;
            case TerrainType.Grass:
                points = Settings.pointsTileSand;
                break;
            case TerrainType.Sand:
                points = Settings.pointsTileGrass;
                break;
            case TerrainType.Water:
                points = Settings.pointsTileDiamond;
                break;
        }
        return points;
    } 
    public int ReturnDistanceTiles(GameObject tileOrigin, GameObject tileDestination)
    {
        Vector3 posOrigin = tileOrigin.GetComponent<TileTerrain>().PosXZ;
        Vector3 posDestination = tileDestination.GetComponent<TileTerrain>().PosXZ;
        float distance = (Mathf.Abs(posDestination.x - posOrigin.x) + Mathf.Abs(posDestination.z - posOrigin.z));
        int distanceInt = (int)distance;
        DetermineMovePath(posOrigin, posDestination);

        return distanceInt;
    } // returns int distance between two tiles        
    public bool ReturnTileEmpty(string nameTile)
    {
        foreach (GameObject go in listTilesGrid)
        {
            if (go.name == nameTile)
            {
                if (go.transform.childCount <= Settings.tileEmptyAmount)
                {
                    return true;
                }
            }
        }
        return false;
    }   // return true if empty == tile GO contains only modelTile child; false if full == tileGO has >1 children

    // setup
    private void SetReferences()
    {
        managerAssets = GetComponent<ManagerAssets>();
    }
    private void SetFields()
    {
        offsetHeightTile = managerAssets.ReturnTileGO(((TerrainType)0).ToString()).transform.localScale.y;
    }
    private void AssignTerrainParameters(int countX, int countZ, int widthEdge)
    {
        CountTilesX = countX;
        CountTilesZ = countZ;
        WidthGridTilesEdge = widthEdge;
    } // set dimensions for tile grid to be made


    // container management
    private void AddTileToTileList(GameObject tile)
    {
        listTilesGrid.Add(tile);
    }
    private void AddTileToAdjacentListFromPos(Vector3 posTile, Vector3 posTileAdjacent)
    {
        Vector3 posTileCurrent = posTile;
        Vector3 posTileToAdd = posTileAdjacent;
        GameObject tileCurrent = ReturnTileFromListByPos(posTileCurrent);
        GameObject tileToAdd = ReturnTileFromListByPos(posTileToAdd);

        if (tileToAdd != null && !tileCurrent.GetComponent<TileTerrain>().listAdjacentTiles.Contains(tileToAdd))
        {
            tileCurrent.GetComponent<TileTerrain>().listAdjacentTiles.Add(tileToAdd);
        }
    }
    

    // tile creation functionality 
    private void CreateTileTerrainQuad()
    {
        int indexCurrentX = 0;
        int indexCurrentY = 0;
        int countTilesToMake = CountTilesZ * CountTilesZ;
        int countTilesMade = 0;

        while (countTilesMade < countTilesToMake)
        {
            CreateTile(countTilesMade, indexCurrentX, indexCurrentY);

            if (indexCurrentX < CountTilesX - 1)
            {
                indexCurrentX++;
            }
            else
            {
                indexCurrentX = 0;
                indexCurrentY++;
            }
            countTilesMade++;
        }
    } // create x-z plane (2D array in 3D space) quadrilateral shapes (tile terrain) of tiles
    private void CreateTile(int tileNumber, int currentIndexX, int currentIndexZ)
    {
        int tileCurrentNumber = tileNumber++;
        string nameTile = Settings.nameTilePrefix + tileCurrentNumber;

        GameObject tileNew = Instantiate(managerAssets.ReturnPrefab(Settings.nameTilePrefix), GridTiles.transform);
        tileNew.name = nameTile;
        tileNew.GetComponent<TileTerrain>().Setup(nameTile, currentIndexX, currentIndexZ);
        SetTileTerrainType(tileNew);        
        AddTileToTileList(tileNew);
        tilesTotal++;
    } // generate tile GameObject
    private void CreateGridTiles() // create gridTiles parent for tiles
    {
        GridTiles = new GameObject();
        GridTiles.name = Settings.nameGridTilesGO;
    } 
    private void SetTileTerrainType(GameObject tile) // assign terrain type to new tile GameObject
    {
        int countTerrainTypes = sizeof(TerrainType);
        terrainType = (TerrainType)UnityEngine.Random.Range(0, countTerrainTypes);
        tile.GetComponent<TileTerrain>().terrainType = terrainType;
        GameObject tileTerrainGO = managerAssets.ReturnTileGO(terrainType.ToString());
        Instantiate(tileTerrainGO, tile.transform);
        tile.GetComponent<TileTerrain>().ModelTile = tileTerrainGO;
    }


    // tileBG creation functionality 
    private void CreateGridBG() // generate background GameObject for tile terrain 
    {
        bgGridTiles = new GameObject
        {
            name = Settings.nameBGGridTilesGO
        };

        GameObject modelGridTilesBG = managerAssets.ReturnPrefab(Settings.nameGridTilesBGPrefabGO);
        Instantiate(modelGridTilesBG, bgGridTiles.transform);
        modelGridTilesBG.name = Settings.nameGridTilesBGPrefabGO;
    }


    // helpers
    private void DetermineMovePath(Vector3 posOrigin, Vector3 posDestination)
    {
        float travelledUp = (posDestination.x - posOrigin.x);
        float travelledLeft = (posDestination.z - posOrigin.z);
        // move path
        if (Mathf.Abs(travelledUp) > Mathf.Abs(travelledLeft))
        {
            // move x then z
        }
        else if (Mathf.Abs(travelledUp) < Mathf.Abs(travelledLeft))
        {
            // move z then x
        }
    } // e.g. (0,0,0) -> (1,0,1) == Up x1, Left x1;
}