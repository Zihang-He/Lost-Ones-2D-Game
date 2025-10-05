using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

public class BackgroundMusic : MonoBehaviour
{
	public static BackgroundMusic Instance;

	[Header("Music Settings")]
	public AudioClip musicClip; // Assign via Inspector, or leave empty to load from Resources
	[Range(0f, 1f)] public float volume = 0.5f;
	public bool autoLoadFromResources = true;
	public string resourcesPath = "Audio/Background"; // Will load from Assets/Resources/Audio/Background.*

	[Header("Art Folder Auto-Load (no Resources needed)")]
	public bool autoLoadFromArtFolder = true; // Uses Assets/Art/Normal BGM.* if no clip provided
	public string artFolderRelative = "Art"; // relative to Assets folder
	public string normalMusicBaseName = "Normal BGM"; // tries .wav then .mp3
	public string maniaMusicBaseName = "Mania BGM"; // tries .wav then .mp3

	private AudioSource audioSource;

	void Awake()
	{
		// Singleton to persist one music player across scenes
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(gameObject);

		// Ensure we have an AudioSource configured for looping music
		if (!TryGetComponent(out audioSource))
		{
			audioSource = gameObject.AddComponent<AudioSource>();
		}

		audioSource.loop = true;
		audioSource.playOnAwake = false;
		audioSource.spatialBlend = 0f; // 2D
		audioSource.volume = Mathf.Clamp01(volume);

		// Decide which clip to play (priority: Inspector -> Art folder -> Resources)
		if (musicClip == null && autoLoadFromArtFolder)
		{
			StartCoroutine(TryLoadFromArtFolderAndPlay());
			return;
		}
		if (musicClip == null && autoLoadFromResources)
		{
			musicClip = Resources.Load<AudioClip>(resourcesPath);
		}

		ApplyClipAndMaybePlay(musicClip);
	}

	// Public controls
	public void Play()
	{
		if (audioSource == null || audioSource.clip == null) return;
		if (!audioSource.isPlaying) audioSource.Play();
	}

	public void Stop()
	{
		if (audioSource == null) return;
		audioSource.Stop();
	}

	public void SetVolume(float newVolume)
	{
		volume = Mathf.Clamp01(newVolume);
		if (audioSource != null) audioSource.volume = volume;
	}

	private void ApplyClipAndMaybePlay(AudioClip clip)
	{
		audioSource.clip = clip;
		if (audioSource.clip != null)
		{
			if (!audioSource.isPlaying)
			{
				audioSource.Play();
			}
		}
		else
		{
			Debug.LogWarning("BackgroundMusic: No AudioClip assigned or found. Assign a clip in the Inspector, place one in Assets/" + artFolderRelative + ", or Assets/Resources/" + resourcesPath + ".");
		}
	}

	private Coroutine loadRoutine;

	private IEnumerator TryLoadFromArtFolderAndPlay()
	{
		// Default to normal track
		yield return TryLoadFromArtFolderAndPlay(normalMusicBaseName);
	}

	private IEnumerator TryLoadFromArtFolderAndPlay(string baseName)
	{
		// Build absolute paths for WAV then MP3 using provided base name
		string assetsPath = Application.dataPath; // .../My project/Assets
		string folderPath = Path.Combine(assetsPath, artFolderRelative);
		string wavPath = Path.Combine(folderPath, baseName + ".wav");
		string mp3Path = Path.Combine(folderPath, baseName + ".mp3");

		AudioClip loaded = null;

		if (File.Exists(wavPath))
		{
			string url = "file://" + wavPath;
			yield return StartCoroutine(LoadClipFromFile(url, AudioType.WAV, (c) => loaded = c));
		}
		else if (File.Exists(mp3Path))
		{
			string url = "file://" + mp3Path;
			yield return StartCoroutine(LoadClipFromFile(url, AudioType.MPEG, (c) => loaded = c));
		}

		if (loaded == null && autoLoadFromResources)
		{
			loaded = Resources.Load<AudioClip>(resourcesPath);
		}

		musicClip = loaded;
		ApplyClipAndMaybePlay(musicClip);
	}

	private IEnumerator LoadClipFromFile(string url, AudioType audioType, System.Action<AudioClip> onLoaded)
	{
		using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, audioType))
		{
			yield return www.SendWebRequest();

			if (www.result != UnityWebRequest.Result.Success)
			{
				Debug.LogWarning("BackgroundMusic: Failed to load clip from " + url + " - " + www.error);
				onLoaded?.Invoke(null);
			}
			else
			{
				var clip = DownloadHandlerAudioClip.GetContent(www);
				onLoaded?.Invoke(clip);
			}
		}
	}

	// Public switching helpers
	public void SwitchToNormal()
	{
		if (!autoLoadFromArtFolder)
		{
			Debug.LogWarning("BackgroundMusic: autoLoadFromArtFolder is disabled; cannot auto-switch.");
			return;
		}
		if (loadRoutine != null) StopCoroutine(loadRoutine);
		loadRoutine = StartCoroutine(TryLoadFromArtFolderAndPlay(normalMusicBaseName));
	}

	public void SwitchToMania()
	{
		if (!autoLoadFromArtFolder)
		{
			Debug.LogWarning("BackgroundMusic: autoLoadFromArtFolder is disabled; cannot auto-switch.");
			return;
		}
		if (loadRoutine != null) StopCoroutine(loadRoutine);
		loadRoutine = StartCoroutine(TryLoadFromArtFolderAndPlay(maniaMusicBaseName));
	}

	// Auto-bootstrap if user forgets to add this to a scene
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	private static void EnsureInstance()
	{
		if (Instance != null) return;
		var go = new GameObject("BackgroundMusic");
		go.AddComponent<BackgroundMusic>();
	}
}


