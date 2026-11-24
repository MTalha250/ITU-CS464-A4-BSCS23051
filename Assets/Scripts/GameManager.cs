using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] int targetScore = 5, lives = 3;
    [SerializeField] float respawnDelay = 0.6f, spawnProtection = 1.5f;
    [SerializeField] string title = "Level01", tip = "";
    [SerializeField] Health player;
    [SerializeField] Transform respawnPoint;
    [SerializeField] LevelLoader loader;
    [SerializeField] TMP_Text titleText, scoreText, livesText, boostText, messageText, winTitle, winTime;
    [SerializeField] GameObject messagePanel, boostPanel, winPanel, losePanel, winFirst, loseFirst;

    int score, shownBoost = -1;
    float messageUntil;
    bool over, respawning;
    PlayerController controller;

    public bool Over => over;

    void Awake() => Instance = this;

    void Start()
    {
        controller = player.GetComponent<PlayerController>();
        titleText.text = title;
        UpdateScore();
        UpdateLives();
        Show(tip, 6f);
    }

    void Update()
    {
        if (messagePanel.activeSelf && Time.time >= messageUntil) messagePanel.SetActive(false);
        int boost = Mathf.CeilToInt(controller.BoostLeft);   // only touch the text when it changes
        if (boost == shownBoost) return;
        shownBoost = boost;
        boostPanel.SetActive(boost > 0);
        boostText.text = $"Speed boost: {boost} s";
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScore();
    }

    void UpdateScore() => scoreText.text = $"Coins: {score} / {targetScore}";
    void UpdateLives() => livesText.text = $"Lives: {lives}";

    public void ReachGoal()
    {
        if (score >= targetScore) Win();
        else Show($"You need {targetScore - score} more coins", 2f);
    }

    public void PlayerDied()
    {
        if (over || respawning) return;   // a fall during the respawn beat doesn't cost a second life
        lives--;
        UpdateLives();
        if (lives <= 0) Lose();
        else StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        respawning = true;
        yield return new WaitForSeconds(respawnDelay);     // a beat, so the hit's knockback is over
        Rigidbody body = player.GetComponent<Rigidbody>();
        body.position = respawnPoint.position;
        player.transform.position = respawnPoint.position;
        body.linearVelocity = Vector3.zero;                // back standing still
        player.Restore(spawnProtection);                   // full health and a moment of protection
        respawning = false;
    }

    void Show(string text, float seconds)
    {
        if (text == "") return;
        messageText.text = text;
        messagePanel.SetActive(true);
        messageUntil = Time.time + seconds;
    }

    // Lecture 10's win, with a win screen before the next level
    void Win()
    {
        if (over) return;
        over = true;
        int level = gameObject.scene.buildIndex;
        float time = Time.timeSinceLevelLoad;
        bool best = SaveData.Record(level, time);
        SaveData.Unlock(level + 1);
        ShowWinScreen(level, time, best);
    }

    void ShowWinScreen(int level, float time, bool best)
    {
        winTitle.text = level == 5 ? "You beat all five levels!" : $"Level {level} complete!";
        winTime.text = best ? $"New best time: {time:0.0} s" : $"Time: {time:0.0} s    Best: {SaveData.BestTime(level):0.0} s";
        Open(winPanel, winFirst);
    }

    void Lose()
    {
        over = true;
        Open(losePanel, loseFirst);
    }

    void Open(GameObject panel, GameObject first)
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
    }

    // the end screens' buttons, all through the one LevelLoader
    public void NextLevel() => loader.LoadNext();
    public void Restart() => loader.Reload();
    public void MainMenu() => loader.LoadMenu();
}
