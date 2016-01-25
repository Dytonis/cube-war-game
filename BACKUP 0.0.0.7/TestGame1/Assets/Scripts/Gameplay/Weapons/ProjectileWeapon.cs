using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Weapons
{
    public class ProjectileWeapon : Weapon
    {
        public GameObject EjectionAngle;
        public Effects.Shell Shell;
        public Effects.Shell Clip;
        public Effects.Shell ClipFull;
        public Flare Flare;
        public Transform MuzzlePosition;
        public Impact.Splash Splash;
        public int MaxAmmo;
        public int CurrentAmmo;
        public int AmmoPerShot;
        public float AmmoReloadTime;
        public float AmmoReloadAnimationTime;
        public bool IsReloading;
        public bool IsReloadingAnimation;

        public void AnimationTriggerAutoReload(float plus)
        {
            IsReloadingAnimation = true;
            IsReloading = true;
            t1 = 0f - plus;
            t2 = 0f - plus;
        }

        public virtual void Reload()
        {
            CurrentAmmo = MaxAmmo;
        }

        public void ReloadTimeComplete()
        {
            Reload();
            IsReloading = false;
        }

        public void ReloadAnimationTimeComplete()
        {
            IsReloadingAnimation = false;
        }

        private float t1;
        private float t2;
        public new void Update()
        {
            base.Update();
            if (IsReloading)
            {
                if (t1 >= AmmoReloadTime)
                {
                    ReloadTimeComplete();
                }
                else
                {
                    t1 += Time.deltaTime;
                }
            }
            if (IsReloadingAnimation)
            {
                if (t2 >= AmmoReloadAnimationTime)
                {
                    ReloadAnimationTimeComplete();
                }
                else
                {
                    t2 += Time.deltaTime;
                }
            }
        }
    }
}
