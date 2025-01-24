using UnityEngine;

public class LookCamera : MonoBehaviour
{


    void Update()
    {
        Vector3 dir = transform.position - Camera.main.transform.position;
        transform.rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0, 0);
    }
}
