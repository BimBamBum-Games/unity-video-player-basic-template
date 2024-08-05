using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public enum RenderExitMode {
    Direct, Delayed
}

public class VideoPlayerManager : MonoBehaviour
{
    [SerializeField] bool canPrintToConsole = false;
    [SerializeField] RenderExitMode renderExitMode = RenderExitMode.Direct;

    [SerializeField] VideoWrapperContainerSO videoWrapperContainer;
    [SerializeField] VideoPlayer videoPlayer;

    [Space(20)]
    [Header("Abstract GameManager")]
    [SerializeField] GameManager gameManager;

    [Space(20)]
    [SerializeField] UnityEvent OnVideoPlayerStart;
    [Space(10)]
    [SerializeField] UnityEvent OnVideoPlayerStop;

    public delegate void CutSceneVideoNonValue();
    public static CutSceneVideoNonValue OnRunVideo;
    public static CutSceneVideoNonValue OnEndVideo;

    bool m_isVideoEnded = false;

    protected void Awake() {
        if(videoPlayer == null) {
            videoPlayer = gameObject.AddComponent<VideoPlayer>();
            Debug.LogWarning($"<color=yellow>Video Player component has not found! A Video Player componen has added to the gameObject!</color>");
        }

        videoPlayer.isLooping = false;
        videoPlayer.playOnAwake = false;

    }

    protected void OnEnable() {
        videoPlayer.loopPointReached += OnVideoPlayEnded;
        gameManager.OnVideoButtonClick_IntParEnmRet += PlayVideoSync;
    }

    protected void OnDisable() {
        videoPlayer.loopPointReached -= OnVideoPlayEnded;
        gameManager.OnVideoButtonClick_IntParEnmRet -= PlayVideoSync;
    }

    [ContextMenu("Show Logs")]
    public void ShowLogs() {
        foreach (VideoWrapperSO videoWrapperSO in videoWrapperContainer.videoWrappers) {
            Debug.LogWarning($"Video Wrapper SO Name > {videoWrapperSO.name}");
        }
    }
  
    public IEnumerator PlayVideoSync(int currentLevel) {

        m_isVideoEnded = false;

        currentLevel = currentLevel < videoWrapperContainer.videoWrappers.Count ? currentLevel : videoWrapperContainer.videoWrappers.Count - 1;

        VideoWrapperSO videoWrapperSO = videoWrapperContainer.videoWrappers[currentLevel];

        videoPlayer.clip = videoWrapperSO.videoClip;

        videoPlayer.StepForward();

        videoPlayer.Play();
        OnVideoPlayerStart?.Invoke();

        yield return new WaitUntil(() => m_isVideoEnded == true);

        if(canPrintToConsole) Debug.LogWarning("Video Player Has Ended!");

        ExitFromRenderWithSelectedMode();

    }

    public void OnVideoPlayEnded(VideoPlayer videoPlayer) {
        //Video Player, oynatilan videoyu bitirdiginde event cagirir. Bu metod o eventa baglanacaktir.
        m_isVideoEnded = true;
        videoPlayer.Stop();
    }

    //Eger Stop sonrasinda hemen release metodu cagrilirsa ufak bir bosluk glitch meydana geliyor eger ienum metodlari zincirle birbirine baglanmissa gozlemlenir.
    private IEnumerator ReleaseTargetRendererTextureSync() {
        yield return new WaitForSecondsRealtime(0.1f);
        videoPlayer.targetTexture.Release();
    }

    private void ReleaseTargetRendererTextureDirect() {
        videoPlayer.targetTexture.Release();
    }

    private void ExitFromRenderWithSelectedMode() {
        if (renderExitMode == RenderExitMode.Direct) {
            ReleaseTargetRendererTextureDirect();
        }
        else if (renderExitMode == RenderExitMode.Delayed) {
            //Ayri bir coroutine ile glitch olan frame kapatilmasi amaclandi.
            StartCoroutine(ReleaseTargetRendererTextureSync());
        }

        OnVideoPlayerStop?.Invoke();
    }

}
