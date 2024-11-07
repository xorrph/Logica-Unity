using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class winCheck : MonoBehaviour
{
    public int level; //up to 18
    public List<power> powers;
    public List<levelSO> levels;
    private int step;
    public GameObject lightObj;
    public bool win;

    private void Update()
    {
        foreach (power power in powers)
        {
            step = 0;

            foreach (GameObject obj in lightObj.GetComponent<endGen>().endCircuit)
            {
                if(obj != null)
                {
                    if (obj.tag == "gate")
                    {
                        if (step < levels[level].gateOrder.Count)
                        {
                            if (obj.GetComponent<gate>().type == levels[level].gateOrder[step])
                            {
                                step += 1;
                            }
                        }
                    }
                }
            }

            if (step == levels[level].gateOrder.Count)
            {
                win = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}



