using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float beginning;
    private Text timeText;
    // Start is called before the first frame update
    void Start()
    {
        this.beginning = Time.time;
        this.timeText = GameObject.Find("TimeText")?.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.timeText) {
            this.timeText.text = this.FormattedTime();
        }
    }

    public void ResetTime() {
        this.beginning = Time.time;
    }

    string FormattedTime() {
        float duration = Time.time - beginning;
        var span = new TimeSpan(0, 0, 0, (int)duration);

        if (span.Hours > 0) {
            return $"{span.Hours}:{pad(span.Minutes)}:{pad(span.Seconds)}";
        }
        return $"{span.Minutes}:{pad(span.Seconds)}";
    }

    string pad(int number) {
        return number < 10 ? $"0{number}" : number.ToString();
    }
}
