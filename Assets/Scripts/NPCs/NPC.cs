using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public bool playerInRange;
    public bool isTalkingWithPlayer = false;
    private InteractableObject interactable;
    string npcName;
    public bool nameLearned;

    TextMeshProUGUI npcDialogText;

    Button option1;
    TextMeshProUGUI option1Text;

    Button option2;
    TextMeshProUGUI option2Text;

    Button option3;
    TextMeshProUGUI option3Text;

    Image npcImage;

    public List<Quest> quests;
    public Quest currentActiveQuest = null;
    public int activeQuestIndex = 0;
    public bool firstTimeInteraction = true;
    public int currentDialog;

    private void Start()
    {
        interactable = GetComponent<InteractableObject>();
        npcName = GetComponent<InteractableObject>().npcName;

        npcDialogText = DialogSystem.Instance.dialogText;
        option1 = DialogSystem.Instance.option1;
        option1Text = DialogSystem.Instance.option1.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        option2 = DialogSystem.Instance.option2;
        option2Text = DialogSystem.Instance.option2.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        option3 = DialogSystem.Instance.option3;
        option3Text = DialogSystem.Instance.option3.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        npcImage = DialogSystem.Instance.npcImage.GetComponent<Image>();
    }

    private void Update()
    {
        playerInRange = interactable.playerInRange;
        
        if (this.gameObject.transform.name.Contains("Samantha"))
        {
            npcName = "Samantha";
            interactable.npcName = npcName;
        }

        if (playerInRange == true)
        {
            if (Input.GetKeyDown(KeyCode.F) && !isTalkingWithPlayer)
            {
                StartConversation();
                nameLearned = true;
            }
            else
            {
                isTalkingWithPlayer = false;
            }
        }
    }

    public void LookAtPlayer()
    {
        var player = PlayerState.Instance.playerBody.transform;
        Vector3 direction = player.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, yRotation, 2);
    }
    public void StartConversation()
    {
        isTalkingWithPlayer = true;
        LookAtPlayer();

        if (firstTimeInteraction) // NPC ile ilk karsilasma
        {
            firstTimeInteraction = false;
            currentActiveQuest = quests[activeQuestIndex];
            StartQuestInitialDialog();
            currentDialog = 0;
        }
        else // NPC ile daha onceden karsilastiysa/konustuysa
        {
            // Eger gorevi reddeddikten sonra geldiysek
            if (currentActiveQuest.declined)
            {
                DialogSystem.Instance.OpenDialogUI();

                npcDialogText.text = currentActiveQuest.info.comebackAfterDecline;

                SetAcceptAndDeclineOptions();
            }

            if(currentActiveQuest.accepted && currentActiveQuest.isCompleted == false)
            {
                if (AreQuestRequirementsMet())
                {
                    SubmitRequirements();

                    DialogSystem.Instance.OpenDialogUI();

                    npcDialogText.text = currentActiveQuest.info.comebackCompleted;

                    option1Text.text = "[Take Reward]";
                    option1.onClick.RemoveAllListeners();
                    option1.onClick.AddListener(() =>
                    {
                        ReceiveRewardAndCompleteQuest();
                    });
                }
                else
                {
                    DialogSystem.Instance.OpenDialogUI();

                    npcDialogText.text = currentActiveQuest.info.comebackInProgress;

                    CloseDialogUI();
                }
            }

            if (currentActiveQuest.isCompleted == true)
            {
                DialogSystem.Instance.OpenDialogUI();

                npcDialogText.text = currentActiveQuest.info.finalWords;

                CloseDialogUI();
            }

            // Baska bir alinabilecek gorev varsa
            if (currentActiveQuest.initialDialogCompleted == false)
            {
                StartQuestInitialDialog();
            }
        }

        #region HardCoded Dialog ||
        //DialogSystem.Instance.OpenDialogUI();
        //DialogSystem.Instance.dialogText.text = "Hi! I'm Samantha.";
        //DialogSystem.Instance.option1.gameObject.SetActive(true);
        //DialogSystem.Instance.option1.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "Who are you? Where am I?";
        //DialogSystem.Instance.option2.gameObject.SetActive(true);
        //DialogSystem.Instance.option2.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "I have to go.";
        //DialogSystem.Instance.option2.onClick.AddListener(() =>
        //{
        //    DialogSystem.Instance.CloseDialogUI();
        //    isTalkingWithPlayer = false;
        //});
        //DialogSystem.Instance.option1.onClick.AddListener(() =>
        //{
        //    DialogSystem.Instance.dialogText.text = "I'm sorry sweetie, I can't tell you.";
        //    DialogSystem.Instance.option1.gameObject.SetActive(false);
        //    DialogSystem.Instance.option2.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = "What? How?";
        //});
        #endregion
    }

    private void SubmitRequirements()
    {
        string firstRequiredItem = currentActiveQuest.info.firstRequirementItem;
        int firstRequiredAmount = currentActiveQuest.info.firstRequirementAmount;

        if (firstRequiredItem != "")
        {
            InventorySystem.Instance.RemoveItem(firstRequiredItem, firstRequiredAmount);
        }

        string secondRequiredItem = currentActiveQuest.info.secondRequirementItem;
        int secondRequiredAmount = currentActiveQuest.info.secondRequirementAmount;

        if (secondRequiredItem != "")
        {
            InventorySystem.Instance.RemoveItem(secondRequiredItem, secondRequiredAmount);
        }
    }

    private bool AreQuestRequirementsMet()
    {
        print("Checking Requirements");

        // Ilk gorev sarti

        string firstRequiredItem = currentActiveQuest.info.firstRequirementItem;
        int firstRequiredAmount = currentActiveQuest.info.firstRequirementAmount;

        var firstItemCounter = 0;

        foreach (string item in InventorySystem.Instance.itemList)
        {
            if (item == firstRequiredItem)
            {
                firstItemCounter++;
            }
        }

        // Ikinci gorev sarti

        string secondRequiredItem = currentActiveQuest.info.secondRequirementItem;
        int secondRequiredAmount = currentActiveQuest.info.secondRequirementAmount;

        var secondItemCounter = 0;

        foreach (string item in InventorySystem.Instance.itemList)
        {
            if (item == secondRequiredItem)
            {
                secondItemCounter++;
            }
        }

        if (firstItemCounter >= firstRequiredAmount && secondItemCounter >= secondRequiredAmount) 
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void StartQuestInitialDialog()
    {
        DialogSystem.Instance.OpenDialogUI();

        npcDialogText.text = currentActiveQuest.info.initialDialog[currentDialog];
        option1.gameObject.SetActive(true);
        option1Text.text = "Next";
        option1.onClick.RemoveAllListeners();
        option1.onClick.AddListener(() =>
        {
            currentDialog++;
            CheckIfDialogDone();
        });

        option2.gameObject.SetActive(false);

        option3.gameObject.SetActive(false);
    }

    private void CheckIfDialogDone()
    {
        if (currentDialog == currentActiveQuest.info.initialDialog.Count - 1) // Son diyalogdan bir oncesi ise
        {
            npcDialogText.text = currentActiveQuest.info.initialDialog[currentDialog];

            currentActiveQuest.initialDialogCompleted = true;

            SetAcceptAndDeclineOptions();
        }
        else
        {
            npcDialogText.text = currentActiveQuest.info.initialDialog[currentDialog];

            option1Text.text = "Next";
            option1.onClick.RemoveAllListeners();
            option1.onClick.AddListener(() =>
            {
                currentDialog++;
                CheckIfDialogDone();
            });
        }
    }

    private void AcceptedQuest()
    {
        QuestManager.Instance.AddActiveQuest(currentActiveQuest);
        currentActiveQuest.accepted = true;
        currentActiveQuest.declined = false;

        if (currentActiveQuest.hasNoRequirements)
        {
            npcDialogText.text = currentActiveQuest.info.comebackCompleted;
            option1Text.text = "[Take Reward]";
            option1.onClick.RemoveAllListeners();
            option1.onClick.AddListener(() =>
            {
                ReceiveRewardAndCompleteQuest();
            });
            option2.gameObject.SetActive(false);
        }
        else
        {
            npcDialogText.text = currentActiveQuest.info.acceptAnswer;
            CloseDialogUI();
        }
    }

    private void ReceiveRewardAndCompleteQuest()
    {
        currentActiveQuest.isCompleted = true;
        QuestManager.Instance.MarkQuestCompleted(currentActiveQuest);

        if (currentActiveQuest.info.rewardItem1 != "")
        {
            StartCoroutine(RewardThatAmount(currentActiveQuest.info.rewardItem1Amount));
        }

        if (currentActiveQuest.info.rewardItem2 != "")
        {
            InventorySystem.Instance.AddToInventory(currentActiveQuest.info.rewardItem2, false);
        }

        activeQuestIndex++;

        if (activeQuestIndex < quests.Count)
        {
            currentActiveQuest = quests[activeQuestIndex];
            currentDialog = 0;
            DialogSystem.Instance.CloseDialogUI();
            isTalkingWithPlayer = false;
        }
        else
        {
            DialogSystem.Instance.CloseDialogUI();
            isTalkingWithPlayer = false;
            print("No more quests");
        }
    }

    private IEnumerator RewardThatAmount(int rewardItem1Amount)
    {
        for (int i  = 0; i < rewardItem1Amount; i++)
        {
            InventorySystem.Instance.AddToInventory(currentActiveQuest.info.rewardItem1, true);
        }
        yield return null;
    }

    private void DeclinedQuest()
    {
        currentActiveQuest.declined = true;

        npcDialogText.text = currentActiveQuest.info.declineAnswer;
        CloseDialogUI();
    }

    private void SetAcceptAndDeclineOptions()
    {
        option1Text.text = currentActiveQuest.info.acceptOption;
        option1.onClick.RemoveAllListeners();
        option1.onClick.AddListener(() =>
        {
            AcceptedQuest();
        });

        option2.gameObject.SetActive(true);
        option2Text.text = currentActiveQuest.info.declineOption;
        option2.onClick.RemoveAllListeners();
        option2.onClick.AddListener(() =>
        {
            DeclinedQuest();
        });
    }

    private void CloseDialogUI()
    {
        option1Text.text = "[Close]";
        option1.onClick.RemoveAllListeners();
        option1.onClick.AddListener(() =>
        {
            DialogSystem.Instance.CloseDialogUI();
            isTalkingWithPlayer = false;
        });
        option2.gameObject.SetActive(false);
    }
}
