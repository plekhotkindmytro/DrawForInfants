using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    public GameObject linePrefab;
    private int lineCount = 0;

    public float lineWidthMultiplier = 1f;
    

    Line activeLine;

    Dictionary<int, Line> activeLineMap;


    private List<Color> colors;
    private int activeColorIndex = 0;

    void Awake()
    {
        activeLineMap = new Dictionary<int, Line>();
        SetCameraBackgroundColor();
        SetColorsForLines();
        
     
    }

    private void SetColorsForLines()
    {
        Color red = new Color32(255, 0, 0, 255);
        Color orange = new Color32(255, 135, 0, 255);
        Color yellow = new Color32(255, 211, 0, 255);
        Color green = new Color32(161, 255, 10, 255);
        Color lightBlue = new Color32(10, 239, 255, 255);
        Color blue = new Color32(20, 125, 245, 255);
        Color indigo = new Color32(88, 10, 255, 255);
        Color violet = new Color32(190, 10, 255, 255);
        colors = new List<Color>();
        colors.Add(red);
        colors.Add(orange);
        colors.Add(yellow);
        colors.Add(green);
        colors.Add(lightBlue);
        colors.Add(blue);
        colors.Add(indigo);
        colors.Add(violet);
    }

    private void SetCameraBackgroundColor()
    {
        Color blackLiver = new Color32(64, 63, 76, 255);
        Color lavanderBlush = new Color32(238, 229, 233, 255);
        Color babyPowder = new Color32(255, 255, 252, 255);
        Color cultured = new Color32(241, 237, 238, 255);
        Color cornsilk = new Color32(254, 250, 220, 255);
        Color white = new Color32(252, 252, 252, 255);
        List<Color> backGroundColors = new List<Color>();
        backGroundColors.Add(lavanderBlush);
        backGroundColors.Add(babyPowder);
        backGroundColors.Add(cultured);
        backGroundColors.Add(cornsilk);
        backGroundColors.Add(white);
        backGroundColors.Add(blackLiver);

        Camera.main.backgroundColor = backGroundColors[UnityEngine.Random.Range(0, backGroundColors.Count - 1)];
    }



    /*
     * 
     * TouchPhase.Began - A user has touched their finger to the screen this frame
     * TouchPhase.Stationary - A finger is on the screen but the user has not moved it this frame
     * TouchPhase.Moved - A user moved their finger this frame
     * TouchPhase.Ended - A user lifted their finger from the screen this frame
     * TouchPhase.Cancelled - The touch was interrupted this frame
     *
     */
    void Update()
    {
        if (Input.touches != null && Input.touches.Length > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    GameObject newLineGO = Instantiate(linePrefab);
                   

                    Line newLine = newLineGO.GetComponent<Line>();
                    SetLineWidth(newLineGO);
                    SetLineColor(newLineGO);
                    SetLineSortingOrder(newLineGO);


                    activeLineMap.Add(touch.fingerId, newLine);
                }

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    activeLineMap.Remove(touch.fingerId);
                    SaveUserData();
                }

                if (activeLineMap.ContainsKey(touch.fingerId))
                {
                    Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                    activeLineMap[touch.fingerId].UpdateLine(touchPos);
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject newLine = Instantiate(linePrefab);
                
                activeLine = newLine.GetComponent<Line>();
                SetLineWidth(newLine);
                SetLineColor(newLine);
                SetLineSortingOrder(newLine);
            }

            if (Input.GetMouseButtonUp(0))
            {
                activeLine = null;
                SaveUserData();

            }

            if (activeLine != null)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                activeLine.UpdateLine(mousePosition);
            }   
        }
    }

    private void SaveUserData()
    {
        int maxLineCount = 0;
        if (PlayerPrefs.HasKey("maxLineCount"))
        {
            maxLineCount = PlayerPrefs.GetInt("maxLineCount");
        }
        if (lineCount > maxLineCount)
        {
            PlayerPrefs.SetInt("maxLineCount", lineCount);
            PlayerPrefs.Save();

           // FileStream file = File.Open(Application.persistentDataPath + "/gameInfo.dat", FileMode.Open);
        }
    }

    private void ResetUserData()
    {
        PlayerPrefs.DeleteAll();
    }

    private void SetLineWidth(GameObject newLine)
    {
        LineRenderer newLineRenderer = newLine.GetComponent<LineRenderer>();
        if (PlayerPrefs.HasKey("brushSize"))
        {
            float brushSize = PlayerPrefs.GetFloat("brushSize");
            lineWidthMultiplier = brushSize;
        }
        newLineRenderer.widthMultiplier = lineWidthMultiplier;
    }

    private void SetLineSortingOrder(GameObject gameObject)
    {
        Renderer newLineRenderer = gameObject.GetComponent<Renderer>();
        newLineRenderer.sortingOrder = lineCount;
        lineCount++;
    }

    private void SetLineColor(GameObject gameObject)
    {
        Renderer newLineRenderer = gameObject.GetComponent<Renderer>();
        Debug.Log(colors[activeColorIndex].ToString());
        newLineRenderer.material.color = colors[activeColorIndex];
        activeColorIndex++;
        if (activeColorIndex >= colors.Count)
        {
            activeColorIndex = 0;
        }
    }
}
