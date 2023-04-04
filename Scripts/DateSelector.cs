using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class DateSelector: MonoBehaviour {

    public static DateSelector current;

    public Text dateTextOnDropdown;    
    
    private DateTime currentDate;
    public DateTime CurrentDate {
        get {
            return currentDate;
        }
        private set {
            if (currentDate != value) {
                currentDate = value;                
            }
        }
    }
    private DateTime pickingDate;

    public DateTime PickingDate {
        get {
            return pickingDate;
        }
        private set {
            if (pickingDate != value) {
                pickingDate = value;
            }
        }
    }

    private Dropdown dropdown;
    private bool isPicking = false;
    private bool hasPicked = false;

    private readonly Color gray = new Color(0.9f, 0.9f, 0.9f, 1.0f);

    public event Action<DateTime> DateSelectorUdate;


    void Start() {

        current = this;
        
        // Dropdown Init
        dropdown = GetComponent<Dropdown>();
        dropdown.options = new List<Dropdown.OptionData>();
        dropdown.options.AddRange(new Dropdown.OptionData[42]);

        // DateTime Init
        CurrentDate = DateTime.Now;
        PickingDate = DateTime.Now;
        dateTextOnDropdown.text = string.Format("{0:D3}/{1:D2}/{2:D2}", CurrentDate.Year - 1911, CurrentDate.Month, CurrentDate.Day);

        // Initial value
        UpdateDropdownOptions();

        // Add event listener
        dropdown.onValueChanged.AddListener(new UnityAction<int>(OnOptionValueUpdate));
    }

    public void UpdateDropdownOptions() {        
        var startWeek = (int)PickingDate.AddDays(1 - PickingDate.Day).DayOfWeek;
        dropdown.value = startWeek + PickingDate.Day - 1;
    }

    private void UpdatePickerTable() {
        // Count from first day of week in this month
        var lastMonthDaysInTable = (int)PickingDate.AddDays(1 - PickingDate.Day).DayOfWeek;
        var lastMonth = PickingDate.AddMonths(-1);
        var lastMonthDays = DateTime.DaysInMonth(lastMonth.Year, lastMonth.Month);
        var days = DateTime.DaysInMonth(PickingDate.Year, PickingDate.Month);

        // Days in last month
        DrawDaysInMonth(0, lastMonthDaysInTable, lastMonthDays - lastMonthDaysInTable + 1, gray);
        // Days in this month
        DrawDaysInMonth(lastMonthDaysInTable, days, 1, Color.white);
        // Days in next month
        DrawDaysInMonth(lastMonthDaysInTable + days, 42 - lastMonthDaysInTable - days, 1, gray);

        DatePicker.current.toggles[dropdown.value].isOn = true;
        
    }
    private void DrawDaysInMonth(int startPos, int count, int startDay, Color color) {
        for (int i = startPos; i < startPos + count; i++) {
            var dayText = (i - startPos + startDay).ToString();
            DatePicker.current.days[i].text = dayText;
            DatePicker.current.toggles[i].image.color = color;
        }        
    }

    private void OnMonthPickerUpdate() {
        UpdatePickerTable();
        UpdateTitleDateText();
    }

    public void NextMonth() {
        PickingDate = PickingDate.AddMonths(1);
        OnMonthPickerUpdate();
    }

    public void LastMonth() {
        PickingDate = PickingDate.AddMonths(-1);
        OnMonthPickerUpdate();
    }

    private void UpdateLabelDateText() {        
        dateTextOnDropdown.text = string.Format("{0:D3}/{1:D2}/{2:D2}", CurrentDate.Year - 1911, CurrentDate.Month, CurrentDate.Day); 
    }
            
    private void UpdateTitleDateText() {        
        if (isPicking) {
            DatePicker.current.text.text = string.Format("{0:D4} ¦~ {1:D2} ¤ë {2:D2} ¤é", PickingDate.Year, PickingDate.Month, PickingDate.Day);
        }
    }

    public void OnOptionValueUpdate(int option) {
        Debug.Log("opt == " + option);
        if (!isPicking) {
            isPicking = true;
            hasPicked = false;
            PickingDate = CurrentDate;
            UpdateTitleDateText();
            UpdatePickerTable();            
            Debug.Log("Pick Start");
        }
        else {
            hasPicked = true;
            PickingDate = SetDateByOption(PickingDate, option);
            UpdateTitleDateText();
            UpdateDropdownOptions();
            Debug.Log("Apply Pick");
        }
    }

    public void OnPickEnd() {
        isPicking = false;        
        if (hasPicked) {
            hasPicked = false;
            CurrentDate = PickingDate;
            UpdateLabelDateText();            
            Debug.Log("Change Date");
            DateSelectorUdate?.Invoke(CurrentDate);
        }
        Debug.Log("Pick End");
    }

    private DateTime SetDateByOption(DateTime pickingDate, int optionValue) {
        var startWeek = (int)pickingDate.AddDays(1 - pickingDate.Day).DayOfWeek;        
        if (optionValue < startWeek) {
            pickingDate = pickingDate.AddMonths(-1);
            pickingDate = pickingDate.AddDays((DateTime.DaysInMonth(pickingDate.Year, pickingDate.Month) - startWeek + optionValue - pickingDate.Day + 1));
        }
        else {
            pickingDate = pickingDate.AddDays(optionValue - startWeek - pickingDate.Day + 1);
        }
        return pickingDate;
    }
}
