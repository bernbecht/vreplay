using UnityEngine;

namespace VRCapture {
    /// <summary>
    /// Simulate HTC Vive device input, replace SteamVR_Controller with
    /// HTCViveInput class will record all user input.
    /// 
    /// Example usage:
    /// HTCViveInput.Input(this, deviceIndex).GetPressDown(SteamVR_Controller.ButtonMask.Trigger)
    /// </summary>
    public class HTCViveInput {
        /// <summary>
        /// Holding the HTC Vive device list.
        /// </summary>
        static HTCViveDevice[] devices;

        /// <summary>
        /// Simulate SteamVR_Controller.Input function.
        /// </summary>
        /// <param name="obj">Game object.</param>
        /// <param name="index">Device index.</param>
        public static HTCViveDevice Input(GameObject obj, int index) {
            // Init devices.
            if (devices == null) {
                devices = new HTCViveDevice[VRReplayUtils.MAX_DEVICE_NUM];
            }
            // Validate index according different scenarios.
            if (VRReplay.Instance.Mode == VRReplay.ModeType.Record) {
                // Mapping device index to object name.
                VRReplay.Instance.RecordDeviceIndex(obj.name, index);
            }
            else if (VRReplay.Instance.Mode == VRReplay.ModeType.Playback) {
                index = Mathf.Abs( VRReplay.Instance.QueryDeviceIndex(obj.name));
            }
            // Correct invalid device index.
            if (index < 0)
                index = 0;
            // Get device according device index.
            HTCViveDevice device = devices[index];
            if (device == null) {
                device = new HTCViveDevice(index);
                devices[index] = device;
            }
            
            return device;
        }

        /// <summary>
        /// Wrapper for SteamVR_Controller.Device class.
        /// </summary>
        public class HTCViveDevice {
            /// <summary>
            /// The index of HTC Vive device.
            /// </summary>
            int index;
            /// <summary>
            /// The wrapped HTC Vive device.
            /// </summary>
            SteamVR_Controller.Device device;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:VRCapture.HTCViveInput.HTCViveDevice"/> class.
            /// </summary>
            /// <param name="index">Index.</param>
            public HTCViveDevice(int index) {
                this.index = index;
                this.device = SteamVR_Controller.Input(index);
            }

            /// <summary>
            /// Get the HTC Vive controller button press state.
            /// </summary>
            /// <returns><c>true</c>, if press was gotten, <c>false</c> otherwise.</returns>
            /// <param name="buttonMask">Button mask.</param>
            public bool GetPress(ulong buttonMask) {
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Playback) {
                    return VRReplay.Instance.QueryVRInputBool(index, buttonMask.ToString(), VRDeviceInputType.Press);
                }
                bool state = device.GetPress(buttonMask);
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Record) {
                    if (state)
                        VRReplay.Instance.RecordVRInputBool(index, buttonMask.ToString(), VRDeviceInputType.Press, state);
                }
                return state;
            }

