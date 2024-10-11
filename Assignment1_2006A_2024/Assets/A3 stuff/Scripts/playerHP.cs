using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class playerHP : MonoBehaviour
{
    private float HP;
    private float lerpTimer;
    public float maxHP = 100f;
    private float chipSpeed = 3f;
    public Image frontHP;
    public Image backHP;
    public TextMeshProUGUI HPText;

    void Start()
    {
        HP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        HP = Mathf.Clamp(HP, 0, maxHP);
        UpdateHPUI();
    }
    public void UpdateHPUI()
    {
        HPText.text = Mathf.Round(HP).ToString() + " / " + Mathf.Round(maxHP).ToString();
        float fillHP = frontHP.fillAmount;
        float fillBHP = backHP.fillAmount;
        float HPfraction = HP / maxHP;
        if (fillBHP > HPfraction)
        {
            frontHP.fillAmount = HPfraction;
            backHP.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentAmount = lerpTimer / chipSpeed;
            percentAmount = percentAmount * percentAmount;
            backHP.fillAmount = Mathf.Lerp(fillBHP, HPfraction, percentAmount);

        }
        if (fillBHP<HPfraction)
        {
            backHP.color = Color.green;
            backHP.fillAmount = HPfraction;
            lerpTimer += Time.deltaTime;
            float percentAmount = lerpTimer / chipSpeed;
            percentAmount = percentAmount * percentAmount;
            frontHP.fillAmount = Mathf.Lerp(fillHP, backHP.fillAmount, percentAmount);
        }
    }

    public void Damage(float damage)
    {
        HP -= damage;
        lerpTimer = 0f;
    }

    public void Heal(float heal)
    {
        HP += heal;
        lerpTimer = 0f;
    }


}
