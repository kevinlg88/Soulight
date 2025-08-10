using UnityEngine;

public class LookingCamera : MonoBehaviour
{
    void Update()
    {
        transform.forward = Camera.main.transform.position;
    }
}
