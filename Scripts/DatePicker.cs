using UnityEngine;
using UnityEngine.UI;

public class DatePicker : MonoBehaviour {
    public Text text;
    public Text[] days;
    public Toggle[] toggles;
    public void Init() {
        days = GetComponentsInChildren<Text>();
        toggles = GetComponentsInChildren<Toggle>();
    }
}
