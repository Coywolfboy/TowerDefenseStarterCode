using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource menuMusic;
    public AudioSource gameMusic;

    public AudioClip[] uiSounds;
    public AudioClip[] towerSounds;
    public AudioClip[] fxSounds;

    public GameObject audioSourcePrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartMenuMusic()
    {
        menuMusic.Play();
        gameMusic.Stop();
    }

    public void StartGameMusic()
    {
        menuMusic.Stop();
        gameMusic.Play();
    }

    public void PlayUISound()
    {
        int index = Random.Range(0, uiSounds.Length);
        PlaySound(uiSounds[index]);
    }

    public void PlayTowerSound(TowerType towerType)
    {
        int index = (int)towerType;
        if (index >= 0 && index < towerSounds.Length)
        {
            PlaySound(towerSounds[index]);
        }
    }

    public void PlayFXSound(FXEventType eventType)
    {
        int index = (int)eventType;
        if (index >= 0 && index < fxSounds.Length)
        {
            PlaySound(fxSounds[index]);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        GameObject soundGameObject = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
        AudioSource audioSource = soundGameObject.GetComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.Play();

        Destroy(soundGameObject, clip.length);
    }
}


