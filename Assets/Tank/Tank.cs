using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour, ITank
{
    public int player_num;
    public int max_shell = 20;
    public int has_shell = 20;
    public float reload_time;
    private float t = 0;
    public float move_speed, turn_speed, turret_turn_speed, hp, max_hp;
    public float limit_angle;

    public float min_fire, max_fire, add_fire;
    private float shell_force = 0;
    public GameObject Shell;
    public GameObject ShellSpawnPoint;

    public event EventHandler DieEvent;

    private AudioSource idle, drive;

    private Transform turret;
    
    private bool on_ground = false;

    void OnTriggerEnter(Collider coll)
    {
        if(coll.tag == "Ground")
            on_ground = true;
    }

    void OnTriggerExit(Collider coll)
    {
        if(coll.tag == "Ground")
            on_ground = false;
    }

    // move forward and backword
    void Move(float axis)
    {
        if(!on_ground)
            return;
        transform.Translate(Vector3.forward * axis * move_speed * Time.deltaTime);
    }

    // turn left and right
    void Turn(float axis)
    {
        if(!on_ground)
            return;
        transform.Rotate(new Vector3(0, 1, 0) * axis * turn_speed * Time.deltaTime);
    }

    // turn left and rigt turret
    void TurretTurn(float axis)
    {
        turret.Rotate(new Vector3(0, 1, 0) * axis * turret_turn_speed * Time.deltaTime);
    }

    // rotation angle limit
    void AngleLimit()
    {
        if(180 > turret.localRotation.eulerAngles.y && turret.localRotation.eulerAngles.y > limit_angle)
            turret.localRotation = Quaternion.Euler(new Vector3(0, limit_angle, 0));
        if(180 < turret.localRotation.eulerAngles.y && turret.localRotation.eulerAngles.y < 360-limit_angle)
            turret.localRotation = Quaternion.Euler(new Vector3(0, -limit_angle, 0));
    }

    void Awake()
    {
        turret = transform.Find("TankTurret");
        idle = GameObject.Find("Idle_Player").GetComponent<AudioSource>();
        drive = GameObject.Find("Drive_Player").GetComponent<AudioSource>();
    }
    
    void Fire()
    {
        if(has_shell <= 0)
            return;
        if(t > 0)
            return;
        
        void ShellSpawn()
        {
            GameObject shell = Instantiate(Shell, ShellSpawnPoint.transform.position, ShellSpawnPoint.transform.rotation);
            Vector3 v = ShellSpawnPoint.transform.forward;
            v.Normalize();
            shell.GetComponent<Rigidbody>().AddForce(v * (shell_force + min_fire));
            shell_force = 0;
            has_shell--;
            t = reload_time;
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            ShellSpawn();
            return;
        }
        if(Input.GetKey(KeyCode.Space))
        {
            shell_force += add_fire * Time.deltaTime;
            if(shell_force > max_fire)
                ShellSpawn();
        }
    }

    void Update()
    {
        Fire();
        Turn(Input.GetAxis("Horizontal"));
        TurretTurn(Input.GetAxis("Horizontal_Turret"));
        Move(Input.GetAxis("Vertical"));
        AngleLimit();
        if(t > 0) t -= Time.deltaTime;
    }

    // add damage to tank, it implement by ITank
    public void AddDamage(float damage)
    {
        hp -= damage;
    }

    // get hp of tank, it implement by ITank
    public float GetHP()
    {
        return hp;
    }

    // get max hp of tank, it implement by ITank
    public float GetMaxHP()
    {
        return max_hp;
    }

    // tank die , it implement by ITank
    public void Die()
    {
        TankEventArgs args = new TankEventArgs();
        args.player_num = player_num;
        DieEvent(this, args);
    }

    public int GetHasShell()
    {
        return has_shell;
    }
    public int GetMaxShell()
    {
        return max_shell;
    }
    public float GetReloadTime()
    {
        return reload_time;
    }
    public float GetReloadingTime()
    {
        return t;
    }
    public float GetForce()
    {
        return shell_force;
    }
    public float GetMaxForce()
    {
        return max_fire;
    }
    public float GetTurretRelativeAngle()
    {
        return transform.eulerAngles.y - turret.transform.eulerAngles.y;
    }
}
