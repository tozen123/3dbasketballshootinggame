using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCustomizer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI spendableText;
    [SerializeField] private PlayerSkinManager skinManager;

    public Button greenShirt;
    public Button greenShorts;
    public Button greenShoes;

    public Button redShirt;
    public Button redShorts;
    public Button redShoes;

    public Button yellowShirt;
    public Button yellowShorts;
    public Button yellowShoes;

    private int playerPoints;
    private Dictionary<string, int> itemCosts = new Dictionary<string, int>()
    {
        { "Shirt_Green", 0 },
        { "Shirt_Red", 100 },
        { "Shirt_Yellow", 250 },
        { "Shorts_Green", 0 },
        { "Shorts_Red", 150 },
        { "Shorts_Yellow", 350 },
        { "Shoes_Green", 0 },
        { "Shoes_Red", 250 },
        { "Shoes_Yellow", 400 }
    };

    void Start()
    {
        playerPoints = PlayerPrefs.GetInt("Player_Points", 500); 
        UpdateSpendableText();

        // Set up button listeners
        greenShirt.onClick.AddListener(() => UnlockAndEquipSkin("Shirt", "Green"));
        greenShorts.onClick.AddListener(() => UnlockAndEquipSkin("Shorts", "Green"));
        greenShoes.onClick.AddListener(() => UnlockAndEquipSkin("Shoes", "Green"));

        redShirt.onClick.AddListener(() => UnlockAndEquipSkin("Shirt", "Red"));
        redShorts.onClick.AddListener(() => UnlockAndEquipSkin("Shorts", "Red"));
        redShoes.onClick.AddListener(() => UnlockAndEquipSkin("Shoes", "Red"));

        yellowShirt.onClick.AddListener(() => UnlockAndEquipSkin("Shirt", "Yellow"));
        yellowShorts.onClick.AddListener(() => UnlockAndEquipSkin("Shorts", "Yellow"));
        yellowShoes.onClick.AddListener(() => UnlockAndEquipSkin("Shoes", "Yellow"));
    }

    private void UpdateSpendableText()
    {
        spendableText.text = "Points Spendables: " + playerPoints.ToString();
    }

    private void UnlockAndEquipSkin(string itemType, string color)
    {
        string key = itemType + "_" + color + "_Unlocked";
        bool isUnlocked = PlayerPrefs.GetInt(key, 0) == 1;

        string costKey = itemType + "_" + color;
        int cost = itemCosts[costKey];

        if (isUnlocked)
        {
            EquipSkin(itemType, color); 
        }
        else if (playerPoints >= cost)
        {
            playerPoints -= cost;
            PlayerPrefs.SetInt("Player_Points", playerPoints);
            PlayerPrefs.SetInt(key, 1); 
            PlayerPrefs.Save();

            EquipSkin(itemType, color);
            UpdateSpendableText();
        }
        else
        {
            Debug.Log("Not enough points to unlock this skin.");
        }
    }

    private void EquipSkin(string itemType, string color)
    {
        switch (itemType)
        {
            case "Shirt":
                if (color == "Green")
                    skinManager.SetShirtToGreen();
                else if (color == "Red")
                    skinManager.SetShirtToRed();
                else if (color == "Yellow")
                    skinManager.SetShirtToYellow();
                break;
            case "Shorts":
                if (color == "Green")
                    skinManager.SetShortsToGreen();
                else if (color == "Red")
                    skinManager.SetShortsToRed();
                else if (color == "Yellow")
                    skinManager.SetShortsToYellow();
                break;
            case "Shoes":
                if (color == "Green")
                    skinManager.SetShoesToGreen();
                else if (color == "Red")
                    skinManager.SetShoesToRed();
                else if (color == "Yellow")
                    skinManager.SetShoesToYellow();
                break;
        }
    }

  
}
