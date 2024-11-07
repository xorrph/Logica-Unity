using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class matchUp : MonoBehaviour
{
    [SerializeField] string oldTag;
    [SerializeField] string tag;
    [SerializeField] Button oldButton;
    [SerializeField]int buttonsClicked;
    string oldName;
    string currentName;
    public int matched;
    public bool win;
    public bool match;
    public TextMeshProUGUI scoreDisp;
    float pScore;
    int finalScore;
    public int LevelNum;

    // Start is called before the first frame update
    void Start()
    {
        pScore = 600f;
    }

    void Update()
    {
        if (pScore > 1)
        {
            if (!win)
            {
                pScore -= 10 * Time.deltaTime;
                //We only need to update the text if the score changed.
                scoreDisp.text = "Score: " + Mathf.FloorToInt(pScore).ToString();
            }
            else
            {
                finalScore = (int)pScore;
                gameObject.GetComponent<student>().studentScore(finalScore, LevelNum);
                SceneManager.LoadScene(23);
            }
        }
        else
        {
            SceneManager.LoadScene(27);
        }
    }
    public void clicked(Button button)
    {
        if (buttonsClicked <= 2) {
            if (buttonsClicked==0)
            {
                tag = button.tag;
                currentName = button.name;
                oldButton = button;
            }
            else
            {
                oldTag = tag;
                oldName = currentName;
                tag = button.tag;
                currentName = button.name;
            }
            buttonsClicked++;
            if (oldButton != null)
            {
                if (oldTag == tag && currentName != oldName)//checks if it matches
                {
                    matched ++;
                    match = true;
                    buttonsClicked = 0;
                    oldButton.GetComponent<Image>().color = new Color32(255, 0, 0, 134);
                    button.GetComponent<Image>().color = new Color32(255, 0, 0, 134);
                }
                if (buttonsClicked == 2 && !match)
                {
                    oldButton.GetComponent<Image>().color = new Color32(255, 255, 255, 150);
                    button.GetComponent<Image>().color = new Color32(255, 255, 255, 150);
                }

            }
            if (buttonsClicked == 2)
            {
                buttonsClicked = 0;
                oldButton = null;
                oldTag = null;
                oldName = null;
            }
        }
        if(matched == 3)
        {
            win = true;
        }
    }
}
