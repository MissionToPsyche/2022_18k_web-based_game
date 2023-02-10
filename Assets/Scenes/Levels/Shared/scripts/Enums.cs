using System;

namespace InterWorld.Shared.Enums
{
    public enum Direction
    {
        Right = 1,
        Left = -1
    }

    // first part of the number is for teir, second is for part
    // 0000 0000 : first 4 bits store teir, second 4 bits store part
    public enum RocketPartType
    {
        None,
        Capsule,
        Engine,
        Seperator,
        Nose,
        Tank,
    }

    public enum RocketPartTeir
    {
        none = 8,
        one = 16,
        two = 32,
        three = 64,
    }
}


