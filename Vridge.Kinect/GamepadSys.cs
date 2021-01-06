using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using SharpDX.XInput;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using VRE.Vridge.API.Client.Remotes;
using KinectLib.Xbox360;
using KinectLib.XboxOne;
using VRE.Vridge.API.Client.Messages.BasicTypes;
using VRE.Vridge.API.Client.Proxy.HeadTracking;
using VRE.Vridge.API.Client.Messages.v3.Controller;

namespace Vridge.Kinect
{
    public class GamepadSys
    {
        public Controller XInputDevice;
        public bool[] LButtons = new bool[6];
        public bool[] RButtons = new bool[6];
        public int stick = 9;
        private Timer Teemer;
        public float Anglex = 0;
        public float Angley = 0;
        private const int RefreshRate = 60;
        public bool ResetCamera = false;
        public VridgeRemote n_VridgeRemote;
        public MainWindow n_Window;
        
        
        public GamepadSys()
        {
            /* The 'Buttons' Array covers 6 total possible buttons on a generic SteamVR controller. By default, the mappings are done similarly
            to an HTC Vive controller. There are 5 Primary bindings on the Vive controller.
            [0] = Menu Button
            [1] = System Button
            [2] = Trigger Pressed
            [3] = Grip Button
            [4] = Touchpad Clicked (Tactile)
            [5] = Touchpad Touched (Capacitive)
            */
            n_VridgeRemote = null;
            LButtons[0] = false;
            LButtons[1] = false;
            LButtons[2] = false;
            LButtons[3] = false;
            LButtons[4] = false;
            LButtons[5] = false;
            RButtons[0] = false;
            RButtons[1] = false;
            RButtons[2] = false;
            RButtons[3] = false;
            RButtons[4] = false;
            RButtons[5] = false;
            Teemer = new Timer(obj => Update());
        }
        public void Start()
        {
            XInputDevice = new SharpDX.XInput.Controller(SharpDX.XInput.UserIndex.One);
            Teemer.Change(0, 1000 / RefreshRate);

        }
        private void Update()
        {
            XInputDevice.GetState(out var state);
            CalcDir(state);
            Inputs(state);
            
        }

        private void Inputs(State state)
        {


            /* Here, Readings can be changed to whatever is needed. 
            Parameters MUST be a Boolean. If the button will not be used, please map it to 'False'
            [0] = Menu Button
            [1] = System Button
            [2] = Trigger Pressed
            [3] = Grip Button
            [4] = Touchpad Clicked (Tactile)
            [5] = Touchpad Touched (Capacitive)


            The following lines can be used depending on what input you want to use;

            -Digital Buttons: 'state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.Button);'
                Button can be A, B, X, Y, LeftShoulder, RightShoulder, LeftThumb, RightThumb, DPadUp, DPadDown, DPadLeft, DPadRight, Start, Back
            -Any Analog Stick Movement: 'state.Gamepad.LatThumbX != 0 || state.Gamepad.LatThumbY != 0;'
                Lat is Left or Right
            -Specific Left Stick Direction: 'stick == Dir;'
                Dir is 0 for Up, 2 for Right, 4 for Down, 6 for Left
            -Trigger Analog Pull; 'state.Gamepad.LatTrigger != 0;' or 'state.Gamepad.LatTrigger >= bit;'
                Lat is Left or Right
                bit is a byte from 0 to 255
                Use the second line if you want to read a specific threshold
            */
            LButtons[0] = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.X);
            LButtons[1] = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.Back);
            LButtons[2] = state.Gamepad.LeftTrigger != 0;
            LButtons[3] = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.LeftShoulder);
            LButtons[4] = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.LeftThumb);
            LButtons[5] = state.Gamepad.LeftThumbX != 0 || state.Gamepad.LeftThumbY != 0;

            RButtons[0] = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.B);
            RButtons[1] = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.Start);
            RButtons[2] = state.Gamepad.RightTrigger != 0;
            RButtons[3] = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.RightShoulder);
            RButtons[4] = state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.RightThumb);
            RButtons[5] = state.Gamepad.RightThumbX != 0 || state.Gamepad.RightThumbY != 0;

            //Currently, the triggers are read as digital rather than analog. This is done to improve controller compatibility.
            var analogR = System.Convert.ToDouble(RButtons[2]);
            var analogL = System.Convert.ToDouble(LButtons[2]);
            n_VridgeRemote.Controller.SetControllerState(
                true ? 0 : 1,
                HeadRelation.Unrelated,
                true ? HandType.Left : HandType.Right,
                n_Window.quatL,
                n_Window.positionL,
                0,
                0,
                analogL,
                LButtons[0],
                LButtons[1],
                LButtons[2],
                LButtons[3],
                LButtons[4],
                LButtons[5]);
            n_VridgeRemote.Controller.SetControllerState(
                false ? 0 : 1,
                HeadRelation.Unrelated,
                false ? HandType.Left : HandType.Right,
                n_Window.quatR,
                n_Window.positionR,
                0,
                0,
                analogR,
                RButtons[0],
                RButtons[1],
                RButtons[2],
                RButtons[3],
                RButtons[4],
                RButtons[5]);
            
        }
        

        private void CalcDir(State state)
        {
            if (state.Gamepad.LeftThumbX != 0 || state.Gamepad.LeftThumbY != 0)
            {
                
                /*The line below can be used if mappings want to be assigned to individual directions (for left stick only). By default, this is true.
                See ConvertXYtoDirection for bindings.*/
                var dev = true;
                if (dev)
                {
                    var U = state.Gamepad.LeftThumbX;
                    var V = state.Gamepad.LeftThumbY;
                    stick = convertXYtoDirection(U, V);
                    // 0 = UP, 1 = UP-RIGHT, 2 = RIGHT ... 7 = UP-LEFT.
                    switch (stick)
                    {
                        case 0:
                            stick = 0;
                            break;
                        case 2:
                            stick = 2;
                            break;
                        case 4:
                            stick = 4;
                            break;
                        case 6:
                            stick = 6;
                            break;
                        
                    }
                }

            }
            else
            {
                
                stick = 9;
            }
          
        }
        double getAngleFromXY(float XAxisValue, float YAxisValue)
        {

            double angleInRadians = Math.Atan2(XAxisValue, YAxisValue);

            if (angleInRadians < 0.0f) angleInRadians += (Math.PI * 2.0f);

            double angleInDegrees = (180.0f * angleInRadians / Math.PI);

            return angleInDegrees;
        }
        int convertXYtoDirection(float X, float Y)
        {
            
            double sectorSize = 360.0f / 8;

            double halfSectorSize = sectorSize / 2.0f;

            double thumbstickAngle = getAngleFromXY(X, Y);

            double convertedAngle = thumbstickAngle + halfSectorSize;
            
            int direction = (int)Math.Floor(convertedAngle / sectorSize);
            return direction;
        }
        

        
                

    }

}

