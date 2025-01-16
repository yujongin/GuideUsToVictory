using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "Scriptable Objects/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public string Name;
    public float Duration;
    public float HitSound;
    public float ProjRange;
    public float ProjSpeed;
}
