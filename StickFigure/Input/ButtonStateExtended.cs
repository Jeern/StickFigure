using System;

namespace StickFigure.Input
{
    [Flags]
    public enum ButtonStateExtended 
    {
        NotSet = 0,
        IsReleased = 1,
        IsPressed = 2,
        WasReleased = 4,
        WasPressed = 8,
        JustReleased = WasPressed | IsReleased,
        JustPressed = WasReleased | IsPressed,
        DraggingReleased = WasReleased | IsReleased,
        DraggingPressed = WasPressed | IsPressed
    }
}
