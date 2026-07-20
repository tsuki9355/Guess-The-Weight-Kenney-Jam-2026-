using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MenuNavigator : MonoBehaviour
{
    [Header("Menu items, top to bottom — must share a parent with the arrows")]
    public RectTransform[] menuItems;

    [Header("The two arrows that flank the selected item")]
    public RectTransform arrowLeft;
    public RectTransform arrowRight;
    public float arrowGap = 40f;

    [Header("Selected item visual cue")]
    public float selectedScale = 1.15f;

    [Header("Sound")]
    public AudioClip hoverSound;
    public AudioClip clickSound;

    [Header("Wiring")]
    public string gameplaySceneName = "SampleScene";
    public GameObject optionsPanel;

    int selectedIndex = -1; // -1 = nothing selected
    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        AudioListener.volume = PlayerPrefs.GetFloat("MasterVolume", 1f);

        // Application.Quit() is a no-op in the browser (WebGL can't force-close
        // itself) — hide the button there so it doesn't sit around doing nothing.
#if UNITY_WEBGL && !UNITY_EDITOR
        if (menuItems.Length > 2)
            menuItems[2].gameObject.SetActive(false);
#endif
    }

    void Start()
    {
        ClearSelection();
    }

    public void Hover(int index)
    {
        selectedIndex = index;
        ShowSelection();
        PlaySound(hoverSound);
    }

    public void ClearHover(int index)
    {
        if (selectedIndex == index)
            ClearSelection();
    }

    void ClearSelection()
    {
        selectedIndex = -1;

        foreach (var item in menuItems)
            item.localScale = Vector3.one;

        arrowLeft.gameObject.SetActive(false);
        arrowRight.gameObject.SetActive(false);
    }

    void ShowSelection()
    {
        for (int i = 0; i < menuItems.Length; i++)
            menuItems[i].localScale = Vector3.one * (i == selectedIndex ? selectedScale : 1f);

        arrowLeft.gameObject.SetActive(true);
        arrowRight.gameObject.SetActive(true);

        RectTransform target = menuItems[selectedIndex];
        Vector2 targetPos = target.anchoredPosition;
        float halfWidth = target.rect.width * selectedScale / 2f;

        arrowLeft.anchoredPosition = new Vector2(targetPos.x - halfWidth - arrowGap, targetPos.y);
        arrowRight.anchoredPosition = new Vector2(targetPos.x + halfWidth + arrowGap, targetPos.y);
    }

    public void Activate(int index)
    {
        PlaySound(clickSound);

        switch (index)
        {
            case 0: NewGame(); break;
            case 1: ContinueGame(); break;
            case 2: QuitGame(); break;
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }

    public void NewGame()
    {
        SaveSystem.ClearSave();
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
