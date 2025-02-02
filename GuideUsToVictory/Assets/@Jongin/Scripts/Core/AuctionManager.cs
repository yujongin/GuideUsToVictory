using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;
public class AuctionManager : MonoBehaviour
{
    public float auctionCooltime = 90f;

    public TMP_Text coolTimeTimerText;

    public GameObject auctionPanel;
    public Image auctionTimerImage;
    public TMP_Text auctionTimerText;
    public Button makeBidButton;

    public EAuctionState auctionState;

    public BlockGenerator blockGenerator;
    public Transform blockSpawnPosition;
    public GameObject blockGenerateEffect;

    public Material blueTeamMat;
    public Material redTeamMat;

    public TMP_InputField priceInput;
    public TMP_Text curBlockPriceText;
    public TMP_Text curBidTeamText;
    
    float auctionTime = 10f;
    float curAuctionTime = 0;

    float curBlockPrice = 0;
    ETeam curBidTeam;
    GameObject curBlock;

    CameraManager cameraManager;

    int auctionPhase = 0;
    void Start()
    {
        auctionState = EAuctionState.None;
        makeBidButton.onClick.AddListener(() =>
        {
            MakeBid(Managers.Game.enemyTeamData.Team);
        });

        cameraManager = FindFirstObjectByType<CameraManager>();

    }

    void Update()
    {
        if (auctionState == EAuctionState.None)
        {
            auctionCooltime -= Time.deltaTime;

            if (auctionCooltime <= 0)
            {
                cameraManager.ActiveCamera((int)ECameraType.Auction);
                auctionState = EAuctionState.BlockGenrate;
                auctionCooltime = 90f;
                auctionPhase = 1;
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
                    GenerateBlock();

                    auctionState = EAuctionState.Auction;
                    break;
                case EAuctionState.Auction:
                    auctionPanel.SetActive(true);
                    curAuctionTime -= Time.deltaTime;
                    auctionTimerText.text = Mathf.FloorToInt(curAuctionTime).ToString();
                    auctionTimerImage.fillAmount = curAuctionTime / auctionTime;

                    if (curAuctionTime <= 0)
                    {
                        auctionPanel.SetActive(false);
                        cameraManager.ActiveCamera((int)ECameraType.SummonGround);
                        SuccessfulBid();
                        auctionState = EAuctionState.Placement;
                    }
                    break;
                case EAuctionState.Placement:

                    break;
            }

        }
    }
    MeshRenderer[] meshes;
    void GenerateBlock()
    {
        curBlock = blockGenerator.GetRandomBlock();
        curBlock.transform.parent = blockSpawnPosition;
        curBlock.transform.localPosition = Vector3.zero;
        meshes = curBlock.GetComponentsInChildren<MeshRenderer>();
        Vector3 totalCenter = Vector3.zero;
        for (int i = 0; i < meshes.Length; i++)
        {
            totalCenter += meshes[i].bounds.center;
        }
        totalCenter = totalCenter / meshes.Length;
        Vector3 dir = blockSpawnPosition.position - totalCenter;
        curBlock.transform.localPosition += dir;
        curBlock.transform.localRotation = Quaternion.identity;
        blockGenerateEffect.SetActive(true);

        InitBlockPrice(meshes.Length);
        aiTeamData = Managers.Game.enemyTeamData;
    }

    void InitBlockPrice(int blockCount)
    {
        curBlockPrice = blockCount * 10;
        int addPrice = Random.Range(-3, 4) * 5;
        curBlockPrice = curBlockPrice + addPrice;
        curBlockPriceText.text = curBlockPrice.ToString();
    }
    public void MakeBid(ETeam team)
    {
        if (priceInput.text == "")
        {
            Managers.Game.CallNoticeTextFade("지불할 신앙을 입력하세요");
            return;
        }
        for (int i = 0; i < meshes.Length; i++)
        {
            Material[] newMaterials = meshes[i].materials;
            newMaterials[1] = team == ETeam.Blue ? blueTeamMat : redTeamMat;

            meshes[i].materials = newMaterials;
        }
        curBlockPrice = int.Parse(priceInput.text);
        curBlockPriceText.text = curBlockPrice.ToString();
        curBidTeam = team;
        curBidTeamText.text = team.ToString();

        curAuctionTime += 5;
        if(curAuctionTime > 30)
        {
            curAuctionTime = 30;
        }
        
        priceInput.text = "";
    }
    public void MakeBidText(int addPrice)
    {
        if (priceInput.text == "")
        {
            if (curBlockPrice + addPrice > Managers.Game.myTeamData.Faith)
            {
                Managers.Game.CallNoticeTextFade("신앙이 부족합니다.");
                return;
            }
            priceInput.text = (curBlockPrice + addPrice).ToString();
        }
        else
        {
            if (int.Parse(priceInput.text) + addPrice > Managers.Game.myTeamData.Faith)
            {
                Managers.Game.CallNoticeTextFade("신앙이 부족합니다.");
                return;
            }
            priceInput.text = (int.Parse(priceInput.text) + addPrice).ToString();
        }
    }

    public void ResetInputField()
    {
        priceInput.text = "";
    }

    public void SuccessfulBid()
    {
        if (curBidTeam == Managers.Game.myTeamData.Team)
        {

        }
        else
        {
            StartCoroutine(PlacementBlockAI());
        }
    }


    //AI 
    TeamData aiTeamData;
    public IEnumerator PlacementBlockAI()
    {
        yield return new WaitForSeconds(1f);
        Managers.SummonGround.AIBlockPlacement(curBlock);

    }

    public IEnumerator MakeBidAI()
    {
        

        yield return null;
    }
}
