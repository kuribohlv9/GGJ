using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    public static GameMaster instance;
    private bool HasRaceStarted = false;

    public float TimeLimit = 60;
    public PlayerController Player;
    public CameraMove cameracontrol;

    // Use this for initialization
    void Start()
    {
        if (instance == null || instance == this)
            instance = this;
        else
            Destroy(this.gameObject);

        Player.enabled = false;
        cameracontrol.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            HasRaceStarted = true;
            Player.enabled = true;
            cameracontrol.enabled = true;
        }
        if (HasRaceStarted == true)
            TimeLimit -= Time.deltaTime;
    }

    void OnGUI()
    {
        if (!HasRaceStarted)
        {

            Rect startarea = new Rect();
            startarea.width = 100;
            startarea.height = 100;
            startarea.x = Camera.main.pixelWidth / 2 - startarea.width / 2;
            startarea.y = Camera.main.pixelHeight / 2 - startarea.height / 2;
            GUI.TextArea(startarea, "Press A to start the race!");
        }

        Rect timerarea = new Rect();
        timerarea.width = 50;
        timerarea.height = 25;
        timerarea.x = Camera.main.pixelWidth / 2 - timerarea.width / 2;
        timerarea.y = 0;
        
        GUI.TextArea(timerarea, TimeLimit.ToString("F2"));
    }
}
