using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;



[CreateAssetMenu(fileName = "new VideoWrapper", menuName = "Cut Scene Video Player/Video Wrapper")]
public class VideoWrapperSO : ScriptableObject
{
    public string videoName;
    public int level;
    public int priority;

    public VideoClip videoClip;

    [TextArea(3, 10)]
    public string descr;
}