            /// <summary>
            /// Get the HTC Vive controller button press down state.
            /// </summary>
            /// <returns><c>true</c>, if press down was gotten, <c>false</c> otherwise.</returns>
            /// <param name="buttonMask">Button mask.</param>
            public bool GetPressDown(ulong buttonMask) {
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Playback) {
                    return VRReplay.Instance.QueryVRInputBool(index, buttonMask.ToString(), VRDeviceInputType.PressDown);
                }
                bool state = device.GetPressDown(buttonMask);
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Record) {
                    if (state)
                        VRReplay.Instance.RecordVRInputBool(index, buttonMask.ToString(), VRDeviceInputType.PressDown, state);
                }
                return state;
            }

            /// <summary>
            /// Get the HTC Vive controller button press up state.
            /// </summary>
            /// <returns><c>true</c>, if press up was gotten, <c>false</c> otherwise.</returns>
            /// <param name="buttonMask">Button mask.</param>
            public bool GetPressUp(ulong buttonMask) {
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Playback) {
                    return VRReplay.Instance.QueryVRInputBool(index, buttonMask.ToString(), VRDeviceInputType.PressUp);
                }
                bool state = device.GetPressUp(buttonMask);
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Record) {
                    if (state)
                        VRReplay.Instance.RecordVRInputBool(index, buttonMask.ToString(), VRDeviceInputType.PressUp, state);
                }
                return state;
            }

            /// <summary>
            /// Get the HTC Vive controller touch press state.
            /// </summary>
            /// <returns><c>true</c>, if touch was gotten, <c>false</c> otherwise.</returns>
            /// <param name="buttonMask">Button mask.</param>
            public bool GetTouch(ulong buttonMask) {
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Playback) {
                    return VRReplay.Instance.QueryVRInputBool(index, buttonMask.ToString(), VRDeviceInputType.Touch);
                }
                bool state = device.GetTouch(buttonMask);
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Record) {
                    if (state)
                        VRReplay.Instance.RecordVRInputBool(index, buttonMask.ToString(), VRDeviceInputType.Touch, state);
                }
                return state;
            }

            /// <summary>
            /// Get the HTC Vive controller touch press down state.
            /// </summary>
            /// <returns><c>true</c>, if touch down was gotten, <c>false</c> otherwise.</returns>
            /// <param name="buttonMask">Button mask.</param>
            public bool GetTouchDown(ulong buttonMask) {
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Playback) {
                    return VRReplay.Instance.QueryVRInputBool(index, buttonMask.ToString(), VRDeviceInputType.TouchDown);
                }
                bool state = device.GetTouchDown(buttonMask);
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Record) {
                    if (state)
                        VRReplay.Instance.RecordVRInputBool(index, buttonMask.ToString(), VRDeviceInputType.TouchDown, state);
                }
                return state;
            }

            /// <summary>
            /// Get the HTC Vive controller touch press up state.
            /// </summary>
            /// <returns><c>true</c>, if touch up was gotten, <c>false</c> otherwise.</returns>
            /// <param name="buttonMask">Button mask.</param>
            public bool GetTouchUp(ulong buttonMask) {
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Playback) {
                    return VRReplay.Instance.QueryVRInputBool(index, buttonMask.ToString(), VRDeviceInputType.TouchUp);
                }
                bool state = device.GetTouchDown(buttonMask);
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Record) {
                    if (state)
                        VRReplay.Instance.RecordVRInputBool(index, buttonMask.ToString(), VRDeviceInputType.TouchUp, state);
                }
                return state;
            }

            /// <summary>
            /// Get the HTC Vive controller axis touch value.
            /// </summary>
            /// <returns>The axis.</returns>
            /// <param name="buttonID">Button identifier.</param>
            public Vector2 GetAxis(Valve.VR.EVRButtonId buttonID = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad) {
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Playback) {
                    return VRReplay.Instance.QueryVRInputVec2(index, buttonID.ToString(), VRDeviceInputType.AxisVec2);
                }
                Vector2 vec2 = device.GetAxis(buttonID);
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Record) {
                    if (Mathf.Abs(vec2.x) > VRCommonUtils.EPSILON || Mathf.Abs(vec2.y) > VRCommonUtils.EPSILON) {
                        VRReplay.Instance.RecordVRInputVec2(index, buttonID.ToString(), VRDeviceInputType.AxisVec2, vec2);
                    }
                }
                return vec2;
            }

            /// <summary>
            /// Get the HTC Vive controller hair trigger state.
            /// </summary>
            /// <returns><c>true</c>, if hair trigger was gotten, <c>false</c> otherwise.</returns>
            public bool GetHairTrigger() {
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Playback) {
                    return VRReplay.Instance.QueryVRInputBool(index, "HairTrigger", VRDeviceInputType.HairTrigger);
                }
                bool state = device.GetHairTrigger();
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Record) {
                    if (state)
                        VRReplay.Instance.RecordVRInputBool(index, "HairTrigger", VRDeviceInputType.HairTrigger, state);
                }
                return state;
            }

            /// <summary>
            /// Get the HTC Vive controller hair trigger down state.
            /// </summary>
            /// <returns><c>true</c>, if hair trigger down was gotten, <c>false</c> otherwise.</returns>
            public bool GetHairTriggerDown() {
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Playback) {
                    return VRReplay.Instance.QueryVRInputBool(index, "HairTriggerDown", VRDeviceInputType.HairTriggerDown);
                }
                bool state = device.GetHairTriggerDown();
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Record) {
                    if (state)
                        VRReplay.Instance.RecordVRInputBool(index, "HairTriggerDown", VRDeviceInputType.HairTriggerDown, state);
                }
                return state;
            }

            /// <summary>
            /// Get the HTC Vive controller hair trigger up state.
            /// </summary>
            /// <returns><c>true</c>, if hair trigger up was gotten, <c>false</c> otherwise.</returns>
            public bool GetHairTriggerUp() {
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Playback) {
                    return VRReplay.Instance.QueryVRInputBool(index, "HairTriggerUp", VRDeviceInputType.HairTriggerUp);
                }
                bool state = device.GetHairTriggerUp();
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Record) {
                    if (state)
                        VRReplay.Instance.RecordVRInputBool(index, "HairTriggerUp", VRDeviceInputType.HairTriggerUp, state);
                }
                return state;
            }

            /// <summary>
            /// Get the HTC Vive controller button press state.
            /// </summary>
            /// <returns><c>true</c>, if press was gotten, <c>false</c> otherwise.</returns>
            /// <param name="buttonID">Button identifier.</param>
            public bool GetPress(Valve.VR.EVRButtonId buttonID) {
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Playback) {
                    return VRReplay.Instance.QueryVRInputBool(index, buttonID.ToString(), VRDeviceInputType.Press);
                }
                bool state = device.GetPress(buttonID);
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Record) {
                    if (state)
                        VRReplay.Instance.RecordVRInputBool(index, buttonID.ToString(), VRDeviceInputType.Press, state);
                }
                return state;
            }

            /// <summary>
            /// Get the HTC Vive controller button press down state.
            /// </summary>
            /// <returns><c>true</c>, if press down was gotten, <c>false</c> otherwise.</returns>
            /// <param name="buttonID">Button identifier.</param>
            public bool GetPressDown(Valve.VR.EVRButtonId buttonID) {
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Playback) {
                    return VRReplay.Instance.QueryVRInputBool(index, buttonID.ToString(), VRDeviceInputType.PressDown);
                }
                bool state = device.GetPressDown(buttonID);
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Record) {
                    if (state)
                        VRReplay.Instance.RecordVRInputBool(index, buttonID.ToString(), VRDeviceInputType.PressDown, state);
                }
                return state;
            }

            /// <summary>
            /// Get the HTC Vive controller button press up state.
            /// </summary>
            /// <returns><c>true</c>, if press up was gotten, <c>false</c> otherwise.</returns>
            /// <param name="buttonID">Button identifier.</param>
            public bool GetPressUp(Valve.VR.EVRButtonId buttonID) {
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Playback) {
                    return VRReplay.Instance.QueryVRInputBool(index, buttonID.ToString(), VRDeviceInputType.PressUp);
                }
                bool state = device.GetPressUp(buttonID);
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Record) {
                    if (state)
                        VRReplay.Instance.RecordVRInputBool(index, buttonID.ToString(), VRDeviceInputType.PressUp, state);
                }
                return state;
            }

            /// <summary>
            /// Get the HTC Vive controller touch press state.
            /// </summary>
            /// <returns><c>true</c>, if touch was gotten, <c>false</c> otherwise.</returns>
            /// <param name="buttonID">Button identifier.</param>
            public bool GetTouch(Valve.VR.EVRButtonId buttonID) {
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Playback) {
                    return VRReplay.Instance.QueryVRInputBool(index, buttonID.ToString(), VRDeviceInputType.Touch);
                }
                bool state = device.GetTouch(buttonID);
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Record) {
                    if (state)
                        VRReplay.Instance.RecordVRInputBool(index, buttonID.ToString(), VRDeviceInputType.Touch, state);
                }
                return state;
            }

            /// <summary>
            /// Get the HTC Vive controller touch press down state.
            /// </summary>
            /// <returns><c>true</c>, if touch down was gotten, <c>false</c> otherwise.</returns>
            /// <param name="buttonID">Button identifier.</param>
            public bool GetTouchDown(Valve.VR.EVRButtonId buttonID) {
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Playback) {
                    return VRReplay.Instance.QueryVRInputBool(index, buttonID.ToString(), VRDeviceInputType.TouchDown);
                }
                bool state = device.GetTouchDown(buttonID);
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Record) {
                    if (state)
                        VRReplay.Instance.RecordVRInputBool(index, buttonID.ToString(), VRDeviceInputType.TouchDown, state);
                }
                return state;
            }

            /// <summary>
            /// Get the HTC Vive controller touch press up state.
            /// </summary>
            /// <returns><c>true</c>, if touch up was gotten, <c>false</c> otherwise.</returns>
            /// <param name="buttonID">Button identifier.</param>
            public bool GetTouchUp(Valve.VR.EVRButtonId buttonID) {
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Playback) {
                    return VRReplay.Instance.QueryVRInputBool(index, buttonID.ToString(), VRDeviceInputType.TouchUp);
                }
                bool state = device.GetTouchDown(buttonID);
                if (VRReplay.Instance.Mode == VRReplay.ModeType.Record) {
                    if (state)
                        VRReplay.Instance.RecordVRInputBool(index, buttonID.ToString(), VRDeviceInputType.TouchUp, state);
                }
                return state;
            }
        }
    }
}