using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class Painter : MonoBehaviour
{
    public float maxInk = 10f;
    public float inkLevel;
    public float threshold = 0.001f;
    public GameObject template;
    public OurNetworkManager network;
    private bool isDragging = false;
    private List<Vector3> lineBuffer;
    private LineRenderer lineRenderer;
    private Camera cam;
    private int lineCount = 0;
    public Text attemptText;
    public Slider inkMeter;
    private float fullInkWidth;
    public bool readOnly;
    private float lineAmount;
    public int attempts;

    // Start is called before the first frame update

    void Awake()
    {
        this.lineRenderer = this.GetComponent<LineRenderer>();
        this.cam = Camera.main;
        //network.SetInkLevel(1);
        this.inkMeter = GameObject.Find("InkLevel").GetComponent<Slider>();
        this.attemptText = GameObject.Find("attemptText").GetComponent <Text>();
    }

    // Update is called once per frame
    void Update()
    {
        this.attemptText.text = this.attempts <= 0 ? "0" : this.attempts.ToString();
        this.inkMeter.maxValue = this.maxInk;
        this.inkMeter.value = this.inkLevel - this.lineAmount;

        if (Input.GetMouseButtonDown(0) && !readOnly) {
            this.isDragging = true;
            this.lineBuffer = new List<Vector3>();
            this.lineCount = 0;
            this.lineAmount = 0;
        }
        if (Input.GetMouseButtonUp(0) && !readOnly) {
            this.isDragging = false;
            // Add the element to the screen
            if (this.lineBuffer.Count > 2) this.AddLine();
            this.lineBuffer = new List<Vector3>();
            this.lineCount = 0;
            this.lineRenderer.positionCount = 0;
            network.SetInkLevel((this.inkLevel - this.lineAmount) / this.maxInk);
            this.lineAmount = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            network.ResetLevel();
        }


        if (this.isDragging && this.inkLevel > 0 && this.inkLevel - this.lineAmount > 0) {
            this.Drag();
        }
    }

    void Drag() {
        var mousePos = Input.mousePosition;
        mousePos.z = cam.nearClipPlane;
        var mouseWorld = cam.ScreenToWorldPoint(mousePos);
        mouseWorld.z = -1;
        
        if (lineBuffer.Count == 0 || Vector2.Distance(mouseWorld, lineBuffer[lineBuffer.Count - 1]) > this.threshold) {
            if (lineBuffer.Count > 0)
            {
                while (inkLevel - (lineBuffer[lineBuffer.Count - 1] - mouseWorld).magnitude < 0)
                {
                    mouseWorld = Vector3.MoveTowards(mouseWorld, lineBuffer[lineBuffer.Count - 1], .01f);
                }
            }

            lineBuffer.Add(mouseWorld);
            this.UpdateLine();
        }
    }

    void UpdateLine() {
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = lineBuffer.Count;
        for (int i = this.lineCount; i < lineBuffer.Count; i++) {
            lineRenderer.SetPosition(i, lineBuffer[i]);
            if (this.lineCount > 1)
            {
                lineAmount += ((lineBuffer[i] - lineBuffer[i - 1]).magnitude) ;
            }

        }
        lineCount = lineBuffer.Count;
    }

    void AddLine() {
        this.network.AddLine(this.lineBuffer);
    }

    public void setInkLevelPercent(float pct)
    {
        inkLevel = maxInk * pct;
        if (Math.Round(inkLevel, 1) == 0) inkLevel = 0;
    }

    public void setAttempts(int num)
    {
        this.attempts = num;
    }
}
