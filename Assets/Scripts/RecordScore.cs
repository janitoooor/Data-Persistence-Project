using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class RecordScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI recordScore;

    private static string bestPlayer;
    private static int bestScore;

    public AudioSource music;
    public AudioSource effect;

    private float voice;
    private float effectVoice;

    private static string secondPlayer;
    private static int secondBestScore;

    void Awake()
    {
        LoadGameRank();
        SetBestPlayer();
        SetVolumeMusic();
    }

    private void SetVolumeMusic()
    {
        music.volume = voice;
        effect.volume = effectVoice;
    }

    private void SetBestPlayer()
    {
        recordScore.text = $"Best Score - {bestPlayer}: {bestScore}" + $"  Second Best Score - {secondPlayer} : {secondBestScore}";
    }

    public void LoadGameRank()
    {
        string path = Application.persistentDataPath + "//savefile.json";

        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestPlayer = data.TheBestPlayer;
            bestScore = data.HighiestScore;
            voice = data.MusicVoice;
            secondPlayer = data.SecondBest;
            secondBestScore = data.SecondScore;
            effectVoice = data.EffectVoice;
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(1);
    }

    [System.Serializable]
    class SaveData
    {
        public int HighiestScore;
        public string TheBestPlayer;
        public float MusicVoice;
        public float EffectVoice;
        public string SecondBest;
        public int SecondScore;
    }
}
