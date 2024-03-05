using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour
{
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TMPro.TMP_Text textEndGame;
    [SerializeField] private ScoreManager scoreManager;

    private const int MaxRounds = 5;

    void Update()
    {
        if (scoreManager.Player1Score >= MaxRounds || scoreManager.Player2Score >= MaxRounds)
        {
            DisplayEndGame();
        }
    }

    private void DisplayEndGame()
    {
        Debug.Log("Game Over. Final Score - " + scoreManager.Player1Score.ToString() + " : " + scoreManager.Player2Score.ToString());

        // Afficher le panneau de fin de jeu
        endGamePanel.SetActive(true);

        // Mettre à jour le texte de fin de jeu
        textEndGame.text = "Game Over. Final Score - " + scoreManager.Player1Score.ToString() + " : " + scoreManager.Player2Score.ToString();
    }

    public void RestartGame()
    {
        Debug.Log("Restarting the game");
        SceneManager.LoadScene("BallTest");

        // Réinitialiser les scores
        scoreManager.ResetScores();

        // Masquer le panneau de fin de jeu
        endGamePanel.SetActive(false);
    }

    public void BackToMenu()
    {
        Debug.Log("Returning to the menu");
        SceneManager.LoadScene("MenuScene");
    }
}
