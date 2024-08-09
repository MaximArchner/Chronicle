using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

            RectTransform rectTransform = trackerPrefab.GetComponent<RectTransform>();
            if (rectTransform != null && trackedQuest.info.secondRequirementItem != "")
            {
                rectTransform.sizeDelta = new Vector2(420, 130);
            }

            QuestTracking questTracking = trackerPrefab.GetComponent<QuestTracking>();

            questTracking.questName.text = trackedQuest.questName;

            var req1 = trackedQuest.info.firstRequirementItem;
            var req1Amount = trackedQuest.info.firstRequirementAmount;
            var req2 = trackedQuest.info.secondRequirementItem;
            var req2Amount = trackedQuest.info.secondRequirementAmount;


            if (trackedQuest.info.secondRequirementItem != "")
            {
                questTracking.questRequirement.text = $"{req1} " + InventorySystem.Instance.CheckItemAmount(req1) + "/" + $"{req1Amount}\n" +
                    $"{req2} " + InventorySystem.Instance.CheckItemAmount(req2) + "/" + $"{req2Amount}\n";
            }
            else
            {
                questTracking.questRequirement.text = $"{req1} " + InventorySystem.Instance.CheckItemAmount(req1) + "/" + $"{req1Amount}\n";
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
        allActiveQuests.Remove(quest);
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

            quests.theQuest = activeQuest;

            quests.questName.text = activeQuest.questName;
            quests.questGiver.text = activeQuest.questGiver;

            quests.isActive = true;
            quests.isTracking = true;

            if (activeQuest.info.rewardItem1 != "")
            {
                quests.countableReward.sprite = GetSpriteForItem(activeQuest.info.rewardItem1);
                quests.countableRewardAmount.text = activeQuest.info.rewardItem1Amount.ToString();
            }
            else
            {
                quests.countableReward.gameObject.SetActive(false);
                quests.countableRewardAmount.text = "";
            }

            if (activeQuest.info.rewardItem2 != "")
            {
                quests.uncountableReward.sprite = GetSpriteForItem(activeQuest.info.rewardItem2);
            }
            else
            {
                quests.uncountableReward.gameObject.SetActive(false);
            }

            if (activeQuest.info.rewardItem1 == "" && activeQuest.info.rewardItem2 == "")
            {
                quests.gameObject.transform.Find("Rewards").gameObject.SetActive(false);
            }
        }

        foreach (Quest completedQuest in allCompletedQuests)
        {
            GameObject questPrefab = Instantiate(completedQuestPrefab, Vector3.zero, Quaternion.identity);
            questPrefab.transform.SetParent(questMenuContent.transform, false);

            QuestsInProgress quests = questPrefab.GetComponent<QuestsInProgress>();

            quests.questName.text = completedQuest.questName;
            quests.questGiver.text = completedQuest.questGiver;

            quests.isActive = false; // Mark as not active since it's completed
            quests.isTracking = false;

            if (completedQuest.info.rewardItem1 != "")
            {
                quests.countableReward.sprite = GetSpriteForItem(completedQuest.info.rewardItem1);
                quests.countableRewardAmount.text = completedQuest.info.rewardItem1Amount.ToString();
            }
            else
            {
                quests.countableReward.gameObject.SetActive(false);
                quests.countableRewardAmount.text = "";
            }

            if (completedQuest.info.rewardItem2 != "")
            {
                quests.uncountableReward.sprite = GetSpriteForItem(completedQuest.info.rewardItem2);
            }
            else
            {
                quests.uncountableReward.gameObject.SetActive(false);
            }
        }
    }

    private Sprite GetSpriteForItem(string item)
    {
        var itemToGet = Resources.Load<GameObject>(item);
        return itemToGet.GetComponent<Image>().sprite;
    }
}
