using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gate : MonoBehaviour
{
    public List<GameObject> inputs; // list of all the inputs that the gate is connected too
    public List<GameObject> origins;// list of all the switches / sources of power that the gate has

    public int type; // type of gate

    public bool onOff; // the current represented as a boolean, on or off

    void Update() // called every frame
    {
        //the current is equal to the amount of input it recieves and if it satisfies the boolean
        if (type == 0)
        {
            //notGate
            onOff = !(inputs.Count >= 1);
        }

        if (type == 1)
        {
            //andGate
            onOff = (inputs.Count >= 2);
        }

        if (type == 2)
        {
            //orGate
            onOff = (inputs.Count >= 1);
        }

        if (type == 3)
        {
            //xorGate
            onOff = (inputs.Count == 1);
        }

        if (type == 4)
        {
            //nandGate
            onOff = !(inputs.Count >= 2);
        }

        if (type == 5)
        {
            //norGate
            onOff = !(inputs.Count >= 1);
        }

        if (type == 6)
        {
            //xnorGate
            onOff = !(inputs.Count == 1);
        }
    }
}
