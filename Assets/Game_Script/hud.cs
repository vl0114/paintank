using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hud : MonoBehaviour, ICrosshair
{
    public GameObject PlayerTank;
    private Slider tank_hp_slider, tank_shell_info, tank_shell_force;
    private ITank player;
    private float max_hp;
    private Text shell_n;
    private RectTransform rotation_line;
    void Awake()
    {
        tank_hp_slider = GameObject.Find("Tank_HP").GetComponent<Slider>();
        player = PlayerTank.GetComponent<Tank>();
        max_hp = player.GetMaxHP();
        GameObject.Find("Tank_Aim_Point").GetComponent<RawImage>().enabled = false;
        tank_shell_info = GameObject.Find("Tank_Shell_Info").GetComponent<Slider>();
        shell_n = GameObject.Find("Shell_Amount").GetComponent<Text>();
        tank_shell_force = GameObject.Find("Tank_Shell_Force").GetComponent<Slider>();
        rotation_line = GameObject.Find("Line").GetComponent<RectTransform>();
    }
    void Update()
    {
        tank_hp_slider.value = player.GetHP() / max_hp;
        tank_shell_info.value = 1 - (player.GetReloadingTime() / player.GetReloadTime());
        shell_n.text = "Shell " + (player.GetHasShell().ToString());
        tank_shell_force.value = player.GetForce() / player.GetMaxForce();
        rotation_line.transform.eulerAngles = new Vector3(0, 0, player.GetTurretRelativeAngle() + 90);
    }

    public void TurnOnCrosshair()
    {
            GameObject.Find("Tank_Aim_Point").GetComponent<RawImage>().enabled = true;
    }
    public void TurnOffCrosshair()
    {
        GameObject.Find("Tank_Aim_Point").GetComponent<RawImage>().enabled = false;
    }
}
