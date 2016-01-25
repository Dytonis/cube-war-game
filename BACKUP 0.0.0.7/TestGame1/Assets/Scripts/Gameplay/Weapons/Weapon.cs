using UnityEngine;
using System.Collections;
using Assets.Scripts.Gameplay.Interaction;

namespace Assets.Scripts.Gameplay.Weapons
{
    public class Weapon : MonoBehaviour
    {
        // Use this for initialization
        public Equip EquipManager;
        public float Damage_Head;
        public float Damage_Torso;
        public float Damage_Limbs;
        public float DamageFalloutPer10Percent;
        public float Range;
        public float RateOfFire;
        public float RecoilAmount;
        public Camera PlayerCamera;

        private float cooldownTime = 0f;
        private float currentCooldown = 0f;
        private float recoilTime = 0f;
        private float recoilMaximum = 0.1f;
        private float startTime = 0f;
        private float speed = 60f;
        private Quaternion startRot;

        public bool canFire;
        private Vector3 endPos = Vector3.zero;
        private bool isRecoiling;
        private bool isApplyingCameraRecoil;

        void Start()
        {
            canFire = true;
            cooldownTime = 60 / RateOfFire;
        }

        // Update is called once per frame
        public void Update()
        {
            if (currentCooldown >= cooldownTime)
            {
                canFire = true;
            }
            else
            {
                canFire = false;
                currentCooldown += Time.deltaTime;
            }
            if(isRecoiling)
            {
                recoilTime += Time.deltaTime;
                if(recoilTime >= recoilMaximum)
                {
                    isRecoiling = false;
                    EquipManager.StandingYMouseInput = 0;
                    recoilTime = 0;
                }
            }
            if(isApplyingCameraRecoil)
            {
                float distCovered = (Time.time - startTime) * speed;
                float fracJourney = distCovered / RecoilAmount;
                if(fracJourney >= 0.999)
                {
                    isApplyingCameraRecoil = false;
                }
                EquipManager.CameraContainer.transform.localRotation = Quaternion.Slerp(startRot, Quaternion.Euler(Vector3.zero), fracJourney);
            }
        }

        public virtual void Fire()
        {
            ResetCooldown();
            HandleRecoil();
        }

        public virtual void HandleRecoil()
        {
            EquipManager.velocity += new Vector3(-RecoilAmount / 12, UnityEngine.Random.Range(-RecoilAmount / 20, RecoilAmount / 20));
            EquipManager.CameraContainer.transform.localEulerAngles += new Vector3(-RecoilAmount / 5, UnityEngine.Random.Range(-RecoilAmount / 12, RecoilAmount / 12));
            startRot = EquipManager.CameraContainer.transform.localRotation;
            startTime = Time.time;
            isRecoiling = true;
            isApplyingCameraRecoil = true;
            speed = RecoilAmount * 2;
        }

        protected void ResetCooldown()
        {
            cooldownTime = 60 / RateOfFire;
            currentCooldown = 0;
        }
    }
}
