using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class endGen : MonoBehaviour
{
    [SerializeField] winCheck winCheck; // allows to access the wincheck script in inventory manager
    public List<GameObject> endCircuit;
    int i;

    // Update is called once per frame
    void Update()
    {
        foreach (power pow in winCheck.powers)
        {
            if (pow.circuit.Count != 0)
            {
                if (gameObject != null)
                {
                    if (pow.circuit.Last() != null)
                    {
                        if (pow.circuit.Last().name == "Light")
                        {
                            endCircuit = endCircuit.Union(pow.circuit).ToList();
                            if (gameObject.name == "Light")

                                endCircuit.Remove(gameObject);
                        }
                    }
                }
            }
        }
    }
}
