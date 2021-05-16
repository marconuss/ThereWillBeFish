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
    private RawImage title;
    [SerializeField]
    private Canvas myCanvas;    

    public static bool inMenu;

    public float speed;
    public Vector3 targetPos = new Vector3(0.0f, 0.0f, -1.0f);


    private void Awake()
    {
        inMenu = true;
        Time.timeScale = 0;
        myCamera = Camera.main;
        myCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        myCanvas.GetComponent<Canvas>().worldCamera = myCamera;

        HideButtons();
    }

    private void Start()
    {
        myCamera.orthographicSize = 2.5f;
        myCamera.transform.position =  new Vector3(Submarine.Instance.transform.position.x, Submarine.Instance.transform.position.y, -1.0f);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            playPauseGame();

        if(inMenu){
            HideButtons();
            myCamera.orthographicSize = Mathf.Lerp(myCamera.orthographicSize, 2.5f, speed * Time.unscaledDeltaTime);
            myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, new Vector3(Submarine.Instance.transform.position.x, Submarine.Instance.transform.position.y, -1.0f), speed*Time.unscaledDeltaTime);
            Time.timeScale = 0;
        }
        else
        {
            HideButtons();
            myCamera.orthographicSize = Mathf.Lerp(myCamera.orthographicSize, 5.0f, speed * Time.unscaledDeltaTime);
            myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, targetPos, speed*Time.unscaledDeltaTime);
            Time.timeScale = 1;
        }
    }

    private void HideButtons()
    {
        title.gameObject.SetActive(inMenu);
        startBtn.gameObject.SetActive(inMenu);
        exitBtn.gameObject.SetActive(inMenu);
        pauseBtn.gameObject.SetActive(!inMenu);
    }

    public void playPauseGame()
    {
        inMenu = !inMenu;
    }

    public void ExitGame()
    {
        Debug.Log("bye");
        Application.Quit();
    }
}
