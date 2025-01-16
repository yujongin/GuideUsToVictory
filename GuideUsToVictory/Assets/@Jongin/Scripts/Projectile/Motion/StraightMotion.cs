using System;
using System.Collections;
using UnityEngine;

public class StraightMotion : ProjectileMotionBase
{
    public new void SetInfo(ProjectileData projectileData, Vector3 startPosition, UnitBase target, Action endCallback = null)
    {
        base.SetInfo(projectileData, startPosition, target, endCallback);
    }

    protected override IEnumerator LaunchProjectile()
    {
        float journetLength = Vector3.Distance(StartPosition, Target.CenterPosition);
        float totalTime = journetLength / ProjectileData.ProjSpeed;

        float elapsedTime = 0;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;

            float normalizedTime = elapsedTime / totalTime;

            Vector3 targetPosition = Target.CenterPosition;
            transform.position = Vector3.Lerp(StartPosition, targetPosition, normalizedTime);


            if (LookAtTarget)
            {
                LookAtDir(Target.CenterPosition - transform.position);
            }

            yield return null;
        }
        transform.position = Target.CenterPosition;
        EndCallback?.Invoke();
    }
}
