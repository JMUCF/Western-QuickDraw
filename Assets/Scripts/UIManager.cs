using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UIManager : MonoBehaviour
{
    // ===================== UI Elements ====================
    [Header("UI Elements")]
    [SerializeField] private GameObject drawUI;
    [SerializeField] private GameObject resetButtonUI;
    [SerializeField] private TextMeshProUGUI fastestDrawText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI streakText;

    // ===================== Game Stats =====================
    [HideInInspector] public float fastestDraw = 1f;
    [HideInInspector] public int score = 0;
    [HideInInspector] public int streak = 0;

    // ===================== UI Methods =====================
    public void ShowDrawUI()
    {
        drawUI.SetActive(true);
    }

    public void HideDrawUI()
    {
        drawUI.SetActive(false);
    }

    public void ShowResetButton()
    {
        resetButtonUI.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(resetButtonUI.gameObject);
    }

    public void HideResetButton()
    {
        resetButtonUI.SetActive(false);
    }

    public void UpdateFastestDrawText()
    {
        fastestDrawText.text = "Fastest Draw " + fastestDraw.ToString("F3");
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Score     " + score.ToString("D5");
    }

    public void UpdateStreakText()
    {
        streakText.text = "Streak  " + streak.ToString("D2");
    }
}
