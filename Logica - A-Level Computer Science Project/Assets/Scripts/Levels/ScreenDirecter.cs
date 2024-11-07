using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenDirecter : MonoBehaviour
{
    public static int levelindex;
    public static int backindex;
    static string sName;
    public static bool endLvl;
    public TextMeshProUGUI display;
    public static bool studentIn;


    public void Start()
    {
        QualitySettings.SetQualityLevel(0);

        if (SceneManager.GetActiveScene().buildIndex == 23)
        {
            if (sName.IndexOf("6") != -1)
            {
                endLvl = true;
                display.text = "Restart from level 1";
            }
        }
    }

    public void SetIn()
    {
        studentIn = true;
    }

    public void MainMenu()
    {
        levelindex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(22);
        studentIn = true;
    }
    public void Load(Button button)
    {
        SceneManager.LoadScene(Int32.Parse(button.name));
        if (SceneManager.GetActiveScene().buildIndex == 25)
        {
            backindex = 25;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 22)
        {
            backindex = 22;
        }
        else
        {
            levelindex = Int32.Parse(button.name);
            Scene temp = SceneManager.GetSceneByBuildIndex(levelindex);
            sName = temp.name;
        }
    }

    public void gameBack()
    {
        SceneManager.LoadScene(backindex);
    }

    public void loadStart()
    {
        SceneManager.LoadScene(0);
    }


    public void NextLevel()
    {
        if (sName.IndexOf("6") == -1)
        {
            SceneManager.LoadScene(levelindex + 1);
            levelindex += 1;
            endLvl = false;
        }
        else
        {
           SceneManager.LoadScene(levelindex-5);
           levelindex -= 5;
        }
        Scene temp = SceneManager.GetSceneByBuildIndex(levelindex);
        sName = temp.name;

    }

    public void setQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        gameObject.GetComponent<student>().UpdateGraphics(qualityIndex);
        gameObject.GetComponent<PlayerInfo>().setGraphics(qualityIndex);
    }

    public void backToTSL()
    {
        SceneManager.LoadScene(30);
    }

    public void backToSSL()
    {
        SceneManager.LoadScene(33);
    }


    public void gameContinue()
    {
        SceneManager.LoadScene(levelindex);
    }

    private void Update()
    {
        if (studentIn)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                levelindex = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(25);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}


