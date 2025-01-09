using DG.Tweening;
using System.Collections;
using UnityEngine;
public class MoveCamera : MonoBehaviour
{
    [SerializeField] float moveSpeed = 50f;
    [SerializeField] float moveForwardSpeed = 1f;
    [SerializeField] float moveForwardDistance = 2f;
    [SerializeField] Transform center;
    float maxMoveVector = 100f;
    float minMoveVector = 30f;
    float rightLimit = 220f;
    float leftLimit = -220f;
    Transform cameraTransform;
    Vector3 direction;

    Coroutine cameraMoveForward;

    Tween cameraMoveTween;
    private void Start()
    {
        cameraTransform = Camera.main.transform;
        direction = (cameraTransform.position - transform.position).normalized;
    }
    void Update()
    {
        if (Input.mousePosition.x >= Screen.width)
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
            if (transform.position.x >= rightLimit)
            {
                transform.position = new Vector3(rightLimit, transform.position.y,transform.position.z);
            }
        }
        else if (Input.mousePosition.x <= 0)
        {
            transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
            if (transform.position.x <= leftLimit)
            {
                transform.position = new Vector3(leftLimit, transform.position.y, transform.position.z);
            }
        }
        if (Input.mouseScrollDelta.y != 0)
        {
            if (cameraMoveTween != null)
            {
                cameraMoveTween.Kill();
            }

            // transform 방향으로 이동 계산
            Vector3 direction = (cameraTransform.localPosition - center.localPosition).normalized;
            float targetDistance = Mathf.Clamp(
                Vector3.Distance(cameraTransform.localPosition, center.localPosition) - Input.mouseScrollDelta.y * moveForwardDistance,
                minMoveVector,
                maxMoveVector
            );

            Vector3 targetPosition = center.localPosition + direction * targetDistance;

            cameraMoveTween = cameraTransform.DOLocalMove(targetPosition, 0.5f)
                .SetEase(Ease.OutSine);
        }
    }

}
