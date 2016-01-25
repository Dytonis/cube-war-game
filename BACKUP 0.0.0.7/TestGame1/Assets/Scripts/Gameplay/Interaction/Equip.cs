using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Gameplay.Weapons;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.Scripts.Gameplay.Interaction
{
    public class Equip : MonoBehaviour
    {
        public FirstPersonController MouseController;
        public GameObject Holster;
        public Weapon_M1 Weapon;
        public GameObject Equipped;
        public GameObject CameraContainer;
        public Camera POVCamera;
        public Camera PlayerCamera;
        public Vector3 DefaultEquipRotation;
        public Vector3 DefaultEquipPosition;
        public Vector3 ADSEquipRotation;
        public Vector3 ADSEquipPosition;
        public float StandingXMouseInput;
        public float StandingYMouseInput;
        public float ADSPOVZoom;
        public float ADSMainZoom;
        public float ADSTime;
        public float ADSMouseSensitivityDrop;

        public bool ADSEnabled;
        public bool SwayEnabled;

        [HideInInspector]
        public Vector3 velocity = Vector3.zero;
        private Vector3 standingRotation = Vector3.zero;
        private float yChange = 0f;
        private float xChange = 0f;
        private float speedX = 0f;
        private float speedY = 0f;
        private float leftX = 0f;
        private float leftY = 0f;
        private float defaultPOVZoom = 55f;
        private float defaultMainZoom = 60f;

        private float mouseSensDropped;
        private float defaultMouseSensitivity;
        private float ADSTimeStart = 0;
        private float j = 0;
        private Vector3 startPos;
        private Vector3 startRot;
        private float startPOV;
        private float startMain;
        private float startSens;
        private bool latch;
        // Update is called once per frame
        void Update()
        {
            if (SwayEnabled)
            {
                xChange = Input.GetAxis("Mouse X") + StandingXMouseInput;
                yChange = Input.GetAxis("Mouse Y") * -1 - StandingYMouseInput;

                leftX = Difference(xChange, velocity.y);
                leftY = Difference(yChange, velocity.x);
                speedX = Mathf.Abs(leftX * 0.1f);
                speedY = Mathf.Abs(leftY * 0.1f);

                if (Mathf.Abs(leftX) < 0.001)
                    leftX = 0;
                if (Mathf.Abs(leftY) < 0.001)
                    leftY = 0;
                if (Mathf.Abs(speedX) < 0.001)
                    speedX = 0;
                if (Mathf.Abs(speedY) < 0.001)
                    speedY = 0;

                if (velocity.y < xChange)
                    velocity.y += speedX;
                if (velocity.y > xChange)
                    velocity.y -= speedX;
                if (velocity.x < yChange)
                    velocity.x += speedY;
                if (velocity.x > yChange)
                    velocity.x -= speedY;

                Holster.transform.localEulerAngles = velocity + standingRotation;
            }
            else if (Holster.transform.rotation.eulerAngles != DefaultEquipRotation)
                Holster.transform.rotation = Quaternion.Euler(DefaultEquipRotation);

            if (ADSEnabled)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    ADSTimeStart = Time.time;
                    j = Vector3.Distance(Holster.transform.localPosition, ADSEquipPosition);
                    startPos = Holster.transform.localPosition;
                    startRot = Holster.transform.localRotation.eulerAngles;
                    startPOV = POVCamera.fieldOfView;
                    startMain = PlayerCamera.fieldOfView;
                    startSens = MouseController.m_MouseLook.XSensitivity;
                }
                if (Input.GetMouseButtonUp(1))
                {
                    ADSTimeStart = Time.time;
                    j = Vector3.Distance(Holster.transform.localPosition, DefaultEquipPosition);
                    startPos = Holster.transform.localPosition;
                    startRot = Holster.transform.localRotation.eulerAngles;
                    startPOV = POVCamera.fieldOfView;
                    startMain = PlayerCamera.fieldOfView;
                    startSens = MouseController.m_MouseLook.XSensitivity;
                }
                //ADS
                if (Input.GetMouseButton(1))
                {
                    if (!VectorApprox(Holster.transform.localPosition, ADSEquipPosition))
                    {
                        mouseSensDropped = defaultMouseSensitivity * ((100 - ADSMouseSensitivityDrop) / 100);

                        float distCovered = (Time.time - ADSTimeStart) * ADSTime;
                        float fracJourney = distCovered / j;
                        Holster.transform.localPosition = Vector3.Slerp(startPos, ADSEquipPosition, fracJourney);
                        standingRotation = Quaternion.Slerp(Quaternion.Euler(startRot), Quaternion.Euler(ADSEquipRotation), fracJourney).eulerAngles;
                        POVCamera.fieldOfView = Mathf.Lerp(startPOV, ADSPOVZoom, fracJourney);
                        PlayerCamera.fieldOfView = Mathf.Lerp(startMain, ADSMainZoom, fracJourney);
                        MouseController.m_MouseLook.XSensitivity = Mathf.Lerp(startSens, mouseSensDropped, fracJourney);
                        MouseController.m_MouseLook.YSensitivity = Mathf.Lerp(startSens, mouseSensDropped, fracJourney);
                    }
                }
                //ADS RETURN
                if (!Input.GetMouseButton(1))
                {
                    if (!VectorApprox(Holster.transform.localPosition, DefaultEquipPosition))
                    {
                        mouseSensDropped = defaultMouseSensitivity * ((100 - ADSMouseSensitivityDrop) / 100);

                        float distCovered = (Time.time - ADSTimeStart) * ADSTime;
                        float fracJourney = distCovered / j;
                        Holster.transform.localPosition = Vector3.Slerp(startPos, DefaultEquipPosition, fracJourney);
                        standingRotation = Quaternion.Slerp(Quaternion.Euler(startRot), Quaternion.Euler(DefaultEquipRotation), fracJourney).eulerAngles;
                        POVCamera.fieldOfView = Mathf.Lerp(startPOV, defaultPOVZoom, fracJourney);
                        PlayerCamera.fieldOfView = Mathf.Lerp(startMain, defaultMainZoom, fracJourney);
                        MouseController.m_MouseLook.XSensitivity = Mathf.Lerp(startSens, defaultMouseSensitivity, fracJourney);
                        MouseController.m_MouseLook.YSensitivity = Mathf.Lerp(startSens, defaultMouseSensitivity, fracJourney);
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                Weapon.Fire();
            }
        }

        public void Start()
        {
            defaultMouseSensitivity = MouseController.m_MouseLook.XSensitivity;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        float Larger(float a, float b)
        {
            if (a > b)
                return a;
            else return b;
        }
        float Lesser(float a, float b)
        {
            if (a < b)
                return a;
            else return b;
        }
        float Difference(float a, float b)
        {
            return Larger(a, b) - Lesser(a, b);
        }

        bool VectorApprox(Vector3 a, Vector3 b)
        {
            bool returns = true;

            if (Mathf.Abs(a.x - b.x) > 0.001)
                returns = false;
            if (Mathf.Abs(a.y - b.y) > 0.001)
                returns = false;
            if (Mathf.Abs(a.z - b.z) > 0.001)
                returns = false;

            return returns;
        }
    }
}
