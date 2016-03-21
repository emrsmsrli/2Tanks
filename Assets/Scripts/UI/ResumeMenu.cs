using UnityEngine;
using System.Collections;

public class ResumeMenu : MonoBehaviour {

    public GameManager gm;
    public GameObject resumeMenu;

    private bool menuOn = false;

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
        gm.DisableTankControl();
        resumeMenu.SetActive(true);
        menuOn = true;
    }

	public void onClickResume() {
        gm.EnableTankControl();
        resumeMenu.SetActive(false);
        menuOn = false;
    }

    public void onClickQuit() {
        Application.Quit();
    }
}
