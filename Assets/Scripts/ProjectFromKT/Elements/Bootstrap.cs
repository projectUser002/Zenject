using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip destructionSound;
    [SerializeField] private bool useJsonSaver = true;

    private void Awake()
    {
        InitializeServices();
    }

    private void InitializeServices()
    {
        var fadeService = new FadeService();
        ServiceLocator.Instance.RegisterService<IFadeService>(fadeService);

        var soundPlayer = new SoundPlayer(audioSource, openSound, closeSound, shootSound, destructionSound);
        ServiceLocator.Instance.RegisterService<ISoundPlayer>(soundPlayer);

        ISaver saver;
        if (useJsonSaver)
        {
            saver = new JsonSaver();
        }
        else
        {
            saver = new PlayerPrefsSaver();
        }
        ServiceLocator.Instance.RegisterService<ISaver>(saver);
    }
}