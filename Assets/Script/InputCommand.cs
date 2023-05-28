using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CommandType
{
    single, multi, hold
}
public enum CommandKey
{
    q, w, e, r
}
public enum CommandPhase
{
    start, hold, end
}

public class InputCommand
{
    public CommandType type {get; private set;}
    public List<CommandKey> keys {get; private set;}
    public CommandPhase phase {get; private set;}

    public InputCommand(CommandKey key)
    {
        this.type = CommandType.single;
        this.keys = new List<CommandKey>{key};
        this.phase = CommandPhase.start;
    }

    public InputCommand(CommandType type, List<CommandKey> keys, CommandPhase phase)
    {
        this.type = type;
        this.keys = keys;
        this.phase = phase;
    }
}
