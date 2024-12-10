using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponStockData
{
    public GameObject weaponPrefab;
    public int initialAmmoCount;
    public int maxAmmoCount;
    public int addAmmoCount;

    private int currentAmmoCount;

    public int CurrentAmmoCount
    {
        get { return currentAmmoCount; }
    }

    public void Initialize()
    {
        currentAmmoCount = initialAmmoCount;
    }

    public void AddAmmo()
    {
        currentAmmoCount += addAmmoCount;
        if (currentAmmoCount > maxAmmoCount)
        {
            currentAmmoCount = maxAmmoCount;
        }
    }

    public void UseAmmo()
    {
        currentAmmoCount--;
        if (currentAmmoCount < 0)
        {
            currentAmmoCount = 0;
        }
    }
}
