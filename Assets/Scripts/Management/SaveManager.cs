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

    public bool isSavingToJson;

    #region || General Section ||

    #region || Saving Section ||
    public void SaveGame()
    {
        AllGameData data = new AllGameData();

        data.playerData = GetPlayerData();

        SavingTypeSwitch(data);
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

        return new PlayerData(playerStats, playerTransform);
    }
    public void SavingTypeSwitch(AllGameData gameData)
    {
        if (isSavingToJson)
        {
            // SaveGameDataToJsonFile(gameData);
        }
        else
        {
            SaveGameDataToBinaryFile(gameData);
        }
    }
    #endregion

    #region || Loading Section ||
    public AllGameData LoadingTypeSwitch()
    {
        if (isSavingToJson)
        {
            AllGameData gameData = LoadGameDataFromBinaryFile();
            return gameData;
        }
        else
        {
            AllGameData gameData = LoadGameDataFromBinaryFile();
            return gameData;
        }
    }

    public void LoadGame()
    {
        // Player Data
        SetPlayerData(LoadingTypeSwitch().playerData);

        //Environment Data
        //SetEnvironment(LoadAllGameData().environmentData);
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
    }

    public void StartLoadedGame()
    {
        SceneManager.LoadScene("Island");

        StartCoroutine(DelayedLoading());
    }

    private IEnumerator DelayedLoading()
    {
        yield return new WaitForSeconds(0.3f);

        LoadGame();
    }

    #endregion

    #endregion

    #region || To Binary Section ||
    public void SaveGameDataToBinaryFile(AllGameData gameData)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/save_game.bin";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, gameData);
        stream.Close();

        print("Data saved to " + Application.persistentDataPath + "/save_game.bin");
    }
    public AllGameData LoadGameDataFromBinaryFile()
    {
        string path = Application.persistentDataPath + "/save_game.bin";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            AllGameData data = formatter.Deserialize(stream) as AllGameData;
            stream.Close();

            print("Data loaded from " + Application.persistentDataPath + "/save_game.bin");

            return data;
        }
        else
        {
            return null;
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
}
