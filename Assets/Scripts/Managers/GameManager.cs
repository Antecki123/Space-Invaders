using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static Action OnGameOverConditions;
    public enum GameScene { Game, Gameover }

    [Header("Game Stats")]
    [SerializeField] private FloatVariable playerHealth;
    [SerializeField] private FloatVariable playerScore;
    [Space]
    [SerializeField] private StringVariable gameOverMessage;

    private readonly string winText = "You win!";
    private readonly string loseText = "You lose!";

    private void OnEnable() => OnGameOverConditions += GameOverCondition;
    private void OnDisable() => OnGameOverConditions -= GameOverCondition;

    private void Start()
    {
        playerHealth.value = 3.0f;
        playerScore.value = 0.0f;
        gameOverMessage.text = string.Empty;
    }

    private void GameOverCondition()
    {
        if (playerHealth.value <= 0)
        {
            gameOverMessage.text = loseText;
            LoadScene(GameScene.Gameover);
        }

        if (Invader.Invaders.Count <= 0)
        {
            gameOverMessage.text = winText;
            LoadScene(GameScene.Gameover);
        }
    }

    public void LoadScene(GameScene gameScene)
    {
        SceneManager.LoadSceneAsync(gameScene.ToString(), LoadSceneMode.Single);
    }
}