/*
 * Auth: Ian
 * 
 * Proj: SpaceS
 * 
 * Desc: Maybe Name Audio Container
 * 
 * Date: 8/23/24
 */
using UnityEngine;


[CreateAssetMenu(fileName ="NewAudioContainer", menuName ="Audio/Audio Container")]
public class TestClip : ScriptableObject, IAudioDataProvider
{
    [SerializeField] TestClipSpecs[] potentialClipSpecs;







    public AudioData GetAudioData() => LeafRand.I.Element(potentialClipSpecs).GetAudioData(); 


    /// <summary>
    /// Returns whether an Audio Manager is loaded and prints a warning to console if one is not.
    /// </summary>
    /// <returns></returns>
    bool AudioManagerStarted()
    {
        if (AudioManager.Play == null)
        {
            Debug.Log("Audio failed to play. No AudioManager loaded in the scene!");
            return false;
        }
        return true;
    }
}
