# GuideUsToVictory
team project


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

##### *참고사항* 기존에는 팀 프로젝트였기 때문에 세 명이서 진행하려 했으나 다른 두 팀원이 참여하지 않아 혼자 진행하고 완성하게 됐습니다.
##### 그래서 "@Jongin" 폴더에 있는 스크립트만 확인해 주시면 됩니다.












