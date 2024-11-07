using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public static float playerSens;
    public static int playerGraphics;
    public Slider slid;
    private void Start()
    {
        QualitySettings.SetQualityLevel(playerGraphics);
        if (gameObject.name == "Manager")
        {
            if (playerSens != 0)
            {
                GameObject[] gameObjects;
                gameObjects = GameObject.FindGameObjectsWithTag("settings");
                if (gameObjects.Length != 0)
                {
                    gameObjects[0].GetComponent<Slider>().value = playerSens;
                    gameObjects[1].GetComponent<TMP_Dropdown>().value = playerGraphics;
                }
            }
        }

    }

    public void Update()
    {
        if (slid != null)
        {
            sensitvity(slid);
        }
    }
    public void sensitvity(Slider sensSlide)
    {
        slid = sensSlide;
        if (sensSlide.GetComponent<slider>().drag) 
        {
            float temp = Mathf.Round(sensSlide.value * 100) / 100;
            setSens(temp);
            gameObject.GetComponent<student>().UpdateSens(playerSens);
            sensSlide.GetComponent<slider>().drag = false;
        }
    }

    public float getSens()
    {
        return playerSens;
    }

    public void setSens(float sens)
    {
        playerSens = sens;
    }
    
    public void setGraphics(int i)
    {
        playerGraphics = i;
    }
}
