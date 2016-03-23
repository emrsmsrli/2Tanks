using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResumeMenu : MonoBehaviour {

    public GameManager gm;
    public GameObject resumeMenu;
    public GameObject optionsMenu;

    public AudioSource music;

    public Button resumeBut;
    public Button optionsBut;
    public Button optionsOKBut;
    public Button quitBut;

    private Vector3 dampOffset = new Vector3(200, 0, 0);
    private float dampTime = 0.2f;
    private bool menuOn = false;
    private bool optionsMenuOn = false;
    private bool onAnimation = false;

    void Update() {

        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(!menuOn) {
                onMenuShow();
            } else {
                onClickResume();
            }
        }
    }

    private void onMenuShow() {
        menuOn = true;
        gm.DisableTankControl();
        resumeMenu.SetActive(true);
    }

    public void onClickResume() {
        menuOn = false;
        gm.EnableTankControl();
        resumeMenu.SetActive(false);
    }

    public void onClickOptions() {
        optionsMenuOn = true;
        disableButtons();
        if(!onAnimation)
            StartCoroutine(moveUI(-1));
    }

    public void onMusicSliderValueChanged() {
        music.volume = optionsMenu.GetComponentInChildren<Slider>().value;
    }

    public void onClickOptionsOK() {
        optionsMenuOn = false;
        disableButtons();
        if(!onAnimation)
            StartCoroutine(moveUI(1));
    }

    public void onClickQuit() {
        Application.Quit();
    }

    private void disableButtons() {
        if(optionsMenuOn) {
            resumeBut.interactable = false;
            optionsBut.interactable = false;
            quitBut.interactable = false;

            optionsOKBut.interactable = true;
        } else {
            resumeBut.interactable = true;
            optionsBut.interactable = true;
            quitBut.interactable = true;

            optionsOKBut.interactable = false;
        }
    }

    private IEnumerator moveUI(int direction) {
        onAnimation = true;
        StartCoroutine(mover(resumeBut.gameObject, direction));
        yield return new WaitForSeconds(.05f);

        StartCoroutine(mover(optionsBut.gameObject, direction));
        StartCoroutine(mover(optionsMenu, direction));
        yield return new WaitForSeconds(.05f);

        StartCoroutine(mover(quitBut.gameObject, direction));
        yield return new WaitForSeconds(.5f);

        StopAllCoroutines();
        onAnimation = false;
    }

    private IEnumerator mover(GameObject obj, int direction) {
        Vector3 initialpos = obj.transform.position;
        Vector3 targetpos = initialpos + (dampOffset * direction);
        Vector3 velocity = Vector3.zero;

        while(obj.transform.position != targetpos) {
            obj.transform.position = Vector3.SmoothDamp(obj.transform.position, targetpos, ref velocity, dampTime);
            yield return null;
        }
    }
}
