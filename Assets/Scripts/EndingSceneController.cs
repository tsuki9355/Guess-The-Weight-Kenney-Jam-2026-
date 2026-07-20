using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndingSceneController : MonoBehaviour
{
    public TMP_Text starsText;
    public int maxPossibleStars = 45;
    public string titleSceneName = "TitleScreen";

    void Start()
    {
        var (_, totalStars) = SaveSystem.Load();
        starsText.text = $"You earned {totalStars} / {maxPossibleStars} stars!\n";
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene(0);
    }
}
