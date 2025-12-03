using UnityEngine;

public interface ISoundPlayer
{
    void PlayOpenSound();
    void PlayCloseSound();
    void PlayShootSound();
    void PlayDestructionSound();
}

public class SoundPlayer : ISoundPlayer
{
    private readonly AudioSource _audioSource;
    private readonly AudioClip _openSound;
    private readonly AudioClip _closeSound;
    private readonly AudioClip _shootSound;
    private readonly AudioClip _destructionSound;

    public SoundPlayer(AudioSource audioSource, AudioClip openSound, AudioClip closeSound,
                      AudioClip shootSound, AudioClip destructionSound)
    {
        _audioSource = audioSource;
        _openSound = openSound;
        _closeSound = closeSound;
        _shootSound = shootSound;
        _destructionSound = destructionSound;
    }

    public void PlayOpenSound()
    {
        if (_openSound != null)
            _audioSource.PlayOneShot(_openSound);
    }

    public void PlayCloseSound()
    {
        if (_closeSound != null)
            _audioSource.PlayOneShot(_closeSound);
    }
    
    public void PlayShootSound()
    {
        if (_shootSound != null)
            _audioSource.PlayOneShot(_shootSound);
    }
    
    public void PlayDestructionSound()
    {
        if (_destructionSound != null)
            _audioSource.PlayOneShot(_destructionSound);
    }
}