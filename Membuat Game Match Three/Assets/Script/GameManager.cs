using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int playerScore;
    public Text scoreText;
    public Text timerText;
    [SerializeField] private int timer = 180;
    public UnityEvent onGameOver;
    public bool gameOver = false;
    public float multiplier = 1;
    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        if (onGameOver == null)
            onGameOver = new UnityEvent();

        DontDestroyOnLoad(gameObject);
        UpdateTimer();
        StartCoroutine(StartTimer());
    }

    private void Update()
    {
        if(timer <= 0)
        {
            GameOver();
        }
    }

    public void GetScore(int point)
    {
        playerScore += (int)Mathf.Round(point * multiplier);
        scoreText.text = playerScore.ToString();
    }

    private void UpdateTimer()
    {
        timerText.text = timer.ToString();
    }

    IEnumerator StartTimer()
    {
        while (timer > 0)
        {
            yield return new WaitForSeconds(1);
            timer--;
            UpdateTimer();
        }

    }

    private void GameOver()
    {
        onGameOver.Invoke();
        gameOver = true;
    }
}
