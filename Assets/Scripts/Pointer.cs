//#define _DEBUG

using UnityEngine;
using System.Collections;

public class Pointer : MonoBehaviour {
#if _DEBUG
    public void Start() {
        print("started");
    }

    IEnumerator moveY() {
        Vector3 target1 = transform.position + new Vector3(0, 2, 0);
        Vector3 target2 = transform.position + new Vector3(0, -2, 0);
        Vector3 velocity = new Vector3(0, 0, 0);
        while(true) {
            transform.position = Vector3.SmoothDamp(transform.position, target1, ref velocity, 0.2f);
            transform.position = Vector3.SmoothDamp(transform.position, target2, ref velocity, 0.2f);
            //for(int i = 5; i > 0; --i) {
            //    transform.Translate(0, i / 5, 0);
            //    yield return new WaitForSeconds(.02f);
            //}
            //for(int i = 5; i > 0; --i) {
            //    transform.Translate(0, -i / 5, 0);
            //    yield return new WaitForSeconds(.02f);
            //}
        }
    }
#endif

    void Update() {
        transform.Rotate(0, 5, 0);
    }

}
