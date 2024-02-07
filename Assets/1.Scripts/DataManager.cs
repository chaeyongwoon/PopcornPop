using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Sirenix.OdinInspector;

public class DataManager : MonoBehaviour
{

    #region Singleton
    public static DataManager instance = null;
    public GameManager _gamemanager;

    public double reward_money1 = 0;
    public double reward_money2 = 0;
    public bool reward_save = false;

    private void Awake() => Init();
    private void Init()
    {
        _gamemanager = GetComponent<GameManager>();


        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Application.targetFrameRate = 60;
        DontDestroyOnLoad(gameObject);

        Load_Data();

    }
    #endregion


    private bool isData;
    public bool IsData
    {
        get { return IsData; }
        set { IsData = value; }
    }

    public void OnApplicationPause(bool pause)
    {
        if (pause)
            Save_Data();
    }

    [ContextMenu("Save")]
    public void Save_Data()
    {
        try
        {
            _gameData.Max_Stage_Level = _gamemanager.Current_Max_Stage_Level;
            _gameData.Stage_Level = _gamemanager.Current_Stage_Level;
            _gameData.Popcorn_Level[_gameData.Stage_Level] = _gamemanager.Current_StageManager.Popcorn_Level;
            _gameData.Income_Level[_gameData.Stage_Level] = _gamemanager.Current_StageManager.Income_Level;
            _gameData.Object_Level[_gameData.Stage_Level] = _gamemanager.Current_StageManager.Object_Level;
            _gameData.Money = _gamemanager.Money;
        }
        catch
        {
            _gameData.Max_Stage_Level = 0;
            _gameData.Stage_Level = 0;
        }

        if (_gamemanager.Money < 0)
        {
            _gamemanager.Money = 100d;
            _gameData.Money = _gamemanager.Money;
        }

        //string jsonData = JsonUtility.ToJson(_gameData, true);
        //string path = Path.Combine(Application.persistentDataPath, "Load.json");
        //File.WriteAllText(path, jsonData);

        ES3.Save<GameData>("GameData", _gameData);

    }

    public void Load_Data()
    {
        Debug.Log("Load Data");
        //if (!File.Exists(Path.Combine(Application.persistentDataPath, "SaveFile.es3")))
        //{
        //    //Save_Data();        
        //}

        if (File.Exists(Path.Combine(Application.persistentDataPath, "Load.json")))
        {
            Debug.Log(Application.persistentDataPath);
            Debug.Log("Json File Exists!");
            reward_money1 = 200000000;
            File.Delete(Path.Combine(Application.persistentDataPath, "Load.json"));
            Debug.Log("Delete File");
            reward_save = true;
        }

        if (File.Exists(Path.Combine(Application.persistentDataPath, "SaveFile.es3")))
        {
            Debug.Log(Application.persistentDataPath);
            Debug.Log("Json File Exists!");
            reward_money2 = 800000000;
            File.Delete(Path.Combine(Application.persistentDataPath, "SaveFile.es3"));
            Debug.Log("Delete File");
            reward_save = true;
        }



        //string path = Path.Combine(Application.persistentDataPath, "Load.json");
        //string jsondata = File.ReadAllText(path);
        //_gameData = JsonUtility.FromJson<GameData>(jsondata);


        _gameData = ES3.Load<GameData>("GameData", new GameData());

        _gameData.Money = _gameData.Money + reward_money1 + reward_money2;

        _gamemanager.Current_Max_Stage_Level = _gameData.Max_Stage_Level;
        _gamemanager.Money = _gameData.Money;
        //_gamemanager.Money = _gamemanager.Money + reward_money1 + reward_money2;
        reward_money1 = 0;
        reward_money2 = 0;

        if (reward_save) Save_Data();
    }


    public GameData _gameData;

    [System.Serializable]
    public class GameData
    {
        public int Max_Stage_Level;
        public int Stage_Level;
        public int[] Popcorn_Level = new int[6]; //  스폰갯수 증가      
        public int[] Income_Level = new int[6]; // 업그레이드 레벨     
        public int[] Object_Level = new int[6];
        public double Money;
    }

    [Button]
    public void Init_Data()
    {
        _gamemanager.Current_Max_Stage_Level = 0;
        _gamemanager.Current_Stage_Level = 0;
        _gamemanager.Current_Popcorn_Level = 0;
        _gamemanager.Current_Income_Level = 0;
        _gamemanager.Current_Object_Level = 0;
        _gamemanager.Money = 0;



        Save_Data();
    }

    // ////////////////////////////////////////////////////////////////////////////////


    //void tSave_Data(int stageLevel)
    //{        
    //    ES3.Save("Max_Stage_Level", _gamemanager.Current_Max_Stage_Level);
    //    ES3.Save("Stage_Level", _gamemanager.Current_Stage_Level);
    //    ES3.Save("Money", _gamemanager.Money);

    //    ES3.Save("Popcorn_Level_" + stageLevel, _gamemanager.Current_StageManager.Popcorn_Level);
    //    ES3.Save("Income_Level_" + stageLevel, _gamemanager.Current_StageManager.Income_Level);
    //    ES3.Save("Object_Level_" + stageLevel, _gamemanager.Current_StageManager.Object_Level);


    //}

    //void tLoad_Data(int stageLevel)
    //{

    //    _gamemanager.Current_StageManager.Popcorn_Level = 



    //}






}
