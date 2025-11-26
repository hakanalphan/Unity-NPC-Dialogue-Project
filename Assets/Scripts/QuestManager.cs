using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [Header("Görev")]
    public List<string> collectedClues = new List<string>();
    public int totalClues = 5;

    private NPCInteractionUI uiManager;

    void Start()
    {
        uiManager = FindObjectOfType<NPCInteractionUI>();
        UpdateQuestUI();
    }

    public void CollectClue(string clue)
    {
        if (!collectedClues.Contains(clue))
        {
            collectedClues.Add(clue);
            Debug.Log("İpucu toplandı: " + clue);
            UpdateQuestUI();

            if (collectedClues.Count >= totalClues)
            {
                CompleteQuest();
            }
        }
    }

    void UpdateQuestUI()
    {
        string questInfo = $"Görev: Hazine İpuçları\n{collectedClues.Count}/{totalClues} ipucu toplandı";

        if (uiManager != null)
        {
            uiManager.UpdateQuestUI(questInfo);
        }
    }

    void CompleteQuest()
    {
        Debug.Log("GÖREV TAMAMLANDI!");
        if (uiManager != null)
        {
            uiManager.UpdateQuestUI("TEBRİKLER! Hazineyi buldun!\nTüm ipuçları toplandı!");
        }
    }
}
