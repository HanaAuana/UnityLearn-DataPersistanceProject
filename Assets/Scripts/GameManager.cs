using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string playerName;

    public void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetName(string newName)
    {
        playerName = newName;
        Debug.Log("Name set to: " + playerName);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
