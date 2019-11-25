using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITank{
    void AddDamage(float damage);
    float GetHP();
    float GetMaxHP();
    void Die();
    int GetHasShell();
    int GetMaxShell();
    float GetReloadTime();
    float GetReloadingTime();
    float GetMaxForce();
    float GetForce();
    float GetTurretRelativeAngle();
}
