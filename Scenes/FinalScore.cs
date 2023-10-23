using UnityEngine;
using TMPro;

public class YourScript : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Start()
    {
        if (scoreText != null)
            scoreText.text = "Final Score: " + BallBehaviour.finalScore;        
    }
}
