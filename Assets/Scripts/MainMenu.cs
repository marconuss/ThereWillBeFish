using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [SerializeField] 
    private Camera myCamera;

    [SerializeField]
    private Button startBtn;
    [SerializeField]
    private Button exitBtn;
    [SerializeField]
    private Button pauseBtn;
    [SerializeField]
    private Button autoMoveBtn;
    [SerializeField]
    private RawImage title;
    [SerializeField]
    private Canvas myCanvas;    

    public static bool inMenu;

    public float speed;
    public Vector3 targetPos = new Vector3(0.0f, 0.0f, -1.0f);


    private void Awake()
    {
        //inMenu = true;
        //Time.timeScale = 0;
        myCamera = Camera.main;
        myCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        myCanvas.GetComponent<Canvas>().worldCamera = myCamera;
    }

    private void OnEnable()
    {
        PauseManager.Instance.OnPause += OnPause;
    }

    private void OnDisable()
    {
        if (PauseManager.Instance)
            PauseManager.Instance.OnPause -= OnPause;
    }
    private void Start()
    {
        myCamera.orthographicSize = 2.5f;
        myCamera.transform.position =  new Vector3(Submarine.Instance.transform.position.x, Submarine.Instance.transform.position.y, -1.0f);
        PauseManager.Instance.IsPaused = true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            playPauseGame();

        if(PauseManager.Instance.IsPaused){
            myCamera.orthographicSize = Mathf.Lerp(myCamera.orthographicSize, 2.5f, speed * Time.unscaledDeltaTime);
            myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, new Vector3(Submarine.Instance.transform.position.x, Submarine.Instance.transform.position.y, -1.0f), speed*Time.unscaledDeltaTime);
            //Time.timeScale = 0;
        }
        else
        {
            myCamera.orthographicSize = Mathf.Lerp(myCamera.orthographicSize, 5.0f, speed * Time.unscaledDeltaTime);
            myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, targetPos, speed*Time.unscaledDeltaTime);
            //Time.timeScale = 1;
        }
    }

    private void OnPause(bool isPaused)
    {
        title.gameObject.SetActive(isPaused);
        startBtn.gameObject.SetActive(isPaused);
        exitBtn.gameObject.SetActive(isPaused);
        pauseBtn.gameObject.SetActive(!isPaused);
        autoMoveBtn.gameObject.SetActive(!isPaused);
    }

    public void playPauseGame()
    {
        //inMenu = !inMenu;
        PauseManager.Instance.IsPaused = !PauseManager.Instance.IsPaused;
    }

    public void ToggleAutoPilot()
    {
        Submarine.Instance.IsAutoPilotEnabled = !Submarine.Instance.IsAutoPilotEnabled;
    }

    public void ExitGame()
    {
        Debug.Log("bye");
        Application.Quit();
    }
}
