using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static UnityEngine.UI.Image;

public class generate : MonoBehaviour
{
    [SerializeField] GameObject laserSeg;

    [SerializeField] Vector3Int dir;
    [SerializeField] Vector3Int genDir;

    Vector3 genPos;


    public List<GameObject> segs;
    public bool onOff;
    [SerializeField] bool bounced = false;
    [SerializeField] Vector3 pos;
    public GameObject gate;

    public GameObject unGate;

    public bool onGate;
    [SerializeField] bool oldOnOff;

    GameObject hitObj;

    public GameObject lightObj; // hit object light
    public bool lightHit;
    void Start()
    {
        if (Mathf.RoundToInt(transform.eulerAngles.y) == 0)
        {
            genDir = new Vector3Int(0, 0, 1);
        }

        if (transform.eulerAngles.y == 180)
        {
            genDir = new Vector3Int(0, 0, -1);
        }

        if (transform.eulerAngles.y == 90)
        {
            genDir = new Vector3Int(1, 0, 0);
        }

        if (transform.eulerAngles.y == 270)
        {
            genDir = new Vector3Int(-1, 0, 0);
        }
        genDis = 0;
        dir = genDir;

        coords = new List<Vector3Int>();
        dirs = new List<Vector3Int>();
        // initalises the variables at the start of this component being called
    }

    void Update()
    {
        if (GetComponent<power>() != null)
        {
            onOff = GetComponent<power>().onOff;
            onGate = false;
        }
        if (GetComponent<gate>() != null)
        {
            onOff = GetComponent<gate>().onOff;
            onGate = true;
        }


        if (coords.Count != segs.Count)
        {
            if (coords.Count > segs.Count)
            {
                for (int i = segs.Count; i < coords.Count; i++)
                {

                    Vector3 rot = new Vector3(0, 0, 0);
                    if (dirs[i].x != 0)
                    {
                        rot.y = 90;
                    }
                    GameObject placed = Instantiate(laserSeg, coords[i], Quaternion.Euler(rot));
                    placed.name = "laser";
                    segs.Add(placed);
                }
            }
            if (coords.Count < segs.Count)
            {
                Destroy(segs.Last());
                segs.RemoveAt(segs.Count - 1);
            }
        }

        if (onOff != oldOnOff)
        {

            genUpdate();
            oldOnOff = onOff;
        }
    }

    [SerializeField] List<Vector3Int> coords;
    [SerializeField]  List<Vector3Int> dirs;

    bool gen;
    int genDis;

