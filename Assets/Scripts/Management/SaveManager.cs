using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; set; }
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

        DontDestroyOnLoad(gameObject);
    }

    // Json Project Save Path
    string jsonPathProject;
    // Json External/Real Save Path
    string jsonPathPersistent;
    // Binary Save Path
    string binaryPath;

    string fileName = "SaveGame";

    public bool isSavingToJson;

    public bool isLoading;

    public Canvas loadingScreen;

    private void Start()
    {
        jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar;
        jsonPathPersistent = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
        binaryPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
    }

    #region || General Section ||

    #region || Saving Section ||
    public void SaveGame(int slotNumber)
    {
        AllGameData data = new AllGameData();

        data.playerData = GetPlayerData();

        data.environmentData = GetEnvironmentData();

        SavingTypeSwitch(data, slotNumber);
    }

    private EnvironmentData GetEnvironmentData()
    {
        List<string> itemsPickedup = InventorySystem.Instance.itemsPickedup;

        return new EnvironmentData(itemsPickedup);
    }

    public PlayerData GetPlayerData()
    {
        float[] playerStats = new float[3];
        playerStats[0] = PlayerState.Instance.currentHealth;
        playerStats[1] = PlayerState.Instance.currentHunger;
        playerStats[2] = PlayerState.Instance.currentThirstPercent;

        float[] playerTransform = new float[6];
        playerTransform[0] = PlayerState.Instance.playerBody.transform.position.x;
        playerTransform[1] = PlayerState.Instance.playerBody.transform.position.y;
        playerTransform[2] = PlayerState.Instance.playerBody.transform.position.z;

        playerTransform[3] = PlayerState.Instance.playerBody.transform.rotation.x;
        playerTransform[4] = PlayerState.Instance.playerBody.transform.rotation.y;
        playerTransform[5] = PlayerState.Instance.playerBody.transform.rotation.z;

        List<InventoryItemData> inventory = new List<InventoryItemData>();

        foreach(GameObject slot in InventorySystem.Instance.slotList)
        {
            InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
            if (inventorySlot != null && inventorySlot.itemInSlot != null)
            {
                inventory.Add(new InventoryItemData(inventorySlot.itemInSlot.thisName, inventorySlot.itemInSlot.amountInInventory));
            }
        }

        // string[] quickSlots = GetQuickSlotsContent();

        return new PlayerData(playerStats, playerTransform, inventory);
    }

    // private string[] GetQuickSlotsContent()
    // {
    //      List<string> temp =  new List<string>();
    //      foreach(GameObject slot in EquipSystem.Instance.quickSlotsList)
    //      {
    //          if (slot.transform.childCount != 0)
    //          {
    //              string name = slot.transform.GetChild(0).name;
    //              string str2 = "(Clone)";
    //              string cleanName = nameReplace(str2, "");
    //              temp.Add(cleanName);
    //          }
    //      }
    //      return temp.ToArray();
    // }
    public void SavingTypeSwitch(AllGameData gameData, int slotNumber)
    {
        if (isSavingToJson)
        {
            SaveGameDataToJsonFile(gameData, slotNumber);
        }
        else
        {
            SaveGameDataToBinaryFile(gameData, slotNumber);
        }
    }
    #endregion

    #region || Loading Section ||
    public AllGameData LoadingTypeSwitch(int slotNumber)
    {
        if (isSavingToJson)
        {
            AllGameData gameData = LoadGameDataFromJsonFile(slotNumber);
            return gameData;
        }
        else
        {
            AllGameData gameData = LoadGameDataFromBinaryFile(slotNumber);
            return gameData;
        }
    }

    public void LoadGame(int slotNumber)
    {
        // Player Data
        SetPlayerData(LoadingTypeSwitch(slotNumber).playerData);

        //Environment Data
        SetEnvironmentData(LoadingTypeSwitch(slotNumber).environmentData);

        isLoading = false;
        DisableLoadingScreen();
    }

    private void SetEnvironmentData(EnvironmentData environmentData)
    {
        foreach (Transform itemType in EnvironmentManager.Instance.collectibles.transform)
        {
            foreach (Transform item in itemType.transform)
            {
                if (environmentData.pickedUpItems.Contains(item.name))
                {
                    Destroy(item.gameObject);
                }
            }
        }

        InventorySystem.Instance.itemsPickedup = environmentData.pickedUpItems;
    }

    private void SetPlayerData(PlayerData playerData)
    {
        // Oyuncunun can, aclik ve susuzlugunu yukleme
        PlayerState.Instance.currentHealth = playerData.playerStats[0];
        PlayerState.Instance.currentHunger = playerData.playerStats[1];
        PlayerState.Instance.currentThirstPercent = playerData.playerStats[2];

        // Oyuncunun uzaydaki konumunu yukleme
        Vector3 loadedPosition;
        loadedPosition.x = playerData.playerTransform[0];
        loadedPosition.y = playerData.playerTransform[1];
        loadedPosition.z = playerData.playerTransform[2];

        PlayerState.Instance.playerBody.transform.position = loadedPosition;

        // Oyuncunun uzaydaki rotasyonunu yukleme
        Vector3 loadedRotation;
        loadedRotation.x = playerData.playerTransform[3];
        loadedRotation.y = playerData.playerTransform[4];
        loadedRotation.z = playerData.playerTransform[5];

        PlayerState.Instance.playerBody.transform.rotation = Quaternion.Euler(loadedRotation);

        // Oyuncunun envanterini yukleme
        foreach (InventoryItemData itemData in playerData.inventoryContent)
        {
            for (int i = 0; i < itemData.quantity; i++)
            {
                InventorySystem.Instance.AddToInventory(itemData.itemName, true);
            }
        }

        //foreach (string item in playerData.quickSlotsContent)
        //{
        //    // Sonraki bos quickslotu bul
        //    GameObject availableSlot = EquipSystem.Instance.FindNextEmptySlot();

        //    var itemToAdd = Instantiate(Resources.Load<GameObject>(item));

        //    itemToAdd.transform.SetParent(availableSlot.transform, false);
        //}
    }

    public void StartLoadedGame(int slotNumber)
    {
        ActivateLoadingScreen();
        
        isLoading = true;
        SceneManager.LoadScene("Island");

        StartCoroutine(DelayedLoading(slotNumber));
    }

    private IEnumerator DelayedLoading(int slotNumber)
    {
        yield return new WaitForSeconds(1.5f);

        LoadGame(slotNumber);
    }

    #endregion

    #endregion  

    #region || To Binary Section ||
    public void SaveGameDataToBinaryFile(AllGameData gameData, int slotNumber)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(binaryPath + fileName + slotNumber + ".bin", FileMode.Create);

        formatter.Serialize(stream, gameData);
        stream.Close();

        print("Data saved to " + binaryPath + fileName + slotNumber + ".bin");
    }
    public AllGameData LoadGameDataFromBinaryFile(int slotNumber)
    {
        if (File.Exists(binaryPath + fileName + slotNumber + ".bin"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(binaryPath + fileName + slotNumber + ".bin", FileMode.Open);

            AllGameData data = formatter.Deserialize(stream) as AllGameData;
            stream.Close();

            print("Data loaded from " + binaryPath + fileName + slotNumber + ".bin");

            return data;
        }
        else
        {
            return null;
        }
    }
    #endregion

    #region || To Json Section ||
    public void SaveGameDataToJsonFile(AllGameData gameData, int slotNumber)
    {
        string json = JsonUtility.ToJson(gameData);

        //string encrypted = EncryptionDecryption(json);

        using (StreamWriter writer = new StreamWriter(jsonPathProject + fileName + slotNumber + ".json"))
        {
            writer.Write(json);
            print("Saved Game to Json file at: " + jsonPathProject + fileName + slotNumber + "j.son");
        }
    }
    public AllGameData LoadGameDataFromJsonFile(int slotNumber)
    {
        using (StreamReader reader = new StreamReader(jsonPathProject + fileName + slotNumber + ".json"))
        {
            string json = reader.ReadToEnd();

            //string decrypted = EncryptionDecryption(json);

            AllGameData data = JsonUtility.FromJson<AllGameData>(json);
            return data;
        }
    }
    #endregion

    #region || Settings Section ||

    #region || Volume Settings ||
    [System.Serializable]
    public class VolumeSettings
    {
        public float music;
        public float effects;
        public float master;
    }
    public void SaveVolumeSettings(float _music, float _effects, float _master)
    {
        VolumeSettings volumeSettings = new VolumeSettings()
        {
            music = _music,
            effects = _effects,
            master = _master,
        };

        PlayerPrefs.SetString("Volume", JsonUtility.ToJson(volumeSettings));
        PlayerPrefs.Save();
    }

    public VolumeSettings LoadVolumeSettings()
    {
        return JsonUtility.FromJson<VolumeSettings>(PlayerPrefs.GetString("Volume"));
    }

    #endregion

    #endregion

    #region || Encryption Section ||
    public string EncryptionDecryption(string jsonString)
    {
        string keyword = "123456789";

        string result = "";

        for (int i = 0; i < jsonString.Length; i++)
        {
            result += (char)(jsonString[i] ^ keyword[i % keyword.Length]);
        }

        return result;

        // XOR = "is there a difference"

        // --- Encrypt ---
        // Mike - 01101101 01101001 01101011 01100101
        // M -          01101101
        // Key -        0000001
        //
        // Encrypted -  01101101

        // --- Decrypt ---
        // Encrypted -  01101100
        // Key -        00000001
        //
        // M -          01101101
    }
    #endregion

    #region || LoadingScreen Section ||

    public void ActivateLoadingScreen()
    {
        loadingScreen.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // yuklenme animasyonu gelebilir belki

        // oyun ipuclari veya bilgileri verilebilir
    }

    public void DisableLoadingScreen()
    {
        loadingScreen.gameObject.SetActive(false);
    }

    #endregion

    #region || Utility ||

    public bool DoesFileExist(int slotNumber)
    {
        if (isSavingToJson)
        {
            if (System.IO.File.Exists(jsonPathProject + fileName + slotNumber + ".json"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (System.IO.File.Exists(binaryPath + fileName + slotNumber + ".bin"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public bool IsSlotEmpty(int slotNumber)
    {
        if (DoesFileExist(slotNumber))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    #endregion
}
