using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static UnityEngine.UI.Image;

public class poop : MonoBehaviour
{
    [SerializeField] GameObject laserSeg;

    [SerializeField] Vector3Int dir;
    [SerializeField] Vector3Int genDir;

    Vector3 genPos;

    [SerializeField] int dis;

    float dur;

    public List<GameObject> segs;
    public bool onOff;
    [SerializeField] bool bounced = false;
    [SerializeField] Vector3 pos;
    public GameObject gate;

    public GameObject unGate;

    public bool onGate;
    [SerializeField] bool oldOnOff;
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
        dis = 0;
        dir = genDir;
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
        if (onOff != oldOnOff)
        {
            dir = genDir;
            oldOnOff = onOff;
        }
        if (onOff)
        {
            if (dis < 20)
            {
                if (dur > 0)
                {
                    dur -= Time.deltaTime;
                }
                else
                {
                    dur = 0.05f;
                    dis += 1;
                    genAtPos();
                }
            }
        }
        else
        {
            if (segs.Count > 0)
            {
                if (dur > 0)
                {
                    dur -= Time.deltaTime;
                }
                else
                {
                    dur = 0.05f;

                    Destroy(segs[0]);
                    segs.RemoveAt(0);
                }

                

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
                                origin.GetComponent<power>().circuit.Remove(gate);
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
            bounced = false;
            dir = genDir;
            dis = 0;
        }
    }

    void genAtPos()
    {
        bool gen = true;

        if (!bounced)
        {
            pos = transform.position;
        }

        genPos = pos + new Vector3(dir.x * dis, 0, dir.z * dis);

        RaycastHit hit;
        if (Physics.Raycast(genPos + new Vector3(0, 0.6f, 0), Vector3.down, out hit, 0.2f))
        {
            string tag = hit.collider.gameObject.tag;
            GameObject hitObj = hit.collider.gameObject;

            if (tag == "light")
            {
                if (onGate)
                {
                    foreach (GameObject origin in GetComponent<gate>().origins)
                    {
                        if (!inList(hitObj, origin.GetComponent<power>().circuit))
                        {
                            origin.GetComponent<power>().circuit.Add(hitObj);
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

                dis = 100;
                gen = false;
            }

            if (tag == "block")
            {
                dis = 20;
                gen = false;
            }
            if (tag == "mirror")
            {
                Debug.Log(Mathf.RoundToInt(hitObj.transform.parent.transform.eulerAngles.y));
                // checks if the mirror is rotated 315 degrees to base the output in the correct direction
                if (Mathf.RoundToInt(hitObj.transform.parent.transform.eulerAngles.y) == 315 || Mathf.RoundToInt(hitObj.transform.parent.transform.eulerAngles.y) == 135 ) 
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
                dis = 0;
                pos = hitObj.transform.position;
                bounced = true;
                gen = false;
            }
      if (tag == "gate")
            {
                if (hitObj.GetComponent<gate>().type == 0)
                {
                    if (hitObj.transform.eulerAngles.y == 0 && dir.z == 1 || hitObj.transform.eulerAngles.y == 180 && dir.z == -1 || hitObj.transform.eulerAngles.y == 90 && dir.x == 1 || hitObj.transform.eulerAngles.y == 270 && dir.x == -1)
                    {
                        hitObj.GetComponent<gate>().inputs.Add(gameObject);
                        gate = hitObj;
                        dis = 100;
                        gen = false;

                        if (onGate)
                        {
                            foreach (GameObject origin in GetComponent<gate>().origins)
                            {
                                if (!hitObj.GetComponent<gate>().origins.Contains(origin))
                                {
                                    hitObj.GetComponent<gate>().origins.Add(origin);
                                }
                            }
                        }
                        else
                        {
                            if (!hitObj.GetComponent<gate>().origins.Contains(gameObject))
                            {
                                hitObj.GetComponent<gate>().origins.Add(gameObject);
                            }
                        }
                    }
                }
                else
                {
                    if (hitObj.transform.eulerAngles.y == 90 && dir.z != 0 || hitObj.transform.eulerAngles.y == 270 && dir.z != 0 || hitObj.transform.eulerAngles.y == 0 && dir.x != 0 || hitObj.transform.eulerAngles.y == 180 && dir.x != 0)
                    {
                        hitObj.GetComponent<gate>().inputs.Add(gameObject);
                        gate = hitObj;
                        dis = 100;
                        gen = false;

                        if (onGate)
                        {
                            foreach (GameObject origin in GetComponent<gate>().origins)
                            {
                                if (!hitObj.GetComponent<gate>().origins.Contains(origin))
                                {
                                    hitObj.GetComponent<gate>().origins.Add(origin);
                                }
                            }
                        }
                        else
                        {
                            if (!hitObj.GetComponent<gate>().origins.Contains(gameObject))
                            {
                                hitObj.GetComponent<gate>().origins.Add(gameObject);
                            }
                        }
                    }
                    else
                    {
                        dis = 100;
                        gen = false;
                    }
                }

                if (onGate)
                {
                    foreach (GameObject origin in GetComponent<gate>().origins)
                    {
                        if (!inList(hitObj, origin.GetComponent<power>().circuit))
                        {
                            origin.GetComponent<power>().circuit.Add(hitObj);
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
            }


        }

        if (gen)
        {
            GameObject seg = Instantiate(laserSeg, genPos, Quaternion.identity);
            if (dir.x != 0)
            {
                seg.transform.eulerAngles = new Vector3(0, 90, 0);
            }
            seg.name = "laserSeg";

            segs.Add(seg);
            Debug.Log(segs.Last());
        }


    }

    bool inList(GameObject obj, List<GameObject> objs)
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
