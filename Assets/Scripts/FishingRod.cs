using UnityEngine;
using UnityEngine.InputSystem;
public class FishingRod : MonoBehaviour
{
    float throwDist;
    bool baitThrown;
    public Bait bait;
    [SerializeField]LineRenderer lineRenderer;
    [SerializeField] Rigidbody rigid ;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         lineRenderer = GetComponent<LineRenderer>();
         rigid = GetComponent<Rigidbody>() ;
        
    }

    // Update is called once per frame
    void Update()
    {
        var mouse = Mouse.current;
        if (mouse.leftButton.wasPressedThisFrame)
        {
            {

                Debug.Log("throow");

                var obj = Instantiate  (bait, transform.position + transform.forward* 3,Quaternion.identity);
              var rbObj =   obj.GetComponent<Rigidbody>();
                rbObj.AddForce( transform.parent.forward * 50);
                obj.InitBait(lineRenderer, this.transform, rigid);
            }
        }
    }
}
