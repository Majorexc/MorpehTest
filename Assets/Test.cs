using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    void Awake() {
        GetComponent<ScrollRect>().onValueChanged.AddListener((Vector2 val) => Debug.Log(val));
    }
}
