using UnityEngine;

public class AuctionRockMoving : MonoBehaviour
{ //cubeMoving

    Vector3 pos; //������ġ
    float delta = 3.0f; // ��(��)�� �̵������� (x)�ִ밪
    float speed = 3.0f; // �̵��ӵ�

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = pos;

        v.y += delta * Mathf.Sin(Time.time * speed);

        

        transform.position = v;
    }
}


