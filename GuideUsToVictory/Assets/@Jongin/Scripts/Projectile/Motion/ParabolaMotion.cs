using System;
using System.Collections;
using UnityEngine;

public class ParabolaMotion : ProjectileMotionBase
{
    public float HeightArc { get; private set; } = 1;

    public new void SetInfo(ProjectileData projectileData, Vector3 startPosition, UnitBase target, Action endCallback = null)
    {
        base.SetInfo(projectileData, startPosition, target, endCallback);
    }

    protected override IEnumerator LaunchProjectile()
    {
        float journetLength = Vector3.Distance (StartPosition,Target.transform.position);
        float totalTime = journetLength / ProjectileData.ProjSpeed;

        float elapsedTime = 0;

        while(elapsedTime< totalTime)
        {
            elapsedTime += Time.deltaTime;

            float normalizedTime = elapsedTime / totalTime;
            
            Vector3 targetPosition = Target.CenterPosition;
            float x = Mathf.Lerp(StartPosition.x, targetPosition.x, normalizedTime);
            float z = Mathf.Lerp(StartPosition.z, targetPosition.z, normalizedTime);
            float baseY = Mathf.Lerp(StartPosition.y,targetPosition.y, normalizedTime);
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
