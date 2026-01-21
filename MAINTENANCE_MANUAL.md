# 유지보수 매뉴얼 (초안)
대상: `SLC_Controller` WinForms 앱  
핵심 코드: `SLC_Controller\Form1.cs`, UI 이벤트 연결: `SLC_Controller\Form1.Designer.cs`, 진입점: `SLC_Controller\Program.cs`

## 전체 구조
- 앱 진입: `Program.Main()` → `new Form1()` 실행 (`SLC_Controller\Program.cs`)
- 주요 로직: `Form1` 클래스 (`SLC_Controller\Form1.cs`)
- UI 컨트롤/이벤트 연결: `Form1.Designer.cs`의 `InitializeComponent`

## 기능별 수정 위치
- **연결/네트워크 변경**
  - 연결 흐름: `btnConnect_Click` (`Form1.cs`)
  - IP 표시/검출: `ShowIP`
  - 장치 IP 기본값 변경: `btnConnect_Click` 내부 `lc.IP = "172.28.37.101"`
- **테스트 모드(자동 순차 구동)**
  - 시작/중지 로직: `btnTest_Click`
  - 주기 타이머: `TestTimer_Elapsed`, `testTimer.Interval`
  - 채널 순차 구동: `TrigChannels`
- **채널 트리거(수동)**
  - 버튼 클릭 처리: `btnTrigger_Click`
  - 채널 출력 종료 처리: `SetOffMode`
  - UI 하이라이트: `HighlightChannel`
- **채널 설정(Set / SetAll)**
  - 개별 설정 저장: `btnSet_Click`
  - 전체 저장: `btnSetAll_Click`
  - 설정 저장소: `channelSettings` (Dictionary)
  - 전류 최대값: `MAX_Ima`
- **모드/입력 UI 동작**
  - 모드 변경 UI 반영: `cbMode_SelectedIndexChanged` → `ModeSetting`
  - 기본값 설정: `InitUI`
  - 입력 필드 활성/표시: `SetTbState`, `SetControlsEnabled`
- **사이클/딜레이 검증**
  - 계산/제한: `UpdateMaxDelayAndValidate`
  - 이벤트 연결: `tbCycleTime_TextChanged`, `tbDelay_TextChanged`
- **시뮬레이션 모드**
  - 토글 처리: `cbSimulationMode_CheckedChanged`
  - 동작 분기: `isSimulationMode` 플래그 사용 (예: `btnTest_Click`, `btnTrigger_Click`, `TrigChannels`)
- **로그 출력**
  - 공통 로그: `Log(string message)`
  - 타임스탬프 포함, `rtbLog`에 출력

## UI 이벤트 연결 위치 (디자이너)
- `Form1.Designer.cs` 참고:
  - `btnConnect.Click` → `btnConnect_Click`
  - `btnTest.Click` → `btnTest_Click`
  - `btnTrigger1~4.Click` → `btnTrigger_Click`
  - `btnSet1~4.Click` → `btnSet_Click`
  - `btnSetAll.Click` → `btnSetAll_Click`
  - `cbMode1~4.SelectedIndexChanged` → `cbMode_SelectedIndexChanged`
  - `cbSimulationMode.CheckedChanged` → `cbSimulationMode_CheckedChanged`

## 데이터 흐름 요약
- UI 입력 → `btnSet_Click/btnSetAll_Click` → `channelSettings` 저장
- 테스트 시작 → `btnTest_Click` → `TrigChannels` 순차 실행
- 실장치 출력 → `AbeoLightCon` (`lc`) 메서드 호출
- 시뮬레이션 → `isSimulationMode`로 하드웨어 호출 스킵

## 수정 시 주의점
- 장치 출력 관련 호출은 `isSimulationMode`와 `lc.Connected` 조건을 반드시 확인
- `MAX_Ima` 초과 시 자동 보정 동작 있음 (전류 상한 변경 시 함께 수정)
- `Delay`는 `CycleTime / 4` 제한 (변경 시 `UpdateMaxDelayAndValidate` 수정)
- `SetOffMode`는 “끄기”를 펄스로 처리 (하드웨어 요구사항 변경 시 수정)
