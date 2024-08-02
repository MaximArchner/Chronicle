using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/QuestInfo", order = 1)]
public class QuestInfo : ScriptableObject
{
    [TextArea(5,10)]
    public List<string> initialDialog;

    [Header("Options")]

    [TextArea(5, 10)]
    public string acceptOption;

    [TextArea(5, 10)]
    public string acceptAnswer;

    [TextArea(5, 10)]
    public string declineOption;

    [TextArea(5, 10)]
    public string declineAnswer;

    [TextArea(5, 10)]
    public string comebackAfterDecline;

    [TextArea(5, 10)]
    public string comebackInProgress;

    [TextArea(5, 10)]
    public string comebackCompleted;

    [TextArea(5, 10)]
    public string finalWords;

    [Header("Rewards")]
    public string rewardItem1;
    public int rewardItem1Amount;

    public string rewardItem2;

    [Header("Requirements")]
    public string firstRequirementItem;
    public int firstRequirementAmount;

    public string secondRequirementItem;
    public int secondRequirementAmount;

}