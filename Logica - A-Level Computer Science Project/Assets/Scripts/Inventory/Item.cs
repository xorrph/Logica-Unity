using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName ="Scriptable object / Item")]
public class Item : ScriptableObject
{
    public GameObject obj;
    public Sprite sprite;
    public string itemName;
}
