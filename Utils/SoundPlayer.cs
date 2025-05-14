using Haraka.Utils;
using NAudio.Vorbis;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

public class SoundPlayer : IDisposable
{
    private readonly Dictionary<string, string> _supportedSounds = new()
    {
        {HarakaConstants.ENABLE_HARAKA_NOTIFICATION_SOUND, HarakaConstants.ENABLE_HARAKA_NOTIFICATION_SOUND_PATH},
        {HarakaConstants.DISABLE_HARAKA_NOTIFICATION_SOUND, HarakaConstants.DISABLE_HARAKA_NOTIFICATION_SOUND_PATH}
    };
    private readonly ConcurrentDictionary<string, byte[]> _cachedSounds = new();
    private IWavePlayer? _waveOut;

    public SoundPlayer()
    {
        _waveOut = new WaveOutEvent();
        Preload();
    }

    private void Preload()
    {
        string name;
        string path;
        foreach (KeyValuePair<string, string> entry in _supportedSounds)
        {
            name = entry.Key;
            path = entry.Value;
            
            if (!_cachedSounds.ContainsKey(name))
            {
                var fileBytes = File.ReadAllBytes(path);
                _cachedSounds[name] = fileBytes;
            }
        }
    }

    public void Play(string name)
    {
        if (_cachedSounds.TryGetValue(name, out var soundData))
        {
            var stream = new MemoryStream(soundData);
            var reader = new VorbisWaveReader(stream);

            var sampleChannel = new SampleChannel(reader, false)
            {
                Volume = ConfigManager.Config.NotificationVolume
            };
            var waveProvider = new SampleToWaveProvider(sampleChannel);

            _waveOut?.Stop();
            _waveOut?.Dispose();

            _waveOut = new WaveOutEvent();
            _waveOut.Init(waveProvider);
            _waveOut.Play();
        }    
    }

    public void PlayEnableSound() => Play(HarakaConstants.ENABLE_HARAKA_NOTIFICATION_SOUND);
    public void PlayDisableSound() => Play(HarakaConstants.DISABLE_HARAKA_NOTIFICATION_SOUND);

    public void Dispose()
    {
        _waveOut?.Stop();
        _waveOut?.Dispose();
        _waveOut = null;
    }
}