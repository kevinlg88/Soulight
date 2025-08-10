using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] GameObject npcDialoguePanel;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            npcDialoguePanel.SetActive(true);
        }  
    }
}
