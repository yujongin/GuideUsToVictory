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

    [HideInInspector]
    public bool isPlacement;

    public GameObject summonGroundPanel;
    public Image placementTimerImage;
    public TMP_Text placementTimerText;

    public EAuctionState auctionState;

    public Transform blockSpawnPosition;
    public GameObject blockGenerateEffect;

    public Material blueTeamMat;
    public Material redTeamMat;

    public TMP_InputField priceInput;
    public TMP_Text curBlockPriceText;
    public TMP_Text curBidTeamText;

    float auctionTime = 20f;
    float curAuctionTime = 0;

    float curBlockPrice = 0;
    ETeam curBidTeam;
    GameObject curBlock;

    BlockGenerator blockGenerator;
    CameraManager cameraManager;
    PlayerBlockPlacement playerBlockPlacement;

    int auctionPhase = 0;

    bool isStart = false;
    void Start()
    {
        auctionState = EAuctionState.None;
        curBidTeam = ETeam.None;
        makeBidButton.onClick.AddListener(() =>
        {
            if (priceInput.text == "")
            {
                Managers.Game.CallNoticeTextFade("지불할 신앙을 입력하세요", Color.red);
                return;
            }
            MakeBid(Managers.Game.myTeamData.Team, int.Parse(priceInput.text));
            CallMakeBIdAI();
        });

        cameraManager = FindFirstObjectByType<CameraManager>();
        blockGenerator = FindFirstObjectByType<BlockGenerator>();
        playerBlockPlacement = FindFirstObjectByType<PlayerBlockPlacement>();
        playerBlockPlacement.originalMaterial = blueTeamMat;
    }

    void Update()
    {
        if (Managers.Game.GameState == EGameState.End)
        {
            StopAllCoroutines();
            return;
        }
        if (auctionState == EAuctionState.None)
        {
            auctionCooltime -= Time.deltaTime;

            if (auctionCooltime <= 0)
            {
                cameraManager.ActiveCamera((int)ECameraType.Auction);
                auctionState = EAuctionState.BlockGenerate;
                auctionCooltime = 90f;
                auctionPhase = 1;
                isAIWon = false;
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
                case EAuctionState.BlockGenerate:
                    //block generate
                    curAuctionTime = auctionTime;
                    GenerateBlock();

                    auctionState = EAuctionState.Auction;
                    break;
                case EAuctionState.Auction:
                    if(!isStart)
                    {
                        CallMakeBIdAI();
                        isStart = true;
                    }
                    auctionPanel.SetActive(true);
                    curAuctionTime -= Time.deltaTime;
                    auctionTimerText.text = Mathf.FloorToInt(curAuctionTime).ToString();
                    auctionTimerImage.fillAmount = curAuctionTime / auctionTime;

                    if (curAuctionTime <= 0)
                    {
                        if(curBidTeam == ETeam.None)
                        {
                            Destroy(curBlock);
                            EndPlacementTurn();
                            return;
                        }
                        auctionPanel.SetActive(false);
                        cameraManager.ActiveCamera((int)ECameraType.SummonGround);
                        SuccessfulBid();
                        auctionState = EAuctionState.Placement;
                        isStart = false;
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
    }

    void InitBlockPrice(int blockCount)
    {
        curBlockPrice = blockCount * 10;
        int addPrice = Random.Range(-3, 4) * 5;
        curBlockPrice = curBlockPrice + addPrice;
        curBlockPriceText.text = curBlockPrice.ToString();
    }
    public void MakeBid(ETeam team, int price)
    {
        for (int i = 0; i < meshes.Length; i++)
        {
            Material[] newMaterials = meshes[i].materials;
            newMaterials[1] = team == ETeam.Blue ? blueTeamMat : redTeamMat;

            meshes[i].materials = newMaterials;
        }
        curBlockPrice = price;
        curBlockPriceText.text = curBlockPrice.ToString();
        curBidTeam = team;
        curBidTeamText.text = team.ToString();

        curAuctionTime += 3;
        if (curAuctionTime > auctionTime)
        {
            curAuctionTime = auctionTime;
        }

        priceInput.text = "";
    }
    public void MakeBidText(int addPrice)
    {
        if (priceInput.text == "")
        {
            if (curBlockPrice + addPrice > Managers.Game.myTeamData.Faith)
            {
                Managers.Game.CallNoticeTextFade("신앙이 부족합니다.", Color.red);
                return;
            }
            priceInput.text = (curBlockPrice + addPrice).ToString();
        }
        else
        {
            if (int.Parse(priceInput.text) + addPrice > Managers.Game.myTeamData.Faith)
            {
                Managers.Game.CallNoticeTextFade("신앙이 부족합니다.", Color.red);
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
        Managers.Game.UseFaith(curBidTeam, curBlockPrice);
        if (curBidTeam == Managers.Game.myTeamData.Team)
        {
            StartCoroutine(PlacementBlockPlayer());
        }
        else
        {
            StartCoroutine(PlacementBlockAI());
        }
    }

    public IEnumerator PlacementBlockPlayer()
    {
        yield return new WaitForSeconds(1f);
        summonGroundPanel.SetActive(true);
        isPlacement = false;
        float time = 10f;
        curBlock.transform.rotation = Quaternion.identity;
        playerBlockPlacement.neighbors = Managers.SummonGround.GetNeighborNodes(Managers.Game.myTeamData.Team);
        playerBlockPlacement.target = curBlock;

        while (time > 0)
        {
            if (isPlacement)
                break;
            time -= Time.deltaTime;
            placementTimerImage.fillAmount = time / 10f;
            placementTimerText.text = Mathf.FloorToInt(time).ToString();

            if (time <= 0)
            {
                //auto placement
                Managers.SummonGround.AutoBlockPlacement(curBlock);
                playerBlockPlacement.AutoBlockPlacement();
                time = 0;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        summonGroundPanel.SetActive(false);
        EndPlacementTurn();
    }

    //AI 
    public IEnumerator PlacementBlockAI()
    {
        yield return new WaitForSeconds(1f);
        Managers.SummonGround.AIBlockPlacement(curBlock);
        yield return new WaitForSeconds(1f);
        EndPlacementTurn();
    }

    void EndPlacementTurn()
    {
        if (auctionPhase == 1)
        {
            cameraManager.ActiveCamera((int)ECameraType.Auction);
            auctionPhase++;
            auctionState = EAuctionState.BlockGenerate;
        }
        else if (auctionPhase == 2)
        {
            cameraManager.ActiveCamera((int)ECameraType.Battle);
            auctionPhase = 0;
            auctionState = EAuctionState.None;
        }
        curBidTeam = ETeam.None;
    }


    void CallMakeBIdAI()
    {
        if(makeBidAI!= null)
        {
            StopCoroutine(makeBidAI);
        }

        StartCoroutine(MakeBidAI());
    }

    Coroutine makeBidAI;
    bool isAIWon = false;
    IEnumerator MakeBidAI()
    {
        yield return new WaitForSeconds(1);
        TeamData aiData = Managers.Game.enemyTeamData;
        float remainFaith = aiData.Faith;
        float minUnitPrice = curBlock.transform.childCount * 20f;

        if (auctionPhase == 1)
        {
            if (curBlockPrice <= remainFaith - minUnitPrice - 5)
            {
                MakeBid(aiData.Team, (int)curBlockPrice + 5);
            }
        }
        else
        {
            if (!isAIWon)
            {
                if (curBlockPrice <= remainFaith - 5)
                {
                    MakeBid(aiData.Team, (int)curBlockPrice + 5);
                }
            }
            else
            {
                if (curBlockPrice <= remainFaith - minUnitPrice - 5)
                {
                    MakeBid(aiData.Team, (int)curBlockPrice + 5);
                }
            }
        }
    }
}
