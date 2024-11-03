using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    public GameObject linePrefab;
    public Line activeLine;
    public LineRenderer activeLineRenderer;
    public float meter;
    public float meterMax;
    public float regenRate;
    public bool pressingMouse;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && meter > 0)
        {
            GameObject newLine = Instantiate(linePrefab);
            activeLine = newLine.GetComponent<Line>();
            activeLineRenderer = newLine.GetComponent<LineRenderer>();
            pressingMouse = true;
        }

        if (activeLineRenderer != null && pressingMouse && meter > 0)
        {
            meter -= activeLineRenderer.positionCount * Time.deltaTime;
        }

        if (activeLine == null && meter < meterMax)
        {
            meter += regenRate * Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0) || meter <= 0)
        {
            pressingMouse = false;
            activeLine = null;
        }

        if (activeLine != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            activeLine.UpdateLine(mousePos);
        }
    }
}
