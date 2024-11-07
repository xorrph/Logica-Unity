using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;

public class sandbox : MonoBehaviour
{
    Inventory inventoryC;
    Item selected;
    GameObject selectObj;
    public GameObject delObj;
    GameObject mirrorChild ;
    [SerializeField] List<GameObject> delSegs;
    [SerializeField] Transform cameraT;
    [SerializeField] Transform playerT;
    Vector3 posDistance;
    Vector3 placePos;
    Vector3 pos;
    public Vector3 placeDir;
    float dur;
    public float temp;
    float delay;
    [SerializeField] LayerMask mask;
    [SerializeField] winCheck winCheck;
    [SerializeField] List<generate> generates;
    [SerializeField] List<GameObject> updateObjs;
    public GameObject lightObj;

    private void Awake()
    {
        inventoryC = GetComponent<Inventory>();
    }

    private void Update()
    {
        selected = inventoryC.items[inventoryC.inv[inventoryC.select]];
        selectObj = selected.obj;

        RaycastHit hit;

        if (Physics.Raycast(cameraT.position, cameraT.forward, out hit, 5, mask)) // shoots a line and if is true if its collides with an object (that has a box collider)
        {
            posDistance = hit.collider.transform.position - hit.point;
            pos = hit.point - cameraT.forward * 0.01f;
            placePos = new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));
            if (placePos.y == 1)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    temp = (Mathf.RoundToInt(playerT.eulerAngles.y / 90))*90;
                    placeDir = new Vector3(0, temp, 0);
                    if (selected.name == "mirrorBlock")
                    {
                        temp = (Mathf.RoundToInt(playerT.eulerAngles.y / 45)) * 45;
                        if (temp % 90==0)
                        {
                            if (playerT.eulerAngles.y < temp)
                            {
                                temp = 315;
                            }
                            else if(playerT.eulerAngles.y > temp)
                            {
                                temp = 45;
                            }
                            else
                            {
                                Debug.Log("easter egg lol");
                            }
                        }
                        placeDir = new Vector3(0, temp, 0);

                    }
                    GameObject placed = Instantiate(selectObj, placePos, Quaternion.Euler(placeDir));
                    if(placed.transform.childCount != 0)
                    {
                        mirrorChild = placed.transform.GetChild(0).gameObject;
                        mirrorChild.transform.localRotation = Quaternion.Euler(new Vector3(0, 45, 0));
                    }
                    placed.name = selectObj.name;
                    if (selected.name == "powerBlock")
                    {
                        placed.GetComponentInChildren<UIprompt>().cameraT = cameraT;
                        winCheck.powers.Add(placed.GetComponent<power>());
                    }
                    if(placed.GetComponent<generate>() != null)
                    {
                        generates.Add(placed.GetComponent<generate>());
                    }
                    if (placed.name == "NOT") 
                    {
                        updateObjs.Add(placed);
                    }
                    if (placed.tag == "gate")
                    {
                        placed.GetComponent<generate>().lightObj = lightObj;
                    }
                }

            }

            if (Input.GetMouseButtonDown(0))
            {
                delObj = hit.collider.gameObject;
                if (delObj.tag != "block" && delObj.tag != "light")
                {
                    if (hit.collider.transform.position.y == 1)
                    {
                        if (delObj.GetComponent<generate>() != null)
                        {
                            foreach (GameObject seg in delObj.GetComponent<generate>().segs)
                            {
                                delSegs.Add(seg);
                            }
                            for (int i = 0; i < 2; i++)
                            {
                                GameObject hitObj = null;
                                if (i == 0 )
                                {
                                    hitObj = delObj.GetComponent<generate>().gate;
                                }
                                if (i == 1)
                                {
                                    hitObj = delObj.GetComponent<generate>().unGate;
                                }
                                if (hitObj != null)
                                {
                                    hitObj.GetComponent<gate>().inputs.Remove(delObj); // gets the gate component from the generate script of the object we are destroying and removes deleted object
                                    if (hitObj.GetComponent<gate>() != null)
                                    {
                                        while (hitObj.GetComponent<gate>().origins.Count > 0)
                                        {
                                            hitObj.GetComponent<gate>().origins.RemoveAt(0);
                                        }
                                    }
                                    else
                                    {
                                        if (hitObj.GetComponent<gate>().origins.Contains(delObj))
                                        {
                                            hitObj.GetComponent<gate>().origins.Remove(delObj);
                                        }
                                    }
                                }
                            }
                            if (delObj.GetComponent<gate>() != null)
                            {
                                if (delObj.GetComponent<gate>().origins != null)
                                {
                                    foreach (GameObject origin in delObj.GetComponent<gate>().origins)
                                    {
                                        if(origin != null)
                                        {
                                            int x = getIndex(delObj, origin.GetComponent<power>().circuit);
                                            for (; x < origin.GetComponent<power>().circuit.Count;)
                                            {
                                                origin.GetComponent<power>().circuit.RemoveAt(x);

                                            }
                                        }


                                    }
                                }
                            }
                        }
                        if (lightObj.GetComponent<endGen>().endCircuit.Count != 0)
                        {
                            if (lightObj.GetComponent<endGen>().endCircuit.Contains(delObj))
                            {
                                lightObj.GetComponent<endGen>().endCircuit.Remove(delObj);
                            }
                        }
                        if (delObj.GetComponent<generate>() != null)
                        {
                            generates.Remove(delObj.GetComponent<generate>());
                            if (getIndex(delObj, updateObjs) != 99)
                            {
                                updateObjs.Remove(delObj);
                            }
                        }

                        if (delObj.GetComponent<power>() != null)
                        {
                            winCheck.powers.Remove(delObj.GetComponent<power>());
                        }

                        Destroy(delObj);
                    }

                    if (hit.collider.gameObject.name == "collider")
                    {
                        Destroy(delObj);
                        Destroy(delObj.transform.parent.gameObject);
                    }



                }
            }
            if(Input.GetMouseButtonDown(0)|| Input.GetMouseButtonDown(1))
            {
                delay = 0.01f;
                foreach(generate gen in generates)
                {
                    if (gen.gameObject.GetComponent<gate>() != null)
                    {
                        gen.gameObject.GetComponent<gate>().inputs.Clear();
                        gen.gameObject.GetComponent<gate>().origins.Clear();
                    }
                }

            }
        }

        if(delSegs != null)
        {
            if (delSegs.Count > 0)
            {

                    Destroy(delSegs[0]);
                    delSegs.RemoveAt(0);

            }
        }


        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            if (delay != -0.1f)
            {
                foreach (power pow in winCheck.powers)
                {
                    pow.gameObject.GetComponent<generate>().genUpdate();
                }
                foreach (GameObject obj in updateObjs)
                {
                    obj.GetComponent<generate>().genUpdate();
                }
                delay = -0.1f;
            }
        }

    }

    int getIndex(GameObject obj, List<GameObject> list)
    {
        int i = 0;
        foreach (GameObject lObj in list)
        {
            if (lObj == obj)
            {
                return i;
            }
            i++;
        }
        return 99;
    }
}
