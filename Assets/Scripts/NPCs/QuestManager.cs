using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public List<Quest> allActiveQuests;
    public List<Quest> allCompletedQuests;

    public GameObject questMenu;
    public bool isQuestMenuOpen;

    public GameObject activeQuestPrefab;
    public GameObject completedQuestPrefab;

    public GameObject questMenuContent;

    [Header("Quest Tracker")]
    public GameObject questTrackerContent;
    public GameObject trackedQuestPrefab;

    public List<Quest> allTrackedQuests;

    public void TrackQuest(Quest quest)
    {
        allTrackedQuests.Add(quest);
        RefreshTrackerList();
    }

    public void UntrackQuest(Quest quest)
    {
        allTrackedQuests.Remove(quest);
        RefreshTrackerList();
    }

    public void RefreshTrackerList()
    {
        foreach (Transform child in questTrackerContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Quest trackedQuest in allTrackedQuests)
        {
            GameObject trackerPrefab = Instantiate(trackedQuestPrefab, Vector3.zero, Quaternion.identity);
            trackerPrefab.transform.SetParent(questTrackerContent.transform, false);

            QuestTracking questTracking = trackerPrefab.GetComponent<QuestTracking>();

            questTracking.questName.text = trackedQuest.questName;

            if (trackedQuest.info.secondRequirementItem != "")
            {
                questTracking.questRequirement.text = $"{trackedQuest.info.firstRequirementItem}" + " " + InventorySystem.Instance.CheckItemAmount(trackedQuest.info.firstRequirementItem) + "/" + $"{trackedQuest.info.firstRequirementAmount}\n" +
                    $"{trackedQuest.info.secondRequirementItem}" + " " + InventorySystem.Instance.CheckItemAmount(trackedQuest.info.secondRequirementItem) + "/" + $"{trackedQuest.info.secondRequirementAmount}\n";
            }
            else
            {
                questTracking.questRequirement.text = $"{trackedQuest.info.firstRequirementItem}" + " 0/" + $"{trackedQuest.info.firstRequirementAmount}\n";
            }
        }
    }

    public void Update()
    {
        if (!isQuestMenuOpen && Input.GetKeyDown(KeyCode.G) && !MenuManager.Instance.isMenuOpen)
        {
            isQuestMenuOpen = true;
            questMenu.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (isQuestMenuOpen && Input.GetKeyDown(KeyCode.G))
        {
            isQuestMenuOpen = false;
            questMenu.SetActive(false);

            if (!CraftingSystem.Instance.isOpen && !InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
    public void AddActiveQuest(Quest quest)
    {
        allActiveQuests.Add(quest);
        TrackQuest(quest);
        RefreshQuestList();
    }

    public void MarkQuestCompleted(Quest quest)
    {
        // Gorevi aktif gorevler listesinden silme
        allActiveQuests.Remove(quest);

        // Gorevi tamamlanmis gorevler listesine ekleme
        allCompletedQuests.Add(quest);

        UntrackQuest(quest);

        RefreshQuestList();
    }
    public void RefreshQuestList()
    {
        foreach (Transform child in questMenuContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Quest activeQuest in allActiveQuests)
        {
            GameObject questPrefab = Instantiate(activeQuestPrefab, Vector3.zero, Quaternion.identity);
            questPrefab.transform.SetParent(questMenuContent.transform, false);

            QuestsInProgress quests = questPrefab.GetComponent<QuestsInProgress>();

            quests.questName.text = activeQuest.questName;
            quests.questGiver.text = activeQuest.questGiver;

            quests.isActive = true;
            quests.isTracking = true;

            // quests.countableReward.sprite = "";
            quests.countableRewardAmount.text = "0";

            // quests.uncountableReward.sprite = "";
        }

        foreach (Quest completedQuest in allActiveQuests)
        {
            GameObject questPrefab = Instantiate(completedQuestPrefab, Vector3.zero, Quaternion.identity);
            questPrefab.transform.SetParent(questMenuContent.transform, false);

            QuestsInProgress quests = questPrefab.GetComponent<QuestsInProgress>();

            quests.questName.text = completedQuest.questName;
            quests.questGiver.text = completedQuest.questGiver;

            quests.isActive = true;
            quests.isTracking = true;

            // quests.countableReward.sprite = "";
            quests.countableRewardAmount.text = "0";

            // quests.uncountableReward.sprite = "";
        }
    }
}
