using UnityEngine;

namespace Audio
{
	public class AudioManager : MonoBehaviour
	{
		public Sound[] sounds;

		public bool muted;

		public bool muteMusic = true;

		private float volume;

		public static AudioManager Instance
		{
			get;
			set;
		}

		private void Start()
		{
			volume = 1f;
			Instance = this;
			Sound[] array = sounds;
			foreach (Sound sound in array)
			{
				sound.source = base.gameObject.AddComponent<AudioSource>();
				sound.source.clip = sound.clip;
				sound.source.loop = sound.loop;
				sound.source.volume = sound.volume;
				sound.source.pitch = sound.pitch;
				sound.source.bypassListenerEffects = sound.bypass;
			}
			Play("Song");
			SetVolumeMusic(0.4f);
		}

		public void MuteSounds()
		{
			muted = !muted;
		}

		public void MuteMusic()
		{
			muteMusic = !muteMusic;
			float num = (!muteMusic) ? 1f : 0f;
			Sound[] array = sounds;
			int num2 = 0;
			Sound sound;
			while (true)
			{
				if (num2 < array.Length)
				{
					sound = array[num2];
					if (sound.name == "Song")
					{
						break;
					}
					num2++;
					continue;
				}
				return;
			}
			sound.source.volume = num;
		}

		public void SetVolumeMusic(float v)
		{
			Sound[] array = sounds;
			int num = 0;
			Sound sound;
			while (true)
			{
				if (num < array.Length)
				{
					sound = array[num];
					if (sound.name == "Song")
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			sound.source.volume = v;
		}

		public void SetVolumeSfx(float v)
		{
			volume = v;
		}

		public void UnmuteMusic()
		{
			Sound[] array = sounds;
			int num = 0;
			Sound sound;
			while (true)
			{
				if (num < array.Length)
				{
					sound = array[num];
					if (sound.name == "Song")
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			sound.source.volume = 1.15f;
		}

		public void Play(string n)
		{
			if (muted && n != "Song")
			{
				return;
			}
			Sound[] array = sounds;
			int num = 0;
			Sound sound;
			while (true)
			{
				if (num < array.Length)
				{
					sound = array[num];
					if (sound.name == n)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			sound.source.Play();
			sound.source.volume = volume;
		}

		public void Play(string n, float v)
		{
			if (muted && n != "Song")
			{
				return;
			}
			Sound[] array = sounds;
			int num = 0;
			Sound sound;
			while (true)
			{
				if (num < array.Length)
				{
					sound = array[num];
					if (sound.name == n)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			float volume2 = sound.source.volume;
			sound.source.volume = v;
			sound.source.Play();
		}

		public void Stop(string n)
		{
			Sound[] array = sounds;
			int num = 0;
			Sound sound;
			while (true)
			{
				if (num < array.Length)
				{
					sound = array[num];
					if (sound.name == n)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			sound.source.Stop();
		}
	}
}
