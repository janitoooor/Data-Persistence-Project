using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class Options : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Slider effectSlider;

    public AudioSource music;
    public AudioSource effect;

    private float voice;
    private float effectVolume;

    private static string bestPlayer;
    private static int bestScore;

    private static string currentName;

    private static string secondPlayer;
    private static int secondBestScore;

    private void Awake()
    {
        LoadMusicVolume();
        SetVolumeMusic();
        Slider();

    }
    private void Update()
    {
        voice = music.volume;
        effectVolume = effect.volume;
        
    }

    private void Slider()
    {
        slider.value = music.volume;
        effectSlider.value = effect.volume;
    }

    public void SetVolumeMusic()
    {
        {
            music.volume = voice;
            effect.volume = effectVolume;
        }
    }
    public void BackToMenu()
    {
        SaveMusicVolume();
        SceneManager.LoadScene(1);
    }

    public void LoadMusicVolume()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        bestPlayer = data.TheBestPlayer;
        bestScore = data.HighiestScore;
        voice = data.MusicVoice;
        currentName = data.CurrentName;
        effectVolume = data.EffectVoice;
        secondPlayer = data.SecondBest;
        secondBestScore = data.SecondScore;
    }
    public void SaveMusicVolume()
    {
        SaveData data = new SaveData();

        data.TheBestPlayer = bestPlayer;
        data.HighiestScore = bestScore;
        data.MusicVoice = voice;
        data.CurrentName = currentName;
        data.EffectVoice = effectVolume;
        data.SecondBest = secondPlayer;
        data.SecondScore = secondBestScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    [System.Serializable]
    class SaveData
    {
        public int HighiestScore;
        public string TheBestPlayer;
        public float MusicVoice;
        public string CurrentName;
        public float EffectVoice;
        public string SecondBest;
        public int SecondScore;
    }
}
