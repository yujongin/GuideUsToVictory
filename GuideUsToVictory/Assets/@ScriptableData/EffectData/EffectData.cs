using UnityEngine;
using static Define;

[CreateAssetMenu(fileName = "EffectData", menuName = "Scriptable Objects/EffectData")]
public class EffectData : ScriptableObject
{
    public string Name;
    public float Amount;
    public float PercentAdd;
    public float PercentMult;
    public EEffectType EffectType;
}
