using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Gameplay.Weapons
{
    public class Weapon_M1 : ProjectileWeapon
    {
        // Use this for initialization

        Animator anim;

        void Start()
        {
            anim = GetComponent<Animator>();
        }

        public override void Fire()
        {
            if (canFire && CurrentAmmo > 0 && IsReloadingAnimation == false)
            {
                RaycastHit hit;
                int lm = LayerMask.GetMask("Self");
                Physics.Raycast(new Ray(PlayerCamera.transform.position, PlayerCamera.transform.forward), out hit, Range, ~lm);
                Debug.DrawRay(PlayerCamera.transform.position, PlayerCamera.transform.forward * Range, Color.red, 5f);

                if (Flare != null)
                {
                    Flare flare = Instantiate(Flare, MuzzlePosition.position, MuzzlePosition.rotation) as Flare;
                    flare.transform.SetParent(gameObject.transform);
                }

                if(hit.transform != null)
                {
                    Impact.Splash s = Instantiate(Splash, hit.point, Quaternion.Euler(Vector3.zero)) as Impact.Splash;
                    s.Deploy(hit.normal, Damage_Torso);
                }

                if (CurrentAmmo > 1)
                {
                    anim.SetTrigger("Fire");
                }
                else if (CurrentAmmo == 1)
                {
                    anim.SetTrigger("FireLast");
                }

                CurrentAmmo -= AmmoPerShot;
                base.Fire();
            }
        }
        public new void Update()
        {
            base.Update();

            if (IsReloadingAnimation || IsReloading)
                anim.applyRootMotion = false;
            else
                anim.applyRootMotion = true;
        }
    }
}
