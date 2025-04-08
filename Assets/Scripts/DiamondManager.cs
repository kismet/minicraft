using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class DiamondManager : MonoBehaviour
{

    // Flag per i diamanti
    public bool DeserDiamond = true;
    public bool ForestDiamond = true;
    public bool MesaDiamond = true;
    public bool MountainDiamond = true;




    public static DiamondManager Instance { get; private set; }

    [Header("UI Reference")]
    [SerializeField] private Text diamondText; // Assegna MANUALMENTE in ogni scena

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
    }

    // Chiamalo all'inizio di ogni scena (es. da uno script sul testo UI)
    public void AssignUIText(Text targetText)
    {
        diamondText = targetText;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (diamondText != null)
            diamondText.text = diamonds.ToString(); // Solo il numero
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

    public void AddDiamond() => Diamonds++;
    public void ResetDiamonds() => Diamonds = 0;
    private void LoadDiamonds() => Diamonds = PlayerPrefs.GetInt("DiamondCount", 0);
}