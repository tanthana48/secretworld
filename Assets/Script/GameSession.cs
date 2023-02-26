using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    public int playerLives = 3;
    public int modeBoost = 1;
    [SerializeField] private int score;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image[] livesImage;

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;

        if (numGameSessions >= 2)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        UpdateLives();
        scoreText.text = score.ToString();
    }

    public void AddToScore(int points)
    {
        score += points*modeBoost;
        scoreText.text = score.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            playerLives--;
            UpdateLives();
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
        else
        {
            FindObjectOfType<ScenePersist>().ResetScenePersist();
            SceneManager.LoadScene(0);
            Destroy(gameObject);
        }
    }

    public void UpdateLives()
    {
        for (int i = 0; i < livesImage.Length; i++)
        {
            if (i >= playerLives)
            {
                livesImage[i].color = Color.black;
            }
        }
    }

    public void SetPlayerLives(int newLives)
    {
        playerLives = newLives;
        UpdateLives();
    }
    
    public void SetModeBoost(int newBoost)
    {
        modeBoost = newBoost;
    }

    private void Update()
    {
        // If the game session is in Scene 0, clear the data
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            playerLives = 3;
            score = 0;
            UpdateLives();
            scoreText.text = score.ToString();
            Destroy(gameObject);
        }
    }
}
