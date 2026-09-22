using TMPro;
using UnityEngine;

// 4번(UI) 작업 전까지 헤드셋에서 점수 변화를 바로 확인하기 위한 임시 표시용 (Canvas 없이 TextMeshPro 3D Text로 월드스페이스 표시)
[RequireComponent(typeof(TMP_Text))]
public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;

    private TMP_Text label;

    private void Awake()
    {
        label = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        scoreManager.OnScoreChanged += UpdateLabel;
        UpdateLabel(scoreManager.Score);
    }

    private void OnDisable()
    {
        scoreManager.OnScoreChanged -= UpdateLabel;
    }

    private void UpdateLabel(int score)
    {
        label.text = $"Score: {score}";
    }
}
