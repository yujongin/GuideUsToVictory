using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class StatModifier
{

    public readonly float value;
    public readonly EStatModType type;
    public readonly int order;
    public readonly object source;

    public StatModifier(float value, EStatModType type, int order, object source)
    {
        this.value = value;
        this.type = type;
        this.order = order;
        this.source = source;
    }

    public StatModifier(float value, EStatModType type) : this(value, type, (int)type, null) { }

    public StatModifier(float value, EStatModType type, int order) : this(value, type, order, null) { }

    public StatModifier(float value, EStatModType type, object source) : this(value, type, (int)type, source) { }
}