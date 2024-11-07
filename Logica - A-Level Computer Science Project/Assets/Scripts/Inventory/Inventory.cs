using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public List<int> inv;

    public List<Item> items;

    [SerializeField] List<Image> slotImgs;
    [SerializeField] List<Image> barImgs;
    [SerializeField] List<RectTransform> slotRTs;
    [SerializeField] List<RectTransform> barRTs;

    Item item;

    [SerializeField] int dragItem;

    public Vector2 mPos;

    [SerializeField] Image dragImg;
    [SerializeField] RectTransform dragRT;
    [SerializeField] RectTransform selectedRT;
    float temp;
    [SerializeField] TextMeshProUGUI nameDisp;

    public int select; // the index of my hotbar
    void Update()
    {

        //slotRender
        for (int i = 0; i < inv.Count; i += 1)
        {
            if (inv[i] == 999)
            {
                slotImgs[i].color = new Color32(255, 255, 255, 0);
                barImgs[i].color = new Color32(255, 255, 255, 0);
            }
            else
            {
                slotImgs[i].color = new Color32(255, 255, 255, 255);
                barImgs[i].color = new Color32(255, 255, 255, 255);

                item = items[inv[i]];
                slotImgs[i].sprite = item.sprite;
                barImgs[i].sprite = item.sprite;
            }
        }

        mPos = Input.mousePosition - new Vector3(960, 540, 0);

        if (dragItem != 999)
        {
            dragImg.color = new Color32(255, 255, 255, 255);
            dragImg.sprite = items[dragItem].sprite;

            dragRT.anchoredPosition = mPos;
        }
        else
        {
            dragImg.color = new Color32(255, 255, 255, 0);
        }

        temp = Input.GetAxis("Mouse ScrollWheel");
        if (temp == 0.1f)
        {
            select -= 1;
        }
        if (temp == -0.1f)
        {
            select += 1;
        }

        if (select > 8)
        {
            select = 0;
        }

        if (select < 0)
        {
            select = 8;
        }

        selectedRT.anchoredPosition = barRTs[select].anchoredPosition;

        if (inv[select] != 999)
        {
            nameDisp.text = items[inv[select]].itemName;
        }

    }
   

    public void slotClicked(int j)
    {
        int a = inv[j];
        inv[j] = dragItem;
        dragItem = a;
    }

}
