using System;
using System.Collections;
using UnityEngine;

public abstract class ProjectileMotionBase : MonoBehaviour
{
    Coroutine launchProjectile;

    public Vector3 StartPosition { get; private set; }
    public UnitBase Target {  get; private set; }
    public bool LookAtTarget;
    protected Action EndCallback { get; private set; }
    public ProjectileData ProjectileData { get; private set; }
    protected void SetInfo(ProjectileData projectileData,Vector3 spawnPosition, UnitBase target, Action endCallback = null)
    {
        ProjectileData = projectileData;
        StartPosition = spawnPosition;
        Target = target;
        EndCallback = endCallback;

        ////Temp
        //LookAtTarget = true;

        if (launchProjectile != null) 
            StopCoroutine(launchProjectile);

        launchProjectile = StartCoroutine(LaunchProjectile());
    }   

    protected void LookAtDir(Vector3 dir)
    {
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        transform.rotation = lookRotation * Quaternion.Euler(0, -90f, 0);
    }
    protected abstract IEnumerator LaunchProjectile();
}
