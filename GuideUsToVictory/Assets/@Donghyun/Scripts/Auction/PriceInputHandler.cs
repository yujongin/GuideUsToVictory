using TMPro;
using UnityEngine;

public class PriceInputHandler : MonoBehaviour
{
    public TMP_InputField priceInputField; // Price Input Field ����

    public void OnSubmitBid()
    {
        // �Էµ� ���� ��������
        string inputText = priceInputField.text;

        // ���ڷ� ��ȯ
        if (int.TryParse(inputText, out int bidPrice))
        {
            Debug.Log($"�Էµ� ����: {bidPrice}");

            // �Էµ� ������ ó���ϴ� �߰� ����
            ProcessBid(bidPrice);
        }
        else
        {
            Debug.LogWarning("��ȿ�� ���ڰ� �Էµ��� �ʾҽ��ϴ�.");
        }
    }

    private void ProcessBid(int bidPrice)
    {
        // ����: �Էµ� ������ �α׷� ����ϰų� ����
        Debug.Log($"���ȵ� ������ {bidPrice}�Դϴ�.");
        // �ʿ��� �߰� ���� �ۼ�
    }
}