    public void genUpdate()
    {
        if (GetComponent<power>() != null)
        {
            for (int i = 0; i < GetComponent<power>().circuit.Count;)
            {
                if (GetComponent<power>().circuit[i] != null)
                {
                    if (GetComponent<power>().circuit[i].name != "NOT")
                    {
                        GetComponent<power>().circuit.Remove(GetComponent<power>().circuit[i]);

                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }
        for (int i = 0; segs.Count>0; i++) {
            Destroy(segs.Last());
            segs.RemoveAt(segs.Count -1);
        }
        oldOnOff = onOff;
        coords.Clear();
        dirs.Clear();
        if(lightObj != null)
        {

            if (onGate)
            {
                foreach (GameObject origin in GetComponent<gate>().origins)
                {
                    if (origin != null)
                    {
                        origin.GetComponent<power>().circuit.Remove(lightObj);
                    }
                }
                if (lightObj.GetComponent<endGen>().endCircuit.Count != 0)
                {
                    foreach (GameObject obj in lightObj.GetComponent<endGen>().endCircuit.ToList())
                    {
                        lightObj.GetComponent<endGen>().endCircuit.Remove(obj);
                    }
                }
            }
            else
            {
                if (inList(gameObject, lightObj.GetComponent<endGen>().endCircuit))
                {
                    lightObj.GetComponent<endGen>().endCircuit.Remove(gameObject);
                }

            }
            lightObj = null;
        }
        if (gate != null)
        {
            if (gate.GetComponent<gate>() != null)
            {
                gate.GetComponent<gate>().inputs.Remove(gameObject);

                if (gate.GetComponent<gate>().type != 0)
                {
                    if (onGate)
                    {
                        foreach (GameObject origin in GetComponent<gate>().origins)
                        {
                            if (origin != null)
                            {
                                origin.GetComponent<power>().circuit.Remove(gate);
                            }
                        }
                    }
                    else
                    {
                        GetComponent<power>().circuit.Remove(gate);

                    }
                }
            }
            gate = null;
        }

        if (onOff)
        {
            dir = genDir;
            bounced = false;

            pos = transform.position;
            

            for (genDis = 1; genDis < 25; genDis++)
            {
                if (!bounced)
                {
                    pos = transform.position;
                }
                genPos = pos + new Vector3(dir.x * genDis, 0, dir.z * genDis);
                gen = true;
                checkAtPos();
                if (gen)
                {
                    coords.Add(new Vector3Int(Mathf.RoundToInt(genPos.x), Mathf.RoundToInt(genPos.y), Mathf.RoundToInt(genPos.z)));
                    dirs.Add(dir);
                }
            }
        }

    }

    void checkAtPos()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(genPos + new Vector3(0, 0.6f, 0), Vector3.down, out hit, 0.2f))
        {
            hitObj = hit.collider.gameObject;
            if (hitObj != gameObject)
            {
                string tag = hitObj.tag;

                //check stuff
                if (tag == "block" || tag == "switch")
                {
                    gen = false;
                    genDis = 25;
                }

                if (tag == "light")
                {

                    if (onGate)
                    {
                        foreach (GameObject origin in GetComponent<gate>().origins)
                        {
                            if(origin != null)
                            {
                                if (origin.GetComponent<power>().circuit != null)
                                {
                                    if (!inList(hitObj, origin.GetComponent<power>().circuit))
                                    {
                                        origin.GetComponent<power>().circuit.Add(hitObj);
                                       
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        if (!inList(hitObj, GetComponent<power>().circuit))
                        {
                            GetComponent<power>().circuit.Add(hitObj);
                           
                        }
                    }
                    lightObj = hitObj;
                    genDis = 25;
                    gen = false;
                    lightHit = true;
                }

                if (tag == "mirror")
                {
                    // checks if the mirror is rotated 315 degrees to base the output in the correct direction
                    if (Mathf.RoundToInt(hitObj.transform.parent.transform.eulerAngles.y) == 315 || Mathf.RoundToInt(hitObj.transform.parent.transform.eulerAngles.y) == 135)
                    {
                        if (dir.x != 0)
                        {
                            dir = new Vector3Int(0, 0, dir.x);
                        }
                        else if (dir.z != 0)
                        {
                            dir = new Vector3Int(dir.z, 0, 0);
                        }
                    }
                    // checks if the mirror is rotated 45 degrees to base the output in the correct direction
                    if (Mathf.RoundToInt(hitObj.transform.parent.transform.eulerAngles.y) == 45 || Mathf.RoundToInt(hitObj.transform.parent.transform.eulerAngles.y) == 225)
                    {
                        if (dir.x != 0)
                        {
                            // sets the new direction of the laser 90 degrees to the left
                            dir = new Vector3Int(0, 0, -dir.x);
                        }
                        else if (dir.z != 0)
                        {
                            // sets the new direction of the laser 90 degrees to the right
                            dir = new Vector3Int(-dir.z, 0, 0);
                        }
                    }
                    genDis = 0;
                    pos = hitObj.transform.position;
                    bounced = true;
                    gen = false;
                }

                if (tag == "gate")
                {
                    if (hitObj.GetComponent<gate>().type == 0)
                    {
                        if (Mathf.RoundToInt(hitObj.transform.eulerAngles.y) == 0 && dir.z == 1 || hitObj.transform.eulerAngles.y == 180 && dir.z == -1 || hitObj.transform.eulerAngles.y == 90 && dir.x == 1 || hitObj.transform.eulerAngles.y == 270 && dir.x == -1)
                        {
                            if (!inList(gameObject, hitObj.GetComponent<gate>().inputs))
                            {
                                hitObj.GetComponent<gate>().inputs.Add(gameObject);
                            }
                            gate = hitObj;
                            genDis = 25;
                            gen = false;

                            if (onGate)
                            {
                                foreach (GameObject origin in GetComponent<gate>().origins)
                                {
                                    if (origin != null)
                                    {
                                        if (!hitObj.GetComponent<gate>().origins.Contains(origin))
                                        {
                                            hitObj.GetComponent<gate>().origins.Add(origin);
                                        }
                                        if (!inList(hitObj, origin.GetComponent<power>().circuit))
                                        {
                                            origin.GetComponent<power>().circuit.Add(hitObj);

                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!hitObj.GetComponent<gate>().origins.Contains(gameObject))
                                {
                                    hitObj.GetComponent<gate>().origins.Add(gameObject);
                                }
                                if (!inList(hitObj, GetComponent<power>().circuit))
                                {
                                    GetComponent<power>().circuit.Add(hitObj);
                                }
                            }
                            hitObj.GetComponent<generate>().genUpdate();
                        }
                        else
                        {
                            genDis = 25;
                            gen = false;

                        }
                    }
                    else
                    {
                        if (hitObj.transform.eulerAngles.y == 90 && dir.z != 0 || hitObj.transform.eulerAngles.y == 270 && dir.z != 0 || Mathf.RoundToInt(hitObj.transform.eulerAngles.y) == 0 && dir.x != 0 || hitObj.transform.eulerAngles.y == 180 && dir.x != 0)
                        {
                            if (!inList(gameObject, hitObj.GetComponent<gate>().inputs))
                            {
                                hitObj.GetComponent<gate>().inputs.Add(gameObject);
                            }
                            gate = hitObj;
                            genDis = 25;
                            gen = false;

                            if (onGate)
                            {
                                foreach (GameObject origin in GetComponent<gate>().origins)
                                {
                                    if (origin != null)
                                    {
                                        if (!hitObj.GetComponent<gate>().origins.Contains(origin))
                                        {
                                            hitObj.GetComponent<gate>().origins.Add(origin);
                                        }
                                        if (!inList(hitObj, origin.GetComponent<power>().circuit))
                                        {
                                            origin.GetComponent<power>().circuit.Add(hitObj);

                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!hitObj.GetComponent<gate>().origins.Contains(gameObject))
                                {
                                    hitObj.GetComponent<gate>().origins.Add(gameObject);

                                }
                                if (!inList(hitObj, GetComponent<power>().circuit))
                                {
                                    GetComponent<power>().circuit.Add(hitObj);
                                }
                            }
                            hitObj.GetComponent<generate>().genUpdate();
                        }
                        else
                        {
                            genDis = 25;
                            gen = false;
                        }
                    }
                }
            }
        }

    }
    public bool inList(GameObject obj, List<GameObject> objs)
    {
        bool bObj = false;
        foreach (GameObject o in objs)
        {
            if (obj == o)
            {
                bObj = true;
            }
        }
        return bObj;
    }

}
