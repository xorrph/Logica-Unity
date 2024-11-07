using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask interactables;
    [SerializeField] private UIprompt uiPrompt;
    Collider[] col = new Collider[5];
    Collider[] oldCol = new Collider[5];
    [SerializeField] private int numFound;
    public bool pressed;

    private void Update()
    {
        pressed = Input.GetKeyDown("e");
        col = new Collider[5];
        numFound = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionPointRadius, col, interactables); // checks for interactrable object
        for (int i = 0; i < 5; i++)
        {
            if (oldCol[i] != col[i]) 
            {
                if (oldCol[i] != null)
                {
                    if (oldCol[i].gameObject.GetComponentInChildren<UIprompt>().displayed)
                    {
                        oldCol[i].gameObject.GetComponentInChildren<UIprompt>().Close();
                    }
                }
                oldCol[i] = col[i];
            }
        }
        foreach (Collider c in col)
        {
            if (c != null)
            {
                if (numFound > 0)
                {
                    if (!c.gameObject.GetComponentInChildren<UIprompt>().displayed) c.gameObject.GetComponentInChildren<UIprompt>().SetUp("Press E to turn ON and OFF");

                    if (pressed)
                    {
                        c.gameObject.GetComponent<power>().Interact();
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }
}
