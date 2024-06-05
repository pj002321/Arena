using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float rotationSpeed = 100f;

    void Update()
    {
        // 회전 속도에 따른 회전 값 계산
        float horizontal = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;

        // 쿼터니언 회전 적용
        Quaternion rotationX = Quaternion.AngleAxis(horizontal, Vector3.up);
        Quaternion rotationY = Quaternion.AngleAxis(vertical, Vector3.right);

        // 현재 회전에 새로운 회전 적용
        transform.rotation = rotationX * rotationY * transform.rotation;
    }
}
