using UnityEngine;
using TMPro;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;

    [SerializeField] private GameObject dialogPanel;  
    [SerializeField] private TextMeshProUGUI dialogText;  

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowDialog(string message)
    {
        dialogPanel.SetActive(true);
        dialogText.text = message;
    }

    public void HideDialog()
    {
        dialogPanel.SetActive(false);
    }
}
