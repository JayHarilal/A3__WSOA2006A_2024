using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ammoManager : MonoBehaviour
{
    public static ammoManager Instance { get; set; }

    public TextMeshProUGUI ammoDisplay;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
