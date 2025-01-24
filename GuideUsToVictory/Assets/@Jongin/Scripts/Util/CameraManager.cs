using DG.Tweening;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
public class CameraManager : MonoBehaviour
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

    Tween cameraMoveTween;

    //0 : battleField Camera
    //1 : SummonGroundCamera
    public CinemachineCamera[] cameras;
    private int activeCameraIndex = 0;

    private Vector3 startLocalPos;
    private void Start()
    {
        cameraTransform = cameras[0].transform;
        startLocalPos = cameraTransform.localPosition;
        direction = (cameraTransform.position - transform.position).normalized;
        //Camera.main.GetComponent<CinemachineBrain>().DefaultBlend.Style = CinemachineBlendDefinition.Styles.EaseInOut;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActiveCamera(0);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActiveCamera(1);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            MoveToTeamTower(Define.ETeam.Blue);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            MoveToTeamTower(Define.ETeam.Red);
        }

        if (activeCameraIndex != 0) return;

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

    void MoveToTeamTower(Define.ETeam team)
    {
        float mul = team == Define.ETeam.Blue ? -200 : 200;
        if (cameraMoveTween != null)
        {
            cameraMoveTween.Kill();
        }
        transform.position = Vector3.right * mul;
        cameraTransform.localPosition = startLocalPos;
    }
    public void GameEnd(Define.ETeam loseTeam)
    {
        //MoveToTeamTower(loseTeam);

    }

    void ActiveCamera(int cameraIndex)
    {
        foreach(var camera in cameras)
        {
            camera.Priority = 0;
        }

        cameras[cameraIndex].Priority = 10;
        activeCameraIndex = cameraIndex;
    }
}
