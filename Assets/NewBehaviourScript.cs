using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float rotationSpeed = 100f;

    void Update()
    {
        // ȸ�� �ӵ��� ���� ȸ�� �� ���
        float horizontal = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        float vertical = Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;

        // ���ʹϾ� ȸ�� ����
        Quaternion rotationX = Quaternion.AngleAxis(horizontal, Vector3.up);
        Quaternion rotationY = Quaternion.AngleAxis(vertical, Vector3.right);

        // ���� ȸ���� ���ο� ȸ�� ����
        transform.rotation = rotationX * rotationY * transform.rotation;
    }
}
