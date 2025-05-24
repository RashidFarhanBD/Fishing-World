using UnityEngine;

public class FauxGravityBody : MonoBehaviour
{
    [SerializeField] Transform planet;
    [SerializeField] float gravity;
    [SerializeField] Rigidbody rb;
   [SerializeField] bool applyGravity= false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created]

   
       
    void Start()
    {
            planet = GameObject.FindWithTag("Planet").transform;
               
    }
    

    // Update is called once per frame
    void Update()
    {
    
    }
    public void ToggleGravity(bool condition)
    {
        applyGravity = condition;
    }
    private void FixedUpdate()
    {
        if (planet != null)
        {
         

            var dir = (planet.position - transform.position).normalized;
            if (applyGravity) rb.AddForceAtPosition((gravity / 1) * dir, transform.position, ForceMode.Acceleration);
            Quaternion targetRot = Quaternion.FromToRotation(transform.up, -dir) * transform.rotation;
           // transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 5 * Time.deltaTime);
              rb.rotation = Quaternion.Slerp(rb.rotation,targetRot, 5 * Time.deltaTime); 
           
        }
    }
}
