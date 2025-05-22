using UnityEngine;

public class FauxGravityBody : MonoBehaviour
{
    [SerializeField] Transform planet;
    [SerializeField] float gravity;
    [SerializeField] Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (planet != null) 
        {
            var dir =( planet.position - transform.position).normalized;
            rb.AddForce (gravity * dir,ForceMode.Force );
            Quaternion targetRot = Quaternion.FromToRotation(transform.up,-dir) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 5* Time.deltaTime);
        
        }
    }
}
