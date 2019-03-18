// Handles cursor appearance

// Attach to ManagersContainer GameObject

using UnityEngine;

public class ManagerCursor : MonoBehaviour
{
    // private class references
    private ManagerAssets managerAssets;
    private Vector2 hotSpot = new Vector2();
    
    // public properties
    public CursorMode CursorMode
    {
        get
        {
            return cursorMode;
        }

        set
        {
            cursorMode = value;
        }
    }
    // private fields
    private CursorMode cursorMode = CursorMode.Auto;


    // public interface: setup
    public void Setup()
    {
        SetReferences();
        SetCursor(Settings.nameCursorDefault);
    }

    // public interface: cursor appearance
    public void SetCursor(string cursorName)
    {
        Cursor.SetCursor(managerAssets.ReturnTexture2D(Settings.nameCursorDefault), hotSpot, CursorMode);

        if (managerAssets.ReturnTexture2D(cursorName) != null)
        {
            Cursor.SetCursor(managerAssets.ReturnTexture2D(cursorName), hotSpot, CursorMode);
        }
    }


    // setup
    private void SetReferences()
    {
        managerAssets = GetComponent<ManagerAssets>();
    }
}