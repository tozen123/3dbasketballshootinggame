using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCustomizer : MonoBehaviour
{
    [Header("Points Spendables")]
    [SerializeField] private int startingPoints;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI spendableText;
    [SerializeField] private PlayerSkinManager skinManager;

    [Header("Skin References")]
    public Button greenShirt;
    public Button greenShorts;
    public Button greenShoes;

    public Button redShirt;
    public Button redShorts;
    public Button redShoes;

    public Button yellowShirt;
    public Button yellowShorts;
    public Button yellowShoes;

    public TextMeshProUGUI greenShirtText;
    public TextMeshProUGUI greenShortsText;
    public TextMeshProUGUI greenShoesText;

    public TextMeshProUGUI redShirtText;
    public TextMeshProUGUI redShortsText;
    public TextMeshProUGUI redShoesText;

    public TextMeshProUGUI yellowShirtText;
    public TextMeshProUGUI yellowShortsText;
    public TextMeshProUGUI yellowShoesText;

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

    private string equippedShirt;
    private string equippedShorts;
    private string equippedShoes;

    void Start()
    {
        playerPoints = PlayerPrefs.GetInt("Player_Points", startingPoints);

        equippedShirt = PlayerPrefs.GetString("Equipped_Shirt", "Green");
        equippedShorts = PlayerPrefs.GetString("Equipped_Shorts", "Green");
        equippedShoes = PlayerPrefs.GetString("Equipped_Shoes", "Green");

        SetDefaultUnlock("Shirt_Green");
        SetDefaultUnlock("Shorts_Green");
        SetDefaultUnlock("Shoes_Green");

        UpdateSpendableText();

        greenShirt.onClick.AddListener(() => UnlockAndEquipSkin("Shirt", "Green", greenShirtText));
        greenShorts.onClick.AddListener(() => UnlockAndEquipSkin("Shorts", "Green", greenShortsText));
        greenShoes.onClick.AddListener(() => UnlockAndEquipSkin("Shoes", "Green", greenShoesText));

        redShirt.onClick.AddListener(() => UnlockAndEquipSkin("Shirt", "Red", redShirtText));
        redShorts.onClick.AddListener(() => UnlockAndEquipSkin("Shorts", "Red", redShortsText));
        redShoes.onClick.AddListener(() => UnlockAndEquipSkin("Shoes", "Red", redShoesText));

        yellowShirt.onClick.AddListener(() => UnlockAndEquipSkin("Shirt", "Yellow", yellowShirtText));
        yellowShorts.onClick.AddListener(() => UnlockAndEquipSkin("Shorts", "Yellow", yellowShortsText));
        yellowShoes.onClick.AddListener(() => UnlockAndEquipSkin("Shoes", "Yellow", yellowShoesText));

        UpdateButtonsText();
    }

    private void UpdateSpendableText()
    {
        spendableText.text = "Points Spendables: " + playerPoints.ToString();
    }

    private void UnlockAndEquipSkin(string itemType, string color, TextMeshProUGUI buttonText)
    {
        ButtonSoundController.Instance.PlayButtonSound();

        string key = itemType + "_" + color + "_Unlocked";
        bool isUnlocked = PlayerPrefs.GetInt(key, 0) == 1;

        string costKey = itemType + "_" + color;
        int cost = itemCosts[costKey];

        if (isUnlocked)
        {
            // If the item is already unlocked, directly equip it without buying
            EquipSkin(itemType, color);
            buttonText.text = "EQUIPPED";
        }
        else
        {
            // Item is not unlocked, show dialog to buy
            if (playerPoints >= cost)
            {
                // Show the "Buy and Wear" or "Buy Only" options
                WindowActionDialogSystem.Instance.ShowWearOrBuyDialog(
                    itemType,
                    color,
                    cost,
                    onWear: () =>
                    {
                        // Deduct points, unlock, and equip the item immediately
                        playerPoints -= cost;
                        PlayerPrefs.SetInt("Player_Points", playerPoints);
                        PlayerPrefs.SetInt(key, 1);
                        PlayerPrefs.Save();

                        EquipSkin(itemType, color);
                        UpdateSpendableText();
                        buttonText.text = "EQUIPPED";
                    },
                    onBuy: () =>
                    {
                        // Deduct points and unlock the item without equipping it
                        playerPoints -= cost;
                        PlayerPrefs.SetInt("Player_Points", playerPoints);
                        PlayerPrefs.SetInt(key, 1);
                        PlayerPrefs.Save();

                        UpdateSpendableText();
                        buttonText.text = "BOUGHT";
                    }
                   
                );
            }
            else
            {
                // Insufficient points, show message using WindowDialogSystem
                WindowDialogSystem.Instance
                    .SetTitle("Insufficient Points")
                    .SetMessage("You do not have enough points to buy this item.")
                    .OnClick(() => WindowDialogSystem.Instance.Hide())
                    .Show();
            }
        }
    }

    private void EquipSkin(string itemType, string color)
    {
        ButtonSoundController.Instance.PlayButtonSound();

        switch (itemType)
        {
            case "Shirt":
                if (color == "Green")
                    skinManager.SetShirtToGreen();
                else if (color == "Red")
                    skinManager.SetShirtToRed();
                else if (color == "Yellow")
                    skinManager.SetShirtToYellow();

                equippedShirt = color;
                PlayerPrefs.SetString("Equipped_Shirt", color);
                break;

            case "Shorts":
                if (color == "Green")
                    skinManager.SetShortsToGreen();
                else if (color == "Red")
                    skinManager.SetShortsToRed();
                else if (color == "Yellow")
                    skinManager.SetShortsToYellow();

                equippedShorts = color;
                PlayerPrefs.SetString("Equipped_Shorts", color);
                break;

            case "Shoes":
                if (color == "Green")
                    skinManager.SetShoesToGreen();
                else if (color == "Red")
                    skinManager.SetShoesToRed();
                else if (color == "Yellow")
                    skinManager.SetShoesToYellow();

                equippedShoes = color;
                PlayerPrefs.SetString("Equipped_Shoes", color);
                break;
        }

        PlayerPrefs.Save();
        UpdateButtonsText();
    }

    private void UpdateButtonsText()
    {
        UpdateButtonText("Shirt", "Green", greenShirtText, equippedShirt);
        UpdateButtonText("Shirt", "Red", redShirtText, equippedShirt);
        UpdateButtonText("Shirt", "Yellow", yellowShirtText, equippedShirt);

        UpdateButtonText("Shorts", "Green", greenShortsText, equippedShorts);
        UpdateButtonText("Shorts", "Red", redShortsText, equippedShorts);
        UpdateButtonText("Shorts", "Yellow", yellowShortsText, equippedShorts);

        UpdateButtonText("Shoes", "Green", greenShoesText, equippedShoes);
        UpdateButtonText("Shoes", "Red", redShoesText, equippedShoes);
        UpdateButtonText("Shoes", "Yellow", yellowShoesText, equippedShoes);
    }

    private void UpdateButtonText(string itemType, string color, TextMeshProUGUI buttonText, string equippedItem)
    {
        string key = itemType + "_" + color + "_Unlocked";
        bool isUnlocked = PlayerPrefs.GetInt(key, 0) == 1;

        if (color == equippedItem)
        {
            buttonText.text = "EQUIPPED";
        }
        else if (isUnlocked)
        {
            buttonText.text = "BOUGHT";
        }
        else
        {
            int cost = itemCosts[itemType + "_" + color];
            buttonText.text = cost == 0 ? "FREE" : cost.ToString();
        }
    }

    private void SetDefaultUnlock(string itemKey)
    {
        if (PlayerPrefs.GetInt(itemKey + "_Unlocked", 0) == 0)
        {
            PlayerPrefs.SetInt(itemKey + "_Unlocked", 1);
            PlayerPrefs.Save();
        }
    }
}
