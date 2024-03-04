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
        menuButton.onClick.AddListener(BackToMenu);
    }

    void Update()
    {
        if (totalRounds >= MaxRounds && !isGameOver)
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
            Debug.Log("Player 2 scored! Player 2 Score: " + player2Score);
        }
    }

    private void UpdateScoreText()
    {
        textScore.text = player1Score.ToString() + " : " + player2Score.ToString();
    }

    private void DisplayEndGame()
    {
        endGamePanel.SetActive(true);
        textEndGame.text = "Game Over. Final Score - Player 1: " + player1Score + ", Player 2: " + player2Score;
    }

    public void RestartGame()
    {
        //Cursor.lockState = CursorLockMode.None;
        // Réinitialiser les variables pour recommencer le jeu
        SceneManager.LoadScene("BallTest");
        player1Score = 0;
        player2Score = 0;
        totalRounds = 0;
        UpdateScoreText();
        isGameOver = false;
        endGamePanel.SetActive(false); // Cacher la fenêtre pop-up après le redémarrage.
        
    }

    public void BackToMenu()
    {
        //Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MenuScene");
        
    }
}
