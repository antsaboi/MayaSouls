using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats", menuName = "Scriptables/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Range(0,100)]
    public float HP = 100;
    [Range(0, 10)]
    public float HPReduceAmount = 0.5f;
    [Range(0, 5)]
    public float PowerUseHPReduceAmount = 0.25f;
    public float hpReduceInterval;
    public System.Action OnDeath;

    public void ResetStats()
    {
        HP = 100;
    }

    public float ReduceHP(float amount, bool usePower = false)
    {
        if (!IsAlive()) return 0;
        HP -= amount;
        return HP;
    }

    public float ReduceHPBySecondRate(bool usePower = false)
    {
        if (!IsAlive()) return 0;
        HP -= HPReduceAmount;
        return HP;
    }

    public float ReduceHPByPowerUse(bool usePower = false)
    {
        if (!IsAlive()) return 0;
        HP -= PowerUseHPReduceAmount;
        return HP;
    }

    public bool IsAlive()
    {
        if (HP <= 0)
        {
            if (!GameManager.instance.isAlive) return false;
            Debug.Log("DIE");
            GameManager.instance.GameOver();
            return false;
        }
        else {
            return true;
        }
    }
}
