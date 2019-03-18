// Handles mouseOver player gameObject

// Routes LMB and RMB clicks of player gameObject to managerControls.cs
// HandleLMB/RMB() only call managerControls if !managerControls.overUI

using UnityEngine;

public class Player : MonoBehaviour
{
    // private class delcarations
    private GameObject managersContainer;
    private ManagerControls managerControls;
    private ManagerGame managerGame;


    // setup
    private void Setup()
    {
        managersContainer = GameObject.Find(Settings.nameManagersContainerGO);
        managerControls = managersContainer.GetComponent<ManagerControls>();
        managerGame = managersContainer.GetComponent<ManagerGame>();
    }


    // mouse
    private void OnMouseOver() 
    {
        if (!managerGame.InputStopped)
        {
            GameObject player = gameObject.transform.parent.gameObject;
            if (Input.GetMouseButtonDown(0))
            {
                HandleLMB();
            }
            if (Input.GetMouseButtonDown(1))
            {
                HandleRMB();
            }
        }


    } // check clicks, route to HandleLMB/RMB
    private void HandleLMB()
    {
        if (!managerControls.OverUI
            && managerGame)
        {
        managerControls.LMBPlayer(gameObject);
        }
    }
    private void HandleRMB()
    {
        if (!managerControls.OverUI)
        {
        managerControls.RMBPlayer(gameObject);
        }
    }

    // Unity game cycle
    private void Awake()
    {
        Setup();
    }
}