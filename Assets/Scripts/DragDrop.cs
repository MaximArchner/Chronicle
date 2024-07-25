using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;

    private void Awake()
    {

        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f;
        //Böylelikle ray cast öðeyi görmezden gelecek
        canvasGroup.blocksRaycasts = false;
        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(transform.root);
        itemBeingDragged = gameObject;

    }

    public void OnDrag(PointerEventData eventData)
    {
        //Öge faremizle ayný hýzda hareket edecek
        rectTransform.anchoredPosition += eventData.delta;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var tempItemReference = itemBeingDragged;
        itemBeingDragged = null;

        if (tempItemReference.transform.parent == tempItemReference.transform.root)
        {
            tempItemReference.SetActive(false);

            AlertDialogManager dialogManager = FindObjectOfType<AlertDialogManager>();

            dialogManager.ShowDialog("Do you want to drop this item?", (response) =>
            {
                if (response)
                {
                    DropItemIntoTheWorld(tempItemReference);
                }
                else
                {
                    CancelDragging(tempItemReference);
                }
            });
        }

        if (tempItemReference.transform.parent == startParent)
        {
            CancelDragging(tempItemReference);
        }

        if (tempItemReference.transform.parent != tempItemReference.transform.root
              && tempItemReference.transform.parent != startParent)
        {
            if (tempItemReference.transform.parent.childCount > 98)
            {
                CancelDragging(tempItemReference);
            }
            else
            {
                if (Input.GetKey(KeyCode.Z))
                {
                    DivideStack(tempItemReference);
                }
            }
        }

        if (transform.parent == startParent || transform.parent == transform.root)
        {
            CancelDragging(tempItemReference);
        }

        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    private void DropItemIntoTheWorld(GameObject tempItemReference)
    {
        // Eger envanterde itemden birden fazla varsa her esya attiginda stack miktarini dusurme
        if (tempItemReference.GetComponent<InventoryItem>().amountInInventory > 1)
        {
            tempItemReference.GetComponent<InventoryItem>().amountInInventory--;
        }

        // Birakilacak objenin ismini temizleme
        string cleanName = tempItemReference.name.Split(new string[] {"(Clone)"}, System.StringSplitOptions.None)[0];

        // Objeyi dosyadan cagirma
        GameObject item = Instantiate(Resources.Load<GameObject>(cleanName + "_Model"));

        // Cagirilacak yerin random lokasyonu
        float minOffset = 1.5f;
        float maxOffset = 3f;
        Vector3 randomOffset = new Vector3(
            Random.Range(minOffset, maxOffset),
            0, // y degeri orijinal hizasinda olabilir
            Random.Range(minOffset, maxOffset)
        );

        // Randomly decide positive or negative direction for x and z offsets
        randomOffset.x *= Random.Range(0, 2) == 0 ? 1 : -1;
        randomOffset.z *= Random.Range(0, 2) == 0 ? 1 : -1;

        var dropSpawnPosition = PlayerState.Instance.playerBody.transform.Find("DropSpawn").transform.position;
        item.transform.position = dropSpawnPosition + randomOffset;

        // Objeyi olusturduktan sonra player'dan cikarip Collectibles grubuna dahil etme
        var itemsObject = FindObjectOfType<EnvironmentManager>().gameObject.transform.Find("Collectibles");
        item.transform.SetParent(itemsObject.transform);

        if (tempItemReference.GetComponent<InventoryItem>().amountInInventory == 1)
        {// Atilan objeyi envanterden silme
            DestroyImmediate(tempItemReference.gameObject);
        }

        InventorySystem.Instance.ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }

    private void DivideStack(GameObject tempItemReference)
    {
        InventoryItem item = tempItemReference.GetComponent<InventoryItem>();

        if (item.amountInInventory > 1)
        {
            item.amountInInventory -= 1;
            InventorySystem.Instance.AddToInventory(item.thisName, false);
        }
    }

    private void CancelDragging(GameObject tempItemReference)
    {
        transform.position = startPosition;
        transform.SetParent(startParent);

        tempItemReference.SetActive(true);
    }
}
