using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCInteractionUI : MonoBehaviour
{
    [Header("UI Elemanları")]
    public GameObject interactionPrompt;
    public TextMeshProUGUI promptText;

    public GameObject dialoguePanel;
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;
    public Button closeButton;

    [Header("Görev Paneli")]
    public GameObject questPanel;
    public TextMeshProUGUI questText;

    private NPCCharacter currentNPC;
    private int currentLineIndex = 0;

    void Start()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        // Butonlara koddan da listener ekleyelim (Inspector’dan da ekliyse sorun olmaz)
        if (nextButton != null)
            nextButton.onClick.AddListener(NextLine);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseDialogue);
    }

    public void ShowInteractionPrompt(NPCCharacter npc)
    {
        currentNPC = npc;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(true);

        if (promptText != null)
            promptText.text = "E tuşuna bas - " + npc.npcName;
    }

    public void HideInteractionPrompt()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        currentNPC = null;
    }

    public void ShowDialogue(NPCCharacter npc)
    {
        currentNPC = npc;
        currentLineIndex = 0;

        if (npc.dialogueLines == null || npc.dialogueLines.Length == 0)
        {
            Debug.LogWarning($"NPC '{npc.npcName}' için dialogueLines boş veya tanımlı değil!");
            return;
        }

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (npcNameText != null)
            npcNameText.text = npc.npcName;

        if (dialogueText != null)
            dialogueText.text = npc.dialogueLines[currentLineIndex];

        // Karakterin hareketini durdurmakk için
        Time.timeScale = 0f;

        // Mouse'u serbest bırak ve göster 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void NextLine()
    {
        if (currentNPC == null) return;
        if (currentNPC.dialogueLines == null || currentNPC.dialogueLines.Length == 0) return;

        currentLineIndex++;

        if (currentLineIndex >= currentNPC.dialogueLines.Length)
        {
            CloseDialogue();
            return;
        }

        if (dialogueText != null)
            dialogueText.text = currentNPC.dialogueLines[currentLineIndex];
    }

    public void CloseDialogue()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        currentNPC = null;

        // Oyunu devam ettir
        Time.timeScale = 1f;

        //  Mouse'u tekrar kilitler ve gizler
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UpdateQuestUI(string text)
    {
        if (questPanel != null && questText != null)
        {
            questText.text = text;
        }
    }
}
