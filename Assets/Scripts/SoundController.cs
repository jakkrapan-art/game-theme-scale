using System.Collections;
using UnityEngine;

public static class SoundController
{
  private static AudioSource soundSource;
  private static AudioSource bgmSource;

  private const float transitionDuration = 2.0f;

  private class SoundMonoBehaviour : MonoBehaviour {}

  private static AudioSource GetSoundSource()
  {
    if (!soundSource)
    {
      GameObject soundControllerObject = new GameObject("SoundController_Sound");
      soundSource = soundControllerObject.AddComponent<AudioSource>();
    }

    return soundSource;
  }

  private static AudioSource GetBgmSource()
  {
    if (!bgmSource)
    {
      GameObject soundControllerObject = new GameObject("SoundController_BGM");
      bgmSource = soundControllerObject.AddComponent<AudioSource>();
    }

    return bgmSource;
  }

  public static void PlaySound(AudioClip clip, float volumn = 1)
  {
    AudioSource source = GetSoundSource();
    source.clip = clip;
    source.volume = volumn;
    source.Play();
  }

  public static void StopSound()
  {
    AudioSource source = GetSoundSource();
    source.Stop();
  }

  public static void PlayBackgroundMusic(AudioClip bgmClip)
  {
    AudioSource source = GetBgmSource();
    source.clip = bgmClip;
    source.loop = true;
    source.Play();
  }

  public static void ChangeBackgroundMusic(AudioClip newBgmClip)
  {
    AudioSource oldBgmSource = GetBgmSource();
    AudioSource newBgmSource = GetBgmSource();

    newBgmSource.clip = newBgmClip;
    newBgmSource.volume = 0f;

    MonoBehaviour monoBehaviour = newBgmSource.gameObject.AddComponent<SoundMonoBehaviour>();
    monoBehaviour.StartCoroutine(TransitionBackgroundMusic(oldBgmSource, newBgmSource));
  }

  private static IEnumerator TransitionBackgroundMusic(AudioSource oldBgmSource, AudioSource newBgmSource)
  {
    float elapsedTime = 0f;

    while (elapsedTime < transitionDuration)
    {
      float t = elapsedTime / transitionDuration;

      oldBgmSource.volume = Mathf.Lerp(1f, 0f, t);
      newBgmSource.volume = Mathf.Lerp(0f, 1f, t);

      elapsedTime += Time.deltaTime;
      yield return null;
    }

    oldBgmSource.Stop();
    newBgmSource.volume = 1f;
    MonoBehaviour.Destroy(newBgmSource.gameObject.GetComponent<SoundMonoBehaviour>());
  }

  public static void StopBackgroundMusic()
  {
    AudioSource source = GetBgmSource();
    source.Stop();
  }
}