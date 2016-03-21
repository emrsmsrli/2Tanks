using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public GameObject canvasManager;
    public Canvas mainCanvas;
    public Canvas colorCanvas;

    public Text roundText;
    public Scrollbar roundnum;

    public Slider red1;
    public Slider red2;
    public Slider green1;
    public Slider green2;
    public Slider blue1;
    public Slider blue2;


    [HideInInspector]
    public Color tc1;
    [HideInInspector]
    public Color tc2;

    public GameObject tank1;
    public GameObject tank2;

    private int numberOfRounds;
    private Color[] colors;
    private bool colorbuttonselected;

    void Awake() {
        colors = new Color[2];
        colors[0] = tc1;
        colors[1] = tc2;
        colorbuttonselected = false;


        MeshRenderer[] t1 = tank1.GetComponentsInChildren<MeshRenderer>();
        MeshRenderer[] t2 = tank2.GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer mr in t1) {
            mr.material.color = colors[0];
        }
        foreach(MeshRenderer mr in t2) {
            mr.material.color = colors[1];
        }
    }

    void Update() {
        if(colorbuttonselected) {
            MeshRenderer[] t1 = tank1.GetComponentsInChildren<MeshRenderer>();
            MeshRenderer[] t2 = tank2.GetComponentsInChildren<MeshRenderer>();
            foreach(MeshRenderer mr in t1) {
                mr.material.color = new Color(red1.value / 255, green1.value / 255, blue1.value / 255);
            }
            foreach(MeshRenderer mr in t2) {
                mr.material.color = new Color(red2.value / 255, green2.value / 255, blue2.value / 255);
            }
        }
    }

    public void onRoundSliderValueChanged() {
        switch((int)Mathf.Ceil(roundnum.value / .25f + 0.1f)) {
            case 1:
                roundText.text = "3";
                break;
            case 2:
                roundText.text = "5";
                break;
            case 3:
                roundText.text = "7";
                break;
            case 5:
                roundText.text = "9";
                break;
        }
    }


    public void onStartClick() {
        if(colorbuttonselected)
            calculateColors();
        SceneManager.LoadScene("Game");
        GameManager.m_NumRoundsToWin = int.Parse(roundText.text);
        GameManager.colors = colors;
    }

    public void onQuitClick() {
        Application.Quit();
    }

    public void onOptionsClick() {

        colorbuttonselected = true;

        Button[] buttons = mainCanvas.GetComponentsInChildren<Button>();
        foreach(Button b in buttons)
            b.interactable = false;
        buttons = colorCanvas.GetComponentsInChildren<Button>();
        foreach(Button b in buttons)
            b.interactable = true;

        StartCoroutine(rotator(-1));
    }

    public void onOptionsOKClick() {

        Button[] buttons = mainCanvas.GetComponentsInChildren<Button>();
        foreach(Button b in buttons)
            b.interactable = true;
        buttons = colorCanvas.GetComponentsInChildren<Button>();
        foreach(Button b in buttons)
            b.interactable = false;

        StartCoroutine(rotator(1));
    }

    void calculateColors() {
        colors[0] = new Color(red1.value / 255, green1.value / 255, blue1.value / 255);
        colors[1] = new Color(red2.value / 255, green2.value / 255, blue2.value / 255);
    }

    IEnumerator rotator(int dir) {
        float angle = 45;
        for(int i = 0; i < 100; ++i) {
            canvasManager.transform.Rotate(0, dir * angle, 0);
            angle = Mathf.LerpAngle(angle, 0, .5f);
            yield return new WaitForSeconds(0.03f);
        }
    }
}
