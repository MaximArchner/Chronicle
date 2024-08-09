using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestsInProgress : MonoBehaviour
{
    public TextMeshProUGUI questName;
    public TextMeshProUGUI questGiver;

    public Button trackingButton;

    public bool isActive;
    public bool isTracking;

    public Image countableReward;
    public TextMeshProUGUI countableRewardAmount;

    public Image uncountableReward;

    public Quest theQuest;

    private void Start()
    {
        trackingButton.onClick.AddListener(() =>
        {
            if (isActive)
            {
                if (isTracking)
                {
                    isTracking = false;
                    trackingButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Not Tracking";
                    QuestManager.Instance.UntrackQuest(theQuest);
                }
                else
                {
                    isTracking = true;
                    trackingButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Tracking";
                    QuestManager.Instance.TrackQuest(theQuest);
                } 
            }
        });
    }
}
