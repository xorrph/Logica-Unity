using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class slider : MonoBehaviour, IDeselectHandler
{
    public bool drag;

    public void OnDeselect(BaseEventData data)
    {
        drag = true;
        Debug.Log("dragging");
    }

}
