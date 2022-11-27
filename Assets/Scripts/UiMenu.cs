using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;

[DefaultExecutionOrder(1000)]
public class UiMenu : MonoBehaviour
{
    [SerializeField] TMP_InputField playerNameInput;
    [SerializeField] TMP_Text playerName;

    public AudioSource music;
    public AudioSource effect;

    public float voice;
    public float effectVoice;

    private static int bestScore;
    private static string bestPlayer;
    private static string currentName;
    private static string secondPlayer;
    private static int secondBestScore;

    private void Awake()
    {
        LoadGameRank();
        SetBestPlayer();
        SetVolumeMusic();
    }

    public void SetVolumeMusic()
    {
        music.volume = voice;
        effect.volume = effectVoice;
    }

    public void CreateName()
    {
        UIManager.Instance.namePlayer = playerNameInput.text;
    }

    private void SetBestPlayer()
    {
        if (bestPlayer == null && bestScore == 0)
        {
            playerName.text = "";
        }
        else
        {
            playerName.text = $"Best Score - {bestPlayer}: {bestScore}" + $"  Second Best Score - {secondPlayer} : {secondBestScore}";
            playerNameInput.text = currentName;
        }

    }

    public void LoadGameRank()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestPlayer = data.TheBestPlayer;
            bestScore = data.HighiestScore;
            currentName = data.CurrentName;
            voice = data.MusicVoice;
            secondPlayer = data.SecondBest;
            secondBestScore = data.SecondScore;
            effectVoice = data.EffectVoice;
        }
    }

    public void StartNew()
    {
        SceneManager.LoadScene(0);
    }

    public void OpenRecord()
    {
        SceneManager.LoadScene(2);
    }

    public void OpenOptions()
    {
        SceneManager.LoadScene(3);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif 
    }
    
    [System.Serializable]
    class SaveData
    {
        public int HighiestScore;
        public string TheBestPlayer;
        public string CurrentName;
        public float MusicVoice;
        public float EffectVoice;
        public string SecondBest;
        public int SecondScore;
    }
}
