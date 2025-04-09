using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuickdrawManager : MonoBehaviour
{
    // ===================== Characters =====================
    [Header("Characters")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;

    // ===================== Gameplay Settings ==============
    [Header("Gameplay Settings")]
    [SerializeField] private float minDelay = 1.5f;
    [SerializeField] private float maxDelay = 3.5f;
    [SerializeField] private float reactionTime = 1.0f;

    // ===================== Internal Stuff =================
    private bool playerCanShoot = false;
    private float drawTime;
    private bool roundEnded = false;
    private Character playerCharacter;
    private Character enemyCharacter;

    // ===================== Managers =======================
    [Header("Managers")]
    public UIManager uiManager;
    public AudioManager audioManager;

    void Start()
    {
        StartCoroutine(BeginRound());
        playerCharacter = player.GetComponent<Character>();
        enemyCharacter = enemy.GetComponent<Character>();
    }

    IEnumerator BeginRound()
    {
        uiManager.HideDrawUI();
        playerCanShoot = false;
        audioManager.PlayGunClick();
        float waitTime = Random.Range(minDelay, maxDelay);
        yield return new WaitForSeconds(waitTime);

        uiManager.ShowDrawUI();
        drawTime = Time.time;
        playerCanShoot = true;
        enemyCharacter.Shoot();

        yield return new WaitForSeconds(reactionTime);

        if (!roundEnded)
        {
            audioManager.PlayGunShot();
            LoseRound("Too Slow!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !roundEnded)
        {
            if (!playerCanShoot)
            {
                LoseRound("Shot Too Early!");
            }
            else
            {
                audioManager.PlayGunShot();
                float reaction = Time.time - drawTime;
                playerCharacter.Shoot();
                WinRound(reaction);
            }
        }
    }

    void WinRound(float reaction)
    {
        if (reaction < uiManager.fastestDraw)
        {
            uiManager.fastestDraw = reaction;
            uiManager.UpdateFastestDrawText();
        }

        uiManager.streak++;
        uiManager.UpdateStreakText();
        int scoreIncrease = Mathf.FloorToInt(uiManager.streak * ((1f / reaction) * 20)); //multiplies streak by reaction speed, makes the number bigger and rounds down to nearest int
        uiManager.score += scoreIncrease;
        uiManager.UpdateScoreText();

        roundEnded = true;
        Debug.Log("You won! Reaction time: " + reaction.ToString("F3") + "s");
        enemy.GetComponent<Character>().Die();

        ResetRound();
    }

    void LoseRound(string reason)
    {
        StartCoroutine(Delay());
        IEnumerator Delay() //delay to stop you from immediately restarting round after death
        {
            roundEnded = true;
            uiManager.HideDrawUI();
            Debug.Log("You lost! Reason: " + reason);
            player.GetComponent<Character>().Die();
            yield return new WaitForSeconds(1f);
            uiManager.ShowResetButton();
        }
    }

    void ResetRound()
    {
        Debug.Log("in reset round");
        StartCoroutine(Resetting());

        IEnumerator Resetting() //delay to stop you from previous shot counting as too early on new round
        {
            yield return new WaitForSeconds(1f);
            enemy.GetComponent<Character>().Reset();
            player.GetComponent<Character>().Reset();
            uiManager.HideDrawUI();
            if (uiManager.streak <= 14)
                reactionTime -= 0.05f;
            else
                reactionTime -= 0.01f;
            roundEnded = false;
            Debug.Log(reactionTime);
            StartCoroutine(BeginRound());
        }
    }

    public void ResetGame()
    {
        StartCoroutine(Resetting());
        IEnumerator Resetting() //delay to stop you from previous shot counting as too early on new round
        {
            Debug.Log("in reset game");
            enemy.GetComponent<Character>().Reset();
            player.GetComponent<Character>().Reset();
            uiManager.HideResetButton();
            reactionTime = 1f;
            uiManager.score = 0;
            uiManager.UpdateScoreText();
            uiManager.streak = 0;
            uiManager.UpdateStreakText();
            yield return new WaitForSeconds(1f);
            roundEnded = false;
            StartCoroutine(BeginRound());
        }
    }
}
