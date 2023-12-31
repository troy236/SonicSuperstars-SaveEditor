﻿namespace SonicSuperstarsSaveEditor;

public enum EAction {
    MoveUp = 0,
    MoveDown = 1,
    MoveRight = 2,
    MoveLeft = 3,
    Action = 4,
    EmeraldSelectButtonL = 7,
    EmeraldSelectButtonR = 6,
    PauseOpenClose = 9,
    SubButton1 = 10,
    SubButton2 = 11,
    MenuMoveUp = 13,
    MenuMoveDown = 14,
    MenuMoveRight = 15,
    MenuMoveLeft = 16,
    MenuDecide = 17,
    MenuCancel = 18,
    MenuSubButton1 = 19,
    MenuSubButton2 = 20,
    MenuSubButton3 = 21,
    MenuRB = 22,
    MenuLB = 23,
    UIChangeVisible = 24,
    EmeraldSelectStickR = 25,
    Max = 26
}


[Flags]
public enum EButton {
    None = 0,
    RRight = 1,
    RDown = 2,
    RUp = 4,
    RLeft = 8,
    StickL = 0x10,
    StickR = 0x20,
    L = 0x40,
    R = 0x80,
    ZL = 0x100,
    ZR = 0x200,
    Plus = 0x400,
    Minus = 0x800,
    LLeft = 0x1000,
    LUp = 0x2000,
    LRight = 0x4000,
    LDown = 0x8000,
    StickLLeft = 0x10000,
    StickLUp = 0x20000,
    StickLRight = 0x40000,
    StickLDown = 0x80000,
    StickRLeft = 0x100000,
    StickRUp = 0x200000,
    StickRRight = 0x400000,
    StickRDown = 0x800000,
    LeftSL = 0x1000000,
    LeftSR = 0x2000000,
    RightSL = 0x4000000,
    RightSR = 0x8000000,
    DirLeft = 0x11000,
    DirUp = 0x22000,
    DirRight = 0x44000,
    DirDown = 0x88000
}

public enum EUseType {
    InGame,
    Menu
}

public enum EKey {
    None = 0,
    Space = 1,
    Enter = 2,
    Tab = 3,
    Backquote = 4,
    Quote = 5,
    Semicolon = 6,
    Comma = 7,
    Period = 8,
    Slash = 9,
    Backslash = 10,
    LeftBracket = 11,
    RightBracket = 12,
    Minus = 13,
    Equals = 14,
    A = 15,
    B = 16,
    C = 17,
    D = 18,
    E = 19,
    F = 20,
    G = 21,
    H = 22,
    I = 23,
    J = 24,
    K = 25,
    L = 26,
    M = 27,
    N = 28,
    O = 29,
    P = 30,
    Q = 31,
    R = 32,
    S = 33,
    T = 34,
    U = 35,
    V = 36,
    W = 37,
    X = 38,
    Y = 39,
    Z = 40,
    Digit1 = 41,
    Digit2 = 42,
    Digit3 = 43,
    Digit4 = 44,
    Digit5 = 45,
    Digit6 = 46,
    Digit7 = 47,
    Digit8 = 48,
    Digit9 = 49,
    Digit0 = 50,
    LeftShift = 51,
    RightShift = 52,
    LeftAlt = 53,
    RightAlt = 54,
    AltGr = 54,
    LeftCtrl = 55,
    RightCtrl = 56,
    LeftMeta = 57,
    RightMeta = 58,
    LeftWindows = 57,
    RightWindows = 58,
    LeftApple = 57,
    RightApple = 58,
    LeftCommand = 57,
    RightCommand = 58,
    ContextMenu = 59,
    Escape = 60,
    LeftArrow = 61,
    RightArrow = 62,
    UpArrow = 63,
    DownArrow = 64,
    Backspace = 65,
    PageDown = 66,
    PageUp = 67,
    Home = 68,
    End = 69,
    Insert = 70,
    Delete = 71,
    CapsLock = 72,
    NumLock = 73,
    PrintScreen = 74,
    ScrollLock = 75,
    Pause = 76,
    NumpadEnter = 77,
    NumpadDivide = 78,
    NumpadMultiply = 79,
    NumpadPlus = 80,
    NumpadMinus = 81,
    NumpadPeriod = 82,
    NumpadEquals = 83,
    Numpad0 = 84,
    Numpad1 = 85,
    Numpad2 = 86,
    Numpad3 = 87,
    Numpad4 = 88,
    Numpad5 = 89,
    Numpad6 = 90,
    Numpad7 = 91,
    Numpad8 = 92,
    Numpad9 = 93,
    F1 = 94,
    F2 = 95,
    F3 = 96,
    F4 = 97,
    F5 = 98,
    F6 = 99,
    F7 = 100,
    F8 = 101,
    F9 = 102,
    F10 = 103,
    F11 = 104,
    F12 = 105,
    OEM1 = 106,
    OEM2 = 107,
    OEM3 = 108,
    OEM4 = 109,
    OEM5 = 110,
    IMESelected = 111
}

