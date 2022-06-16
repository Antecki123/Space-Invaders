using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameScene { Game, Gameover }

    [Header("Game Stats")]
    [SerializeField] private FloatVariable playerHealth;
    [SerializeField] private FloatVariable playerScore;
    [Space]
    [SerializeField] private StringVariable gameOverMessage;

    private readonly string winText = "You win!";
    private readonly string loseText = "You lose!";

    private void Start()
    {
        playerHealth.value = 3.0f;
        playerScore.value = 0.0f;
        gameOverMessage.text = string.Empty;

        if (SceneManager.GetActiveScene().name == GameScene.Game.ToString())
            InvokeRepeating(nameof(CheckGameOverCondition), 0f, 2.0f);
    }

    private void CheckGameOverCondition()
    {
        if (playerHealth.value <= 0)
        {
            CancelInvoke();
            Invader.InvadersCount = 0;

            gameOverMessage.text = loseText;
            LoadScene(GameScene.Gameover);

            return;
        }

        if (Invader.InvadersCount <= 0)
        {
            CancelInvoke();

            gameOverMessage.text = winText;
            LoadScene(GameScene.Gameover);

            return;
        }
    }

    public void LoadScene(GameScene gameScene)
    {
        SceneManager.LoadScene(gameScene.ToString());
    }
}