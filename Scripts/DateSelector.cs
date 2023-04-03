using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEditor.Sprites;
using UnityEngine.UIElements;

public class DateSelector: MonoBehaviour, IPointerClickHandler {
    public Text dateTextOnDropdown;
    private DatePicker picker;

    private DateTime dateTime;
    private DateTime displayDate;

    private Dropdown dropdown;
    private bool monthChanged;

    void Start() {

        dropdown = GetComponent<Dropdown>();        
        dateTime = DateTime.Now;
        displayDate = DateTime.Now;
        dropdown.options = new List<Dropdown.OptionData>();
        for (int i = 0; i < 42; i++) {
            dropdown.options.Add(new Dropdown.OptionData() { text = "0" });
        }

        // 更新日期文本
        UpdateDropdownOptions();
        UpdateDateText();
    }

    public void UpdateDropdownOptions() {
        
        var startWeek = (int)displayDate.AddDays(1 - displayDate.Day).DayOfWeek;

        var lastMonth = dateTime.AddMonths(-1);
        var lastMonthDays = DateTime.DaysInMonth(lastMonth.Year, lastMonth.Month);
        for (int i = 0; i < startWeek; i++) {
            var dayText = (lastMonthDays - startWeek + i + 1).ToString();
            dropdown.options[i].text = dayText;
        }
        var days = DateTime.DaysInMonth(displayDate.Year, displayDate.Month);        
        for (int i = 0; i <= days; i++) {
            var dayText = (i + 1).ToString();
            dropdown.options[i + startWeek].text = dayText;
        }        
        for (int i = 0; i < 42 - days - startWeek; i++) {
            var dayText = (i + 1).ToString();
            dropdown.options[i + days + startWeek].text = dayText;
        }

        dropdown.value = startWeek + dateTime.Day - 1;

        if (!picker) return;

        if (picker.days.Length == 0) picker.Init();

        for (int i = 0; i < picker.days.Length; i++) {
            picker.days[i].text = dropdown.options[i].text;
            if(i >= startWeek && i < startWeek + days) {
                picker.toggles[i].image.color = Color.white;                
            }
            else {
                picker.toggles[i].image.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
            }
        }
    }

    public void NextMonth() {
        displayDate = displayDate.AddMonths(1);
        UpdateDropdownOptions();        
        UpdateDateText();
        monthChanged = true;
    }

    public void LastMonth() {
        displayDate = displayDate.AddMonths(-1);
        UpdateDropdownOptions();        
        UpdateDateText();
        monthChanged = true;
    }

    private void UpdatePickedDateText() {
        dateTextOnDropdown.text = string.Format("{0:D3}/{1:D2}/{2:D2}", dateTime.Year - 1911, dateTime.Month, dateTime.Day); 
    }


    // 更新日期文本
    private void UpdateDateText() {        
        if (picker) {
            picker.text.text = string.Format("{0:D4} 年 {1:D2} 月 {2:D2} 日", displayDate.Year, displayDate.Month, displayDate.Day);
        }
    }

    // 點擊選擇日期按鈕時
    public void OnDatePicked() {

        var startWeek = (int)displayDate.AddDays(1 - displayDate.Day).DayOfWeek;        
        if (dropdown.value < startWeek) {
            displayDate = displayDate.AddMonths(-1);
            displayDate = displayDate.AddDays((DateTime.DaysInMonth(displayDate.Year, displayDate.Month) - startWeek + dropdown.value + 1 - displayDate.Day));
        }
        else {
            displayDate = displayDate.AddDays(dropdown.value - startWeek - displayDate.Day + 1);
        }
        dateTime = displayDate;
        UpdateDateText();
        UpdateDropdownOptions();
        UpdatePickedDateText();
    }

    public void OnPointerClick(PointerEventData eventData) {
        displayDate = dateTime;
        picker = FindObjectOfType<DatePicker>();
        UpdateDropdownOptions();
        picker.text.text = string.Format("{0:D4} 年 {1:D2} 月 {2:D2} 日", displayDate.Year, displayDate.Month, displayDate.Day);        
    }
}
