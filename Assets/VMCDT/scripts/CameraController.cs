using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /// <summary>
    /// 主相机的子物体
    /// </summary>
    [HideInInspector]
    public GameObject m_ChildObj = null;
    /// <summary>
    /// 主相机位置
    /// </summary>
    [SerializeField]
    private Camera m_Camera;
    /// <summary>
    /// 屏幕中心点
    /// </summary>
    private Vector2 m_ScreenCenterPos = Vector2.zero;
    /// <summary>
    /// 鼠标第一位置点
    /// </summary>
    private Vector3 mouseFirstPos = Vector3.zero;

    [Tooltip("摄像机弧度的增量 ")]
    public float incRadian = 10f;
    [Tooltip("摄像机的最低高度 ")]
    public float camMinHeight = 0.1f;
    [Tooltip("摄像机的最低高度 ")]
    public float camMaxHeight = 100f;

    [Tooltip("鼠标移动速度 ")]
    public float moveSpeed = 0.1f;
    [Tooltip("鼠标旋转速度 ")]
    public float rotateSpeed = 0.08f;
    [Tooltip("鼠标滑轮速度 ")]
    public float scrollSpeed = 10f;

    // Use this for initialization
    void Start()
    {

        m_ChildObj = new GameObject("ChildObj");
        m_ChildObj.transform.parent = m_Camera.transform;
        m_ChildObj.transform.localPosition = Vector3.zero;
        //屏幕中心
        m_ScreenCenterPos = new Vector2(Screen.width / 2f, Screen.height / 2f);
        // 相机 、及子物体ChildObj的轴方向矫正
        m_ChildObj.transform.eulerAngles = new Vector3(0, m_Camera.transform.eulerAngles.y, m_Camera.transform.eulerAngles.z);
    }


    void Update()
    {
        CameraMoveController();
    }

    /// <summary>
    /// 摄像机控制
    /// </summary>
    private void CameraMoveController()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            if (mouseFirstPos != Vector3.zero && mouseFirstPos != Input.mousePosition)
            {
                var position = new Vector2(Input.mousePosition.x - mouseFirstPos.x, Input.mousePosition.y - mouseFirstPos.y);
                m_ChildObj.transform.eulerAngles = new Vector3(0, m_Camera.transform.eulerAngles.y, m_Camera.transform.eulerAngles.z);

                if (Input.GetMouseButton(0))
                {
                    CameraMove(position.x, position.y, moveSpeed);
                }
                else
                {
                    CameraRotate(position.x, position.y, rotateSpeed);
                }

                mouseFirstPos = Input.mousePosition;
            }
            else
            {
                mouseFirstPos = Input.mousePosition;
            }
        }

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            mouseFirstPos = Vector3.zero;
        }

        //鼠标滑轮
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
            CameraNear_Far(Input.GetAxis("Mouse ScrollWheel"), scrollSpeed);
    }


    /// <summary>
    /// 相机移动  mouseX  沿X轴拖动；mouseY  沿Y轴拖动
    /// </summary>
    private void CameraMove(float mouseX, float mouseY, float speed)
    {
        m_Camera.transform.Translate(Vector3.right * -mouseX * speed, m_ChildObj.transform);
        m_Camera.transform.Translate(Vector3.forward * -mouseY * speed, m_ChildObj.transform);
    }

    /// <summary>
    /// 相机旋转   mouseX  沿X轴旋转；mouseY  沿Y轴旋转
    /// </summary>
    private void CameraRotate(float mouseX, float mouseY, float speed)
    {
        Vector3 centerPos = GetScreenCenterHitPoint();
        m_Camera.transform.RotateAround(centerPos, Vector3.up, mouseX * speed);

        if (mouseY * speed >= incRadian) return;
        //通过通过m_CameraPos.forward与m_ChildObj.transform.forward向量的夹角限制相机的旋转角度
        float angle = Vector3.Angle(m_Camera.transform.forward, m_ChildObj.transform.forward);

        //相机的高度限制
        if (m_Camera.transform.position.y < camMinHeight)
            m_Camera.transform.position = new Vector3(m_Camera.transform.position.x, camMinHeight, m_Camera.transform.position.z);

        if (m_Camera.transform.position.y > camMaxHeight)
            m_Camera.transform.position = new Vector3(m_Camera.transform.position.x, camMaxHeight, m_Camera.transform.position.z);

        if (angle <= incRadian)
        {
            if (mouseY < 0)
                m_Camera.transform.RotateAround(centerPos, m_Camera.transform.right, -mouseY * speed);
        }
        else if (angle >= (90 - incRadian))
        {
            if (mouseY > 0)
                m_Camera.transform.RotateAround(centerPos, m_Camera.transform.right, -mouseY * speed);
        }
        else
        {
            m_Camera.transform.RotateAround(centerPos, m_Camera.transform.right, -mouseY * speed);
        }
    }

    /// <summary>
    /// 相机拉远拉近  鼠标滑轮 mouseScrollWheel；拉扯速度 speed
    /// </summary>
    private void CameraNear_Far(float mouseScrollWheel, float speed)
    {
        if (m_Camera.transform.position.y >= camMinHeight && m_Camera.transform.position.y <= camMaxHeight)
        {
            Vector3 position = m_Camera.transform.position;
            m_Camera.transform.Translate(Vector3.forward * mouseScrollWheel * speed, Space.Self);
            if (m_Camera.transform.position.y < camMinHeight || m_Camera.transform.position.y > camMaxHeight)
            {
                m_Camera.transform.position = position;
            }
        }
    }

    /// <summary>
    /// 获取屏幕中心点位置
    /// </summary>
    private Vector3 GetScreenCenterHitPoint()
    {
        Vector3 tempPos = Vector3.zero;
        Ray ray = m_Camera.ScreenPointToRay(m_ScreenCenterPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            tempPos = hit.point;
        }
        return tempPos;
    }

    /// <summary>
    /// 将物体旋转角度数值重置成编辑器上显示的值
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public float CheckAngle(float value)
    {
        float angle = value - 180;

        if (angle > 0)
            return angle - 180;

        return angle + 180;
    }
}
