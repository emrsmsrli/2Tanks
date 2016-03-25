#define _DEBUG

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public float m_StartDelay = 3f;
    public float m_EndDelay = 3f;
    public CameraControl m_CameraControl;
    public Text m_MessageText;
    public GameObject m_TankPrefab;
    public TankManager[] m_Tanks;
    public Transform[] spawnPoints;
    public Camera mainCamera;
    public GameObject pointerPrefab;
    public GameObject powerUpPrefab;
    [HideInInspector]
    public static int m_NumRoundsToWin;
    [HideInInspector]
    public static Color[] colors;
    [HideInInspector]
    public static bool powerUpped = false;

    private int m_RoundNumber;
    private WaitForSeconds m_StartWait;
    private WaitForSeconds m_EndWait;
    private TankManager m_RoundWinner;
    private TankManager m_GameWinner;

    private void Start() {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);

        SpawnAllTanks();
        SetCameraTargets();

        StartCoroutine(GameLoop());
    }

    private void SpawnAllTanks() {
        List<Transform> sp = new List<Transform>();
        foreach(Transform spawnPoint in spawnPoints) {
            sp.Add(spawnPoint);
        }

        for(int i = 0; i < m_Tanks.Length; i++) {
            Transform point = sp[Random.Range(0, sp.Count)];
            m_Tanks[i].m_Instance =
                Instantiate(m_TankPrefab, point.position, point.rotation) as GameObject;
            m_Tanks[i].m_SpawnPoint = point;
            m_Tanks[i].m_PlayerNumber = i + 1;
#if _DEBUG
#else
            m_Tanks[i].m_PlayerColor = colors[i];
#endif
            m_Tanks[i].Setup();
            sp.Remove(point);
        }
    }


    private void SetCameraTargets() {
        Transform[] targets = new Transform[m_Tanks.Length];

        for(int i = 0; i < targets.Length; i++) {
            targets[i] = m_Tanks[i].m_Instance.transform;
        }

        m_CameraControl.m_Targets = targets;
    }


    private IEnumerator GameLoop() {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if(m_GameWinner != null) {
            SceneManager.LoadScene(0);
        } else {
            StartCoroutine(GameLoop());
        }
    }


    private IEnumerator RoundStarting() {
        ResetAllTanks();
        DisableTankControl();
        m_CameraControl.SetStartPositionAndSize();

        m_RoundNumber++;
        m_MessageText.text = "ROUND " + m_RoundNumber;

        yield return m_StartWait;
    }


    private IEnumerator RoundPlaying() {
        EnableTankControl();

        m_MessageText.text = string.Empty;

        StartCoroutine("visibilityCheck");
        StartCoroutine("spawnPowerUp");

        while(!OneTankLeft()) {
            yield return null;
        }

        StopCoroutine("spawnPowerUp");
        StopCoroutine("visibilityCheck");
    }


    private IEnumerator RoundEnding() {
        DisableTankControl();

        m_RoundWinner = null;
        m_RoundWinner = GetRoundWinner();
        if(m_RoundWinner != null)
            m_RoundWinner.m_Wins++;

        m_GameWinner = GetGameWinner();

        m_MessageText.text = EndMessage();

        yield return m_EndWait;
    }

    private IEnumerator visibilityCheck() {
        while(true) {
            foreach(TankManager tank in m_Tanks) {
                RaycastHit hit;
                Physics.Raycast(mainCamera.transform.position, tank.m_Instance.transform.position - mainCamera.transform.position, out hit);
                if(!hit.collider.gameObject.CompareTag("Player")) {
                    if(tank.isVisible) {
                        tank.isVisible = false;
                        GameObject o = Instantiate(pointerPrefab, tank.m_Instance.transform.position + new Vector3(0, 4.5f, 0), Quaternion.identity) as GameObject;
                        foreach(MeshRenderer mr in o.GetComponentsInChildren<MeshRenderer>())
                            mr.material.color = tank.m_PlayerColor;
                        o.transform.SetParent(tank.m_Instance.transform, true);
                    }
                } else {
                    if(!tank.isVisible) {
                        tank.isVisible = true;
                        GameObject o = tank.m_Instance.GetComponentInChildren<Pointer>().gameObject;
                        Destroy(o);
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator spawnPowerUp() {
        while(true) {
            if(!powerUpped && Random.Range(0, 10) < 8) {
                yield return new WaitForSeconds(1f);
                Instantiate(powerUpPrefab, transform.position + new Vector3(Random.Range(-43f, 43f), 1.2f, Random.Range(-43f, 43f)), Quaternion.Euler(45f, 0, 45f));
            }
        }
    }

    private bool OneTankLeft() {
        int numTanksLeft = 0;

        for(int i = 0; i < m_Tanks.Length; i++) {
            if(m_Tanks[i].m_Instance.activeSelf)
                numTanksLeft++;
        }

        return numTanksLeft <= 1;
    }


    private TankManager GetRoundWinner() {
        for(int i = 0; i < m_Tanks.Length; i++) {
            if(m_Tanks[i].m_Instance.activeSelf)
                return m_Tanks[i];
        }

        return null;
    }


    private TankManager GetGameWinner() {
        for(int i = 0; i < m_Tanks.Length; i++) {
            if(m_Tanks[i].m_Wins == m_NumRoundsToWin)
                return m_Tanks[i];
        }

        return null;
    }


    private string EndMessage() {
        string message = "DRAW!";

        if(m_RoundWinner != null)
            message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

        message += "\n\n\n\n";

        for(int i = 0; i < m_Tanks.Length; i++) {
            message += m_Tanks[i].m_ColoredPlayerText + ": " + m_Tanks[i].m_Wins + " WINS\n";
        }

        if(m_GameWinner != null)
            message = m_GameWinner.m_ColoredPlayerText + " WINS THE GAME!";

        return message;
    }

    private void ResetAllTanks() {
        for(int i = 0; i < m_Tanks.Length; i++) {
            m_Tanks[i].Reset();
        }
    }


    public void EnableTankControl() {
        for(int i = 0; i < m_Tanks.Length; i++) {
            m_Tanks[i].EnableControl();
        }
    }


    public void DisableTankControl() {
        for(int i = 0; i < m_Tanks.Length; i++) {
            m_Tanks[i].DisableControl();
        }
    }
}