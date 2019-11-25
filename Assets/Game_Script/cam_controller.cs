using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class cam_controller : MonoBehaviour
{
    public Camera StartCam;
    public Camera[] Cams;
    public int[] CrosshairOn;
    public int selectedCam = 0;
    public GameObject HudManager;

    void ChangeCam()
    {
        selectedCam++;
        if(selectedCam >= Cams.Length)
            selectedCam = 0;
        ICrosshair ch = HudManager.GetComponent<hud>();
        if(CrosshairOn.Contains(selectedCam))
            ch.TurnOnCrosshair();
        else
            ch.TurnOffCrosshair();
        
        for(int i = 0; i < Cams.Length; i++)
        {
            if(i == selectedCam)
                Cams[i].enabled = true;
            else
                Cams[i].enabled = false;
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
            ChangeCam();
    }

    void Start()
    {
        foreach(var c in Cams)
            c.enabled = false;
        StartCam.enabled = true;
    }
}
