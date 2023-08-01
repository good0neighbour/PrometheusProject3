using System.IO;
using UnityEngine;

/// <summary>
/// 변경이 있을 때 호출할 대리자.
/// </summary>
public delegate void OnChangeDelegate();

/// <summary>
/// 전체적으로 프로그램을 관리
/// </summary>
public class GameManager
{
    private static GameManager _instance = null;

    private JsonSettings _userSettings = new JsonSettings(true);

    public static GameManager Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = new GameManager();
            }

            return _instance;
        }
    }

    public LanguageType CurrentLanguage
    {
        get
        {
            return _userSettings.UserLanguage;
        }
        set
        {
            _userSettings.UserLanguage = value;
        }
    }

    public bool IsThereSavedGame
    {
        get
        {
            return _userSettings.IsGameSaved;
        }
        set
        {
            _userSettings.IsGameSaved = value;
        }
    }

    public bool IsNewGame
    {
        get;
        set;
    }

    public bool IsGameWin
    {
        get;
        set;
    }

    public bool IsTechTreeInitialized
    {
        get;
        set;
    }

    public bool IsApplicationFirstStarted
    {
        get;
        set;
    }

    public string LatestSocietyName
    {
        get;
        set;
    }

    public string EndGameMessage
    {
        get;
        set;
    }

    public int TargetFrameRate
    {
        get
        {
            return _userSettings.TargetFrameRate;
        }
        set
        {
            _userSettings.TargetFrameRate = value;
        }
    }

    public float SoundVolume
    {
        get
        {
            return _userSettings.SoundVolume;
        }
        set
        {
            _userSettings.SoundVolume = value;
        }
    }

    public float AirMass
    {
        get;
        set;
    }

    public float WaterVolume
    {
        get;
        set;
    }

    public float CarbonRatio
    {
        get;
        set;
    }

    public float Radius
    {
        get;
        set;
    }

    public float Density
    {
        get;
        set;
    }

    public float Distance
    {
        get;
        set;
    }

    public ushort StartFund
    {
        get;
        set;
    }

    public byte StartResearch
    {
        get;
        set;
    }

    public byte StartResources
    {
        get;
        set;
    }


    public void SaveSettings()
    {
#if PLATFORM_STANDALONE_WIN
        File.WriteAllText($"{Application.dataPath}/Settings.Json", JsonUtility.ToJson(_userSettings, false));
#endif
#if PLATFORM_ANDROID
        File.WriteAllText($"{Application.persistentDataPath}/Settings.Json", JsonUtility.ToJson(_userSettings, false));
#endif
    }


    private GameManager()
    {
        try
        {
#if PLATFORM_STANDALONE_WIN
            _userSettings = JsonUtility.FromJson<JsonSettings>(File.ReadAllText($"{Application.dataPath}/Settings.Json"));
#endif
#if PLATFORM_ANDROID
            _userSettings = JsonUtility.FromJson<JsonSettings>(File.ReadAllText($"{Application.persistentDataPath}/Settings.Json"));
#endif
        }
        catch
        {
            _userSettings = new JsonSettings(true);
            SaveSettings();
        }
        IsApplicationFirstStarted = true;
    }



    /* ==================== Struct ==================== */

    private struct JsonSettings
    {
        public LanguageType UserLanguage;
        public int TargetFrameRate;
        public float SoundVolume;
        public bool IsGameSaved;

        public JsonSettings(bool initialize)
        {
            UserLanguage = LanguageType.End;
            TargetFrameRate = 60;
            SoundVolume = 1.0f;
            IsGameSaved = false;
        }
    }
}
