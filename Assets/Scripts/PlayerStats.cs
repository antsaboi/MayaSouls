using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats", menuName = "Scriptables/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Range(0,100)]
    public float HP = 100;
    [Range(0, 10)]
    public float HPReduceRatePerSecond = 0.5f;
    [Range(0, 5)]
    public float PowerUseHPReduceAmountPerSecond = 0.25f;
    public bool isAlive = true;
    public System.Action OnDeath, OnReset;
    public delegate void HPReduce(bool usePower);
    public HPReduce OnHPReduced;

    public void ResetStats()
    {
        HP = 100;
        isAlive = true;
        OnReset?.Invoke();
    }

    public float ReduceHP(float amount, bool usePower = false)
    {
        if (!IsAlive()) return 0;
        HP -= amount;
        OnHPReduced?.Invoke(usePower);
        return HP;
    }

    public float ReduceHPBySecondRate(bool usePower = false)
    {
        if (!IsAlive()) return 0;
        HP -= HPReduceRatePerSecond;
        OnHPReduced?.Invoke(usePower);
        return HP;
    }

    public float ReduceHPByPowerUse(bool usePower = false)
    {
        if (!IsAlive()) return 0;
        HP -= PowerUseHPReduceAmountPerSecond;
        OnHPReduced?.Invoke(usePower);
        return HP;
    }

    public bool IsAlive()
    {
        if (HP <= 0)
        {
            isAlive = false;
            OnDeath?.Invoke();
        }
        else {
            isAlive = true;
        }

        return isAlive;
    }
}
