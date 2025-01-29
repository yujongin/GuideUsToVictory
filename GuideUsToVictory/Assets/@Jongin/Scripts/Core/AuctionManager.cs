using TMPro;
using UnityEngine;

public class AuctionManager : MonoBehaviour
{
    public float auctionCooltime = 70f;

    public bool isOpenAuction = false;
    public TMP_Text timerText;

    void Start()
    {
        
    }

    void Update()
    {
        if (!isOpenAuction)
        {
            auctionCooltime -= Time.deltaTime;
            
            if(auctionCooltime <= 0)
            {
                isOpenAuction = true;
                auctionCooltime = 90f;
            }
            string m = Mathf.FloorToInt(auctionCooltime / 60).ToString();
            string s = Mathf.FloorToInt(auctionCooltime % 60) < 10 ?
                "0" + Mathf.FloorToInt(auctionCooltime % 60).ToString() 
                : Mathf.FloorToInt(auctionCooltime % 60).ToString();
            timerText.text = m + " : " + s;
        }
    }
}
