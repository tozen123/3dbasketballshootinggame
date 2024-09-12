using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCustomizer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI spendableText;
    void Start()
    {
        spendableText.text = "Points Spendables: " + PlayerPrefs.GetInt("Player_Points").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
