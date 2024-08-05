using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new VideoWrapperContainer", menuName = "Cut Scene Video Player/Video Wrapper Container")]
public class VideoWrapperContainerSO : ScriptableObject
{
    [field: SerializeField] public List<VideoWrapperSO> videoWrappers = new(); 
}
