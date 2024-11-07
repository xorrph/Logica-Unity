using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using static UnityEditor.PlayerSettings;


public class power : MonoBehaviour
{
    public bool onOff;
    public List<GameObject> circuit;

    private void Start()
    {
        circuit = new List<GameObject>();
    }
    public void Interact()
    {
        onOff = !onOff;
    }
}