using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraMov : MonoBehaviour
{
    public Transform mainCameraTransform; // 카메라 이동 제어용 Transform
    public Transform mainCameraRoTransform; // 카메라 회전 제어용 Transform

    public int minMoveX = -270;
    public int maxMoveX = 670;

    public float fixedY = 60f; // 줌 시 고정할 Y값
    public float zoomSpeed = 2000f; // 줌 속도
    public float rotateSpeed = 10f; // 회전 속도

    void Update()
    {
        Zoom(); // 줌 컨트롤
        Rotate(); // 회전 컨트롤
    }

    private void Zoom()
    {
        // 마우스 스크롤 휠로 줌 제어
        float distance = Input.GetAxis("Mouse ScrollWheel") * -1 * zoomSpeed * Time.deltaTime;

        // 카메라의 회전 기준 방향으로 이동
        mainCameraTransform.Translate(mainCameraRoTransform.forward * distance, Space.World);

        // 경계 내로 포지션 클램핑
        Vector3 position = mainCameraTransform.position;
        position.x = Mathf.Clamp(position.x, minMoveX, maxMoveX);
        position.y = fixedY; // 줌 시 Y값 고정
        mainCameraTransform.position = position;
    }

    private void Rotate()
    {
        if (Input.GetMouseButton(1)) // 오른쪽 마우스 버튼을 누를 때 회전
        {
            // 마우스 입력값에 따라 회전값 계산
            float mouseX = Input.GetAxis("Mouse X") * rotateSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotateSpeed;

            Vector3 rotation = mainCameraRoTransform.rotation.eulerAngles;

            // Y축 회전 (수평 회전)
            rotation.y += mouseX;

            // X축 회전 (수직 회전) 제한
            rotation.x -= mouseY;
            if (rotation.x < 300 && rotation.x > 180) rotation.x = 300; // 아래쪽 제한
            else if (rotation.x > 60 && rotation.x < 180) rotation.x = 60; // 위쪽 제한

            rotation.z = 0f; // Z축 회전 고정

            // 부드럽게 회전
            Quaternion targetRotation = Quaternion.Euler(rotation);
            mainCameraRoTransform.rotation = Quaternion.Slerp(mainCameraRoTransform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}
