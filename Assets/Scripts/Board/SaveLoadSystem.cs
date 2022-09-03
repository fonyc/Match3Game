using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerData
{

}

public class SaveLoadSystem : MonoBehaviour
{

    //public static SaveLoadSystem Instance;

    //public int selectedSlot;
    //public PlayerData gameData;

    //void Awake()
    //{

    //    if (Instance == null)
    //    {
    //        Instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else Destroy(gameObject);
    //}

    //public void LoadExistingGames() 
    //{
    //    string persistentDataPath = Application.persistentDataPath + "/Saves";

    //    string fileName = string.Format("/{0}.dat", "PlayerData");

    //    if (Directory.Exists(persistentDataPath) && File.Exists(persistentDataPath + fileName))
    //    {
    //        gameData = LoadTemporalPlayerInfo;
    //    }
    //    else
    //    {
    //        gameData = new PlayerData();
    //    }
    //}

    //PlayerData LoadTemporalPlayerInfo()
    //{
    //    PlayerData _tempPlayerData = new PlayerData();

    //    string _path = Application.persistentDataPath + "/Saves";
    //    string fileName = string.Format("/{0}.dat", "PlayerData_");

    //    BinaryFormatter bf = new BinaryFormatter();
    //    FileStream file = File.Open(_path + fileName, FileMode.Open);

    //    // Casteo
    //    _tempPlayerData = (PlayerData)bf.Deserialize(file);
    //    file.Close();

    //    return _tempPlayerData;
    //}

    //public void SaveGame_Start()
    //{

    //    Debug.Log("** Init Save Process ** ");

    //    string persistentDataPath = Application.persistentDataPath + "/Saves";
    //    string fileName = string.Format("/{0}.dat", "PlayerData_" + selectedSlot);

    //    if (!Directory.Exists(persistentDataPath))
    //    {
    //        Directory.CreateDirectory(persistentDataPath);
    //    }

    //    SaveGame(persistentDataPath, fileName);
    //}

    //public void SaveGame(string _dataPath, string _fileName)
    //{
    //    Debug.Log("Saving file in route... " + _dataPath + _fileName);

    //    PlayerDataManager.Instance.playerData.gameDate = "" + System.DateTime.Now.ToString("dd / MM / yyy");
    //    PlayerDataManager.Instance.playerData.gameHour = "" + System.DateTime.Now.ToString("hh: mm : ss");

    //    BinaryFormatter bf = new BinaryFormatter();
    //    FileStream file = File.Create(_dataPath + _fileName);

    //    bf.Serialize(file, PlayerDataManager.Instance.playerData);

    //    file.Close();
    //}

    //public void LoadGame()
    //{
    //    string _path = Application.persistentDataPath + "/Saves";
    //    string fileName = string.Format("/{0}.dat", "PlayerData_" + selectedSlot);

    //    // Si existe una partida previa...
    //    if (Directory.Exists(_path) && File.Exists(_path + fileName))
    //    {

    //        BinaryFormatter bf = new BinaryFormatter();
    //        FileStream file = File.Open(_path + fileName, FileMode.Open);

    //        // Cast
    //        PlayerDataManager.Instance.playerData = (PlayerData)bf.Deserialize(file);
    //        file.Close();

    //        Debug.Log("Load file on path: " + _path + fileName);
    //    }
    //    else
    //    {
    //        Debug.Log("No player Data");
    //        return;
    //    }
    //}

    //public void DeleteGame()
    //{
    //    string _path = Application.persistentDataPath + "/Saves";
    //    string fileName = string.Format("/{0}.dat", "PlayerData_");

    //    if (Directory.Exists(_path) && File.Exists(_path + fileName))
    //    {
    //        File.Delete(_path + fileName);
    //    }
    //    else return;
    //}

    //private void OnApplicationQuit()
    //{
    //    if (GameManager.Instance.gameState != GameStates.startingVideogame) SaveGame_Start();
    //}

    //private void OnApplicationFocus(bool focus)
    //{
    //    if (!focus && GameManager.Instance.gameState != GameStates.startingVideogame) SaveGame_Start();
    //}
}