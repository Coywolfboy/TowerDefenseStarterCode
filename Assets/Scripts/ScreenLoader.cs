using UnityEngine;

public class ScreenLoader : MonoBehaviour
{
    public bool MenuScene;

    private void Start()
    {
        SoundManager soundManager = SoundManager.Instance;
        if (soundManager == null)
        {
            Debug.LogError("SoundManager instance not found!");
            return;
        }

        if (MenuScene)
        {
            soundManager.StartMenuMusic();
        }
        else
        {
            soundManager.StartGameMusic();
        }
    }
}
