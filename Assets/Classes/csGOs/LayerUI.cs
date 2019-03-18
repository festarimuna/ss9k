// Manages the seperation of mouseOver, onClick, functionality of UI and game-world layers
// This is used to e.g. prevent the player clicking on and through UI elements simultaneously (e.g. click button and also tile)

// Instances of this class are created on UI GOs along with GraphicRaycasters when created by ManagerUI
// PointerEventData events call methods which in turn call ManagerControls.cs methods

using UnityEngine;
using UnityEngine.EventSystems;

public class LayerUI : MonoBehaviour
    , IPointerEnterHandler
    , IPointerExitHandler
    , IPointerClickHandler
{
    // private class references
    ManagerControls managerControls;
    ManagerGame managerGame;


    // public interface: setup
    public void Setup(GameObject callingObject)
    {
        SetReferences(callingObject);
    }


    // setup
    private void SetReferences(GameObject callingObject)
    {
        managerControls = callingObject.GetComponent<ManagerControls>();
        managerGame = callingObject.GetComponent<ManagerGame>();
    }

    // PointerEventData methods
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!managerGame.InputStopped)
        {
            managerControls.MouseOverUI();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!managerGame.InputStopped)
        {
            managerControls.MouseoffUI();
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!managerGame.InputStopped)
        {
            managerControls.LMBUI();
        }
    }
}
