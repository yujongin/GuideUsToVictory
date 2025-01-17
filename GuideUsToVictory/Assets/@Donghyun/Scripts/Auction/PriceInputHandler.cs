using TMPro;
using UnityEngine;

public class PriceInputHandler : MonoBehaviour
{
    public TMP_InputField priceInputField; // Price Input Field 참조

    public void OnSubmitBid()
    {
        // 입력된 가격 가져오기
        string inputText = priceInputField.text;

        // 숫자로 변환
        if (int.TryParse(inputText, out int bidPrice))
        {
            Debug.Log($"입력된 가격: {bidPrice}");

            // 입력된 가격을 처리하는 추가 로직
            ProcessBid(bidPrice);
        }
        else
        {
            Debug.LogWarning("유효한 숫자가 입력되지 않았습니다.");
        }
    }

    private void ProcessBid(int bidPrice)
    {
        // 예시: 입력된 가격을 로그로 출력하거나 저장
        Debug.Log($"제안된 가격은 {bidPrice}입니다.");
        // 필요한 추가 로직 작성
    }
}
