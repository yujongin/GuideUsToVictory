using DG.Tweening;
using System.Collections;
using UnityEngine;
public class MoveCamera : MonoBehaviour
{
    [SerializeField] float moveSpeed = 50f;
    [SerializeField] float moveForwardSpeed = 1f;
    [SerializeField] float moveForwardDistance = 2f;
    Vector3 maxMoveVector = new Vector3(200f, 0, 0);
    Transform cameraTransform;
    Coroutine cameraMoveForward;

    Tween cameraMoveTween;
    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }
    void Update()
    {
        if (Input.mousePosition.x > Screen.width)
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
            if (transform.position.x >= 200)
            {
                transform.position = maxMoveVector;
            }
        }
        else if (Input.mousePosition.x < 0)
        {
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
            if (transform.position.x <= -200f)
            {
                transform.position = -maxMoveVector;
            }
        }
        if (Input.mouseScrollDelta.y != 0)
        {
            if (cameraMoveTween != null)
            {
                cameraMoveTween.Kill();
            }
            Vector3 endVector = cameraTransform.localPosition + cameraTransform.forward * Input.mouseScrollDelta.y * moveForwardDistance;
            cameraMoveTween = cameraTransform.DOLocalMove(endVector, 1f).SetEase(Ease.InOutSine);
        }

        //if(Input.mouseScrollDelta.y != 0)
        //{
        //    if (cameraMoveForward != null)
        //    {
        //        StopCoroutine(cameraMoveForward);
        //    }
        //    cameraMoveForward = StartCoroutine(MoveCameraForward(cameraTransform.localPosition + cameraTransform.forward * Input.mouseScrollDelta.y * moveForwardDistance));
        //}
    }

    IEnumerator MoveCameraForward(Vector3 newPos)
    {
        float elapsedTime = 0;
        Vector3 oldPos = cameraTransform.localPosition;
        while (elapsedTime <= 1)
        {
            cameraTransform.localPosition = Vector3.Lerp(oldPos, newPos, elapsedTime);
            elapsedTime += Time.deltaTime * moveForwardSpeed;


            yield return null;
        }
    }
}
