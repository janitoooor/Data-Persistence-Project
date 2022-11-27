using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;

    public Text BestPlayerNameAndScore;

    public GameObject GameOverText;

    public AudioSource music;
    public AudioSource effect;

    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    private static int bestScore;
    private static string bestPlayer;
    private static int secondBestScore;
    private static string secondPlayer;
    private static string currentName;

    public float voice;
    public float effectVoice;

    private void Awake()
    {
        LoadGameRank();
        SetVolumeMusic();
    }

    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        SetVolumeMusic();
        SetBestPlayer();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        UIManager.Instance.score = m_Points;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        CheckBestPlayer();
    }

    private void CheckBestPlayer()
    {
        int CurrentScore = UIManager.Instance.score;
        currentName = UIManager.Instance.namePlayer;

        if (CurrentScore > bestScore)
        {
            secondPlayer = bestPlayer;
            bestPlayer = UIManager.Instance.namePlayer;
            secondBestScore = bestScore;
            bestScore = CurrentScore;

            BestPlayerNameAndScore.text = $"Best Score - {secondPlayer}: {secondBestScore}" + $" Second Best Score - {bestPlayer} : {bestScore}"; 
            SaveGameRank(bestPlayer, bestScore, currentName, secondPlayer, secondBestScore);
        }
        else if(CurrentScore < bestScore && CurrentScore > secondBestScore)
        {
            secondPlayer = UIManager.Instance.namePlayer;
            secondBestScore = CurrentScore;

            BestPlayerNameAndScore.text = $"Best Score - {bestPlayer}: {bestScore} " + $" Second Best Score - {secondPlayer} : {secondBestScore}";
            SaveGameRank(bestPlayer, bestScore, currentName, secondPlayer, secondBestScore);
        }
        else
        {
            SaveGameRank(bestPlayer, bestScore, currentName, secondPlayer, secondBestScore);
        }
    }
    private void SetBestPlayer()
    {
        if (bestPlayer == null && bestScore == 0)
        {
            BestPlayerNameAndScore.text = "";
        }
        else
        {
            BestPlayerNameAndScore.text = $"Best Score - {bestPlayer}: {bestScore} " + $" Second Best Score - {secondPlayer} : {secondBestScore}";
        }

    }

    private void SetVolumeMusic()
    {
        music.volume = voice;
        effect.volume = effectVoice;
    }

    public void SaveGameRank(string bestPlayerName, int bestPlayerScore, string currentLastName, string secondBestPlayerName, int secondBestPlayerScore)
    {
        SaveData data = new SaveData();

        data.TheBestPlayer = bestPlayerName;
        data.HighiestScore = bestPlayerScore;
        data.CurrentName = currentLastName;
        data.SecondBest = secondBestPlayerName;
        data.SecondScore = secondBestPlayerScore;
        data.MusicVoice = voice;
        data.EffectVoice = effectVoice;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
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
            voice = data.MusicVoice;
            secondPlayer = data.SecondBest;
            secondBestScore = data.SecondScore;
            effectVoice = data.EffectVoice;
        }
    }
    public void ExitToMenu()
    {
        SceneManager.LoadScene(1);
    }


    [System.Serializable]
    class SaveData
    {
        public int HighiestScore;
        public int SecondScore;
        public string TheBestPlayer;
        public string CurrentName;
        public string SecondBest;
        public float MusicVoice;
        public float EffectVoice;
    }
}
