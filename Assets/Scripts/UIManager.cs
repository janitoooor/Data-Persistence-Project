using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public string namePlayer;

    public int score;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        //end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
