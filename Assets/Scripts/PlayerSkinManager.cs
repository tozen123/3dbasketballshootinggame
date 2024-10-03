using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinManager : MonoBehaviour
{

    public Renderer playerShirtMaterial;
    public Renderer playerShortsMaterial;
    public Renderer playerShoesMaterial;


    [Header("Green")]
    public Texture2D green_jersey_shirt;
    public Texture2D green_jersey_short;
    public Texture2D green_jersey_shoes;

    [Header("Red")]
    public Texture2D red_jersey_shirt;
    public Texture2D red_jersey_short;
    public Texture2D red_jersey_shoes;

    [Header("Yellow")]
    public Texture2D yellow_jersey_shirt;
    public Texture2D yellow_jersey_short;
    public Texture2D yellow_jersey_shoes;

    private void Start()
    {
        LoadSkin();
    }

    public void SaveSkin(string part, string color)
    {
        PlayerPrefs.SetString(part, color);
        PlayerPrefs.Save();
    }

    public void LoadSkin()
    {
        string savedShirtColor = PlayerPrefs.GetString("PlayerShirtColor", "Green"); 
        string savedShortsColor = PlayerPrefs.GetString("PlayerShortsColor", "Green"); 
        string savedShoesColor = PlayerPrefs.GetString("PlayerShoesColor", "Green"); 

        SetShirt(savedShirtColor);
        SetShorts(savedShortsColor);
        SetShoes(savedShoesColor);
    }

    public void SetShirt(string color)
    {
        switch (color)
        {
            case "Red":
                playerShirtMaterial.material.mainTexture = red_jersey_shirt;
                break;
            case "Yellow":
                playerShirtMaterial.material.mainTexture = yellow_jersey_shirt;
                break;
            default:
                playerShirtMaterial.material.mainTexture = green_jersey_shirt;
                break;
        }

        SaveSkin("PlayerShirtColor", color);
    }

    public void SetShorts(string color)
    {
        switch (color)
        {
            case "Red":
                playerShortsMaterial.material.mainTexture = red_jersey_short;
                break;
            case "Yellow":
                playerShortsMaterial.material.mainTexture = yellow_jersey_short;
                break;
            default:
                playerShortsMaterial.material.mainTexture = green_jersey_short;
                break;
        }

        SaveSkin("PlayerShortsColor", color);
    }

    public void SetShoes(string color)
    {
        switch (color)
        {
            case "Red":
                playerShoesMaterial.material.mainTexture = red_jersey_shoes;
                break;
            case "Yellow":
                playerShoesMaterial.material.mainTexture = yellow_jersey_shoes;
                break;
            default:
                playerShoesMaterial.material.mainTexture = green_jersey_shoes;
                break;
        }

        SaveSkin("PlayerShoesColor", color);
    }

    public void SetShirtToRed()
    {
        SetShirt("Red");
    }

    public void SetShirtToGreen()
    {
        SetShirt("Green");
    }

    public void SetShirtToYellow()
    {
        SetShirt("Yellow");
    }

    public void SetShortsToRed()
    {
        SetShorts("Red");
    }

    public void SetShortsToGreen()
    {
        SetShorts("Green");
    }

    public void SetShortsToYellow()
    {
        SetShorts("Yellow");
    }

    public void SetShoesToRed()
    {
        SetShoes("Red");
    }

    public void SetShoesToGreen()
    {
        SetShoes("Green");
    }

    public void SetShoesToYellow()
    {
        SetShoes("Yellow");
    }

}
