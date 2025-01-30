using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;
public class AuctionManager : MonoBehaviour
{
    public float auctionCooltime = 90f;

    public TMP_Text coolTimeTimerText;

    public Image auctionTimerImage;
    public TMP_Text auctionTimerText;

    public EAuctionState auctionState;

    public BlockGenerator blockGenerator;

    float auctionTime = 30f;
    float curAuctionTime = 0;
    GameObject curBlock;
    void Start()
    {
        auctionState = EAuctionState.None;
    }

    void Update()
    {
        if (auctionState == EAuctionState.None)
        {
            auctionCooltime -= Time.deltaTime;

            if (auctionCooltime <= 0)
            {
                auctionState = EAuctionState.BlockGenrate;
                auctionCooltime = 90f;
            }
            string m = Mathf.FloorToInt(auctionCooltime / 60).ToString();
            string s = Mathf.FloorToInt(auctionCooltime % 60) < 10 ?
                "0" + Mathf.FloorToInt(auctionCooltime % 60).ToString()
                : Mathf.FloorToInt(auctionCooltime % 60).ToString();
            coolTimeTimerText.text = m + " : " + s;
        }
        else
        {
            switch (auctionState)
            {
                case EAuctionState.BlockGenrate:
                    //block generate
                    curAuctionTime = auctionTime;
                    curBlock = blockGenerator.GetRandomBlock();
                    auctionState = EAuctionState.Auction;
                    break;
                case EAuctionState.Auction:
                    auctionTimerImage.gameObject.SetActive(true);
                    curAuctionTime -= Time.deltaTime;
                    auctionTimerText.text = Mathf.FloorToInt(curAuctionTime).ToString();
                    auctionTimerImage.fillAmount = curAuctionTime / auctionTime;

                    if (curAuctionTime <= 0)
                    {
                        auctionTimerImage.gameObject.SetActive(false);
                        auctionState = EAuctionState.Placement;
                    }
                    break;
                case EAuctionState.Placement:
                    break;
            }

        }

    }
}
