using UnityEngine;
using UnityEngine.UI;

namespace VRCapture.Demo {

    public class VideoCaptureDemoUI : MonoBehaviour {
        public Image recImage;
        public Text stateText;

        bool enableMainCamera = false;
        bool enable360Camera = false;
        bool enableTopDownCamera = true;
        bool enableLeftRightCamera = true;
        bool enableOnlyAudio = false;

        bool captureFinish = false;

        void Start() {
            VRCapture.Instance.RegisterCompleteDelegate(HandleCaptureFinish);
            Application.runInBackground = true;
            recImage.enabled = true;
            stateText.enabled = true;
            stateText.text = "Ready";
        }

        private void Update() {
            if (captureFinish) {
                stateText.text = "Finish";
                captureFinish = false;
            }
        }

        void OnGUI() {
            enableMainCamera = GUI.Toggle(
                new Rect(50, 50, 150, 50),
                enableMainCamera,
                " Enable Main Camera");
            enableTopDownCamera = GUI.Toggle(
                new Rect(50, 100, 150, 50),
                enableTopDownCamera,
                " Enable Top-Down Camera");
            enableLeftRightCamera = GUI.Toggle(
                new Rect(50, 150, 150, 50),
                enableLeftRightCamera,
                " Enable Left-Right Camera");
            enable360Camera = GUI.Toggle(
                new Rect(50, 200, 150, 50),
                enable360Camera,
                " Enable 360 Camera");
            enableOnlyAudio = GUI.Toggle(
                new Rect(50, 250, 150, 50),
                enableOnlyAudio,
                " Enable Only Audio");
            if (enable360Camera) {
                enableMainCamera = false;
                enableTopDownCamera = false;
                enableLeftRightCamera = false;
                enableOnlyAudio = false;
            }
            if (enableOnlyAudio) {
                enableMainCamera = false;
                enableTopDownCamera = false;
                enableLeftRightCamera = false;
                enable360Camera = false;
            }
            if (enableMainCamera) {
                VRCapture.Instance.CaptureVideos[0].isEnabled = true;
            }
            else {
                VRCapture.Instance.CaptureVideos[0].isEnabled = false;
            }
            if (enableTopDownCamera) {
                VRCapture.Instance.CaptureVideos[1].isEnabled = true;
            }
            else {
                VRCapture.Instance.CaptureVideos[1].isEnabled = false;
            }
            if (enableLeftRightCamera) {
                VRCapture.Instance.CaptureVideos[2].isEnabled = true;
            }
            else {
                VRCapture.Instance.CaptureVideos[2].isEnabled = false;
            }
            if (enable360Camera) {
                VRCapture.Instance.CaptureVideos[3].isEnabled = true;
            }
            else {
                VRCapture.Instance.CaptureVideos[3].isEnabled = false;
            }
            if (GUI.Button(new Rect(50, 350, 150, 50), "Capture Start")) {
                if (VRCapture.Instance.StartCapture() == VRCapture.StatusCode.Success) {
                    stateText.text = "Recording";
                }
                else {
                    stateText.text = "Error";
                }
            }
            if (GUI.Button(new Rect(50, 450, 150, 50), "Capture Stop")) {
                VRCapture.Instance.StopCapture();
                stateText.text = "Processing";
            }
            if (GUI.Button(new Rect(50, 550, 150, 50), "Open Video Folder")) {
                System.Diagnostics.Process.Start(VRCaptureUtils.SaveFolder);
            }
        }

        void HandleCaptureFinish() {
            captureFinish = true;
        }
    }
}