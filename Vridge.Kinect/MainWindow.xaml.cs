using System.Windows;
using System.Numerics;
using VRE.Vridge.API.Client.Remotes;
using KinectLib.Xbox360;
using KinectLib.XboxOne;
using VRE.Vridge.API.Client.Messages.BasicTypes;
using VRE.Vridge.API.Client.Proxy.HeadTracking;
using VRE.Vridge.API.Client.Messages.v3.Controller;
using static System.Math;
using System;
using System.Windows.Controls;
using System.Diagnostics;
using SharpDX.XInput;
using Windows.UI;
using Windows.Gaming.UI;
using Capabilities = VRE.Vridge.API.Client.Remotes.Capabilities;
using System.IO;

namespace Vridge.Kinect
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Kinect360Manager m_Kinect360Manager;
        private KinectOneManager m_KinectOneManager;
        private VridgeRemote m_VridgeRemote;
        private bool m_Use360Driver;
        private GamepadSys m_Gamepad;
        private bool m_SendLeftPosition = false;
        private bool m_SendLeftRotation = false;
        private bool m_SendRightPosition = false;
        private bool m_SendRightRotation = false;
        private bool m_DisableHeadPosition = false;
        public Quaternion quatL = new System.Numerics.Quaternion(0, 0, 0, 0);
        public Quaternion quatR = new System.Numerics.Quaternion(0, 0, 0, 0);
        public System.Numerics.Vector3? positionL = null;
        public System.Numerics.Vector3? positionR = null;





        public MainWindow()
        {
            m_VridgeRemote = new VridgeRemote("localhost", "Vridge.Kinect", Capabilities.Controllers | Capabilities.HeadTracking);
            
            InitializeComponent();

            Closing += (s, e) => Shutdown();
            Closed += (s, e) => Shutdown();

            KinectTypeCombo.SelectedIndex = m_Use360Driver ? 1 : 0;
            SetToggleValue(SendLeftPosition, m_SendLeftPosition);
            SetToggleValue(SendLeftRotation, m_SendLeftRotation);
            SetToggleValue(SendRightPosition, m_SendRightPosition);
            SetToggleValue(SendRightRotation, m_SendRightRotation);
            SetToggleValue(DisableHeadPosition, m_DisableHeadPosition);
            m_Gamepad = new GamepadSys();
            
            Debug.WriteLine("Initialize Check");
            
        }

        private void Start()
        {
            Shutdown();
            
            Debug.WriteLine("Sanity Check");
            var UseGamepad = false;
            var connected = false;
            m_Gamepad.Start();
            m_Gamepad.n_VridgeRemote = m_VridgeRemote;
            m_Gamepad.n_Window = this;
            if (m_Gamepad.XInputDevice.IsConnected)
            {
                UseGamepad = true;
                Debug.WriteLine("Controller Connected");
            }
            
            if (m_Use360Driver)
            {
                m_Kinect360Manager = new Kinect360Manager();

                if (m_Kinect360Manager.Start())
                {
                    m_Kinect360Manager.NewTrackingData += OnKinect360Data;
                    connected = true;
                   
                }
            }
            else
            {
                m_KinectOneManager = new KinectOneManager();

                if (m_KinectOneManager.Start())
                {
                    m_KinectOneManager.NewTrackingData += OnKinectOneData;
                    connected = true;
                }
            }

            ConnectionStatus.Text = connected ? "Connected" : "Not Connected";
        }

       

        private void Shutdown()
        {
            if (m_Kinect360Manager != null)
            {
                m_Kinect360Manager.Stop();
                m_Kinect360Manager = null;
            }

            if (m_KinectOneManager != null)
            {
                m_KinectOneManager.Stop();
                m_KinectOneManager = null;
            }

            ConnectionStatus.Text = "Not Connected";
        }
        //", VRE.Vridge.API.Client.Remotes.HeadRemote nam" goes in line below after "data obj"
        private void OnKinectOneData(KinectLib.XboxOne.TrackingData obj)
        {

            SendHeadData(ref obj.HeadPosition);
            //", nam.GetCurrentPose()" goes in the following 2 lines below after "Transform"
            SendHandData(true, ref obj.LeftTransform, m_VridgeRemote.Head.GetCurrentPose(), m_DisableHeadPosition);
            SendHandData(false, ref obj.RightTransform, m_VridgeRemote.Head.GetCurrentPose(), m_DisableHeadPosition);
        }
        //", VRE.Vridge.API.Client.Remotes.HeadRemote nam" goes in line below after "data obj"
        private void OnKinect360Data(KinectLib.Xbox360.TrackingData obj)
        {

            SendHeadData(ref obj.HeadPosition);
            //", nam.GetCurrentPose()" goes in the following 2 lines below after "Transform"
            SendHandData(true, ref obj.LeftTransform, m_VridgeRemote.Head.GetCurrentPose(), m_DisableHeadPosition);
            SendHandData(false, ref obj.RightTransform, m_VridgeRemote.Head.GetCurrentPose(), m_DisableHeadPosition);
        }

        private void SendHeadData(ref float[] data)
        {
            if (m_VridgeRemote.Head == null)
                return;
           m_VridgeRemote.Head.SetPosition(data[0], data[1], data[2]);
           
        }
        //", float[] headData" goes in the line below after "data"
        private void SendHandData(bool left, ref float[] data, float[] headData, bool head)
        {
            float extra = 0;
            if (m_VridgeRemote.Controller == null)
                return;

            if (left && !m_SendLeftPosition && !m_SendLeftRotation)
                return;

            if (!left && !m_SendRightPosition && !m_SendRightRotation)
                return;
            
            var analogR = System.Convert.ToDouble(m_Gamepad.RButtons[2]);
            var analogL = System.Convert.ToDouble(m_Gamepad.LButtons[2]);

            
            // var quat = new System.Numerics.Quaternion(0, 0, 0, 0);
            if (head)
            {
                extra = .45f;
            }   
            else
            {
                extra = 0f;
            }

            if (left && m_SendLeftPosition)// || !left && m_SendRightPosition)
                positionL = new System.Numerics.Vector3(data[0], data[1], data[2]);

            if (!left && m_SendRightPosition)
                positionR = new System.Numerics.Vector3(data[0], data[1], data[2]);

            if (left && m_SendLeftRotation)// || !left && m_SendRightRotation)
            {
                quatL = new System.Numerics.Quaternion(data[3], data[4], data[5], data[6]);
            }
            else if (left)
            {
                // Convert.ToDouble(headData);
                Single quatW = Convert.ToSingle(Math.Sqrt(1 + headData[0] + headData[5] + headData[10]) / 2);
                // Convert.ToSingle(webby);
                //Convert.ToSingle(headData);
                quatL = new System.Numerics.Quaternion (((headData[9] - headData[6]) / (quatW * 4) + extra), (((headData[2] - headData[8]) / (quatW * 4))), (headData[4] - headData[1]) / (quatW * 4), quatW);
            }


            if (!left && m_SendRightRotation)
            {
                quatR = new System.Numerics.Quaternion(data[3], data[4], data[5], data[6]);
            }
            else if (!left)
            {
                // Convert.ToDouble(headData);
                Single quatW = Convert.ToSingle(Math.Sqrt(1 + headData[0] + headData[5] + headData[10]) / 2);
                // Convert.ToSingle(webby);
                //Convert.ToSingle(headData);
                quatR = new System.Numerics.Quaternion(((headData[9] - headData[6]) / (quatW * 4) + extra), (((headData[2] - headData[8]) / (quatW * 4))), (headData[4] - headData[1]) / (quatW * 4), quatW);
            }



            if (left)
            {

                m_VridgeRemote.Controller.SetControllerState(
                left ? 0 : 1,
                HeadRelation.Unrelated,
                left ? HandType.Left : HandType.Right,
                quatL,
                positionL,
                0,
                0,
                analogL,
                m_Gamepad.LButtons[0],
                m_Gamepad.LButtons[1],
                m_Gamepad.LButtons[2],
                m_Gamepad.LButtons[3],
                m_Gamepad.LButtons[4],
                m_Gamepad.LButtons[5]);
                
            }
            else
            {
                m_VridgeRemote.Controller.SetControllerState(
                left ? 0 : 1,
                HeadRelation.Unrelated,
                left ? HandType.Left : HandType.Right,
                quatR,
                positionR,
                0,
                0,
                analogR,
                m_Gamepad.RButtons[0],
                m_Gamepad.RButtons[1],
                m_Gamepad.RButtons[2],
                m_Gamepad.RButtons[3],
                m_Gamepad.RButtons[4],
                m_Gamepad.RButtons[5]);
           
            }
        
            
        }

        private void OnConnectClicked(object sender, RoutedEventArgs e) => Start();
        private void OnShutdownClick(object sender, RoutedEventArgs e) => Shutdown();
        

        private void KinectTypeCombo_Selected(object sender, RoutedEventArgs e)
        {
            var combo = sender as ComboBox;

            if (combo == null)
                return;

            m_Use360Driver = combo.SelectedIndex == 1;

            if (m_Kinect360Manager != null || m_KinectOneManager != null)
            {
                Shutdown();
                Start();
            }
        }
        
        private void SendLeftPosition_Click(object sender, RoutedEventArgs e)
        {
            HandleCheckbox(sender, ref m_SendLeftPosition);
        }

        private void SendLeftRotation_Click(object sender, RoutedEventArgs e)
        {
            HandleCheckbox(sender, ref m_SendLeftRotation);
        }

        private void SendRightPosition_Click(object sender, RoutedEventArgs e)
        {
            HandleCheckbox(sender, ref m_SendRightPosition);
            
        }

        private void SendRightRotation_Click(object sender, RoutedEventArgs e)
        {
            HandleCheckbox(sender, ref m_SendRightRotation);
        }

        private void DisableHeadPosition_Click(object sender, RoutedEventArgs e)
        {
            HandleCheckbox(sender, ref m_DisableHeadPosition);
        }

        private void SetToggleValue(CheckBox box, bool value) => box.IsChecked = value;

        private void HandleCheckbox(object sender, ref bool target)
        {
            var checkbox = (CheckBox)sender;

            if (checkbox.IsChecked.HasValue)
                target = checkbox.IsChecked.Value;
            else
                target = false;
        }

        
    }
}

