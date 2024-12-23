using System;
using UnityEngine;
using MySql.Data.MySqlClient;
using TMPro;
using Lean.Gui;

public class DatabaseConnectionChecker : MonoBehaviour
{
    private MySqlConnectionStringBuilder stringBuilder;
    public TMP_InputField ip;
    public TMP_InputField port;
    public TMP_InputField db;
    public TMP_InputField userID;
    public TMP_InputField userPW;
    
    // 기본 값 설정, 사용자가 입력한 값이 없을 경우에 대비
    MySqlSslMode sslMode = MySqlSslMode.None;
    string charset = "utf8";

    public bool serverstatus = false;
    private string timeStamp;
    public string DatabaseIP => ip.text;
    public string PortNumber => port.text;
    public string DatabaseName => db.text;
    public string UserID => userID.text;
    public string UserPassword => userPW.text;

    public GameObject statusToggle;
    public SlecteData data;

    void Awake()
    {
        stringBuilder = new MySqlConnectionStringBuilder
        {
            Server = DatabaseIP,
            Port = uint.Parse(PortNumber),
            Database = DatabaseName,
            UserID = UserID,
            Password = UserPassword,
            SslMode = MySqlSslMode.None,  // 기본 SslMode 설정
            CharacterSet = "utf8"         // 기본 Charset 설정
        };
    }

    public void SetData()
    {

        // 설정 값 반영
        stringBuilder = new MySqlConnectionStringBuilder
        {
            Server = ip.text,  // IP 주소
            Port = uint.Parse(port.text), // 포트 번호
            Database = db.text, // 데이터베이스 이름
            UserID = userID.text, // 사용자 ID
            Password = userPW.text, // 비밀번호
            ConnectionTimeout = 2,  // 연결 시간 초과 설정
            SslMode = sslMode,  // 입력받은 SslMode 설정
            CharacterSet = charset  // 입력받은 Charset 설정
        };
    }

    public void SetDB()
    {
        CheckServerStatus(); // 비동기 메서드 호출 대신 동기 메서드 호출
    }

    public void CheckServerStatus()
    {
        SetData();

        timeStamp = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ");

        try
        {
            using (MySqlConnection connection = new MySqlConnection(stringBuilder.ConnectionString))
            {
                connection.Open(); // 동기적으로 연결 시도

                if (connection.State == System.Data.ConnectionState.Open)
                {
                    serverstatus = true;
                    UpdateToggleStatus();
                    Debug.Log("DBInterface: Successfully connected to the server!");
                }
            }
        }
        catch (MySqlException ex) // MySQL 예외 처리
        {
            serverstatus = false;
            UpdateToggleStatus();
            Debug.LogError("DBInterface: Server status check failed! " + Environment.NewLine + ex.Message);
        }
        catch (Exception ex) // 일반 예외 처리
        {
            serverstatus = false;
            UpdateToggleStatus();
            Debug.LogError("DBInterface: An error occurred! " + Environment.NewLine + ex.Message);
        }
    }

    private void UpdateToggleStatus()
    {
        if (statusToggle != null)
        {
            var toggle = statusToggle.GetComponent<LeanToggle>();
            if (toggle != null)
            {
                toggle.On = serverstatus;
                data.UpdateDatabaseInfo(DatabaseIP, PortNumber, DatabaseName, UserID, UserPassword, serverstatus, sslMode, charset);
                
            }
            else
            {
                Debug.LogError("LeanToggle 컴포넌트를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("statusToggle이 할당되지 않았습니다.");
        }
    }
}
