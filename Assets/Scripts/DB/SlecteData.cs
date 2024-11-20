using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TMPro;

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

    public void UpdateDatabaseInfo(string newIp, string newPort, string newDb, string newUserID, string newUserPW, bool status)
    {
        ip = newIp;
        port = uint.Parse(newPort);
        db = newDb;
        userID = newUserID;
        userPW = newUserPW;
        serverstatus = status;

        stringBuilder = new MySqlConnectionStringBuilder
        {
            Server = ip,
            Port = port,
            Database = db,
            UserID = userID,
            Password = userPW,
            ConnectionTimeout = 10
        };
    }

    //public void Data1()
    //{
    //    try
    //    {
    //        using (MySqlConnection connection = new MySqlConnection(stringBuilder.ConnectionString))
    //        {
    //            connection.Open(); // 동기적으로 연결 시도

    //            // 쿼리 
    //            string query = "SELECT VALUE FROM cps_yparameter WHERE NAME = @name";

    //            using (MySqlCommand cmd = new MySqlCommand(query, connection))
    //            {
    //                cmd.Parameters.AddWithValue("@name", parameterField.text);

    //                using (MySqlDataReader reader = cmd.ExecuteReader())
    //                {
    //                    while (reader.Read())
    //                    {
    //                        yValue.Add(reader.GetString("VALUE"));
    //                    }
    //                    // reader 닫기
    //                    reader.Close();
    //                }
    //            }
    //        }
    //    }
    //    catch (MySqlException ex) // MySQL 예외 처리
    //    {
    //        serverstatus = false;
    //        Debug.LogError("DBInterface: Server status check failed! " + Environment.NewLine + ex.Message);
    //    }
    //    catch (Exception ex) // 일반 예외 처리
    //    {
    //        serverstatus = false;
    //        Debug.LogError("DBInterface: An error occurred! " + Environment.NewLine + ex.Message);
    //    }
    //}

}
