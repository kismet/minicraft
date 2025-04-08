using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DiamondManager : MonoBehaviour
{
    // Flag per i diamanti
    public bool DesertDiamond = true;
    public bool ForestDiamond = true;
    public bool MesaDiamond = true;
    public bool MountainDiamond = true;

    public static DiamondManager Instance { get; private set; }

    [Header("UI Reference")]
    [SerializeField] private Text diamondText;

    private int diamonds;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadDiamonds();
        LoadDiamondFlags();
    }

    public void AssignUIText(Text targetText)
    {
        diamondText = targetText;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (diamondText != null)
            diamondText.text = diamonds.ToString();
    }

    public int Diamonds
    {
        get => diamonds;
        private set
        {
            diamonds = value;
            UpdateUI();
            PlayerPrefs.SetInt("DiamondCount", diamonds);
        }
    }

    public void AddDiamond()
    {
        Diamonds++;
        
        // Imposta il flag della scena corrente a false
        string currentScene = SceneManager.GetActiveScene().name;
        switch (currentScene)
        {
            case "Desert":
                DesertDiamond = false;
                PlayerPrefs.SetInt("DesertDiamond", 0);
                break;
            case "Forest":
                ForestDiamond = false;
                PlayerPrefs.SetInt("ForestDiamond", 0);
                break;
            case "Mesa":
                MesaDiamond = false;
                PlayerPrefs.SetInt("MesaDiamond", 0);
                break;
            case "Mountain":
                MountainDiamond = false;
                PlayerPrefs.SetInt("MountainDiamond", 0);
                break;
        }
    }

    public void ResetDiamonds() => Diamonds = 0;

    private void LoadDiamonds() => Diamonds = PlayerPrefs.GetInt("DiamondCount", 0);

    public void ResetAllDiamondFlags()
    {
        DesertDiamond = true;
        ForestDiamond = true;
        MesaDiamond = true;
        MountainDiamond = true;

        PlayerPrefs.SetInt("DesertDiamond", 1);
        PlayerPrefs.SetInt("ForestDiamond", 1);
        PlayerPrefs.SetInt("MesaDiamond", 1);
        PlayerPrefs.SetInt("MountainDiamond", 1);
    }

    private void LoadDiamondFlags()
    {
        DesertDiamond = PlayerPrefs.GetInt("DesertDiamond", 1) == 1;
        ForestDiamond = PlayerPrefs.GetInt("ForestDiamond", 1) == 1;
        MesaDiamond = PlayerPrefs.GetInt("MesaDiamond", 1) == 1;
        MountainDiamond = PlayerPrefs.GetInt("MountainDiamond", 1) == 1;
    }
}