[Flags]
public enum ESafeKey {
    None = 0,
    Enter = 1,
    Backspace = 2,
    LeftArrow = 4,
    RightArrow = 8,
    UpArrow = 0x10,
    DownArrow = 0x20
}

[Flags]
public enum EMouseButton {
    None = 0,
    Left = 1,
    Right = 2,
    Middle = 4,
    Forward = 8,
    Back = 0x10
}

[Flags]
public enum AzKey : ulong {
    None = 0uL,
    OEM2 = 1uL,
    Quote = 2uL,
    Semicolon = 4uL,
    Comma = 8uL,
    Period = 0x10uL,
    Slash = 0x20uL,
    Backslash = 0x40uL,
    LeftBracket = 0x80uL,
    RightBracket = 0x100uL,
    Minus = 0x200uL,
    Equals = 0x400uL,
    A = 0x800uL,
    B = 0x1000uL,
    C = 0x2000uL,
    D = 0x4000uL,
    E = 0x8000uL,
    F = 0x10000uL,
    G = 0x20000uL,
    H = 0x40000uL,
    I = 0x80000uL,
    J = 0x100000uL,
    K = 0x200000uL,
    L = 0x400000uL,
    M = 0x800000uL,
    N = 0x1000000uL,
    O = 0x2000000uL,
    P = 0x4000000uL,
    Q = 0x8000000uL,
    R = 0x10000000uL,
    S = 0x20000000uL,
    T = 0x40000000uL,
    U = 0x80000000uL,
    V = 0x100000000uL,
    W = 0x200000000uL,
    X = 0x400000000uL,
    Y = 0x800000000uL,
    Z = 0x1000000000uL,
    Digit1 = 0x2000000000uL,
    Digit2 = 0x4000000000uL,
    Digit3 = 0x8000000000uL,
    Digit4 = 0x10000000000uL,
    Digit5 = 0x20000000000uL,
    Digit6 = 0x40000000000uL,
    Digit7 = 0x80000000000uL,
    Digit8 = 0x100000000000uL,
    Digit9 = 0x200000000000uL,
    Digit0 = 0x400000000000uL,
    NumpadDivide = 0x800000000000uL,
    NumpadMultiply = 0x1000000000000uL,
    NumpadPlus = 0x2000000000000uL,
    NumpadMinus = 0x4000000000000uL,
    NumpadPeriod = 0x8000000000000uL,
    Numpad0 = 0x10000000000000uL,
    Numpad1 = 0x20000000000000uL,
    Numpad2 = 0x40000000000000uL,
    Numpad3 = 0x80000000000000uL,
    Numpad4 = 0x100000000000000uL,
    Numpad5 = 0x200000000000000uL,
    Numpad6 = 0x400000000000000uL,
    Numpad7 = 0x800000000000000uL,
    Numpad8 = 0x1000000000000000uL,
    Numpad9 = 0x2000000000000000uL
}
