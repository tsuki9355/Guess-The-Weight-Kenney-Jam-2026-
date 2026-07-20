using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    [Header("Levels, played in this order")]
    public List<LevelData> levels;

    [Header("Scenes")]
    public string titleSceneName = "TitleScreen";
    public string endingSceneName = "EndingScene";

    [Header("References")]
    public ShelfSpawner shelfSpawner;
    public ScalePan rightPan;
    public ScalePan leftPan;
    public TMP_InputField guessInput;
    public Button submitButton;
    public TMP_Text roundText;

    [Header("Result popup")]
    public GameObject resultPopup;
    public Image[] starIcons;
    public Sprite starFilled;
    public Sprite starEmpty;
    public Sprite starPerfect;
    public TMP_Text resultDetailsText;
    public GameObject nextLevelButton;

    [Header("Minimum stars required to unlock Next Level")]
    public int starsToAdvance = 2;

    [Header("Instruction popup")]
    public GameObject instructionPanel;
    public TypewriterText instructionTypewriter;

    [Header("Star thresholds (% error allowed)")]
    public float threeStarPct = 0.05f;
    public float twoStarPct = 0.15f;
    public float oneStarPct = 0.30f;

    [Header("Sound")]
    public AudioClip successChime;

    int currentLevelIndex = -1;
    int totalStars = 0;
    float mysteryWeight;
    AudioSource audioSource;

    bool roundActive;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        submitButton.onClick.AddListener(SubmitGuess);

        var (savedLevel, savedStars) = SaveSystem.Load();
        totalStars = savedStars;
        StartLevel(savedLevel);
    }

    // Called by the "Save" button in the settings panel.
    public void SaveGame()
    {
        SaveSystem.Save(currentLevelIndex, totalStars);
    }

    // Called by the "Exit To Title" button in the settings panel.
    public void ExitToTitle()
    {
        SceneManager.LoadScene(titleSceneName);
    }

    // Called by the instruction popup's close/dismiss button.
    public void CloseInstruction()
    {
        instructionPanel.SetActive(false);
    }

    void StartLevel(int index)
    {
        currentLevelIndex = index;
        resultPopup.SetActive(false);

        if (index >= levels.Count)
        {
            GoToEndingScene();
            return;
        }

        var level = levels[index];

        guessInput.text = "";

        rightPan.ClearPan();
        leftPan.ClearPan();
        mysteryWeight = shelfSpawner.LoadLevel(level);

        roundActive = true;
        roundText.text = $"Stage {index + 1}";

        if (level.showInstruction)
        {
            instructionPanel.SetActive(true);
            instructionTypewriter.StartTyping(level.instructionText);
        }
        else
        {
            instructionPanel.SetActive(false);
        }
    }

    void GoToEndingScene()
    {
        SaveSystem.Save(currentLevelIndex, totalStars);
        SceneManager.LoadScene(endingSceneName);
    }

    void SubmitGuess()
    {
        if (!roundActive)
            return;

        if (!float.TryParse(guessInput.text, out float guess))
            return;

        roundActive = false;

        bool isExact = Mathf.RoundToInt(guess) == Mathf.RoundToInt(mysteryWeight);

        float pctError = Mathf.Abs(guess - mysteryWeight) / mysteryWeight;
        int stars = pctError <= threeStarPct ? 3
                  : pctError <= twoStarPct ? 2
                  : pctError <= oneStarPct ? 1
                  : 0;
        totalStars += stars;

        ShowResultPopup(stars, guess, isExact);
    }

    void ShowResultPopup(int stars, float guess, bool isExact)
    {
        resultPopup.SetActive(true);

        Sprite filledSprite = isExact ? starPerfect : starFilled;
        for (int i = 0; i < starIcons.Length; i++)
            starIcons[i].sprite = i < stars ? filledSprite : starEmpty;

        resultDetailsText.text = isExact ? "Wow, you got the perfect weight, are you cheating?" : GetResultMessage(stars);

        if (stars >= 2 && successChime != null)
            audioSource.PlayOneShot(successChime);

        if (nextLevelButton != null)
            nextLevelButton.SetActive(stars >= starsToAdvance);
    }

    string GetResultMessage(int stars)
    {
        switch (stars)
        {
            case 0: return "Are you even trying?";
            case 1: return "I'm not sure what to say, at least you got a star?";
            case 2: return "Good but not quite yet.";
            case 3: return "You got it!";
            default: return "";
        }
    }

    public void RestartLevel()
    {
        StartLevel(currentLevelIndex);
    }
    public void NextLevel()
    {
        StartLevel(currentLevelIndex + 1);
    }
}
