using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTime : MonoBehaviour
{

    public int playTime  = 0; // manipulate and save using PlayerPrefs.
    private int seconds = 0;
    private int minutes = 0;
    private int hours = 0;
    private int days = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RecordPlayTime());
    }

    private IEnumerator RecordPlayTime()
    {
        while (true) 
        {
            yield return new WaitForSeconds(1);
            playTime += 1;
            seconds = ( playTime % 60 );
            minutes = ( playTime / 60 ) % 60;
            hours = ( playTime / 3600 ) % 24; 
            days = ( playTime / 86400 ) % 365;
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(Screen.width/2 - 250, Screen.height/10, 500, 100), "Playtime = " + days + " Days " + hours + " Hours " + 
            minutes + " Minutes " + seconds + " Seconds");
    }
}
