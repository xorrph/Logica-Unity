using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class score : MonoBehaviour
{
    public TextMeshProUGUI scoreDisp;
    float pScore;
    int finalScore;
    private int timer;
    public winCheck winCheck;
    public int LevelNum;

    // Start is called before the first frame update
    void Start()
    {
        pScore = 3000f;
    }

    void Update()
    {
        if (pScore > 1)
        {
            if (!winCheck.win)
            {
                pScore -= 10 * Time.deltaTime;
                //We only need to update the text if the score changed.
                scoreDisp.text = "Score: " + Mathf.FloorToInt(pScore).ToString();

                //Reset the timer to 0.
                timer = 0;
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
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}