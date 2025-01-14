using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AuctionTimer : MonoBehaviour
{
    public float auctionTime = 30f;
    private float remainingTime;
    public Slider TimerBar;
    public TextMeshProUGUI Timer;
    public RandomTetrisSpawner tetrisSpawner;

    private bool isTimerRunning = false;

    void Start()
    {
        Debug.Log("AuctionTimer Script Started");


        if (tetrisSpawner != null)
        {
            tetrisSpawner.SpawnTetrisBlock();
        }
        StartTimer();
    }

    void Update()
    {
        Debug.Log("Update is running");
        if (isTimerRunning)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                Debug.Log("Remaining Time: " + remainingTime);
                UpdateUI();
            }
            else
            {
                remainingTime = 0;
                isTimerRunning = false;
                TimerEnded();
            }
        }
    }

    public void StartTimer()
    {
        Debug.Log("StartTimer Called");
        remainingTime = auctionTime;
        isTimerRunning = true;
        UpdateUI();
    }

    private void UpdateUI()
    {

        if (TimerBar != null)
        {

            TimerBar.value = (remainingTime / auctionTime);
        }


        if (Timer != null)
        {
            Timer.text = "시간: " + Mathf.Ceil(remainingTime).ToString();
        }
    }

    private void TimerEnded()
    {
        Debug.Log("Timer Ended!");

        // 타이머 종료 시 큐브 삭제
        if (tetrisSpawner != null)
        {
            tetrisSpawner.RemoveTetrisBlock();
        }
    }

    public void AddTime(float additionalTime)
    {
        if (isTimerRunning)
        {
            remainingTime += additionalTime; // 추가 시간 더하기

            // 타이머 시간이 총 경매 시간을 초과하지 않도록 제한
            if (remainingTime > auctionTime)
            {
                remainingTime = auctionTime;
            }

            UpdateUI(); // UI 업데이트
            Debug.Log($"Time added: {additionalTime}, Remaining time: {remainingTime}");
        }
    }
}