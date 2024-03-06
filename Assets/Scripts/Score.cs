using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    [SerializeField] private TMP_Text textScore;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TMP_Text textEndGame;
    [SerializeField] private bool isGameOver = false;

    public Button restartButton;
    public Button menuButton;

    private int player1Score = 0;
    private int player2Score = 0;
    private int totalRounds = 0;
    private const int MaxRounds = 5;

    void Start()
    {
        UpdateScoreText();
        endGamePanel.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);
    }

    void Update()
    {
        if (player1Score >= 3 || player2Score >= 3 && !isGameOver)
        {
            DisplayEndGame();
            isGameOver = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void IncreasePlayer1Score()
    {
        if (!isGameOver && totalRounds < MaxRounds)
        {
            player1Score++;
            totalRounds++;
            UpdateScoreText();
            Debug.Log("Player 1 scored! Player 1 Score: " + player1Score);
        }
    }

    public void IncreasePlayer2Score()
    {
        if (!isGameOver && totalRounds < MaxRounds)
        {
            player2Score++;
            totalRounds++;
            UpdateScoreText();
        }
    }

    private void UpdateScoreText()
    {
        textScore.text = player2Score.ToString() + " : " + player1Score.ToString();
    }

    private void DisplayEndGame()
    {
        endGamePanel.SetActive(true);
        textEndGame.text = "Game Over. Final Score - " + player1Score.ToString() + " : " + player2Score.ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("BallTest");
        player1Score = 0;
        player2Score = 0;
        totalRounds = 0;
        currentTime = 0f;
        UpdateScoreText();
        isGameOver = false;
        endGamePanel.SetActive(false);

    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
        Debug.Log("clique ");

    }
}