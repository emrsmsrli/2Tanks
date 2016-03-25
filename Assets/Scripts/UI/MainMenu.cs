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

    public GameObject tank1;
    public GameObject tank2;

    //private int numberOfRounds;
    private Color[] colors;
    private bool colorbuttonselected;

    void Awake() {
        colors = new Color[2];
        calculateColors();
        assignColors();
        colorbuttonselected = false;
    }

    void Update() {
        if(colorbuttonselected) {
            calculateColors();
            assignColors();
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
        SceneManager.LoadScene("Game");
        GameManager.m_NumRoundsToWin = int.Parse(roundText.text);
        GameManager.colors = colors;
    }

    public void onQuitClick() {
        Application.Quit();
    }

    public void onOptionsClick() {
        colorbuttonselected = true;
        buttonInteractivity(false);
        StartCoroutine(rotator(-1));
    }

    public void onOptionsOKClick() {
        buttonInteractivity(true);
        StartCoroutine(rotator(1));
    }

    private void buttonInteractivity(bool interactive) {

        Button[] buttons = mainCanvas.GetComponentsInChildren<Button>();
        foreach(Button b in buttons)
            b.interactable = interactive;
        buttons = colorCanvas.GetComponentsInChildren<Button>();
        foreach(Button b in buttons)
            b.interactable = !interactive;

    }

    private void calculateColors() {
        colors[0] = new Color(red1.value / 255, green1.value / 255, blue1.value / 255);
        colors[1] = new Color(red2.value / 255, green2.value / 255, blue2.value / 255);
    }

    private void assignColors() {
        assignTankColor(tank1, 0);
        assignTankColor(tank2, 1);
    }

    private void assignTankColor(GameObject tank, int index) {
        MeshRenderer[] mrs = tank.GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer mr in mrs) {
            mr.material.color = colors[index];
        }
    }

    private IEnumerator rotator(int dir) {
        //alternative angle = 45, lerp ratio = .5f
        float angle = 30;
        for(int i = 0; i < 100; ++i) {
            canvasManager.transform.Rotate(0, dir * angle, 0);
            angle = Mathf.LerpAngle(angle, 0, .3333333333333333333f);
            yield return new WaitForSeconds(0.03f);
        }
    }
}
