using UnityEngine;
using System.Collections;

namespace VRCapture.Demo {

    /// <summary>
    /// ReplayObjectController, an example of record gameobject's status
    /// if want to apply replay.
    /// </summary>
    public class ReplayObjectController : MonoBehaviour {

        void Update() {
            if (StandaloneInput.GetKey(KeyCode.UpArrow)) {
                transform.Translate(new Vector3(0f, 0.1f, 0f));
            }
            if (StandaloneInput.GetKey(KeyCode.DownArrow)) {
                transform.Translate(new Vector3(0f, -0.1f, 0f));
            }
            if (StandaloneInput.GetKey(KeyCode.LeftArrow)) {
                transform.Translate(new Vector3(-0.1f, 0f, 0f));
            }
            if (StandaloneInput.GetKey(KeyCode.RightArrow)) {
                transform.Translate(new Vector3(0.1f, 0f, 0f));
            }
        }
    }
}