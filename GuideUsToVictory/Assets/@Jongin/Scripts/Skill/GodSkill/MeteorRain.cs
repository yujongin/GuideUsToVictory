using UnityEngine;

public class MeteorRain : MonoBehaviour
{
    bool isUse = false;
    public GameObject effect;
    public void DoSkill()
    {
        if (isUse) return;

        isUse = true;
        Collider[] colliders = Physics.OverlapSphere(Vector3.zero, 200f, Managers.Game.enemyLayer);
        foreach(var collider in colliders)
        {
            UnitController unit;
            if(collider.TryGetComponent<UnitController>(out unit))
            {
                unit.curHp = 0;
                unit.OnDead();
            }
        }
    }
}
