using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class multipleChoice : MonoBehaviour
{
    public Vector3 pos;
    public GameObject tick;
    public bool win;
    public Transform canvas;
    [SerializeField] Vector3 oldPos;
    [SerializeField]GameObject tickNew;
    string name;
    public int LevelNum;

    public TextMeshProUGUI scoreDisp;
    float pScore;
    int finalScore;

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
    public void ticked(Transform transform)
    {
        if(oldPos == null)
        {
            pos = transform.position;
            GameObject temp = Instantiate(tick, pos, Quaternion.identity);
            temp.name = "tick";
            temp.transform.SetParent(canvas);
            tickNew = temp;
        }
        else
        {
            Destroy(tickNew);
            pos = transform.position;
            GameObject temp = Instantiate(tick, pos, Quaternion.identity);
            temp.name = "tick";
            temp.transform.SetParent(canvas);
            oldPos = pos;
            tickNew = temp;
        }


    }
    public void setButton(Button button)
    {
        name = button.name;
    }
    public void ifwin()
    {
       if(name == "M")
        {
            win = true;
        }
    }
}
