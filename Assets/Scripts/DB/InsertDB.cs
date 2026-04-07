using System;
using System.Collections;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using MySql.Data.MySqlClient;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class InsertDB : MonoBehaviour
{

    // 데이터베이스 연결 문자열
    private string connectionString = "Server=127.0.0.1;Port=3306;Database=testulsan;User ID=root;Password=1234;SslMode=none;";

    public void STARTDB()
    {
        // 코루틴 시작
        StartCoroutine(INSERTDB());
    }

    public IEnumerator INSERTDB()
    {
        Debug.Log("INSERTDB 플랜지 단계 시작");

        // 테스트 데이터 삽입
        InsertData("PRD1000-240822003-0001", "240822003", "6108210640", "PRD1000", "LN01", "EQ01", "PRDC", "PRD1000-240822003", "RC00", "STR", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
        yield return new WaitForSeconds(37f);
        //37초
        InsertData("PRD1000-240822003-0001", "240822003", "6108210640", "PRD1000", "LN01", "EQ01", "PRDC", "PRD1000-240822003", "RC00", "END", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
        yield return new WaitForSeconds(3f);
        //3초
        InsertData("PRD1000-240822003-0001", "240822003", "6108210640", "PRD1000", "LN01", "EQ03", "PRDC", "PRD1000-240822003", "RC00", "STR", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
        yield return new WaitForSeconds(320f);
        //5분 20초

        InsertData("PRD1000-240822003-0001", "240822003", "6108210640", "PRD1000", "LN01", "EQ03", "PRDC", "PRD1000-240822003", "RC00", "END", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
        yield return new WaitForSeconds(3f);
        //3초
        InsertData("PRD1000-240822003-0001", "240822003", "6108210640", "PRD1000", "LN01", "EQ08", "PRDC", "PRD1000-240822003", "RC00", "STR", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
        yield return new WaitForSeconds(37f);
        //37초
        InsertData("PRD1000-240822003-0001", "240822003", "6108210640", "PRD1000", "LN01", "EQ08", "PRDC", "PRD1000-240822003", "RC00", "END", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
        // 1초 대기
        yield return new WaitForSeconds(1f);

        
    }

    public void InsertData(string PRD_SRL_NO, string PRD_PLN_NO, string CMP_CD, string PRD_CD, string CMP_LINE_ID, string CMP_EQ_ID, string PRD_TYP_NO, string PRD_LOT, string RECE_CD, string PRD_WRK_CD, float PRD_CYC_TM, float MN_CYC_TM, float RBT_CYC_TM, string REG_DT)
    {
        // SQL INSERT 문
        string query = "INSERT INTO prc_hist_tb (PRD_SRL_NO, PRD_PLN_NO, CMP_CD, PRD_CD, CMP_LINE_ID,CMP_EQ_ID,PRD_TYP_NO,PRD_LOT,RECE_CD,PRD_WRK_CD,PRD_CYC_TM,MN_CYC_TM,RBT_CYC_TM,REG_DT) " +
            "VALUES (@PRD_SRL_NO, @PRD_PLN_NO, @CMP_CD, @PRD_CD, @CMP_LINE_ID,@CMP_EQ_ID,@PRD_TYP_NO,@PRD_LOT,@RECE_CD,@PRD_WRK_CD,@PRD_CYC_TM,@MN_CYC_TM,@RBT_CYC_TM,@REG_DT)";

        // 데이터베이스 연결
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // 매개변수 바인딩
                    command.Parameters.AddWithValue("@PRD_SRL_NO", PRD_SRL_NO);
                    command.Parameters.AddWithValue("@PRD_PLN_NO", PRD_PLN_NO);
                    command.Parameters.AddWithValue("@CMP_CD", CMP_CD);
                    command.Parameters.AddWithValue("@PRD_CD", PRD_CD);
                    command.Parameters.AddWithValue("@CMP_LINE_ID", CMP_LINE_ID);
                    command.Parameters.AddWithValue("@CMP_EQ_ID", CMP_EQ_ID);
                    command.Parameters.AddWithValue("@PRD_TYP_NO", PRD_TYP_NO);
                    command.Parameters.AddWithValue("@PRD_LOT", PRD_LOT);
                    command.Parameters.AddWithValue("@RECE_CD", RECE_CD);
                    command.Parameters.AddWithValue("@PRD_WRK_CD", PRD_WRK_CD);
                    command.Parameters.AddWithValue("@PRD_CYC_TM", PRD_CYC_TM);
                    command.Parameters.AddWithValue("@MN_CYC_TM", MN_CYC_TM);
                    command.Parameters.AddWithValue("@RBT_CYC_TM", RBT_CYC_TM);
                    command.Parameters.AddWithValue("@REG_DT", REG_DT);
                    

                    // 쿼리 실행
                    int rowsAffected = command.ExecuteNonQuery();
                    Debug.Log($"데이터 삽입 성공! 삽입된 행 수: {rowsAffected}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"데이터 삽입 중 오류 발생: {ex.Message}");
            }
        }
    }

    //public IEnumerator INSERTDB()
    //{
    //    Debug.Log("INSERTDB 플랜지 단계 시작");

    //    // 테스트 데이터 삽입
    //    InsertData("PRD1000-240822003-0001", "240822003", "6108210640", "PRD1000", "LN01", "EQ01", "PRDC", "PRD1000-240822003", "RC00", "STR", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //37초
    //    InsertData("PRD1000-240822003-0001", "240822003", "6108210640", "PRD1000", "LN01", "EQ01", "PRDC", "PRD1000-240822003", "RC00", "END", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //3초
    //    InsertData("PRD1000-240822003-0001", "240822003", "6108210640", "PRD1000", "LN01", "EQ03", "PRDC", "PRD1000-240822003", "RC00", "STR", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //5분 20초
    //    //InsertData("PRD1000-240822003-0001", "240822003", "6108210640", "PRD1000", "LN01", "EQ04", "PRDC", "PRD1000-240822003", "RC00", "STR", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //InsertData("PRD1000-240822003-0001", "240822003", "6108210640", "PRD1000", "LN01", "EQ04", "PRDC", "PRD1000-240822003", "RC00", "END", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //InsertData("PRD1000-240822003-0002", "240822003", "6108210640", "PRD1000", "LN01", "EQ04", "PRDC", "PRD1000-240822003", "RC00", "STR", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //InsertData("PRD1000-240822003-0001", "240822003", "6108210640", "PRD1000", "LN01", "EQ05", "PRDC", "PRD1000-240822003", "RC00", "STR", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //InsertData("PRD1000-240822003-0001", "240822003", "6108210640", "PRD1000", "LN01", "EQ05", "PRDC", "PRD1000-240822003", "RC00", "END", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //InsertData("PRD1000-240822003-0002", "240822003", "6108210640", "PRD1000", "LN01", "EQ04", "PRDC", "PRD1000-240822003", "RC00", "END", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //InsertData("PRD1000-240822003-0001", "240822003", "6108210640", "PRD1000", "LN01", "EQ06", "PRDC", "PRD1000-240822003", "RC00", "STR", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");

    //    //InsertData("PRD1000-240822003-0003", "240822003", "6108210640", "PRD1000", "LN01", "EQ04", "PRDC", "PRD1000-240822003", "RC00", "STR", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //InsertData("PRD1000-240822003-0002", "240822003", "6108210640", "PRD1000", "LN01", "EQ05", "PRDC", "PRD1000-240822003", "RC00", "STR", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //InsertData("PRD1000-240822003-0001", "240822003", "6108210640", "PRD1000", "LN01", "EQ06", "PRDC", "PRD1000-240822003", "RC00", "END", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    InsertData("PRD1000-240822003-0001", "240822003", "6108210640", "PRD1000", "LN01", "EQ03", "PRDC", "PRD1000-240822003", "RC00", "END", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //3초
    //    //InsertData("PRD1000-240822003-0002", "240822003", "6108210640", "PRD1000", "LN01", "EQ05", "PRDC", "PRD1000-240822003", "RC00", "END", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //InsertData("PRD1000-240822003-0003", "240822003", "6108210640", "PRD1000", "LN01", "EQ04", "PRDC", "PRD1000-240822003", "RC00", "END", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //InsertData("PRD1000-240822003-0002", "240822003", "6108210640", "PRD1000", "LN01", "EQ06", "PRDC", "PRD1000-240822003", "RC00", "STR", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //InsertData("PRD1000-240822003-0004", "240822003", "6108210640", "PRD1000", "LN01", "EQ04", "PRDC", "PRD1000-240822003", "RC00", "STR", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //InsertData("PRD1000-240822003-0003", "240822003", "6108210640", "PRD1000", "LN01", "EQ05", "PRDC", "PRD1000-240822003", "RC00", "STR", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //InsertData("PRD1000-240822003-0002", "240822003", "6108210640", "PRD1000", "LN01", "EQ06", "PRDC", "PRD1000-240822003", "RC00", "END", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");

    //    //InsertData("PRD1000-240822003-0003", "240822003", "6108210640", "PRD1000", "LN01", "EQ05", "PRDC", "PRD1000-240822003", "RC00", "END", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //InsertData("PRD1000-240822003-0004", "240822003", "6108210640", "PRD1000", "LN01", "EQ04", "PRDC", "PRD1000-240822003", "RC00", "END", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //InsertData("PRD1000-240822003-0001", "240822003", "6108210640", "PRD1000", "LN01", "EQ07", "PRDC", "PRD1000-240822003", "RC00", "STR", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //InsertData("PRD1000-240822003-0003", "240822003", "6108210640", "PRD1000", "LN01", "EQ06", "PRDC", "PRD1000-240822003", "RC00", "STR", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //InsertData("PRD1000-240822003-0004", "240822003", "6108210640", "PRD1000", "LN01", "EQ05", "PRDC", "PRD1000-240822003", "RC00", "STR", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //InsertData("PRD1000-240822003-0003", "240822003", "6108210640", "PRD1000", "LN01", "EQ06", "PRDC", "PRD1000-240822003", "RC00", "END", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //InsertData("PRD1000-240822003-0004", "240822003", "6108210640", "PRD1000", "LN01", "EQ05", "PRDC", "PRD1000-240822003", "RC00", "END", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //InsertData("PRD1000-240822003-0004", "240822003", "6108210640", "PRD1000", "LN01", "EQ06", "PRDC", "PRD1000-240822003", "RC00", "STR", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //InsertData("PRD1000-240822003-0004", "240822003", "6108210640", "PRD1000", "LN01", "EQ06", "PRDC", "PRD1000-240822003", "RC00", "END", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //InsertData("PRD1000-240822003-0004", "240822003", "6108210640", "PRD1000", "LN01", "EQ07", "PRDC", "PRD1000-240822003", "RC00", "END", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");

    //    InsertData("PRD1000-240822003-0001", "240822003", "6108210640", "PRD1000", "LN01", "EQ08", "PRDC", "PRD1000-240822003", "RC00", "STR", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    //37초
    //    InsertData("PRD1000-240822003-0001", "240822003", "6108210640", "PRD1000", "LN01", "EQ08", "PRDC", "PRD1000-240822003", "RC00", "END", 0.0f, 0.0f, 0.0f, "2024-08-22 10:29:39.670");
    //    // 1초 대기
    //    yield return new WaitForSeconds(1f);


    //}
}
