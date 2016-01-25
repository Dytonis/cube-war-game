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
                    for (int i = 0; i < 2; i++)
                    {
                        Flare flare = Instantiate(Flare, MuzzlePosition.position, MuzzlePosition.rotation) as Flare;
                        flare.transform.SetParent(gameObject.transform);
                    }
                }
                if (Shell != null)
                {
                    Vector3 direction = new Vector3(EjectionAngle.transform.forward.x * 10 + Random.Range(-2, 2), EjectionAngle.transform.forward.y * 10 + Random.Range(-5, 5), EjectionAngle.transform.forward.z * 10 + Random.Range(-5, 5));
                    Effects.Shell shell = Instantiate(Shell, EjectionAngle.transform.position, transform.rotation) as Effects.Shell;
                    shell.GetComponent<Rigidbody>().AddForce(direction);
                    shell.Deploy();
                }

                if (hit.transform != null)
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
                    if (Clip != null)
                    {
                        Vector3 direction = new Vector3(EjectionAngle.transform.forward.x * 20 + Random.Range(-2, 2), EjectionAngle.transform.forward.y * 20 + Random.Range(-5, 5), EjectionAngle.transform.forward.z * 20 + Random.Range(-5, 5));
                        Effects.Shell shell = Instantiate(Clip, EjectionAngle.transform.position, transform.rotation) as Effects.Shell;
                        shell.GetComponent<Rigidbody>().AddForce(direction);
                        shell.Deploy();
                    }
                }

                CurrentAmmo -= AmmoPerShot;
                base.Fire();
            }
        }
        public new void Update()
        {
            base.Update();

            if (Input.GetButtonDown("Reload") && IsReloading == false && IsReloadingAnimation == false)
            {
                anim.SetTrigger("Reload");
                IsReloading = true;
                IsReloadingAnimation = true;                
            }
        }

        public void ClipEjectAuto()
        {
            if (Clip != null)
            {
                Effects.Shell shell = Instantiate(ClipFull, EjectionAngle.transform.position, transform.rotation) as Effects.Shell;
                shell.GetComponent<Rigidbody>().AddForce(EjectionAngle.transform.forward * 5);
                shell.Deploy();
            }
        }
    }
}
