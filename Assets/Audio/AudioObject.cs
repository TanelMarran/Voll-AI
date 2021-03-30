using UnityEngine;

[CreateAssetMenu(fileName = "Audio", menuName = "ScriptableObjects/AudioObject", order = 1)]
public class AudioObject : ScriptableObject
{
    public AudioClip[] clips;
}