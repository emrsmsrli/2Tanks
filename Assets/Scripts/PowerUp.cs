using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

    private float killTime = 20f;
    
    public void Awake() {
        if(isOverlapping())
            kill();
    }

    public void Start() {
        GameManager.powerUpped = true;
        Invoke("kill", killTime);
    }

    public void Update() {
        transform.Rotate(2, 2, 2);
    }

    public void OnTriggerEnter(Collider other) {
        other.gameObject.GetComponent<TankHealth>().powerUp();
        kill();
    }

    private bool isOverlapping() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, .7f);

        foreach(Collider c in colliders) {
            if(c.GetType() != typeof(MeshCollider))
                if(!c.gameObject.CompareTag("PowerUp"))
                    return true;
        }

        return false;
    }

    private void kill() {
        GameManager.powerUpped = false;
        Destroy(gameObject);
    }
}
