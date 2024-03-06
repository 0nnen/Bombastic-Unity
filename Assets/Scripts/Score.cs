using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private TMP_Text textScore;
    /* [SerializeField] private GameObject confettiPrefab; */
    [SerializeField] private GameObject Player1;
    [SerializeField] private GameObject Player2;
    private int player1Score = 0;
    private int player2Score = 0;
    private int totalRounds = 0;
    private bool isGameOver = false;
    private const int MaxRounds = 5;
    private float currentTime = 0f;

    void Start()
    {
        UpdateScoreText();
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        UpdateScoreText();
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                RestartGame();
            }
            else if (Input.GetKeyDown(KeyCode.N))
            {
                Debug.Log("Quitting Game");
                Application.Quit();
            }
        }
    }

    public void IncreasePlayer1Score()
    {
        if (!isGameOver && totalRounds < MaxRounds)
        {
            player1Score++;
            totalRounds++;
            /*UpdateScoreText();*/
            /* ShowConfetti(); */
            Debug.Log("Player 1 scored! Player 1 Score: " + player1Score);
            CheckForEndGame();
        }
    }

    public void IncreasePlayer2Score()
    {
        if (!isGameOver && totalRounds < MaxRounds)
        {
            player2Score++;
            totalRounds++;
            /*UpdateScoreText();*/
            /* ShowConfetti(); */
            Debug.Log("Player 2 scored! Player 2 Score: " + player2Score);
            CheckForEndGame();
        }
    }

    private void UpdateScoreText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        textScore.text = player1Score.ToString() + "  " + string.Format("{0:00}:{1:00}", minutes, seconds) + "  " + player2Score.ToString();
    }

    /* private void ShowConfetti()
    {
        // La logique pour afficher les confettis reste inchang�e
        if (Player1 != null)
        {
            Vector3 playerPosition = Player1.transform.position;
            Vector3 confettiPosition = playerPosition + Vector3.up * 20.0f;
            *//* Instantiate(confettiPrefab, confettiPosition, Quaternion.identity); *//*
            Debug.Log("Confetti spawned at position: " + confettiPosition);
        }
        else
        {
            Debug.LogWarning("Player object is null. Confetti cannot be spawned.");
        }
    } */

    private void RestartGame()
    {
        // R�initialiser les variables pour recommencer le jeu
        player1Score = 0;
        player2Score = 0;
        totalRounds = 0;
        currentTime = 0f;
        isGameOver = false;
        UpdateScoreText();
    }

    private void CheckForEndGame()
    {
        if (totalRounds >= MaxRounds)
        {
            Debug.Log("Game Over. Final Score - Player 1: " + player1Score + ", Player 2: " + player2Score);
            Debug.Log("Press 'Y' to play again or 'N' to quit.");
            isGameOver = true;
        }
    }
}