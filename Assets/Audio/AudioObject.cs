using UnityEngine;

[CreateAssetMenu(fileName = "Audio", menuName = "ScriptableObjects/AudioObject", order = 1)]
public class AudioObject : ScriptableObject
{
    public AudioClip[] clips;

    [Range(0, 1)] public float volume = 1;
    [Range(0, 1)] public float pitchshift = 0.05f;
}