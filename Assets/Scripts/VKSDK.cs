using System;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using SimpleJSON;
using UnityEngine.Events;

public class VKSDK : MonoBehaviour
{
    public static VKSDK Instance { get; private set; }

    public UserGameData _data;
    
    [DllImport("__Internal")] private static extern void Auth();    // Авторизация - внешняя функция для связи с плагином
    [DllImport("__Internal")] private static extern void GetData();    // Получение данных - внешняя функция для связи с плагином
    [DllImport("__Internal")] private static extern void SetData(int data);    // Отправка данных - внешняя функция для связи с плагином
    [DllImport("__Internal")] private static extern void Repost();    // Отправка данных - внешняя функция для связи с плагином
    [DllImport("__Internal")] private static extern void ShowLeaderboard();
    [DllImport("__Internal")] private static extern void ShowAd();

    public UnityEvent<int> onScoreLoaded;

    private void Awake()
    {
        if(Instance)
        {
            Destroy(gameObject);
            return;
        }
            
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _data = new UserGameData(0);
        Authenticate();
    }
    
    //Вызывается из index
    public void SetScoreData(string data)
    {
        print($"SettingData {data}");
        _data.Score = int.Parse(data);
        onScoreLoaded?.Invoke(_data.Score);
    }

    public void Authenticate()
    {
        Auth();
    }

    public void ShowingAd()
    {
        ShowAd();
    }

    //из index
    public void AuthenticateSuccess(string info)
    {
        print(info);
    }
    
    public void GettingData()
    {
        GetData();
    }

    public void SettingData(int data)
    {
        SetData(data);
    }

    public void MakeRepost()
    {
        Repost();
    }

    public void LeaderBoard()
    {
        ShowLeaderboard();
    }

    public void LevelCompleted()
    {
        _data.Score++;
        SettingData(_data.Score);
    }
}

[Serializable]
public class UserData
{
    public string Name;
    public string image;
}

[Serializable]
public class UserGameData
{
    public UserGameData(int score)
    {
        Score = score;
    }
    public int Score;
}

[Serializable]
public class UserDataSaving
{
    public string data;
}