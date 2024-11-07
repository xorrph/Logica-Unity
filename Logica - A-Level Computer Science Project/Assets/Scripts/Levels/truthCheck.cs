using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class truthCheck : MonoBehaviour
{
    public TMP_InputField TL;
    public TMP_InputField ML;
    public TMP_InputField BL;
    public TMP_InputField BM;
    public TMP_InputField BR;
    public List<levelSO> levels;
    public List<string> answers;
    int step;
    public int level; //up to 18
    public bool win;
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


    public void truthInput()
    {

        List<string> temp = new List<string> { TL.text, ML.text, BL.text, BM.text, BR.text };
        answers = temp;

        step = 0;
        foreach (string i in answers) {
            if (step < levels[level].truthOrder.Count)
            {
                if (i == levels[level].truthOrder[step])
                {
                    step += 1;
                   
                }
            }
        }

        if (step == levels[level].truthOrder.Count)
        {
            win = true;
        }
    }
    
}
