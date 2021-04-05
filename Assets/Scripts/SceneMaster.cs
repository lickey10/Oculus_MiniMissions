using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMaster : MonoBehaviour
{
    public Text DisplayBoardTitle;
    public TypingAnimation DisplayBoardText;
    public TextAsset DisplayBoardTextXmlFile;
    public TypingAnimation CurrentLevelDisplay;
    public TypingAnimation HighestLevelDisplay;
    public VRBasics_Slider LevelButton;

    int currentLevel;
    int highestLevel;
    int justFinishedLevel;
    XElement xmlDisplayBoardText = null;
    bool textDisplayed = false;
    private bool buttonPushed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        highestLevel = PlayerPrefs.GetInt("HighestLevel", 1);
        justFinishedLevel = PlayerPrefs.GetInt("JustFinishedLevel", 0);

        PlayerPrefs.SetInt("JustFinishedLevel", 0);//make sure variable is reset

        if (currentLevel > highestLevel)
        {
            PlayerPrefs.SetInt("HighestLevel", currentLevel);

            highestLevel = currentLevel;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        xmlDisplayBoardText = XElement.Parse(DisplayBoardTextXmlFile.text);

        if (justFinishedLevel == 1)//display congratulations text or something fireworks?
        {
            displayCongratulationsText();

            Invoke("displayLevelText", 7);

            justFinishedLevel = 0;
            textDisplayed = true;
        }

        if (!textDisplayed)
        {
            displayLevelText();

            textDisplayed = true;
        }

        displayLevelInfo();
    }

    private void displayLevelInfo()
    {
        //set current level on CurrentLevelDisplay object
        if (CurrentLevelDisplay)
        {
            CurrentLevelDisplay.Clear();
            CurrentLevelDisplay.ReplaceText(currentLevel.ToString());
        }

        if (HighestLevelDisplay)
        {
            HighestLevelDisplay.Clear();
            HighestLevelDisplay.ReplaceText(highestLevel.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //after push, has the button returned to up
        if (buttonPushed)
        {
            if (LevelButton.percentage < 0.5f)
            {
                buttonPushed = false;
            }
        }

        if (LevelButton.percentage > 0.5f && !buttonPushed)
        {
            incrementLevel();

            buttonPushed = true;
        }
    }

    private void incrementLevel()
    {
        if (currentLevel + 1 <= highestLevel)
            currentLevel++;
        else
            currentLevel = 1;

        PlayerPrefs.SetInt("CurrentLevel", currentLevel);

        if (CurrentLevelDisplay)
        {
            CurrentLevelDisplay.Clear();
            CurrentLevelDisplay.ReplaceText(currentLevel.ToString());
        }
    }

    private void displayLevelText()
    {
        XElement currentLevelXml = null;

        currentLevelXml = (from seg in xmlDisplayBoardText.Descendants("Level")
                                select (XElement)seg)
                                    .Where(x => (string)x.Attribute("number") == currentLevel.ToString()).FirstOrDefault();

        string initialDisplay = (string)currentLevelXml.Descendants("InitialDisplay").FirstOrDefault().Value.Trim();

        DisplayBoardTitle.text = (string)currentLevelXml.Descendants("InitialTitle").FirstOrDefault().Value.Trim();
        DisplayBoardText.Clear();
        DisplayBoardText.ReplaceText(initialDisplay);
    }

    private void displayCongratulationsText()
    {
        XElement currentLevelXml = null;

        currentLevelXml = (from seg in xmlDisplayBoardText.Descendants("Level")
                           select (XElement)seg)
                                    .Where(x => (string)x.Attribute("number") == (currentLevel - 1).ToString()).FirstOrDefault();

        DisplayBoardTitle.text = (string)currentLevelXml.Descendants("SuccessTitle").FirstOrDefault().Value.Trim();
        DisplayBoardText.Clear();
        DisplayBoardText.ReplaceText((string)currentLevelXml.Descendants("SuccessText").FirstOrDefault().Value.Trim());
    }
}
