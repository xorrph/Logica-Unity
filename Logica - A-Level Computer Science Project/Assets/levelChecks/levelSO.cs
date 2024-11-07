using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object / Level")]

public class levelSO : ScriptableObject
{
    public List<int> gateOrder;
    public List<string> truthOrder;
}
