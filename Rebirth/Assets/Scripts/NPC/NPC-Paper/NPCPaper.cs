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

    public NPC PaperCompleteNPC;

    private int currentPage = 0;
    private bool isImageShown = false;
    private bool isUDImageShown = false;

    // private bool hasInteractedWithNPC = false;

    public override void HandleDialogueEnd()
    {
        base.HandleDialogueEnd();
        if(DialogueManager.Instance.getLastLeafNode().nodeID == "paper1")
        { 
            ShowDImage();
            isImageShown = true;
            isUDImageShown = false;
        }

        if( DialogueManager.Instance.getLastLeafNode().nodeID == "B"){
            CharacterStatusManager.Instance.SetPaper(true, "b");
        }
        if( DialogueManager.Instance.getLastLeafNode().nodeID == "S"){
            CharacterStatusManager.Instance.SetPaper(true, "s");
        }
        if( DialogueManager.Instance.getLastLeafNode().nodeID == "E"){
            CharacterStatusManager.Instance.SetPaper(true, "e");
        }

        if(DialogueManager.Instance.getLastLeafNode().nodeID == "paper2" || DialogueManager.Instance.getLastLeafNode().nodeID == "B"
        || DialogueManager.Instance.getLastLeafNode().nodeID == "E" || DialogueManager.Instance.getLastLeafNode().nodeID == "S"){
            if(CharacterStatusManager.Instance.GetPaper("s"))
            {
                if(CharacterStatusManager.Instance.GetPaper("b"))
                {
                    if(CharacterStatusManager.Instance.GetPaper("e"))
                    {
                        ShowUDImage(currentPage);
                        isImageShown = true;
                        isUDImageShown = true;

                        NPC newNPC = Instantiate(PaperCompleteNPC) as NPC;
                        if (newNPC != null)
                        {
                            newNPC.Interact();
                        }
                        else
                        {
                            Debug.LogError("NPC 프리팹이 제대로 할당되지 않았습니다.");
                            return;
                        }
                    }
                    else
                    {
                        ShowEImage();
                        isImageShown = true;
                    }
                }
                else
                {
                    if(CharacterStatusManager.Instance.GetPaper("e"))
                    {
                        ShowBImage();
                        isImageShown = true;
                    }
                    else
                    {
                        ShowBEImage();
                        isImageShown = true;
                    }
                }
            }
            else
            {
                if(CharacterStatusManager.Instance.GetPaper("b"))
                {
                    if(CharacterStatusManager.Instance.GetPaper("e"))
                    {
                        ShowSImage();
                        isImageShown = true;
                    }
                    else
                    {
                        ShowESImage();
                        isImageShown = true;
                    }
                }
                else
                {
                    if(CharacterStatusManager.Instance.GetPaper("e"))
                    {
                        ShowSBImage();
                        isImageShown = true;
                    }
                    else
                    {   
                        ShowDImage();  
                        isImageShown = true;  
                    }
                }   
            }
        }
    }

    private void Update()
    {
        if (isImageShown)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isImageShown = false;
                Damaged.SetActive(false);
                Damaged_B.SetActive(false);
                Damaged_BE.SetActive(false);
                Damaged_E.SetActive(false);
                Damaged_ES.SetActive(false);
                Damaged_S.SetActive(false);
                Damaged_SB.SetActive(false);
                unDamaged[0].SetActive(false);
                unDamaged[1].SetActive(false);
                unDamaged[2].SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (currentPage > 0 && isUDImageShown)
                {
                    currentPage--;
                    ShowUDImage(currentPage);
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (currentPage < unDamaged.Length - 1 && isUDImageShown)
                {
                    currentPage++;
                    ShowUDImage(currentPage);
                }
            }
        }
    }

    private void ShowDImage()
    {
        Damaged.SetActive(true);

    }

    private void ShowSImage()
    {
        Damaged_S.SetActive(true);

    }

     private void ShowEImage()
    {
        Damaged_E.SetActive(true);

    }

    private void ShowBImage()
    {
        Damaged_B.SetActive(true);

    }

    private void ShowSBImage()
    {
        Damaged_SB.SetActive(true);

    }

    private void ShowBEImage()
    {
        Damaged_BE.SetActive(true);

    }

    private void ShowESImage()
    {
        Damaged_ES.SetActive(true);

    }

    private void ShowUDImage(int index)
    {
        unDamaged[index].SetActive(true);

    }


}
