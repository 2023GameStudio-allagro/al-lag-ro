using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CommandType
{
    single, multi, hold
}
public enum CommandPhase
{
    start, hold, end
}

public class InputCommand
{
    public CommandType type {get; private set;}
    public AttackKey keys {get; private set;}
    public CommandPhase phase {get; private set;}

    public InputCommand(AttackKey key)
    {
        this.type = CommandType.single;
        this.keys = key;
        this.phase = CommandPhase.start;
    }

    public InputCommand(CommandType type, List<AttackKey> keys, CommandPhase phase)
    {
        this.type = type;
        this.keys = AttackKey.none;
        this.phase = phase;
        foreach(AttackKey key in keys)
        {
            this.keys |= key;
        }
    }
}
