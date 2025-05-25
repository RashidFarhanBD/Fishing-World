using UnityEngine;
using UnityEngine.InputSystem;
public class FishingRod : MonoBehaviour
{
    float throwDist;
    bool baitThrown;
    public Bait baitPrefab;
    [SerializeField]LineRenderer lineRenderer;
    [SerializeField] Rigidbody rigid ;
    [SerializeField] Bait oldBait;
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
                if (oldBait)
                {
                    oldBait.SetBaitDead();
                }
                Debug.Log("throow");

                var baitObj = Instantiate  (baitPrefab, transform.position + transform.forward* 3,Quaternion.identity);
              var rbBait =   baitObj.GetComponent<Rigidbody>();
                rbBait.AddForce( transform.parent.forward * 50);
                baitObj.InitBait(lineRenderer, this.transform, rigid);
                oldBait = baitObj;
            }
        }
    }
}
