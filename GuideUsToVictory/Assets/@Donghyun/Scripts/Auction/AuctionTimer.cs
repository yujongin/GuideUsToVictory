using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AuctionTimer : MonoBehaviour
{
    public float auctionTime = 30f; // 기본 경매 시간
    private float remainingTime;    // 남은 시간
    public Slider TimerBar;         // 타이머 슬라이더
    public TextMeshProUGUI Timer;   // 타이머 텍스트
    public TMP_InputField PriceInputField; // 가격 입력 필드
    public TextMeshProUGUI CurrentBidText; // 현재 응찰가 표시 텍스트
    public TextMeshProUGUI StartingPriceText; // 시작가 텍스트 UI
    public TextMeshProUGUI LastBidderInfo; // 마지막 응찰자 정보 표시 텍스트
    public RandomTetrisSpawner tetrisSpawner; // 블록 스포너
    public TextMeshProUGUI PlayerGoldText; // 플레이어 골드 표시 UI <중앙 최상단>

    private int StartingPrice = 0; // 블록 시작가격
    private bool isTimerRunning = false; // 타이머 상태
    private int currentBidPrice = 0;     // 현재 응찰된 가격
    private string lastBidder = "None";  // 마지막 응찰자 이름
    private bool IsPlayerOneTurn = true;  //플레이어 1,2 번갈아 가기 위해 bool로 선언

    private int AuctionCount = 0;  // 경매진행 카운트 초기
    private const int MaxAuctionCount = 2;  // 최대 경매 가능 횟수 고정

    private int player1Gold = 10000; // Player 1의 초기 골드
    private int player2Gold = 10000; // Player 2의 초기 골드

    public BlockManager blockManager; // 소환의땅 블록저장 스크립트 참조
    void Start()
    {
        Debug.Log("AuctionTimer Script Started");
        UpdateStartingPriceUI();
        UpdatePlayerGoldUI();
        ResetAuctionValues(); // 초기화 값 설정

        if (tetrisSpawner != null)
        {
            tetrisSpawner.SpawnTetrisBlock(); // 첫 블록 생성
        }

        StartTimer(); // 타이머 시작
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
                TimerEnded(); // 시간이 종료되면 호출
            }
        }
    }
    private void DeductGold()
    {
        if (lastBidder == "Player 1")
        {
            player1Gold -= currentBidPrice;
        }
        else if (lastBidder == "Player 2")
        {
            player2Gold -= currentBidPrice;
        }

        UpdatePlayerGoldUI();
    }

    private void UpdatePlayerGoldUI()
    {
        if (PlayerGoldText != null)
        {
            PlayerGoldText.text = $"Player 1: {player1Gold}\nPlayer 2: {player2Gold}";
        }
    }

    private void UpdateStartingPriceUI()
    {
        if (StartingPriceText != null)
        {
            StartingPriceText.text = $"시작가: {StartingPrice} cost";
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
                // 현재 응찰가보다 낮거나 같은 가격인 경우 아무 일도 일어나지 않음
                if (bidPrice <= currentBidPrice)
                {
                    Debug.LogWarning($"bidPrice ({bidPrice}) <= currentBidPrice ({currentBidPrice}). Returning.");
                    return; // 메서드 종료
                }

                // 최초 응찰일 경우, 시작가를 현재 응찰가에 더함
                if (currentBidPrice == 0)
                {
                    currentBidPrice = StartingPrice + bidPrice;
                }
                else
                {
                    currentBidPrice = bidPrice; // 현재 응찰 가격 업데이트
                }

                // Player Turn에 따라 마지막 응찰자 설정
                lastBidder = IsPlayerOneTurn ? "Player 1" : "Player 2";
                IsPlayerOneTurn = !IsPlayerOneTurn; // 턴 변경

                Debug.Log($"입력된 가격: {currentBidPrice} by {lastBidder}");

                AddTime(1f); // 시간 연장

                UpdateCurrentBidUI(); // UI 업데이트
                UpdateLastBidderInfo(); // 마지막 응찰자 정보 업데이트
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


        // 마지막 응찰자 정보 출력
        UpdateLastBidderInfo();

        // 마지막 응찰자 정보 출력 및 골드 차감
        if (currentBidPrice > 0)
        {
            DeductGold(); // 골드 차감
        }

        AuctionCount++;   // 경매횟수 1회 추가

        if (AuctionCount >= MaxAuctionCount)
        {
            Debug.Log("경매 종료");
            EndAuction();
            return;
        }

        ResetAuctionValues(); // 두번째 블록 경매로 넘어갈 시 모든 값 초기화

        // 새로운 블록 생성
        if (tetrisSpawner != null)
        {
            tetrisSpawner.SpawnTetrisBlock();
        }

        StartTimer(); // 타이머 재시작
    }

    private void EndAuction()
    {
        isTimerRunning = false;
        Debug.Log("이번 경매는 종료되었습니다.");
    }

    private void ResetAuctionValues()
    {
        StartingPrice = Random.Range(2, 21) * 5; // 시작가를 5의 배수로 랜덤 초기화
        currentBidPrice = 0; // 현재 응찰가 초기화
        lastBidder = "None"; // 마지막 응찰자 초기화

        // UI 초기화
        UpdateStartingPriceUI(); // 시작가 UI 업데이트
        UpdateCurrentBidUI();
        UpdateLastBidderInfo();

        Debug.Log("Auction values have been reset.");
    }

    private void UpdateCurrentBidUI()
    {
        if (CurrentBidText != null)
        {
            CurrentBidText.text = $"현재 응찰가:\n{currentBidPrice}";
        }
    }

    private void UpdateLastBidderInfo()
    {
        if (LastBidderInfo != null)
        {
            LastBidderInfo.text = $"응찰자:\n{lastBidder}\n응찰가: {currentBidPrice}";
        }
    }
}
