using System;

[Flags]
public enum MotionState
{
    disabled = 0,
    Entering = 1 << 0,
    Idling = 1 << 1,
    Exiting = 1 << 2,
    Hiding = 1 << 3,
    Hidden = 1 << 4,
    Resuming = 1 << 5,
    Active = 1 << 6
}


//最小限のstate
public enum ModuleState
{
    disabled,      //電源がオフ
    sleeping,      //電源オン受け付け不可
    ready,         //受付中
    working,       //仕事中
    compleate      //一個前の仕事が完了
}