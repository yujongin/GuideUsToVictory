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
            Timer.text = "�ð�: " + Mathf.Ceil(remainingTime).ToString();
        }
    }

    private void TimerEnded()
    {
        Debug.Log("Timer Ended!");

        // Ÿ�̸� ���� �� ť�� ����
        if (tetrisSpawner != null)
        {
            tetrisSpawner.RemoveTetrisBlock();
        }
    }

    public void AddTime(float additionalTime)
    {
        if (isTimerRunning)
        {
            remainingTime += additionalTime; // �߰� �ð� ���ϱ�

            // Ÿ�̸� �ð��� �� ��� �ð��� �ʰ����� �ʵ��� ����
            if (remainingTime > auctionTime)
            {
                remainingTime = auctionTime;
            }

            UpdateUI(); // UI ������Ʈ
            Debug.Log($"Time added: {additionalTime}, Remaining time: {remainingTime}");
        }
    }
}