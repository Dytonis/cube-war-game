using UnityEngine;
using System.Collections;

public class DayCycle : MonoBehaviour
{
    public float speed = 0.1f;
	// Update is called once per frame
	void Update ()
    {
        transform.Rotate((speed * Time.deltaTime), 0f, 0f);
	}
}
