using System;
using System.Collections;
using UnityEngine;

public class ParabolaMotion : ProjectileMotionBase
{
    public float HeightArc { get; private set; } = 1;

    public new void SetInfo(ProjectileData projectileData, Vector3 startPosition, Vector3 targetPosition, Action endCallback = null)
    {
        base.SetInfo(projectileData, startPosition, targetPosition, endCallback);
    }

    protected override IEnumerator LaunchProjectile()
    {
        float journetLength = Vector3.Distance (StartPosition,TargetPosition);
        float totalTime = journetLength / ProjectileData.ProjSpeed;

        float elapsedTime = 0;

        while(elapsedTime< totalTime)
        {
            elapsedTime += Time.deltaTime;

            float normalizedTime = elapsedTime / totalTime;

            float x = Mathf.Lerp(StartPosition.x, TargetPosition.x, normalizedTime);
            float z = Mathf.Lerp(StartPosition.z, TargetPosition.z, normalizedTime);
            float baseY = Mathf.Lerp(StartPosition.y,TargetPosition.y, normalizedTime);
            float arc = HeightArc * Mathf.Sin(normalizedTime * Mathf.PI);

            float y = baseY + arc;

            var nextPos = new Vector3(x, y, z);

            if(LookAtTarget)
            {
                LookAtDir(nextPos - transform.position);
            }
            transform.position = nextPos;

            yield return null;
        }
        EndCallback?.Invoke();
    }
}
