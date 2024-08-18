using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreText : MonoBehaviour
{
    private ScoreSystem _scoreSystem;

    private Text _scoreText;
    
    private void Awake()
    {
        _scoreSystem = FindObjectOfType<ScoreSystem>();
        _scoreText = GetComponent<Text>();
    }

    private void Start()
    {
        _scoreText.text = ((int)_scoreSystem.Score).ToString();
        _scoreSystem.OnScoreChanged.AddListener(OnScoreChanged);
    }

    private void OnScoreChanged(float score, float amount)
    {
        _scoreText.text = ((int)score).ToString();
        // DoTween to animate the score text (make it increase size and then return to normal)
        DOTween.Sequence()
            .Append(_scoreText.transform.DOScale(1.5f, 0.1f))
            .Append(_scoreText.transform.DOScale(1.0f, 0.1f));
    }
}
