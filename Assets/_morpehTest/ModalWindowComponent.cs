﻿using Morpeh;

using TMPro;

using Unity.IL2CPP.CompilerServices;

using UnityEngine;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[System.Serializable]
public struct ModalWindowComponent : IComponent {
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;
    public CanvasGroup CanvasGroup;
    public float AutohideDuration;
    public float LastShowTimestamp;
    public bool IsShowing;
}