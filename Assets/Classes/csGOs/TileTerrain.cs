// Handles tile collider toggle
// contains tileGO setup and mouse interactions (routes LMB and RMB clicks of player gameObject to managerControls.cs)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class TileTerrain : MonoBehaviour
{
    // public class properties
    public Vector3 PosXZ
    {
        get
        {
            return posXZ;
        }
        set
        {
            posXZ = value;
        }
    }
    public GameObject ModelTile
    {
        get
        {
            return modelTile;
        }
        set
        {
            if (value.GetType() == typeof(GameObject))
            {
                modelTile = value;
            }
        }
    }

    // private class Declarations
    private BoxCollider boxColliderTile;
    private GameObject canvasGO;
    private GameObject modelTile;
    private ManagerControls managerControls;
    private ManagerGame managerGame;
    private ManagerPlayer managerPlayer;
    private ManagerUI managerUI;
    private Rigidbody rb;
    private Vector3 posXZ = new Vector3();

    // public containers
    public List<GameObject> listAdjacentTiles = new List<GameObject>();
    public List<GameObject> listFrontier = new List<GameObject>();
    public List<GameObject> listNeighbours = new List<GameObject>();

    // public properties
    public bool TileActive
    {
        get
        {
            return tileActive;
        }

        set
        {
            tileActive = value;
        }
    }
    public bool TileCaptured
    {
        get
        {
            return tileCaptured;
        }

        set
        {
            tileCaptured = value;
        }
    }
    public int BlightTimer
    {
        get
        {
            return blightTimer;
        }

        set
        {
            blightTimer = value;
        }
    }
    public string NameTile
    {
        get
        {
            return nameTile;
        }

        set
        {
            nameTile = value;
        }
    }

    // private fields
    private bool tileCaptured, tileActive;
    private int blightTimer;
    private string nameTile;
    
    // public enums
    public TerrainType terrainType;

    // public interface: setup
    public void Setup(string name, float posX, float posZ)
    {
        SetDefaultProperties(name, posX, posZ);
        SetReferences();
        SetupCanvas();
    }


    // public interface: containers
    public void AddTileToList(GameObject tile, ListNames nameList)
    {
        switch (nameList)
        {
            case ListNames.tileListAdjacentTiles:
                listAdjacentTiles.Add(tile);
                break;
            case ListNames.tileListFrontier:
                listFrontier.Add(tile);
                break;
            case ListNames.tileListNeighbours:
                listNeighbours.Add(tile);
                break;
                default:
                break;
        }
    }

    // public interface: fade
    public IEnumerator FadeTile()
    {
        Material tileMaterial = gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().material;

        Color32 tileMatCol = tileMaterial.color;

        float timeElapsed = 0;

        while (timeElapsed < Settings.loseNoMoveTime)
        {
            yield return new WaitForSeconds(Settings.incrementor);
            timeElapsed += Settings.incrementor;
        }
    }

    // setup
    private void SetDefaultProperties(string name, float posX, float posZ)
    {
        NameTile = name;
        PosXZ = new Vector3(posX, 0, posZ);
        TileCaptured = false;

        transform.position = new Vector3(posX, 0, posZ);
    }
    private void SetReferences()
    {
        GameObject managersContainer = GameObject.Find(Settings.nameManagersContainerGO);
        managerControls = managersContainer.GetComponent<ManagerControls>(); // for click functionality
        managerGame = managersContainer.GetComponent<ManagerGame>();
        managerPlayer = managersContainer.GetComponent<ManagerPlayer>(); // for check whether this.TileTerrain is already moused-over 
        managerUI = managersContainer.GetComponent<ManagerUI>(); // for button setup
    }    
    private void HandleAttachComponments()
    {
        AttachBoxCollider();
        AttachRigidBody();
    }
    private void AttachBoxCollider()
    {
        boxColliderTile = gameObject.AddComponent<BoxCollider>();
        boxColliderTile.size = Settings.colTileSizeDefault;
        boxColliderTile.isTrigger = true;
    }
    private void AttachRigidBody()
    {
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.angularDrag = 0;
        rb.mass = 0;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }       


    // mouse functionality
    private void HandleLMB()
    {
        if (!managerControls.OverUI)
        {
            managerControls.LMBTile(gameObject);
        }
    } // check if overUI before send click to managerControls
    private void HandleRMB()
    {
        if (!managerControls.OverUI)
        {
            managerControls.RMBTile(gameObject);

        }
    } // check if overUI before send click to managerControls
    private void HandleMouseOverNewTile()
    {
        if (managerPlayer.TileMousedOver != gameObject && !managerControls.OverUI)
        {
            managerControls.MouseOverTile(gameObject);
        }
    }
    private void HandleMouseOver()
    {
        if (!managerControls.OverUI)
        {
            if (!managerControls.OverUI)
            {
                HandleMouseOverNewTile();

            }
        }
    }
    private void HandleMouseOff() // hides tile Button
    {
        if (!managerControls.OverUI)
        {
            managerUI.ToggleMaximiseElementUI(canvasGO.GetComponent<RectTransform>(), false);
            managerPlayer.TileMousedOver = null;
        }
    }


    private void OnMouseEnter()
    {
        if (!managerGame.InputStopped)
        {
            HandleMouseOverNewTile();
        }
    }
    private void OnMouseExit()
    {
        if (!managerGame.InputStopped)
        {
            HandleMouseOff();
            managerControls.MouseOffTile(canvasGO);
        }
    }
    private void OnMouseOver()
    {
        if (!managerGame.InputStopped)
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleRMB();
            }
            if (Input.GetMouseButtonDown(1))
            {
                HandleRMB();
            }
        }
    }
    

    private void SetupCanvas()
    {
        // Canvas GO
        canvasGO = managerUI.CreateCanvasGO(gameObject);
        canvasGO.name = Settings.nameCanvas;
        // minimise canvas
        managerUI.ToggleMaximiseElementUI(canvasGO.GetComponent<RectTransform>(), false);
    } // create child canvas, grandchild button GOs

    // helpers
    private void HandleAdjacentTile(Collider other)
    {
        Collider colTriggerGO = other;
        GameObject otherTile = colTriggerGO.transform.parent.gameObject;

        if (otherTile.name.Contains(Settings.nameTilePrefix) && !listAdjacentTiles.Contains(otherTile))
        {
            listAdjacentTiles.Add(colTriggerGO.transform.parent.gameObject);
        }
    } // adds colliding tiles to tile's if not already in listAdjacentTiles
           

    // Unity game cycle
    private void Awake()
    {
        HandleAttachComponments();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains(Settings.nameTilePrefix))
        {
            HandleAdjacentTile(other);
        }
        // Can detect collision with non-tile GOs here
    }
}