using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private int playerLives = 3;
    [SerializeField] private int coin = 0;
    [SerializeField] private TextMeshProUGUI lifePointText;
    [SerializeField] private TextMeshProUGUI coinPointText;

    private void Awake() 
    {
        int numberOfGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numberOfGameSessions > 1)
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
        lifePointText.text = playerLives.ToString();
        coinPointText.text = coin.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
            TakeLife();
        else
            ResetGameSession();
    }

    private void TakeLife()
    {
        playerLives--;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        lifePointText.text = playerLives.ToString();
    }

    private void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void IncreaseCoinPoint(int coinPoint)
    {
        coin += coinPoint;
        coinPointText.text = coin.ToString();
    }
}
