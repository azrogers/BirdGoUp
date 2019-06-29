using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : Singleton<AudioManager>
{
	public AudioClip[] WhooshClips;
	public AudioClip CollectClip;
	public float CollectTimeout = 0.5f;
	public int CollectMaxHits = 5;
	public float CollectPitchChange = 0.2f;
	
	private float _collectTimer = 0.0f;
	private int _collects = 0;
	private readonly List<AudioSource> _sourcePool = new List<AudioSource>();

	public void PlayWhoosh() => PlayRandom(WhooshClips);

	public void PlayCollect()
	{
		var source = GetSfxSource();
		source.pitch = 1.0f + CollectPitchChange * _collects;
		source.PlayOneShot(CollectClip);
		_collectTimer = CollectTimeout;
		_collects++;
		if(_collects >= CollectMaxHits)
		{
			_collects = CollectMaxHits;
		}
	}

	private void PlayRandom(AudioClip[] clips)
	{
		PlayOneShot(clips[Random.Range(0, clips.Length)]);
	}

	private void PlayOneShot(AudioClip clip)
	{
		var source = GetSfxSource();
		source.PlayOneShot(clip);
	}
	
	private AudioSource GetSfxSource()
	{
		var available = _sourcePool.Where(s => !s.isPlaying);
		var audioSources = available as AudioSource[] ?? available.ToArray();
		if(audioSources.Any())
		{
			return audioSources.First();
		}

		var ac = gameObject.AddComponent<AudioSource>();
		_sourcePool.Add(ac);
		return ac;
	}

	private void Update()
	{
		_collectTimer -= Time.deltaTime;
		if(_collectTimer <= 0)
		{
			_collects = 0;
		}
	}
}