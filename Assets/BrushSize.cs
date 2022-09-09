using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushSize : MonoBehaviour
{

    public Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        if (PlayerPrefs.HasKey("brushSize"))
        {
            slider.value = PlayerPrefs.GetFloat("brushSize");
        }
       
    }

    public void ChangeBrushSize()
    {
        PlayerPrefs.SetFloat("brushSize", slider.value);
    }

    
}
