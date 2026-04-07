using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MachineStatus : MonoBehaviour
{
    public SlecteData slecteData;
    public RawImage[] images; // 상태를 표시할 RawImage 배열
    private Color color;      // 변경할 색상

    private Coroutine statusCheckCoroutine; // 상태 확인 Coroutine

    // 시작 시 Coroutine 실행
    private void Start()
    {
        // 상태 확인 Coroutine 시작
        statusCheckCoroutine = StartCoroutine(UpdateStatusRoutine());
    }

    // 상태 확인 루틴
    private IEnumerator UpdateStatusRoutine()
    {
        while (true) // 무한 반복
        {
            if (slecteData.serverstatus)
            {
                SetStatus(); // 상태 업데이트
            }
            else
            {
                SetAllWhite(); // 서버 상태가 false일 때 모든 이미지를 흰색으로 변경
            }
            yield return new WaitForSeconds(1f); // 1초 대기
        }
    }

    // 상태 변경 로직
    public void SetStatus()
    {
        foreach (var item in slecteData.prc_hist_tb_list)
        {
            // 상태에 따른 색상 결정
            if (item["PRD_WRK_CD"].ToString() == "END")
            {
                color = Color.white; // 흰색 설정
            }
            else if (item["PRD_WRK_CD"].ToString() == "STR")
            {
                color = Color.green; // 초록색 설정
            }

            // 선택된 데이터에 따라 이미지 색상 변경
            string cmpEqId = item["CMP_EQ_ID"].ToString();

            switch (cmpEqId)
            {
                case "EQ01":
                    images[0].color = color;
                    break;
                case "EQ02":
                    images[1].color = color;
                    break;
                case "EQ03":
                    images[2].color = color;
                    break;
                case "EQ04":
                    images[3].color = color;
                    break;
                case "EQ05":
                    images[4].color = color;
                    break;
                case "EQ06":
                    images[5].color = color;
                    break;
                case "EQ07":
                    images[6].color = color;
                    break;
                case "EQ08":
                    images[7].color = color;
                    break;
                case "EQ09":
                    images[8].color = color;
                    break;
                case "EQ10":
                    images[9].color = color;
                    break;
                case "EQ11":
                    images[10].color = color;
                    break;
                default:
                    Debug.LogWarning($"Unknown CMP_EQ_ID: {cmpEqId}");
                    break;
            }
        }
        

        
    }

    // 모든 이미지를 흰색으로 변경
    public void SetAllWhite()
    {
        foreach (var image in images)
        {
            image.color = Color.white;
        }
    }

    // 상태 확인 중지
    private void OnDisable()
    {
        if (statusCheckCoroutine != null)
        {
            StopCoroutine(statusCheckCoroutine); // Coroutine 중지
        }
    }
}
