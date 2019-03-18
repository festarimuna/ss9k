// Manages asset resources used by this project. All game assets are accessed by other classes through this class. 

// Different asset types stored in different list containers
// Public interface allows other classes to access assets for use in-game
// This allows adding resources to lists by click-dragging resources into exposed slots in the Unity Inspector for this script 

// Attach to ManagersContainer GameObject

using System.Collections.Generic;
using UnityEngine;

public class ManagerAssets : MonoBehaviour
{
    // public asset class references
    public Material matPlayer; // materials
    public GameObject modelTileDirt, modelTileGrass, modelTileSand, modelTileWater; // prefab tiles
    public GameObject modelPlayer; // prefab player 
    public GameObject prefabGridTilesGO, prefabGridTilesBGGO, prefabTileGO; // prefabs game tools
    public Texture colBlack, colWite; // textures colours
    public Texture2D cursorDefault, cursorGreen, cursorRed; // textures cursors

    // public asset container references
    public List<Texture> listColourTextures;
    public List<Material> listMaterials;
    public List<GameObject> listModelTilesGOs;
    public List<GameObject> listModelsUnitsGOs;
    public List<GameObject> listPrefabGOs;
    public List<Texture2D> listTexture2Ds;

    // public interface: setup
    public void Setup()
    {
        SetupListColourTextures();
        SetupListModelGOs();
        SetupListPrefabGOs();
        SetupListTileGOs();
        SetupListTexture2Ds();
        SetupListMaterials();
    }

    // public interface: return list objects
    public GameObject ReturnTileGO(string name)
    {
        foreach (GameObject go in listModelTilesGOs)
        {
            if (go.name.Contains(name))
            {
                return go;
            }
        }
        return null;
    }
    public Texture2D ReturnTexture2D(string name)
    {
        foreach (Texture2D texture in listTexture2Ds)
        {
            if (texture.name.Contains(name))
            {
                return texture;
            }
        }
        return null;
    }
    public GameObject ReturnUnitGO(string name)
    {
        foreach (GameObject go in listModelsUnitsGOs)
        {
            if (go.name.Contains(name))
            {
                return go;
            }
        }
        return null;
    }
    public GameObject ReturnPrefab(string name)
    {
        foreach (GameObject go in listPrefabGOs)
        {
            if (go.name.Contains(name))
            {
                return go;
            }
        }
        return null;
    }
    public Texture ReturnColourTexture(string name)
    {
        foreach (Texture texture in listColourTextures)
        {
            if (texture.name.Contains(name))
            {
                return texture;
            }
        }
        return null;
    }
    public Material ReturnMaterial(string name)
    {
        foreach (Material material in listMaterials)
        {
            if (material.name.Contains(name))
            {
                return material;
            }
        }
        return null;
    }


    // setup
    private void SetupListTileGOs()
    {
        listModelTilesGOs = new List<GameObject>
        {
            modelTileDirt,
            modelTileGrass,
            modelTileSand,
            modelTileWater
        };
    }
    private void SetupListTexture2Ds()
    {
        listTexture2Ds = new List<Texture2D>
        {
            cursorDefault,
            cursorGreen,
            cursorRed
        };
    }
    private void SetupListModelGOs()
    {
        listModelsUnitsGOs = new List<GameObject>
        {
            modelPlayer
        };
    }
    private void SetupListPrefabGOs()
    {
        listPrefabGOs = new List<GameObject>
        {
            prefabTileGO,
            prefabGridTilesGO,
            prefabGridTilesBGGO
        };
    }
    private void SetupListColourTextures()
    {
        listColourTextures = new List<Texture>
        {
            colWite,
            colBlack
        };
    }
    private void SetupListMaterials()
    {
        listMaterials = new List<Material>
        {
            matPlayer
        };
    }
}