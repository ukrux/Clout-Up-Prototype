using UnityEngine;
using TMPro;

public class MainScreenLevel : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText;

    private void Start()
    {
        // Always starts at 1
        int level = PlayerPrefs.GetInt("MatchPlayed", 0) == 1 ? 2 : 1;

        levelText.text = level.ToString();
    }
}