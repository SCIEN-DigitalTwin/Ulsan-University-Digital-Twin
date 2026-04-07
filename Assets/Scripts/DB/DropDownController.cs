using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using TMPro;
using UnityEngine;

public class DropDownController : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    public Transform contentPanel; // 스크롤뷰의 Content 영역
    public GameObject contentPanelPrefab;


    public GameObject prc_hist_tb;
    public GameObject strg_fns_in; 
    public GameObject strg_fns_out; 
    public GameObject strg_raw_in; 
    public GameObject strg_raw_out;

    public SlecteData slecteData;

   

    // 데이터를 바탕으로 Dictionary 설정
    private Dictionary<string, List<string>> dropdownOptions = new Dictionary<string, List<string>>()
    {
        { "prc_hist_tb", new List<string>
            {
                
                "PRD_SRL_NO",
                "PRD_PLN_NO",
                "CMP_CD",
                "PRD_CD",
                "CMP_LINE_ID",
                "CMP_EQ_ID",
                "PRD_TYP_NO",
                "PRD_LOT",
                "RECE_CD",
                "PRD_WRK_CD",
                "PRD_CYC_TM",
                "MN_CYC_TM",
                "RBT_CYC_TM",
                "REG_DT"
            }
        },
        { "prc_alr_tb", new List<string>
            {
                "PRD_SRL_NO",
                "PRD_PLN_NO",
                "CMP_CD",
                "PRD_CD",
                "CMP_LINE_ID",
                "CMP_EQ_ID",
                "PRD_TYP_NO",
                "PRD_LOT",
                "RECE_CD",
                "ALR_CD",
                "ALR_TYP",
                "REG_DT"
            }
        },
        { "strg_fns_in", new List<string>
            {
                "PRD_SRL_NO",
                "PRD_PLN_NO",
                "CMP_CD",
                "PRD_CD",
                "CMP_LINE_ID",
                "CMP_EQ_ID",
                "PRD_TYP_NO",
                "PRD_LOT",
                "RECE_CD",
                "STRG_CD",
                "SLOT_NM",
                "REG_DT"
            }
        },
        { "strg_fns_out", new List<string>
            {
                "PRD_SRL_NO",
                "PRD_PLN_NO",
                "CMP_CD",
                "PRD_CD",
                "CMP_LINE_ID",
                "CMP_EQ_ID",
                "PRD_LOT",
                "STRG_CD",
                "SLOT_NM",
                "REG_DT"
            }
        },
        { "strg_raw_in", new List<string>
            {
                "RAW_SRL_NO",
                "CMP_CD",
                "RAW_CD",
                "CMP_LINE_ID",
                "CMP_EQ_ID",
                "PRD_LOT",
                "STRG_CD",
                "SLOT_NM",
                "REG_DT"
            }
        },
        { "strg_raw_out", new List<string>
            {
                "PRD_SRL_NO",
                "RAW_SRL_NO",
                "PRD_PLN_NO",
                "CMP_CD",
                "PRD_CD",
                "CMP_LINE_ID",
                "CMP_EQ_ID",
                "PRD_TYP_NO",
                "PRD_LOT",
                "RECE_CD",
                "STRG_CD",
                "SLOT_NM",
                "REG_DT"
            }
        }
    };

    private List<GameObject> activeItems = new List<GameObject>(); // 동적으로 생성된 항목 관리
    private Coroutine updateCoroutine; // 업데이트를 관리하는 코루틴

    void Start()
    {
        // 드롭다운 초기화
        dropdown.options.Clear();
        dropdown.AddOptions(new List<string>(dropdownOptions.Keys));

        // 드롭다운 변경 이벤트 등록
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        // 초기 데이터 로드
        if (dropdown.options.Count > 0)
        {
            OnDropdownValueChanged(0);
        }

        // 데이터 업데이트 시작
        StartUpdatingData();
    }

    private void StartUpdatingData()
    {
        if (updateCoroutine == null)
        {
            updateCoroutine = StartCoroutine(UpdateDataRoutine());
        }
    }

    private void StopUpdatingData()
    {
        if (updateCoroutine != null)
        {
            StopCoroutine(updateCoroutine);
            updateCoroutine = null;
        }
    }

    private IEnumerator UpdateDataRoutine()
    {
        while (true)
        {
            if (slecteData.serverstatus)
            {
                OnDropdownValueChanged(dropdown.value); // 현재 선택된 값을 기반으로 데이터 갱신
            }
            else
            {
                SetAllDelete(); // 서버 상태가 false일 때 모든 이미지를 흰색으로 변경
            }
            yield return new WaitForSeconds(1f); // 1초마다 업데이트
        }
    }

    private void OnDestroy()
    {
        StopUpdatingData(); // 오브젝트 파괴 시 코루틴 중단
    }

    // 드롭다운 값 변경 시 호출
    private void OnDropdownValueChanged(int index)
    {
        if (!slecteData.serverstatus) return;

        var dataMap = slecteData.prc_hist_tb;

        // 선택된 데이터에 따라 dataMap 설정
        switch (dropdown.options[dropdown.value].text)
        {
            case "prc_hist_tb":
                dataMap = slecteData.prc_hist;
                break;
            case "prc_alr_tb":
                dataMap = slecteData.prc_alr;
                break;
            case "strg_fns_in":
                dataMap = slecteData.strg_fns_in_dic;
                break;
            case "strg_fns_out":
                dataMap = slecteData.strg_fns_out_dic;
                break;
            case "strg_raw_in":
                dataMap = slecteData.strg_raw_in_dic;
                break;
            case "strg_raw_out":
                dataMap = slecteData.strg_raw_out_dic;
                break;
        }

        // 이전 항목 제거
        foreach (var item in activeItems)
        {
            Destroy(item);
        }
        activeItems.Clear();

        // 선택된 키의 데이터 로드
        string selectedKey = dropdown.options[index].text;
        if (dropdownOptions.TryGetValue(selectedKey, out var columns))
        {
            foreach (var columnName in columns)
            {
                GameObject newItem = Instantiate(contentPanelPrefab, contentPanel);
                newItem.SetActive(true);

                TMP_Text nameText = newItem.transform.GetChild(0).GetComponent<TMP_Text>();
                TMP_Text valueText = newItem.transform.GetChild(1).GetComponent<TMP_Text>();


                if (nameText != null && valueText != null)
                {
                    nameText.text = columnName; // 컬럼 이름 표시
                    valueText.text = dataMap[columnName].ToString();
                   
                
                }

                activeItems.Add(newItem);
            }
        }
        Debug.Log("반복1");
    }


    // 모든 이미지를 흰색으로 변경
    public void SetAllDelete()
    {
        // 이전 항목 제거
        foreach (var item in activeItems)
        {
            Destroy(item);
        }
        activeItems.Clear();
    }
}
