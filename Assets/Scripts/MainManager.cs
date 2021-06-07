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
    public Text HighScoreText;
    public GameObject GameOverText;

    public string playerName;
    public HighScore highScore;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        playerName = GameManager.Instance.playerName;
        ScoreText.text = $"{playerName} :";
        highScore = GetHighScore();
        if(highScore == null)
        {
            highScore = new HighScore();
            highScore.playerName = "None";
            highScore.score = 0;
        }

        HighScoreText.text = $"Best Score : {highScore.playerName} : {highScore.score}";

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
        ScoreText.text = $"{playerName} : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        SaveHighScore();
    }

    [System.Serializable]
    public class HighScore
    {
        public string playerName;
        public int score;
    }

    private void SaveHighScore()
    {
        if(m_Points > highScore.score)
        {
            HighScore highScore = new HighScore();
            highScore.playerName = playerName;
            highScore.score = m_Points;

            string json = JsonUtility.ToJson(highScore);

            File.WriteAllText(Application.persistentDataPath + "/highscore.json", json);
        }
    }

    private HighScore GetHighScore()
    {
        string path = Application.persistentDataPath + "/highscore.json";
        Debug.Log(path);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            HighScore loadedScore = JsonUtility.FromJson<HighScore>(json);
            return loadedScore;
        }
        else
        {
            return new HighScore();
        }
        
    }
}
