using System.Collections;
using UnityEngine;

namespace VRCapture.Demo {
    /// <summary>
    /// VRReplayDemoUI, for demonstrating replay system; Record cube's
    /// input information or transform information.
    /// </summary>
    public class VRReplayDemoUI : MonoBehaviour {

        public VRReplayAuto replayAuto;
        int seconds;

        GUIStyle labelStyle;

        void Start() {
            seconds = replayAuto.recordSeconds - 1;
            labelStyle = new GUIStyle();
            labelStyle.fontSize = 30;

            StartCoroutine(CountSeconds());
        }

        IEnumerator CountSeconds() {
            while (seconds > 0) {
                yield return new WaitForSeconds(1);
                seconds--;
            }
        }

        void OnGUI() {
            if (replayAuto == null) {
                GUI.Label(new Rect(50, 50, 200, 50),
                          "Please attach VRReplayAuto script!",
                          labelStyle);
            }
            else if (replayAuto.mode == VRReplayAuto.ModeType.None) {
                GUI.Label(new Rect(50, 50, 200, 50),
                          "Please change VRReplayAuto mode to Record or Replay.",
                          labelStyle);
            }
            else if (replayAuto.mode == VRReplayAuto.ModeType.Record) {
                GUI.Label(new Rect(50, 50, 200, 50),
                          "In Record mode, use direction key to control the cube: " + seconds,
                          labelStyle);
            }
            else if (replayAuto.mode == VRReplayAuto.ModeType.Replay ||
                     replayAuto.mode == VRReplayAuto.ModeType.Replay_Capture) {
                GUI.Label(new Rect(50, 50, 200, 50),
                          "In Replay mode, wait and see the replay playback.",
                          labelStyle);
            }
        }
    }
}