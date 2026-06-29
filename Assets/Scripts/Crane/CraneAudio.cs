using UnityEngine;

public class CraneAudio : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip pickupSound;
    public AudioClip moveUpSound;
    public AudioClip moveDownSound;
    public AudioClip moveSound;
    public AudioClip TruckMoveSound;
    public AudioClip TruckLoadSound;
    public AudioClip ButtonClick;


    public void Pickup()
    {
        audioSource.PlayOneShot(pickupSound);
    }

    public void MoveUp()
    {
        if (audioSource.isPlaying && audioSource.clip == moveUpSound) return;

        audioSource.clip = moveUpSound;
        audioSource.loop = true;
        audioSource.Play();
    }

    void OnDisable()
    {
        audioSource.enabled=false;
    }
    void OnEnable()
    {
        audioSource.enabled=true;
    }

    public void MoveDown()
    {
        if (audioSource.isPlaying && audioSource.clip == moveDownSound) return;

        audioSource.clip = moveDownSound;
        audioSource.loop = true;
        audioSource.Play();
    }
    public void Move()
    {
        if (audioSource.isPlaying && audioSource.clip == moveSound) return;

        audioSource.clip = moveSound;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }
    public void PlayTruckMoveSound()
    {
        audioSource.PlayOneShot(TruckMoveSound);
    }
    public void PlayTruckLoadSound()
    {
        audioSource.PlayOneShot(TruckLoadSound);
    }
    public void PlayButtonClickSound()
    {
        audioSource.PlayOneShot(ButtonClick);
    }

}