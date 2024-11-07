using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LPinteraction : MonoBehaviour
{
    [SerializeField] private string prompt;
    public GameObject LaserPower;
    public GameObject Laser;
    public Material changeMat;

    public bool current = false;
    public string interactionPrompt => prompt;

    public bool Interact(Interactor interactor)
    {
        return true;
    }
}
