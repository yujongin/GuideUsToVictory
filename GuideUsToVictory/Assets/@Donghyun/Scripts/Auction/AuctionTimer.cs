using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AuctionTimer : MonoBehaviour
{
    public float auctionTime = 30f; // �⺻ ��� �ð�
    private float remainingTime;    // ���� �ð�
    public Slider TimerBar;         // Ÿ�̸� �����̴�
    public TextMeshProUGUI Timer;   // Ÿ�̸� �ؽ�Ʈ
    public TMP_InputField PriceInputField; // ���� �Է� �ʵ�
    public TextMeshProUGUI CurrentBidText; // ���� ������ ǥ�� �ؽ�Ʈ
    public TextMeshProUGUI StartingPriceText; // ���۰� �ؽ�Ʈ UI
    public TextMeshProUGUI LastBidderInfo; // ������ ������ ���� ǥ�� �ؽ�Ʈ
    public RandomTetrisSpawner tetrisSpawner; // ��� ������
    public TextMeshProUGUI PlayerGoldText; // �÷��̾� ��� ǥ�� UI <�߾� �ֻ��>

    private int StartingPrice = 0; // ��� ���۰���
    private bool isTimerRunning = false; // Ÿ�̸� ����
    private int currentBidPrice = 0;     // ���� ������ ����
    private string lastBidder = "None";  // ������ ������ �̸�
    private bool IsPlayerOneTurn = true;  //�÷��̾� 1,2 ������ ���� ���� bool�� ����

    private int AuctionCount = 0;  // ������� ī��Ʈ �ʱ�
    private const int MaxAuctionCount = 2;  // �ִ� ��� ���� Ƚ�� ����

    private int player1Gold = 10000; // Player 1�� �ʱ� ���
    private int player2Gold = 10000; // Player 2�� �ʱ� ���

    public BlockManager blockManager; // ��ȯ�Ƕ� ������� ��ũ��Ʈ ����
    void Start()
    {
        Debug.Log("AuctionTimer Script Started");
        UpdateStartingPriceUI();
        UpdatePlayerGoldUI();
        ResetAuctionValues(); // �ʱ�ȭ �� ����

        if (tetrisSpawner != null)
        {
            tetrisSpawner.SpawnTetrisBlock(); // ù ��� ����
        }

        StartTimer(); // Ÿ�̸� ����
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
                TimerEnded(); // �ð��� ����Ǹ� ȣ��
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
            StartingPriceText.text = $"���۰�: {StartingPrice} cost";
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
                // ���� ���������� ���ų� ���� ������ ��� �ƹ� �ϵ� �Ͼ�� ����
                if (bidPrice <= currentBidPrice)
                {
                    Debug.LogWarning($"bidPrice ({bidPrice}) <= currentBidPrice ({currentBidPrice}). Returning.");
                    return; // �޼��� ����
                }

                // ���� ������ ���, ���۰��� ���� �������� ����
                if (currentBidPrice == 0)
                {
                    currentBidPrice = StartingPrice + bidPrice;
                }
                else
                {
                    currentBidPrice = bidPrice; // ���� ���� ���� ������Ʈ
                }

                // Player Turn�� ���� ������ ������ ����
                lastBidder = IsPlayerOneTurn ? "Player 1" : "Player 2";
                IsPlayerOneTurn = !IsPlayerOneTurn; // �� ����

                Debug.Log($"�Էµ� ����: {currentBidPrice} by {lastBidder}");

                AddTime(1f); // �ð� ����

                UpdateCurrentBidUI(); // UI ������Ʈ
                UpdateLastBidderInfo(); // ������ ������ ���� ������Ʈ
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


        // ������ ������ ���� ���
        UpdateLastBidderInfo();

        // ������ ������ ���� ��� �� ��� ����
        if (currentBidPrice > 0)
        {
            DeductGold(); // ��� ����
        }

        AuctionCount++;   // ���Ƚ�� 1ȸ �߰�

        if (AuctionCount >= MaxAuctionCount)
        {
            Debug.Log("��� ����");
            EndAuction();
            return;
        }

        ResetAuctionValues(); // �ι�° ��� ��ŷ� �Ѿ �� ��� �� �ʱ�ȭ

        // ���ο� ��� ����
        if (tetrisSpawner != null)
        {
            tetrisSpawner.SpawnTetrisBlock();
        }

        StartTimer(); // Ÿ�̸� �����
    }

    private void EndAuction()
    {
        isTimerRunning = false;
        Debug.Log("�̹� ��Ŵ� ����Ǿ����ϴ�.");
    }

    private void ResetAuctionValues()
    {
        StartingPrice = Random.Range(2, 21) * 5; // ���۰��� 5�� ����� ���� �ʱ�ȭ
        currentBidPrice = 0; // ���� ������ �ʱ�ȭ
        lastBidder = "None"; // ������ ������ �ʱ�ȭ

        // UI �ʱ�ȭ
        UpdateStartingPriceUI(); // ���۰� UI ������Ʈ
        UpdateCurrentBidUI();
        UpdateLastBidderInfo();

        Debug.Log("Auction values have been reset.");
    }

    private void UpdateCurrentBidUI()
    {
        if (CurrentBidText != null)
        {
            CurrentBidText.text = $"���� ������:\n{currentBidPrice}";
        }
    }

    private void UpdateLastBidderInfo()
    {
        if (LastBidderInfo != null)
        {
            LastBidderInfo.text = $"������:\n{lastBidder}\n������: {currentBidPrice}";
        }
    }
}
