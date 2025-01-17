using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AuctionTimer : MonoBehaviour
{
    public float auctionTime = 30f;
    private float remainingTime;
    public Slider TimerBar;
    public TextMeshProUGUI Timer;
    public TMP_InputField PriceInputField;
    public TextMeshProUGUI CurrentBidText; // ���� �������� ǥ���� �ؽ�Ʈ
    public RandomTetrisSpawner tetrisSpawner;

    private bool isTimerRunning = false;
    private int currentBidPrice = 0; // ���� ������ ����

    void Start()
    {
        Debug.Log("AuctionTimer Script Started");

        if (tetrisSpawner != null)
        {
            tetrisSpawner.SpawnTetrisBlock();
        }
        StartTimer();

        UpdateCurrentBidUI(); // �ʱ� ���� UI ������Ʈ
    }

    void Update()
    {
        if (isTimerRunning)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
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

    public void OnBidButtonClicked()
    {
        if (PriceInputField != null)
        {
            string inputText = PriceInputField.text;

            if (int.TryParse(inputText, out int bidPrice))
            {
                currentBidPrice = bidPrice; // ���� ���� ���� ����
                Debug.Log($"�Էµ� ����: {currentBidPrice}");

                AddTime(1f);

                if (tetrisSpawner != null)
                {
                    tetrisSpawner.MoveBlockToStore();
                    tetrisSpawner.SpawnTetrisBlock();
                }

                UpdateCurrentBidUI(); // UI ������Ʈ
            }
            else
            {
                Debug.LogWarning("��ȿ�� ���ڰ� �Էµ��� �ʾҽ��ϴ�.");
            }
        }
    }

    public void AddTime(float additionalTime)
    {
        if (isTimerRunning)
        {
            remainingTime += additionalTime;

            if (remainingTime > auctionTime)
            {
                remainingTime = auctionTime;
            }

            UpdateUI();
            Debug.Log($"Time added: {additionalTime}, Remaining time: {remainingTime}");
        }
    }

    private void TimerEnded()
    {
        Debug.Log("Timer Ended!");
    }

    private void UpdateCurrentBidUI()
    {
        if (CurrentBidText != null)
        {
            CurrentBidText.text = $"���� ������:\n{currentBidPrice}";
        }
    }
}
