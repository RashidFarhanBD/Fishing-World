using UnityEngine;

public class ScaleOscilation : MonoBehaviour
{

    Vector3 initialScale;
    float offset;
    float multiplier = .03f; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

        var h= WaveManager.Instance.GetWaveheight(transform.position.x);
        transform.localScale = initialScale + new Vector3(h,h,h) * multiplier;    


    }
}
