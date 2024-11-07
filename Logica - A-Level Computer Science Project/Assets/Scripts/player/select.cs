using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class select : MonoBehaviour
{
    [SerializeField] Transform cameraT;
    public Vector3 posDistance;
    public Vector3 placePos;
    public Vector3 tempPos;
   [SerializeField] LayerMask mask;
    public GameObject outlineObj;
    public GameObject selectPlaced;
    public GameObject hitObj;
    public int selectInt;

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraT.position, cameraT.forward, out hit, 5, mask)) {
            hitObj = hit.collider.gameObject;   
            posDistance = hit.point - cameraT.forward * 0.01f;
            if (hitObj != null)
            {
                if (placePos != null)
                {
                    tempPos = placePos;
                }

                placePos = new Vector3(Mathf.RoundToInt(posDistance.x), Mathf.FloorToInt(posDistance.y), Mathf.RoundToInt(posDistance.z));

                if (hit.collider.gameObject.tag == "Ground" && selectInt == 0)
                {
                    selectPlaced = Instantiate(outlineObj, placePos, Quaternion.identity);
                    selectPlaced.name = "selectOutline";
                    selectInt = 1;
                    if (tempPos != placePos)
                    {
                        hitObj = null;
                    }

                }

                if (placePos.y == 1 && selectInt == 0)
                {
                    if (hit.collider.transform.position.y == 1)
                    {
                        selectPlaced = Instantiate(outlineObj, hit.collider.transform.position, Quaternion.identity);
                        selectPlaced.name = "selectOutline"; // :(
                        selectInt = 1;
                    }
                    if (tempPos != placePos)
                    {
                        hitObj = null;
                    }

                }
                else
                {
                    if (selectInt == 1 && tempPos != placePos)
                    {
                        Destroy(selectPlaced);
                        selectInt = 0;
                    }
                }
            }
                
            
        }
        else 
        {
            Destroy(selectPlaced);
            selectInt = 0;
        }
        }
}