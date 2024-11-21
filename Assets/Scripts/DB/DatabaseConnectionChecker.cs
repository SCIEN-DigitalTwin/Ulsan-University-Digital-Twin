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
    
    // �⺻ �� ����, ����ڰ� �Է��� ���� ���� ��쿡 ���
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
            SslMode = MySqlSslMode.None,  // �⺻ SslMode ����
            CharacterSet = "utf8"         // �⺻ Charset ����
        };
    }

    public void SetData()
    {

        // ���� �� �ݿ�
        stringBuilder = new MySqlConnectionStringBuilder
        {
            Server = ip.text,  // IP �ּ�
            Port = uint.Parse(port.text), // ��Ʈ ��ȣ
            Database = db.text, // �����ͺ��̽� �̸�
            UserID = userID.text, // ����� ID
            Password = userPW.text, // ��й�ȣ
            ConnectionTimeout = 2,  // ���� �ð� �ʰ� ����
            SslMode = sslMode,  // �Է¹��� SslMode ����
            CharacterSet = charset  // �Է¹��� Charset ����
        };
    }

    public void SetDB()
    {
        CheckServerStatus(); // �񵿱� �޼��� ȣ�� ��� ���� �޼��� ȣ��
    }

    public void CheckServerStatus()
    {
        SetData();

        timeStamp = DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ");

        try
        {
            using (MySqlConnection connection = new MySqlConnection(stringBuilder.ConnectionString))
            {
                connection.Open(); // ���������� ���� �õ�

                if (connection.State == System.Data.ConnectionState.Open)
                {
                    serverstatus = true;
                    UpdateToggleStatus();
                    Debug.Log("DBInterface: Successfully connected to the server!");
                }
            }
        }
        catch (MySqlException ex) // MySQL ���� ó��
        {
            serverstatus = false;
            UpdateToggleStatus();
            Debug.LogError("DBInterface: Server status check failed! " + Environment.NewLine + ex.Message);
        }
        catch (Exception ex) // �Ϲ� ���� ó��
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
                Debug.LogError("LeanToggle ������Ʈ�� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("statusToggle�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }
}
