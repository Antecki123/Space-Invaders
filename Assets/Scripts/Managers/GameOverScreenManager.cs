using UnityEngine;

[RequireComponent(typeof(GameManager))]
public class GameOverScreenManager : MonoBehaviour
{
    [Header("Game Stats")]
    [SerializeField] private FloatVariable playerHealth;
    [SerializeField] private FloatVariable playerScore;
    [SerializeField] private StringVariable gameOverMessage;
    [Space]
    [SerializeField] private GameEvent updateValue;

    private GameManager gameManager;

    private void Awake() => gameManager = GetComponent<GameManager>();
    private void Start() => updateValue.Invoke();

    public void NewGameButton()
    {
        gameManager.LoadScene(GameManager.GameScene.Game);
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }
}