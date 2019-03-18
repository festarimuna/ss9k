// Setup, move camera

// Attach to ManagersContainer GameObject

using System.Collections;
using UnityEngine;

public class ManagerCamera : MonoBehaviour
{
    // private manager class references
    private ManagerGame managerGame;

    // private in-game GO references
    private GameObject cameraGO, dollyGO;

    // private fields
    private int counterZoom = 0;


    // public interface: setup
    public void Setup()
    {
        SetReferences();
        CreateDolly();
        PlaceCameraInDolly();
    }

    // public interface: camera functionality
    public void FaceUnit(GameObject unit)
    {
        dollyGO.transform.parent = unit.transform;
        dollyGO.transform.localPosition = new Vector3 (0f, 0f,0f);
    } // Center camera view on unit by childing to unit, resetting localPos
    public void ResetZoom()
    {
        cameraGO.GetComponent<Camera>().orthographicSize = Settings.zoomCameraDefault;
    }
    public IEnumerator ZoomEndGame()
    {
        while (cameraGO.GetComponent<Camera>().orthographicSize > Settings.zoomCameraEndGame)
        {
            yield return new WaitForSeconds(Settings.zoomCameraInterval);
            ZoomIn(true);
            counterZoom++;
        }

        StartCoroutine(WaitAfterZoom());
    } // zoom camera on end game
    public IEnumerator WaitAfterZoom()
    {
        yield return new WaitForSeconds(Settings.zoomCameraWaitEnd);
        counterZoom = 0;
        ResetZoom();
        managerGame.EndGameEnd();
    }


    // camera functionality
    private void ZoomIn(bool zoomIn)
    {
        if (zoomIn == true)
        {
            cameraGO.GetComponent<Camera>().orthographicSize--;
        }
        else
        {
            cameraGO.GetComponent<Camera>().orthographicSize++;
        }
    }


    // setup
    private void SetReferences()
    {
        cameraGO = GameObject.Find(Settings.nameCamGO);
        managerGame = GetComponent<ManagerGame>();
    }
    private void CreateDolly()
    {
        dollyGO = new GameObject(Settings.nameDollyGO);
        dollyGO.transform.position = new Vector3(0f, 0f, 0f);
    }
    private void PlaceCameraInDolly()
    {
        cameraGO.transform.transform.parent = dollyGO.transform;
    }
}