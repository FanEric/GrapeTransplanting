using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class SystemSettings : MonoBehaviour{
    Transform mTrans;

    public GameObject kMovieContainer;
    public VideoPlayer kMovie;
    //视频控制器
    public Slider sliderVideo;
    //时 分的转换
    private int hour, mint;
    private float time;
    private float time_Count;
    private float time_Current;
    //当前视频时间
    public Text text_Time;
    //视频总时长
    public Text text_Count;

    private Button kSound;
    private Button kSoundMute;
    private Button kQuit;
    private Button kBack;
    private Button kVideo;

    private AudioSource kSource;

    // Use this for initialization
    public void Start () {
        DontDestroyOnLoad(this);

        mTrans = GameObject.Find("TopRightCanvas").transform;

        kSound = mTrans.Find("Button_music").GetComponent<Button>();
        kSoundMute = mTrans.Find("Button_music_mute").GetComponent<Button>();
        kQuit = mTrans.Find("Button_quit").GetComponent<Button>();
        kBack = mTrans.Find("Button_back").GetComponent<Button>();
        kVideo = mTrans.Find("Button_video").GetComponent<Button>();
        kSource = mTrans.Find("BgMusic").GetComponent<AudioSource>();

        kSound.onClick.AddListener(OnMuteOn);
        kSoundMute.onClick.AddListener(OnMuteOff);
        kQuit.onClick.AddListener(OnQuit);
        kBack.onClick.AddListener(OnBack);
        kVideo.onClick.AddListener(OnPlayVideo);

        kBack.gameObject.SetActive(false);
        OnMuteOff();
    }

    void OnMuteOn()
    {
        kSound.gameObject.SetActive(false);
        kSoundMute.gameObject.SetActive(true);
        kSource.mute = true;
    }

    void OnMuteOff()
    {
        kSoundMute.gameObject.SetActive(false);
        kSound.gameObject.SetActive(true);
        kSource.mute = false;
    }

    void OnPlayVideo()
    {
        if (kMovie.isPlaying)
        {
            sliderVideo.value = 0;
            time_Current = 0;

            kMovie.Stop();
            kMovieContainer.SetActive(false);

            OnMuteOff();
        }
        else
        {
            kMovieContainer.SetActive(true);
            kMovie.Play();

            OnMuteOn();

            time = sliderVideo.maxValue = (float)kMovie.clip.length;
            hour = (int)time / 60;
            mint = (int)time % 60;
            text_Count.text = string.Format("/  {0:00}:{1:00}", hour, mint);

            sliderVideo.onValueChanged.AddListener(delegate { ChangeVideo(sliderVideo.value); });

        }
    }

    /// <summary>
    ///     改变视频进度
    /// </summary>
    /// <param name="value"></param>
    public void ChangeVideo(float value)
    {
        if (kMovie.isPrepared)
        {
            kMovie.time = (long)value;
            //Debug.Log("VideoPlayer Time:" + kMovie.time);
            time = (float)kMovie.time;
            hour = (int)time / 60;
            mint = (int)time % 60;
            text_Time.text = string.Format("{0:00}:{1:00}", hour, mint);
        }
    }

    void Update()
    {
        if (kMovie.isPlaying)
        {
            time_Count += Time.deltaTime;
            if ((time_Count - time_Current) >= 1)
            {
                sliderVideo.value += 1;
                //Debug.Log("value:" + sliderVideo.value);
                time_Current = time_Count;
            }
        }
    }

    void OnQuit()
    {
        Application.Quit();
    }

    void OnBack()
    {
        GameInfo.Reset ();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void ShowBackBtn()
    {
        kBack.gameObject.SetActive(true);
    }

    public void ShowVideoBtn(bool toShow)
    {
        kVideo.gameObject.SetActive(toShow);
    }
}

