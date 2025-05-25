using UnityEngine;

public class Billboarding : MonoBehaviour
{
    Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (cam != null)
        {

            transform.rotation = cam.transform.rotation;    

        }
    }
}
