using Morpeh;
using Morpeh.Globals;

using Unity.IL2CPP.CompilerServices;

using UnityEngine.UI;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[System.Serializable]
public struct MainButtonComponent : IComponent {
    public Button Button;
    public int Index;
}