using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text textScore;
    private int player1Score = 0;
    private int player2Score = 0;

    public int Player1Score { get { return player1Score; } }
    public int Player2Score { get { return player2Score; } }

    void Start()
    {
        UpdateScoreText();
    }

    public void IncreasePlayer1Score()
    {
        player1Score++;
        UpdateScoreText();
    }

    public void IncreasePlayer2Score()
    {
        player2Score++;
        UpdateScoreText();
    }

    public void ResetScores()
    {
        // Réinitialise les scores des joueurs à zéro
        player1Score = 0;
        player2Score = 0;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        textScore.text = player1Score.ToString() + " : " + player2Score.ToString();
    }

    // Ajoutez d'autres méthodes liées au score si nécessaire
}
