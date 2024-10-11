using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class playerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI prompText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

   public void updateText(string promptMessage)
    {
        prompText.text = promptMessage;
    }
}
