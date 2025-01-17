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
    public TextMeshProUGUI CurrentBidText; // 현재 응찰가를 표시할 텍스트
    public RandomTetrisSpawner tetrisSpawner;

    private bool isTimerRunning = false;
    private int currentBidPrice = 0; // 현재 응찰된 가격

    void Start()
    {
        Debug.Log("AuctionTimer Script Started");

        if (tetrisSpawner != null)
        {
            tetrisSpawner.SpawnTetrisBlock();
        }
        StartTimer();

        UpdateCurrentBidUI(); // 초기 상태 UI 업데이트
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
            Timer.text = "시간: " + Mathf.Ceil(remainingTime).ToString();
        }
    }

    public void OnBidButtonClicked()
    {
        if (PriceInputField != null)
        {
            string inputText = PriceInputField.text;

            if (int.TryParse(inputText, out int bidPrice))
            {
                currentBidPrice = bidPrice; // 현재 응찰 가격 저장
                Debug.Log($"입력된 가격: {currentBidPrice}");

                AddTime(1f);

                if (tetrisSpawner != null)
                {
                    tetrisSpawner.MoveBlockToStore();
                    tetrisSpawner.SpawnTetrisBlock();
                }

                UpdateCurrentBidUI(); // UI 업데이트
            }
            else
            {
                Debug.LogWarning("유효한 숫자가 입력되지 않았습니다.");
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
            CurrentBidText.text = $"현재 응찰가:\n{currentBidPrice}";
        }
    }
}
