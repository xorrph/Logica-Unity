using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIprompt : MonoBehaviour
{
    public Transform cameraT;
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private TextMeshProUGUI promptText;

    // Start is called before the first frame update
    void Start()
    {
        uiPanel.SetActive(false);
    }

    public bool displayed = false;

    // Update is called once per frame
    public void SetUp(string _promptText)
    {
        promptText.text = _promptText;
        uiPanel.SetActive(true);
        displayed = true;
    }

    public void Close()
    {
        uiPanel.SetActive(false);
        displayed = false;
    }

    void Update()
    {
        transform.forward = cameraT.forward;
    }
}
