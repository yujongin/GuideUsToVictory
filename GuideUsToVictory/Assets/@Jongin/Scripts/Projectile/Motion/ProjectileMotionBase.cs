using System;
using System.Collections;
using UnityEngine;

public abstract class ProjectileMotionBase : MonoBehaviour
{
    Coroutine launchProjectile;

    public Vector3 StartPosition { get; private set; }
    public Vector3 TargetPosition {  get; private set; }
    public bool LookAtTarget { get; private set; }
    protected Action EndCallback { get; private set; }
    public ProjectileData ProjectileData { get; private set; }
    protected void SetInfo(ProjectileData projectileData,Vector3 spawnPosition, Vector3 targetPosition, Action endCallback = null)
    {
        ProjectileData = projectileData;
        StartPosition = spawnPosition;
        TargetPosition = targetPosition;
        EndCallback = endCallback;

        //Temp
        LookAtTarget = true;

        if (launchProjectile != null) 
            StopCoroutine(launchProjectile);

        launchProjectile = StartCoroutine(LaunchProjectile());
    }   

    protected void LookAtDir(Vector3 forward)
    {
        Quaternion targetRotation = Quaternion.LookRotation(forward);
        transform.rotation = targetRotation * Quaternion.Euler(0, -90, 0);
    }
    protected abstract IEnumerator LaunchProjectile();
}
