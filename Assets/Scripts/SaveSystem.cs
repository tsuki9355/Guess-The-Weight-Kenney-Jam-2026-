using UnityEngine;

public static class SaveSystem
{
    const string LevelKey = "Save_LevelIndex";
    const string StarsKey = "Save_TotalStars";
    const string HasSaveKey = "Save_Exists";

    public static bool HasSave => PlayerPrefs.GetInt(HasSaveKey, 0) == 1;

    public static void Save(int levelIndex, int totalStars)
    {
        PlayerPrefs.SetInt(LevelKey, levelIndex);
        PlayerPrefs.SetInt(StarsKey, totalStars);
        PlayerPrefs.SetInt(HasSaveKey, 1);
        PlayerPrefs.Save();
    }

    public static (int levelIndex, int totalStars) Load()
    {
        return (PlayerPrefs.GetInt(LevelKey, 0), PlayerPrefs.GetInt(StarsKey, 0));
    }

    public static void ClearSave()
    {
        PlayerPrefs.DeleteKey(LevelKey);
        PlayerPrefs.DeleteKey(StarsKey);
        PlayerPrefs.DeleteKey(HasSaveKey);
    }
}
