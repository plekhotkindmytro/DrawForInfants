using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    float currentTime = 0;
    public float startMinutes = 0;

    public Text currentTimeText;
    void Start()
    {
        currentTime = startMinutes * 60;
    }

    // Update is called once per frame
    void Update()
    {
      //  currentTimeText.text = currentTime.ToString();
    }
}
