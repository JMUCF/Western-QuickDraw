using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class QuickdrawManager : MonoBehaviour
{
    // ===================== Characters =====================
    [Header("Characters")]
    public GameObject player;
    public GameObject enemy;

    // ===================== UI Elements ====================
    [Header("UI")]
    public GameObject drawUI;
    public GameObject resetButtonUI;
    public float fastestDraw = 1f;
    public TextMeshProUGUI fastestDrawText;
    public int score;
    public TextMeshProUGUI scoreText;
    public int streak;
    public TextMeshProUGUI streakText;

    // ===================== Gameplay Settings ==============
    [Header("Gameplay Settings")]
    public float minDelay = 1.5f;
    public float maxDelay = 3.5f;
    public float reactionTime = 1.0f;

    // ===================== Internal Stuff =================
    private bool playerCanShoot = false;
    private float drawTime;
    private bool roundEnded = false;
    public AudioSource gunSounds;
    public AudioClip gunClick;
    public AudioClip gunShot;

    void Start()
    {
        StartCoroutine(BeginRound());
    }

    IEnumerator BeginRound()
    {
        drawUI.SetActive(false);
        playerCanShoot = false;
        gunSounds.PlayOneShot(gunClick, 1);
        float waitTime = Random.Range(minDelay, maxDelay);
        yield return new WaitForSeconds(waitTime);

        drawUI.SetActive(true);
        drawTime = Time.time;
        playerCanShoot = true;
        enemy.GetComponent<Character>().Shoot();

        yield return new WaitForSeconds(reactionTime);

        if (!roundEnded)
        {
            gunSounds.PlayOneShot(gunShot, 1);
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
                gunSounds.PlayOneShot(gunShot, 1);
                float reaction = Time.time - drawTime;
                player.GetComponent<Character>().Shoot();
                WinRound(reaction);
            }
        }
    }

    void WinRound(float reaction)
    {
        if (reaction < fastestDraw)
        {
            fastestDraw = reaction;
            fastestDrawText.text = "Fastest Draw " + fastestDraw.ToString("F3");
        }

        streak++;
        streakText.text = "Streak  " + streak.ToString("D2");
        int scoreIncrease = Mathf.FloorToInt(streak * ((1f / reaction) * 20)); //multiplies streak by reaction speed, makes the number bigger and rounds down to nearest int
        score += scoreIncrease;
        scoreText.text = "Score     " + score.ToString("D5");

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
            drawUI.SetActive(false);
            Debug.Log("You lost! Reason: " + reason);
            player.GetComponent<Character>().Die();
            yield return new WaitForSeconds(1f);
            resetButtonUI.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(resetButtonUI.gameObject);
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
            drawUI.SetActive(false);
            if (streak <= 14)
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
            resetButtonUI.SetActive(false);
            reactionTime = 1f;
            score = 0;
            scoreText.text = "Score     " + score.ToString("D5");
            streak = 0;
            streakText.text = "Streak  " + streak.ToString("D2");
            yield return new WaitForSeconds(1f);
            roundEnded = false;
            StartCoroutine(BeginRound());
        }
    }
}
