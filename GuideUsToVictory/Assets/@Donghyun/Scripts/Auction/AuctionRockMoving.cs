using UnityEngine;

public class AuctionRockMoving : MonoBehaviour
{ //cubeMoving

    Vector3 pos; //현재위치
    float delta = 3.0f; // 상(하)로 이동가능한 (x)최대값
    float speed = 3.0f; // 이동속도

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


