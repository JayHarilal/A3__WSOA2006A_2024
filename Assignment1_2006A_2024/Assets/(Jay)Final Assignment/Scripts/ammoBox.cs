using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoBox : MonoBehaviour
{
    public int ammoAmount = 200;
    public AmmoType ammoType;

    public enum AmmoType
    {
        rifleAmmo,
        pistolAmmo
    }
}
