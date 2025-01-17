using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AuctionTimer : MonoBehaviour
{
    public float auctionTime = 30f;  // 경매 총 시간
    private float remainingTime;     // 남은 시간
    public Slider TimerBar;          // 타이머 슬라이더
    public TextMeshProUGUI Timer;    // 타이머 텍스트
    public TMP_InputField PriceInputField; // 가격 입력 필드
    public TextMeshProUGUI CurrentBidText; // 현재 응찰가를 표시할 텍스트
    public RandomTetrisSpawner tetrisSpawner; // Tetris 블록 스포너

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

            // 숫자로 변환
            if (int.TryParse(inputText, out int bidPrice))
            {
                currentBidPrice = bidPrice; // 현재 응찰 가격 저장
                Debug.Log($"입력된 가격: {currentBidPrice}");

                // 시간이 5초 늘어나도록 처리
                AddTime(5f);

                // 기존 블록 저장 및 새로운 블록 생성
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

            // 남은 시간이 총 경매 시간을 초과하지 않도록 제한
            if (remainingTime > auctionTime)
            {
                remainingTime = auctionTime;
            }

            UpdateUI(); // UI 업데이트
            Debug.Log($"Time added: {additionalTime}, Remaining time: {remainingTime}");
        }
    }

    private void TimerEnded()
    {
        Debug.Log("Timer Ended!");
        // 타이머가 종료될 때 처리할 로직 추가
    }
}
