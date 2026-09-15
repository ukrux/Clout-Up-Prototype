using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
    [SerializeField] private string mainScreenScene = "Main Screen";

    public void GoHome()
    {
        // Mark that the player has completed one match
        PlayerPrefs.SetInt("MatchPlayed", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(mainScreenScene);
    }
}