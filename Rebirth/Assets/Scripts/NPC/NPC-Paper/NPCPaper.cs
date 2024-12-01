using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPaper : NPC
{
    public GameObject[] unDamaged;
    public GameObject Damaged;
    public GameObject Damaged_S;
    public GameObject Damaged_E;
    public GameObject Damaged_B;
    public GameObject Damaged_SB;
    public GameObject Damaged_BE;
    public GameObject Damaged_ES;

    private int currentPage = 0;
    private bool isImageShown = false;

    public override void HandleDialogueEnd()
    {
        base.HandleDialogueEnd();
        if(DialogueManager.Instance.getLastLeafNode().nodeID == "paper1")
        { 
            ShowDImage(currentPage);
            isImageShown = true;
        }
    }

    private void Update()
    {
        if (isImageShown)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isImageShown = false;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // ShowPreviousPage();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                // ShowNextPage();
            }
        }
    }

    private void ShowDImage(int index)
    {
        Damaged.SetActive(true);

    }

    private void ShowUDImage(int index)
    {
        unDamaged[index].SetActive(true);

    }

}
