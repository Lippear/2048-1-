using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField] TMP_Text score;

    private int scoreNumber = 0;
    private int coefficientDivisoin = 2;

    private void OnEnable()
    {
        CubeMergerer.MergeHappened += UpdateScore;
    }

    private void OnDisable()
    {
        CubeMergerer.MergeHappened -= UpdateScore;
    }

    private void UpdateScore(int tagNumber)
    {
        scoreNumber += tagNumber/coefficientDivisoin;
        score.text = "Score : " +scoreNumber.ToString();
    }
}
