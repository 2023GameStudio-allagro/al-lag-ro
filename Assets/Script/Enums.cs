using System;

public enum Judgement
{
    perfect,
    good,
    early,
    late
}

[Flags]
public enum AttackKey
{
    none = 0,
    z = 1 << 0,
    x = 1 << 1,
    c = 1 << 2,
    v = 1 << 3
}