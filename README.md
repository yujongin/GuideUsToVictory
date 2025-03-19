# GuideUsToVictory


### 3주 게임 제작 프로젝트
#### (01/09 ~ 02/04)

### 게임 기획 링크 노션
[게임 세부 기획 설명 노션](https://devjongin.notion.site/170d6b2ef3d68014a743da128bb94fec?pvs=4)
<br/><br/><br/><br/>

## 기획 의도

### 개략적인 게임 플레이 & 게임의 목표

#### 1. 자원을 수급해 유닛을 생산하고 유닛들이 자동으로 싸워 상대방의 타워를 먼저 부수는 팀의 승리
#### 2. 유닛을 생산하기 위해서는 신앙과 블록이 필요
#### 3. 신앙은 매 5초마다 일정 지급 또는 상대 유닛을 잡아 획득

<br/>
<img src="https://github.com/user-attachments/assets/8f97ac6e-d885-4418-b6f5-a7dbb51daa3c"/> 

### 경매
#### 4. 블록은 신앙을 사용한 경매를 통해 획득
#### 5. 가지고 있는 신앙을 사용해 블록을 응찰
#### 6. 플레이어와 AI중 더 높은 가격으로 응찰한 쪽이 낙찰

[경매 관련 주요 코드](https://github.com/yujongin/GuideUsToVictory/blob/main/GuideUsToVictory/Assets/%40Jongin/Scripts/Core/AuctionManager.cs)

<br/>
<img src="https://github.com/user-attachments/assets/0167c103-1b8a-4a9f-bd69-a416bbefd1b5"/> 

### 블록 배치
#### 7. 낙찰받은 블록은 소환의 땅에 배치
#### 8. 자신의 블록과 연결해서만 배치 가능
#### 9. AI는 블록을 놨을 때 전체 블록의 너비가 가장 작은 장소를 찾아 배치
#### 10. 플레이어는 정해진 시간 안에 가능한 지점 중 한 곳에 배치해야 하고 시간이 지나면 AI처럼 자동 배치

[플레이어 블록 배치 코드](https://github.com/yujongin/GuideUsToVictory/blob/main/GuideUsToVictory/Assets/%40Jongin/Scripts/BlockArrange/PlayerBlockPlacement.cs)
<br/>
[AI 블록 배치 코드](https://github.com/yujongin/GuideUsToVictory/blob/main/GuideUsToVictory/Assets/%40Jongin/Scripts/EnemyAI/BlockPlacementAI.cs)

<br/>
<img src="https://github.com/user-attachments/assets/c034b1ae-e60d-4638-a4e1-45eb1ec0304a"/>

<br/>

## 전체적인 코드 구조

##### 기존에는 팀 프로젝트였기 때문에 세 명이서 진행하려 했으나 다른 두 팀원이 참여하지 않아 혼자 진행하고 완성하게 됐습니다.

<br/>

### Core
#### Core 폴더에는 여러 Manager들이 들어있습니다. 
#### Managers가 싱글톤으로 다른 Manager들을 가지고 있고 
#### Manager들은 각각 자신에게 맞는 역할을 하고 있습니다.
[Core 폴더](https://github.com/yujongin/GuideUsToVictory/tree/main/GuideUsToVictory/Assets/%40Jongin/Scripts/Core)

<br/>

### 유닛 생성
#### ResourceManager에서 모든 종족 유닛들의 프리펩을 가지고 있고
#### GameManager에서 우리 팀이 어떤 종족인지 확인하고 ResourceManager로 부터 알맞은 유닛들을 받아와 TeamData에 저장합니다.
#### UnitSpawnManager에서 GameManager의 TeamData를 통해 유닛을 소환할 수 있도록 해줍니다.


<br/>

### 경매 및 블록 배치
#### AuctionManager에서 정해진 시간마다 경매를 시작하고 BlockGenerator를 통해 블록을 생성해 받아옵니다. 
#### 원하는 쪽이 선착순으로 응찰하고 높은 가격을 제시한 쪽이 블록을 낙찰받습니다.
#### 플레이어가 블록을 낙찰받으면 PlaceBlock, AI가 낙찰받으면 BlockPlacementAI에서 블록을 알맞게 배치할 수 있도록 합니다.
#### 경매는 경매 -> 블록 배치 순으로 2세트 진행합니다.


<br/>

### AI
#### 유닛 선택 AI
#### 유닛 선택 AI는 Greedy 알고리즘으로 해금된 유닛 중 가장 높은 유닛을 뽑을 수 있는 만큼 뽑고
#### 필요 자원이 부족하면 아랫 단계의 유닛을 뽑도록 만들었습니다.
[UnitSelectAI](https://github.com/yujongin/GuideUsToVictory/blob/main/GuideUsToVictory/Assets/%40Jongin/Scripts/EnemyAI/UnitSelectAI.cs)

<br/>

#### 경매 응찰 AI
#### 경매 응찰 AI는 현재 AI가 가지고 있는 신앙(재화)을 기반으로 첫 번째 경매는 블록 수에 맞는 유닛을 뽑을 수 있도록
#### 최소한의 돈을 남기고 모두 응찰하도록 하였고 
#### 두 번째 경매는 만약 첫 번째 경매에서 낙찰받지 못했다면 유닛을 뽑는 것을 고려하지 않고 
#### 모든 돈을 다 쓸 때까지 응찰하도록 하였습니다.
[AuctionManager](https://github.com/yujongin/GuideUsToVictory/blob/main/GuideUsToVictory/Assets/%40Jongin/Scripts/Core/AuctionManager.cs)

<br/>

#### 블록 배치 AI
#### 블록배치 AI는 현재 생성된 블록의 자식 오브젝트들인 한 칸짜리 블록들의 localPosition을 이용하여
#### 중심된 블록이 바뀌었을 때와 블록이 회전했을 때의 모든 블록 localPosition을 고려하여
#### 모든 경우의 수를 놓을 수 있는 자리에 놓아보았을 때
#### 모든 연결된 블록의 너비가 작은 경우의 수로 배치하도록 하였습니다.
[BlockPlacementAI](https://github.com/yujongin/GuideUsToVictory/blob/main/GuideUsToVictory/Assets/%40Jongin/Scripts/EnemyAI/BlockPlacementAI.cs)







