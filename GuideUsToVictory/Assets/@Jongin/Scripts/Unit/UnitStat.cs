using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;
public class UnitStat
{
    public float BaseValue { get; private set; }
    private bool isDirty = true;

    [SerializeField]
    private float _value;
    public virtual float Value
    {
        get
        {
            if (isDirty)
            {
                _value = CalculateFinalValue();
                isDirty = false;
            }
            return _value;
        }

        private set { _value = value; }
    }

    public List<StatModifier> StatModifiers = new List<StatModifier>();

    public UnitStat()
    {
    }

    public UnitStat(float baseValue) : this()
    {
        BaseValue = baseValue;
    }

    public virtual void AddModifier(StatModifier modifier)
    {
        isDirty = true;
        StatModifiers.Add(modifier);
    }

    public virtual bool RemoveModifier(StatModifier modifier)
    {
        if (StatModifiers.Remove(modifier))
        {
            isDirty = true;
            return true;
        }
        return false;
    }

    public virtual bool ClearModifiersFromSource(object source)
    {
        int numRemovals = StatModifiers.RemoveAll(mod => mod.source == source);

        if (numRemovals > 0)
        {
            isDirty = true;
            return true;
        }
        return false;
    }

    private int CompareOrder(StatModifier a, StatModifier b)
    {
        if (a.order == b.order)
            return 0;

        return (a.order < b.order) ? -1 : 1;
    }

    private float CalculateFinalValue()
    {
        float finalValue = BaseValue;
        float sumPercentAdd = 0;

        StatModifiers.Sort(CompareOrder);

        for (int i = 0; i < StatModifiers.Count; i++)
        {
            StatModifier modifier = StatModifiers[i];

            switch (modifier.type)
            {
                case EStatModType.Add:
                    finalValue += modifier.value;
                    break;
                case EStatModType.PercentAdd:
                    sumPercentAdd += modifier.value;
                    if (i == StatModifiers.Count - 1 || StatModifiers[i + 1].type != EStatModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                    break;
                case EStatModType.PercentMult:
                    finalValue *= 1 + modifier.value;
                    break;
            }
        }

        return (float)Math.Round(finalValue, 4);
    }

}
