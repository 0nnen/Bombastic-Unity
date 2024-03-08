using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    [SerializeField] private TMP_Text textScore;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TMP_Text textEndGame;

    private int player1Score = 0;
    private int player2Score = 0;
    private int totalRounds = 0;
    private const int MaxRounds = 5;
    private float currentTime = 0f;

    public Button restartButton;
    public Button menuButton;

    private bool isGameOver = false;

    void Start()
    {
        UpdateScoreText();
        endGamePanel.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);
        menuButton.onClick.AddListener(BackToMenu);
    }

    void Update()
    {
        if (!isGameOver)
        {
            currentTime += Time.deltaTime;
            UpdateScoreText();
            CheckForGameOver();
        }
    }

    private void CheckForGameOver()
    {
        if ((player1Score >= 3 || player2Score >= 3) && !isGameOver)
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
            Debug.Log("Player 1 scored! P1-" + player1Score);
        }
    }

    public void IncreasePlayer2Score()
    {
        if (!isGameOver && totalRounds < MaxRounds)
        {
            player2Score++;
            totalRounds++;
            UpdateScoreText();
            Debug.Log("Player 2 scored! P2-" + player2Score);
        }
    }

    private void UpdateScoreText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        textScore.text = player1Score.ToString() + "  " + string.Format("{0:00}:{1:00}", minutes, seconds) + "  " + player2Score.ToString();
    }

    private void DisplayEndGame()
    {
        Debug.Log("END");
        endGamePanel.SetActive(true);
        textEndGame.text = "Game Over\n Final Score \nP1- " + player1Score.ToString() + " : P2-" + player2Score.ToString();
    }

    private void RestartGame()
    {
        SceneManager.LoadScene("GameBall");
        player1Score = 0;
        player2Score = 0;
        totalRounds = 0;
        currentTime = 0f;
        isGameOver = false;
        UpdateScoreText();
        endGamePanel.SetActive(false);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}