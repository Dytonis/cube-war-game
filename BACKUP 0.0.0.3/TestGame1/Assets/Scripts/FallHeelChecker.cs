using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class FallHeelChecker : MonoBehaviour
{
    private FirstPersonController controller;
    public GameObject Heel1;
    public GameObject Heel2;
    public GameObject Heel3;
    public GameObject Heel4;

    void Start()
    {
        controller = GetComponent<FirstPersonController>();
    }

    RaycastHit hit1;
    RaycastHit hit2;
    RaycastHit hit3;
    RaycastHit hit4;
    public int triggers;
    // Update is called once per frame
    void Update ()
    {
        triggers = 0;
        Physics.Raycast(new Ray(Heel1.transform.position, Vector3.down), out hit1, 0.05f, 1 >> 10);
        Physics.Raycast(new Ray(Heel2.transform.position, Vector3.down), out hit2, 0.05f, 1 >> 10);
        Physics.Raycast(new Ray(Heel3.transform.position, Vector3.down), out hit3, 0.05f, 1 >> 10);
        Physics.Raycast(new Ray(Heel4.transform.position, Vector3.down), out hit4, 0.05f, 1 >> 10);

        if (hit1.transform)
            triggers++;
        if (hit2.transform)
            triggers++;
        if (hit3.transform)
            triggers++;
        if (hit4.transform)
            triggers++;

        if(triggers >= 2)
        {
            controller.CanFall = false;
        }
        else
        {
            controller.CanFall = true;
        }
    }
}
