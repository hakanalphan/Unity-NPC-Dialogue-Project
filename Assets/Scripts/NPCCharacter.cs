using UnityEngine;

public class NPCCharacter : MonoBehaviour
{
    [Header("NPC Bilgileri")]
    public string npcName = "Köylü";
    public NPCRole role;

    [Header("Diyaloglar")]
    [TextArea(3, 6)]
    public string[] dialogueLines;

    [Header("Görev İpucu")]
    public bool hasQuestClue = false;
    public string questClue = "";

    [Header("Etkileşim")]
    public float interactionRange = 3f;
    public GameObject interactionIndicator;

    private Transform player;
    private NPCInteractionUI uiManager;
    private bool playerNearby = false;
    private bool hasSpoken = false;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        uiManager = FindObjectOfType<NPCInteractionUI>();

        if (interactionIndicator != null)
            interactionIndicator.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= interactionRange && !playerNearby)
        {
            playerNearby = true;
            if (interactionIndicator != null)
                interactionIndicator.SetActive(true);

            if (uiManager != null)
                uiManager.ShowInteractionPrompt(this);
        }
        else if (distance > interactionRange && playerNearby)
        {
            playerNearby = false;
            if (interactionIndicator != null)
                interactionIndicator.SetActive(false);

            if (uiManager != null)
                uiManager.HideInteractionPrompt();
        }

        // E tuşu basıldığında etkileşım olsun
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public void Interact()
    {
        if (uiManager != null)
        {
            uiManager.ShowDialogue(this);

            if (hasQuestClue && !hasSpoken)
            {
                QuestManager questManager = FindObjectOfType<QuestManager>();
                if (questManager != null)
                {
                    questManager.CollectClue(questClue);
                }
                hasSpoken = true;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}

public enum NPCRole
{
    Mukhtar,      
    Blacksmith,   
    Elder,        
    Merchant,     
    Child,        
    Treasure      
}
