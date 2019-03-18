// Handles the creation of UI GOs through methods which create GOs and return their references to callers

// 1. Sets up UI elements by creating menus, buttons, text; placing them, re-sizing them;
// 2. General UI functionality (convert colours, min/max UI GOs, update panels)

// Attach to ManagersContainer GameObject

using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManagerUI : MonoBehaviour
{
    // menu UI panel GO class references
    public GameObject PanelMenuHelp
    {
        get
        {
            return panelMenuHelp;
        }
    }
    public GameObject PanelMenuMain
    {
        get
        {
            return panelMenuMain;
        }
    }
    public GameObject PanelMenuNewGame
    {
        get
        {
            return panelMenuNewGame;
        }
    }
    public GameObject PanelMenuEndGame
    {
        get
        {
            return panelMenuEndGame;
        }
    }
    public GameObject PanelBot
    {
        get
        {
            return panelBot;
        }
    }

    // in-game UI class references
    private GameObject panelHUD; // HUD parent
    private GameObject panelBot, panelMenuHelp, panelMenuMain, panelMenuEndGame, panelMenuNewGame; // HUD children
    
    //private game tool class references
    private ManagerAssets managerAssets;
    private ManagerControls managerControls;
    private ManagerGame managerGame;
    private ManagerPlayer managerPlayer;
    private TextMeshProUGUI textUIPCAP, textUITurn, textUIPoints, textUIEndGameMessage, textUIHelpMessage; // displayed text
    private Vector3 minimised = Settings.posZero, maximised = Settings.posOne;

    // private containers
    private List<GameObject> listButtonsMenuMain = new List<GameObject>();
    private List<GameObject> listButtonsMenuNewGame = new List<GameObject>();
    private List<GameObject> listButtonsMenuEndGame = new List<GameObject>();

    // public properties
    public string TextUIEndGameMessageString
    {
        get
        {
            return Settings.textMessageEndGameTiles + managerPlayer.Points + " tiles in " + managerPlayer.Turn + " turns.";
        }
    }
    public string TextUIHelpMessageString
    {
        get
        {
            return Settings.textMessageHelp;
        }
    }

    // private fields
    private readonly float heightMenu = Screen.height * 0.75f;
    private readonly float widthMenu = Screen.width * 0.4f;
    private readonly float heightButtonMenuMain = Screen.width / 20f;
    private readonly float heightTextSmall = Screen.height / 20f;
    private readonly float widthTextSmall = Screen.width / 5f;
    private readonly float borderUI = Screen.width / 200f;
    private readonly float heightPanelBot = Screen.height / 5f;
    private readonly float widthPanelBot = Screen.width;


    // public interface: setup
    public void Setup()
    {
        SetReferences();
        // setup menus
        SetupPanelHUD();
        SetupMenuMain();
        SetupMenuHelp();
        SetupMenuNewGame();
        SetupMenuEndGame();
        // minimise non-main menus
        ToggleMaximiseElementUI(panelMenuNewGame.GetComponent<RectTransform>(), false);
        ToggleMaximiseElementUI(panelMenuEndGame.GetComponent<RectTransform>(), false);
        ToggleMaximiseElementUI(panelMenuHelp.GetComponent<RectTransform>(), false);
    }
    public void SetupGameScreenUI()
    {
        HandleSetupPanelBot();
    }

    // public interface: Create, return elementUI
    public GameObject CreateButtonGO(GameObject targetParent)
    {
        GameObject newButton = ReturnButtonGO();
        PlaceUIObject(newButton, targetParent);
        return newButton;
    }
    public GameObject CreateCanvasGO(GameObject targetParent)
    {
        GameObject newCanvas = ReturnCanvasGO();
        PlaceUIObject(newCanvas, targetParent);
        return newCanvas;
    }
    public GameObject CreateImageGO(GameObject targetParent)
    {
        GameObject newImage = ReturnRawImageGO();
        PlaceUIObject(newImage, targetParent);
        return newImage;
    }
    public GameObject CreateTextGO(GameObject targetParent)
    {
        GameObject newText = ReturnTextGO();
        PlaceUIObject(newText, targetParent);
        return newText;
    }

    // public interface: UI functionality
    public void HandleEntryPointButtonClick(string nameButton)
    {
        if (!managerGame.InputStopped)
        {
            switch (nameButton)
            {
                case Settings.nameButtonEndTurn:
                    managerControls.LMBEndTurn();
                    break;
                case Settings.nameButtonHelp:
                    managerControls.LMBHelp();
                    break;
                case Settings.nameButtonNewGame:
                    managerControls.LBMNewGame();
                    break;
                case Settings.nameButtonStartSmall:
                    managerControls.LMBStartSmall();
                    break;
                case Settings.nameButtonStartMedium:
                    managerControls.LMBStartMedium();
                    break;
                case Settings.nameButtonMenu:
                    managerGame.ReloadMenu();
                    break;
                case Settings.nameButtonStartLarge:
                    managerControls.LMBStartLarge();
                    break;
                case Settings.nameButtonQuitGame:
                    managerControls.LBMQuitGame();
                    break;
            }
        }

    } // all buttonClick events call this
    public Color ReturnColourConverted(float r, float g, float b, float a) 
    {
        Color colourNew = new Color(r/255f, g/255f, b/255, a/255);
        return colourNew;
    } // convert colour Vector3 to Colour struct
    public void ToggleMaximiseElementUI(RectTransform uiElementRt, bool maximise)
    {
        if (maximise == false)
        {
            uiElementRt.transform.localScale = minimised;
        }
        else
        {
            uiElementRt.transform.localScale = maximised;
        }
    } // maximise == on, !maximise == off
    public void UpdateUIPanels()
    {
        UpdatePanelBotUI();
        UpdateMenuEndGame();
    } // update UI info on end turn
    

    // setup class
    private void SetReferences()
    {
        managerAssets = GetComponent<ManagerAssets>();
        managerControls = GetComponent<ManagerControls>();
        managerGame = GetComponent<ManagerGame>();
        managerPlayer = GetComponent<ManagerPlayer>();
    }
    private void SetupMenuMain()
    {
        // setup panel menu main
        panelMenuMain = SetupMenuNew(Settings.nameMenuMain, panelHUD);
        panelMenuMain.GetComponent<LayerUI>().Setup(gameObject);
        SetupMenuHeading(panelMenuMain, Settings.headingMenuMain);
        // setup button start
        GameObject butStart = SetupButtonNew(Settings.nameButtonNewGame, Settings.colBlack, Settings.textButtonNewGame, Color.white, PanelMenuMain);
        listButtonsMenuMain.Add(butStart);
        // setup button help
        GameObject butHelp = SetupButtonNew(Settings.nameButtonHelp, Settings.colBlack, Settings.textButtonHelp, Color.white, PanelMenuMain);
        listButtonsMenuMain.Add(butHelp);
        // setup button quit
        GameObject butQuit = SetupButtonNew(Settings.nameButtonQuitGame, Settings.colBlack, Settings.textButtonQuitGame, Color.white, PanelMenuMain);
        listButtonsMenuMain.Add(butQuit);
        // position buttons menu main
        PositionButtonsMenuAll(listButtonsMenuMain);
    } 
    private void SetupMenuHelp()
    {
        // setup panel menu help
        panelMenuHelp = SetupMenuNew(Settings.nameMenuHelp, panelHUD);
        panelMenuHelp.GetComponent<LayerUI>().Setup(gameObject);
        SetupMenuHeading(panelMenuHelp, Settings.headingMenuHelp);
        // setup message help
        GameObject textBoxMenuHelp = SetupReturnTextBox(TextUIHelpMessageString, Color.black, panelMenuHelp);
        textUIHelpMessage = textBoxMenuHelp.GetComponent<TextMeshProUGUI>();
        textUIHelpMessage.alignment = TextAlignmentOptions.Left;
        textUIHelpMessage.text = TextUIHelpMessageString;
        // position message help
        textBoxMenuHelp.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, borderUI, widthMenu - (borderUI * 2));
        textBoxMenuHelp.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, borderUI, widthMenu - (borderUI * 2));
    }
    private void SetupMenuNewGame()
    {
        // setup panel menu new game
        panelMenuNewGame = SetupMenuNew(Settings.nameMenuNewGame, panelHUD);
        panelMenuNewGame.GetComponent<LayerUI>().Setup(gameObject);
        SetupMenuHeading(panelMenuNewGame, Settings.headingMenuNewGame);
        // setup button start game small
        GameObject butStartSmall = SetupButtonNew(Settings.nameButtonStartSmall, Settings.colBlack, Settings.textButtonStartSmall, Color.white, PanelMenuNewGame);
        listButtonsMenuNewGame.Add(butStartSmall);
        // setup button start game medium
        GameObject butStartMedium = SetupButtonNew(Settings.nameButtonStartMedium, Settings.colBlack, Settings.textButtonStartMedium, Color.white, PanelMenuNewGame);
        listButtonsMenuNewGame.Add(butStartMedium);
        // setup button start game large
        GameObject butStartLarge = SetupButtonNew(Settings.nameButtonStartLarge, Settings.colBlack, Settings.textButtonStartLarge, Color.white, PanelMenuNewGame);
        listButtonsMenuNewGame.Add(butStartLarge);
        // position buttons menu new game
        PositionButtonsMenuAll(listButtonsMenuNewGame);
    }
    private void SetupMenuEndGame()
    {
        // setup panel menu end game
        panelMenuEndGame = SetupMenuNew(Settings.nameMenuEndGame, panelHUD);
        panelMenuEndGame.GetComponent<LayerUI>().Setup(gameObject);
        SetupMenuHeading(panelMenuEndGame, Settings.headingMenuEndGame);
        // setup text menu end game
        GameObject textBoxMenuEnd = SetupReturnTextBox(TextUIEndGameMessageString, Color.black, panelMenuEndGame);
        textUIEndGameMessage = textBoxMenuEnd.GetComponent<TextMeshProUGUI>();
        textUIEndGameMessage.alignment = TextAlignmentOptions.Center;
        textUIEndGameMessage.text = TextUIEndGameMessageString;
        listButtonsMenuEndGame.Add(textBoxMenuEnd);
        // setup button menu main
        GameObject butMenu = SetupButtonNew(Settings.nameButtonMenu, Settings.colBlack, Settings.textButtonMenu, Color.white, PanelMenuEndGame);
        listButtonsMenuEndGame.Add(butMenu);
        // setup button quit
        GameObject butQuit = SetupButtonNew(Settings.nameButtonQuitGame, Settings.colBlack, Settings.textButtonQuitGame, Color.white, PanelMenuEndGame);
        listButtonsMenuEndGame.Add(butQuit);
        // position buttons menu end game
        PositionButtonsMenuAll(listButtonsMenuEndGame);
    }
    private void HandleSetupPanelBot()
    {
        panelBot = SetupReturnPanelBotCanvasImg();
        panelBot.transform.SetSiblingIndex(0);
        SetupTextPanelBotTurns();
        SetupTextPanelBotPoints();
        SetupPanelBotTextAP(panelBot);
        panelBot.GetComponent<LayerUI>().Setup(gameObject);
    }

    // setup: positioning
    private void PositionElementMidUI(GameObject elementUI, int buttonIndex, GameObject targetParent)
    {
        RectTransform elementRT = elementUI.GetComponent<RectTransform>();
        elementRT.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, ((widthMenu / 2) - (widthTextSmall / 2)), widthTextSmall);
        elementRT.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, (heightButtonMenuMain * buttonIndex) + (borderUI * (buttonIndex)), heightButtonMenuMain);
    }

    // setup: hud
    private void SetupPanelHUD()
    {
        panelHUD = ReturnCanvasGO();
        panelHUD.name = Settings.nameHUD;
        panelHUD.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
    }

    // setup: menus
    private GameObject SetupMenuNew(string namePanel, GameObject targetParent)
    {
        GameObject newImageGO = CreateImageGO(panelHUD);
        newImageGO.name = namePanel;
        newImageGO.AddComponent(Type.GetType("LayerUI"));
        newImageGO.AddComponent<GraphicRaycaster>();

        RectTransform newImageGORT = newImageGO.GetComponent<RectTransform>();
        newImageGORT.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, ((Screen.width - widthMenu)/2), widthMenu);
        newImageGORT.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, (heightPanelBot + borderUI), heightMenu);
        //newImageGORT.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, (((Screen.height - heightMenu)) - borderUI), heightMenu);

        return newImageGO;
    }
    private void SetupMenuHeading(GameObject targetParent, string targetText)
    {
        GameObject newTextGO = CreateTextGO(targetParent);
        newTextGO.name = Settings.nameHeading;

        RectTransform newTextGORT = newTextGO.GetComponent<RectTransform>();
        newTextGORT.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, ((widthMenu / 2) - (widthTextSmall / 2)), widthTextSmall);
        newTextGORT.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, borderUI, heightTextSmall);

        TextMeshProUGUI newTextGOText = newTextGO.GetComponent<TextMeshProUGUI>();
        newTextGOText.color = Color.black;
        newTextGOText.text = targetText;
    }

    // setup: buttons
    private GameObject SetupButtonNew(string nameButton, string nameTexture, string textButton, Color textColour, GameObject targetParent)
    {
        GameObject newButton = CreateButtonGO(targetParent);
        newButton.name = nameButton;

        newButton.GetComponent<RawImage>().texture = managerAssets.ReturnColourTexture(nameTexture);
        newButton.GetComponent<Button>().targetGraphic = newButton.GetComponent<RawImage>();
        newButton.GetComponent<Button>().onClick.AddListener(delegate { HandleEntryPointButtonClick(newButton.name); });

        GameObject newButtonTextChild = newButton.transform.GetChild(0).gameObject;
        newButtonTextChild.name = Settings.nameText;
        newButtonTextChild.GetComponent<TextMeshProUGUI>().color = Color.white;
        newButtonTextChild.GetComponent<TextMeshProUGUI>().text = textButton;

        return newButton;
    }  // create, return new button object
    private void PositionButtonMenu(GameObject button, int buttonIndex)
    {
        RectTransform newButtonRT = button.GetComponent<RectTransform>();
        newButtonRT.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, ((widthMenu / 2) - (widthTextSmall / 2)), widthTextSmall);
        newButtonRT.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, (heightButtonMenuMain * buttonIndex) + (borderUI * (buttonIndex)), heightButtonMenuMain);
    } // position button menus
    private void PositionButtonsMenuAll(List<GameObject> list)
    {
        List<GameObject> listButtons = list;

        foreach (GameObject button in listButtons)
        {
            PositionButtonMenu(button, listButtons.IndexOf(button) + 1);
        }
    } // position button GOs in list

    // setup textboxes
    private GameObject SetupReturnTextBox(string textText, Color textColour, GameObject targetParent)
    {
        // GO
        GameObject textBox = CreateTextGO(targetParent);
        textBox.name = Settings.nameTextBox;
        textUIPCAP = textBox.GetComponent<TextMeshProUGUI>();
        textUIPCAP.text = textText;
        textUIPCAP.color = textColour;
        textUIPCAP.alignment = TextAlignmentOptions.Left;

        return textBox;
    }
    private void PositionTextBoxPanelBot(GameObject textBox, int TextBoxIndex)
    {
        RectTransform newButtonRT = textBox.GetComponent<RectTransform>();
        newButtonRT.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, ((Screen.width / 2) - (widthTextSmall / 2)), widthTextSmall);
        newButtonRT.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, (heightTextSmall * (TextBoxIndex - 1)) + (borderUI * (TextBoxIndex)), heightTextSmall);
        //newButtonRT.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, ((heightPanelBot * TextBoxIndex) + (borderUI * (TextBoxIndex))), widthTextSmall);
        //newButtonRT.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, ((heightTextSmall / 2) - (heightTextSmall / 2)), heightTextSmall);
    }

    // setup: bottom panel
    private GameObject SetupReturnPanelBotCanvasImg()
    {
        // RawImage child
        GameObject newImageGO = CreateImageGO(panelHUD);
        newImageGO.name = Settings.namePanelBot;
        newImageGO.AddComponent(Type.GetType("LayerUI"));
        newImageGO.AddComponent<GraphicRaycaster>();     
        
        // RawImage child RT component
        RectTransform newImageGORT = newImageGO.GetComponent<RectTransform>();
        newImageGORT.sizeDelta = new Vector2(widthPanelBot, heightPanelBot);
        newImageGORT.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0f, heightPanelBot);
        newImageGORT.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0f, widthPanelBot);
               
        return newImageGO;
    } // return setup bottom panel image childed to canvas
    private void SetupTextPanelBotTurns()
    {
        GameObject textTurnsGO = SetupReturnTextBox("", Color.black, panelBot);
        textTurnsGO.name = Settings.nameTextTurns;
        textUITurn = textTurnsGO.GetComponent<TextMeshProUGUI>();
        PositionTextBoxPanelBot(textTurnsGO, 1);
    }
    private void SetupTextPanelBotPoints()
    {
        GameObject textPointsGO = SetupReturnTextBox("", Color.black, panelBot);
        textPointsGO.name = Settings.nameTextPoints;
        textUIPoints = textPointsGO.GetComponent<TextMeshProUGUI>();
        PositionTextBoxPanelBot(textPointsGO, 2);
    }
    private void SetupPanelBotTextAP(GameObject targetParent)
    {
        // GO
        GameObject panelUIBotTextPCAP = CreateTextGO(targetParent);
        panelUIBotTextPCAP.name = Settings.nameTextAP;
        textUIPCAP = panelUIBotTextPCAP.GetComponent<TextMeshProUGUI>();
        textUIPCAP.alignment = TextAlignmentOptions.Left;
        RectTransform textUIPCAPRT = textUIPCAP.GetComponent<RectTransform>();

        // positioning
        textUIPCAPRT.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, borderUI * 2, widthTextSmall);
        textUIPCAPRT.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, borderUI, heightTextSmall);
    } // setup & child player action points TMProUGUI text object to targetParent
    private void HandleClickButtonEndTurn()
    {
    }  // handle player click End Turn button


    // create, return UI object references, used in Create[GameObject] public interface methods
    private GameObject ReturnRawImageGO()
    {
        // setup RawImage GO
        GameObject imageGO = new GameObject();
        imageGO.name = Settings.nameImage;
        RawImage imageGOImage = imageGO.AddComponent<RawImage>();
        // setup Texture component
        Texture texture = new Texture();
        texture = managerAssets.ReturnColourTexture(Settings.colWhite);
        imageGOImage.texture = texture;

        return imageGO;
    }
    private GameObject ReturnButtonGO()
    {
        // button GO
        GameObject buttonGO = new GameObject();
        buttonGO.name = Settings.nameButton;
        // setup button components
        Button button = buttonGO.AddComponent<Button>();
        RawImage buttonImage = buttonGO.AddComponent<RawImage>();
        Texture buttonImageTexture = managerAssets.ReturnColourTexture(Settings.colWhite);
        buttonImage.texture = buttonImageTexture;
        button.targetGraphic = buttonImage;
        // setup text child
        GameObject buttonGOText = new GameObject();
        buttonGOText.transform.parent = buttonGO.transform;
        buttonGOText.AddComponent<TextMeshProUGUI>();
        buttonGOText.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
        buttonGOText.name = Settings.nameText;
        buttonGOText.GetComponent<TextMeshProUGUI>().color = Color.white;
        buttonGOText.GetComponent<TextMeshProUGUI>().text = Settings.textButtonQuitGame;

        buttonGO.AddComponent<GraphicRaycaster>();

        return buttonGO;
    }
    private GameObject ReturnCanvasGO()
    {
        // setup Canvas GO
        GameObject canvasGO = new GameObject();
        canvasGO.name = Settings.nameCanvas; 
        // setup Canvas components
        canvasGO.AddComponent<RectTransform>();
        canvasGO.AddComponent<Canvas>();
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        return canvasGO;
    }
    private GameObject ReturnTextGO()
    {
        // text GO
        GameObject textGO = new GameObject();
        textGO.name = Settings.nameText;
        // setup text components
        textGO.AddComponent<TextMeshProUGUI>();
        textGO.GetComponent<TextMeshProUGUI>().color = Color.black;
        textGO.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;

        return textGO;
    }


    // ui functionality
    private void UpdateMenuEndGame()
    {
        textUIEndGameMessage.text = TextUIEndGameMessageString;
    }
    private void UpdatePanelBotUI()
    {
        // points
        textUIPoints.text = Settings.textPoints + managerPlayer.Points.ToString();
        // turn number
        textUITurn.text = Settings.TextTurn + managerPlayer.Turn.ToString();
        //PC action points
        textUIPCAP.text = Settings.textAP + managerPlayer.APCurrent.ToString() + Settings.textSlash + managerPlayer.APMaximum.ToString();
    }


    // helpers
    private void PlaceUIObject(GameObject uiObj, GameObject targetParent)
    {
        uiObj.transform.SetParent(targetParent.transform, false);
        uiObj.transform.localPosition = new Vector3(0f, 0f, 0f);
    }
}