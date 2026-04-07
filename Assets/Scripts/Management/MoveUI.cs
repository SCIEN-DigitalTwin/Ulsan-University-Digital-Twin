using System.Collections;
using UnityEngine;
using DG.Tweening; // DoTween 사용을 위한 네임스페이스 추가

public class MoveUI : MonoBehaviour
{
    public GameObject panel; // 이동할 패널
    public bool isCanTab = true;

    public GameObject DCPanel; // 이동할 패널
    public bool panelStatus = true;

    public void DCBtn()
    {
        if (panelStatus)
        {
            PanelOn();
        }
        else
        {
            PanelOFF();
        }
    }
    public void PanelOn()
    {
        DCPanel.SetActive(true);
        panelStatus = false;
    }

    public void PanelOFF()
    {
        DCPanel.SetActive(false);
        panelStatus = true;
    }

    public void MenuBtn()
    {
        if(isCanTab)
        {
            QuestOn(0.3f); // Tab 키를 누를 때 패널 표시
        }
        else
        {
            QuestOff(0.3f); // Tab 키를 놓을 때 패널 숨기기
        }
    }
    public void QuestOn(float duration)
    {
        isCanTab = false;
        // 패널을 화면 안쪽으로 이동
        panel.GetComponent<RectTransform>().DOAnchorPosX(50f, duration).SetEase(Ease.OutCubic);
    }

    public void QuestOff(float duration)
    {
        isCanTab = true;
        // 패널을 화면 밖으로 이동
        panel.GetComponent<RectTransform>().DOAnchorPosX(-150f, duration).SetEase(Ease.InCubic);
    }
}
