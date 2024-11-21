using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class SlecteData : MonoBehaviour
{
    private MySqlConnectionStringBuilder stringBuilder;
    public string ip;
    public uint port;
    public string db;
    public string userID;
    public string userPW;
    public bool serverstatus;
    MySqlSslMode sslMode;
    string charset;
    private bool isRunning = false;  // FetchDataAsync 실행 여부

    private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    //불러온 데이터 딕셔너리
    public Dictionary<string, object> prc_hist_tb = new Dictionary<string, object>();
    public Dictionary<string, object> strg_fns_in_dic = new Dictionary<string, object>();
    public Dictionary<string, object> strg_fns_out_dic = new Dictionary<string, object>();
    public Dictionary<string, object> strg_raw_in_dic = new Dictionary<string, object>();
    public Dictionary<string, object> strg_raw_out_dic = new Dictionary<string, object>();
    

    public void UpdateDatabaseInfo(string newIp, string newPort, string newDb, string newUserID, string newUserPW, bool status, MySqlSslMode sslMo, string chars)
    {
        ip = newIp;
        port = uint.Parse(newPort);
        db = newDb;
        userID = newUserID;
        userPW = newUserPW;
        serverstatus = status;
        sslMode = sslMo;
        charset = chars;
        stringBuilder = new MySqlConnectionStringBuilder
        {
            Server = ip,
            Port = port,
            Database = db,
            UserID = userID,
            Password = userPW,
            ConnectionTimeout = 100,
            SslMode = sslMode, // SSL 모드 설정
            CharacterSet = charset      // UTF-8 설정
        };

        if (serverstatus)
        {
            if (!isRunning)  // FetchDataAsync가 실행 중이 아니면
            {
                StartFetchingData();
            }
        }
        else
        {
            StopFetchingData();  // 서버 상태가 false일 때 데이터 가져오기 중지
        }
    }
    private void Awake()
    {
        prc_hist_tb.Clear();
        strg_fns_in_dic.Clear();
        strg_fns_out_dic.Clear();
        strg_raw_in_dic.Clear();
        strg_raw_out_dic.Clear();
    }
    private async void StartFetchingData()
    {
        if (isRunning) return;

        isRunning = true;  // FetchDataAsync가 실행 중임을 표시

        cancellationTokenSource = new CancellationTokenSource();  // 새로운 토큰 생성
        await FetchDataAsync(cancellationTokenSource.Token);  // 비동기 데이터 가져오기
    }
    //private async void Start()
    //{
    //    await FetchDataAsync(cancellationTokenSource);
    //}
    
    public async Task FetchDataAsync(CancellationToken cancellationToken)
    {

        while (serverstatus && !cancellationToken.IsCancellationRequested)
        {
            try
            {
                await DBConnection();
            }
            catch (Exception ex)
            {
                Debug.LogError("Error in DBConnection: " + ex.Message);
            }

            await Task.Delay(1000); // 1초 대기
        }

        
    }
    public async Task DBConnection()
    {
        using (var connection = new MySqlConnection(stringBuilder.ConnectionString))
        {
            try
            {
                await connection.OpenAsync();  // 비동기적으로 DB 연결 시도

                await Prc_Hist_Tb_Data(connection);
                await Strg_Fns_In_Data(connection);
                await Strg_Fns_Out_Data(connection);
                await Strg_Raw_In_Data(connection);
                await Strg_Raw_Out_Data(connection);
                Debug.Log("실행");
            }
            catch (Exception ex)
            {
                Debug.LogError($"DB 연결 오류: {ex.Message}");
                Debug.LogError($"DB 연결 오류: {ex.StackTrace}");
            }
        }
    }
    private async Task Prc_Hist_Tb_Data(MySqlConnection connection)
    {
        string query = "SELECT * FROM ulsan_db_v1.prc_hist_tb ORDER BY REG_DT DESC LIMIT 1";

        using (var cmd = new MySqlCommand(query, connection))
        {
            using (var reader = await cmd.ExecuteReaderAsync()) // 동기적으로 데이터를 읽기
            {
                if (reader.HasRows)
                {
                    // 데이터가 있는 경우에만 처리
                    while (await reader.ReadAsync())
                    {
                        try
                        {
                            strg_fns_in_dic["PRD_SRL_NO"] = reader.GetString("PRD_SRL_NO");
                            strg_fns_in_dic["PRD_PLN_NO"] = reader.GetString("PRD_PLN_NO");
                            strg_fns_in_dic["CMP_CD"] = reader.GetString("CMP_CD");
                            strg_fns_in_dic["PRD_CD"] = reader.GetString("PRD_CD");
                            strg_fns_in_dic["CMP_LINE_ID"] = reader.GetString("CMP_LINE_ID");
                            strg_fns_in_dic["CMP_EQ_ID"] = reader.GetString("CMP_EQ_ID");
                            strg_fns_in_dic["PRD_TYP_NO"] = reader.GetString("PRD_TYP_NO");
                            strg_fns_in_dic["PRD_LOT"] = reader.GetString("PRD_LOT");
                            strg_fns_in_dic["RECE_CD"] = reader.GetString("RECE_CD");
                            strg_fns_in_dic["PRD_WRK_CD"] = reader.GetString("PRD_WRK_CD");
                            strg_fns_in_dic["PRD_CYC_TM"] = reader.GetString("PRD_CYC_TM");
                            strg_fns_in_dic["MN_CYC_TM"] = reader.GetString("MN_CYC_TM");
                            strg_fns_in_dic["RBT_CYC_TM"] = reader.GetString("RBT_CYC_TM");
                            strg_fns_in_dic["REG_DT"] = reader.GetString("REG_DT");
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"데이터 읽기 오류: {ex.Message}");
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("쿼리 결과가 없습니다.");
                }
            }
        }
    }
    private async Task Strg_Fns_In_Data(MySqlConnection connection)
    {
        string query = "SELECT * FROM ulsan_db_v1.strg_fns_in ORDER BY REG_DT DESC LIMIT 1";

        using (var cmd = new MySqlCommand(query, connection))
        {
            using (var reader = await cmd.ExecuteReaderAsync()) // 동기적으로 데이터를 읽기
            {
                if (reader.HasRows)
                {
                    // 데이터가 있는 경우에만 처리
                    while (await reader.ReadAsync())
                    {
                        try
                        {
                            strg_fns_in_dic["PRD_SRL_NO"] = reader.GetString("PRD_SRL_NO");
                            strg_fns_in_dic["PRD_PLN_NO"] = reader.GetString("PRD_PLN_NO");
                            strg_fns_in_dic["CMP_CD"] = reader.GetString("CMP_CD");
                            strg_fns_in_dic["PRD_CD"] = reader.GetString("PRD_CD");
                            strg_fns_in_dic["CMP_LINE_ID"] = reader.GetString("CMP_LINE_ID");
                            strg_fns_in_dic["CMP_EQ_ID"] = reader.GetString("CMP_EQ_ID");
                            strg_fns_in_dic["PRD_TYP_NO"] = reader.GetString("PRD_TYP_NO");
                            strg_fns_in_dic["PRD_LOT"] = reader.GetString("PRD_LOT");
                            strg_fns_in_dic["RECE_CD"] = reader.GetString("RECE_CD");
                            strg_fns_in_dic["STRG_CD"] = reader.GetString("STRG_CD");
                            strg_fns_in_dic["SLOT_NM"] = reader.GetString("SLOT_NM");
                            strg_fns_in_dic["REG_DT"] = reader.GetString("REG_DT");
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"데이터 읽기 오류: {ex.Message}");
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("쿼리 결과가 없습니다.");
                }
            }
        }
    }
    private async Task Strg_Fns_Out_Data(MySqlConnection connection)
    {
        string query = "SELECT * FROM ulsan_db_v1.strg_fns_out ORDER BY REG_DT DESC LIMIT 1";

        using (var cmd = new MySqlCommand(query, connection))
        {
            using (var reader = await cmd.ExecuteReaderAsync()) // 동기적으로 데이터를 읽기
            {
                if (reader.HasRows)
                {
                    // 데이터가 있는 경우에만 처리
                    while (await reader.ReadAsync())
                    {
                        try
                        {
                            strg_fns_out_dic["PRD_SRL_NO"] = reader.GetString("PRD_SRL_NO");
                            strg_fns_out_dic["PRD_PLN_NO"] = reader.GetString("PRD_PLN_NO");
                            strg_fns_out_dic["CMP_CD"] = reader.GetString("CMP_CD");
                            strg_fns_out_dic["PRD_CD"] = reader.GetString("PRD_CD");
                            strg_fns_out_dic["CMP_LINE_ID"] = reader.GetString("CMP_LINE_ID");
                            strg_fns_out_dic["CMP_EQ_ID"] = reader.GetString("CMP_EQ_ID");
                            strg_fns_out_dic["PRD_LOT"] = reader.GetString("PRD_LOT");
                            strg_fns_out_dic["STRG_CD"] = reader.GetString("STRG_CD");
                            strg_fns_out_dic["SLOT_NM"] = reader.GetString("SLOT_NM");
                            strg_fns_out_dic["REG_DT"] = reader.GetString("REG_DT");

                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"데이터 읽기 오류: {ex.Message}");
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("쿼리 결과가 없습니다.");
                }
            }
        }
    }
    private async Task Strg_Raw_In_Data(MySqlConnection connection)
    {
        string query = "SELECT * FROM ulsan_db_v1.strg_raw_in ORDER BY REG_DT DESC LIMIT 1";

        using (var cmd = new MySqlCommand(query, connection))
        {
            using (var reader = await cmd.ExecuteReaderAsync()) // 동기적으로 데이터를 읽기
            {
                if (reader.HasRows)
                {
                    // 데이터가 있는 경우에만 처리
                    while (await reader.ReadAsync())
                    {
                        try
                        {
                            strg_raw_in_dic["RAW_SRL_NO"] = reader.GetString("RAW_SRL_NO");
                            strg_raw_in_dic["CMP_CD"] = reader.GetString("CMP_CD");
                            strg_raw_in_dic["RAW_CD"] = reader.GetString("RAW_CD");
                            strg_raw_in_dic["CMP_LINE_ID"] = reader.GetString("CMP_LINE_ID");
                            strg_raw_in_dic["CMP_EQ_ID"] = reader.GetString("CMP_EQ_ID");
                            strg_raw_in_dic["PRD_LOT"] = reader.GetString("PRD_LOT");
                            strg_raw_in_dic["STRG_CD"] = reader.GetString("STRG_CD");
                            strg_raw_in_dic["SLOT_NM"] = reader.GetString("SLOT_NM");
                            strg_raw_in_dic["REG_DT"] = reader.GetString("REG_DT");


                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"데이터 읽기 오류: {ex.Message}");
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("쿼리 결과가 없습니다.");
                }
            }
        }
    }
    private async Task Strg_Raw_Out_Data(MySqlConnection connection)
    {
        string query = "SELECT * FROM ulsan_db_v1.strg_raw_out ORDER BY REG_DT DESC LIMIT 1";

        using (var cmd = new MySqlCommand(query, connection))
        {
            using (var reader = await cmd.ExecuteReaderAsync()) // 동기적으로 데이터를 읽기
            {
                if (reader.HasRows)
                {
                    // 데이터가 있는 경우에만 처리
                    while (await reader.ReadAsync())
                    {
                        try
                        {
                            strg_raw_out_dic["PRD_SRL_NO"] = reader.GetString("PRD_SRL_NO");
                            strg_raw_out_dic["RAW_SRL_NO"] = reader.GetString("RAW_SRL_NO");
                            strg_raw_out_dic["PRD_PLN_NO"] = reader.GetString("PRD_PLN_NO");
                            strg_raw_out_dic["CMP_CD"] = reader.GetString("CMP_CD");
                            strg_raw_out_dic["PRD_CD"] = reader.GetString("PRD_CD");
                            strg_raw_out_dic["CMP_LINE_ID"] = reader.GetString("CMP_LINE_ID");
                            strg_raw_out_dic["CMP_EQ_ID"] = reader.GetString("CMP_EQ_ID");
                            strg_raw_out_dic["PRD_TYP_NO"] = reader.GetString("PRD_TYP_NO");
                            strg_raw_out_dic["PRD_LOT"] = reader.GetString("PRD_LOT");
                            strg_raw_out_dic["RECE_CD"] = reader.GetString("RECE_CD");
                            strg_raw_out_dic["STRG_CD"] = reader.GetString("STRG_CD");
                            strg_raw_out_dic["SLOT_NM"] = reader.GetString("SLOT_NM");
                            strg_raw_out_dic["REG_DT"] = reader.GetString("REG_DT");

                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"데이터 읽기 오류: {ex.Message}");
                        }

                    }
                }
                else
                {
                    Debug.LogWarning("쿼리 결과가 없습니다.");
                }
            }
        }
    }
    // 다른 데이터 처리 메서드들 (Prc_Hist_Tb_Data, Strg_Fns_In_Data 등)은 생략

    public void StopFetchingData()
    {
        serverstatus = false;

        cancellationTokenSource.Cancel();  // 비동기 작업 취소
        cancellationTokenSource.Dispose();  // 리소스 해제
        cancellationTokenSource = new CancellationTokenSource();  // 새로운 토큰 생성

        prc_hist_tb.Clear();
        strg_fns_in_dic.Clear();
        strg_fns_out_dic.Clear();
        strg_raw_in_dic.Clear();
        strg_raw_out_dic.Clear();

        isRunning = false;  // 비동기 작업이 멈췄으므로 isRunning을 false로 설정
    }

    // 애플리케이션 종료 시 비동기 작업을 취소
    private void OnApplicationQuit()
    {
        StopFetchingData(); // 애플리케이션 종료 시 비동기 작업을 중지
    }
}
