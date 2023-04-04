using UnityEngine;
using UnityEngine.UI;

public class DatePicker : MonoBehaviour {
    public static DatePicker current;
    public Text text;
    public Text[] days;
    public Toggle[] toggles;

    public void Start() {
        Init();
    }

    public void OnDestroy() {
        DateSelector.current.OnPickEnd();
    }

    public void Init() {
        current = this;        
        days = GetComponentsInChildren<Text>();
        toggles = GetComponentsInChildren<Toggle>();
        DateSelector.current.OnOptionValueUpdate(-1);
    }
}
