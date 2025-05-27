using Unity.Cinemachine;
using UnityEngine;

public class Orbitter : MonoBehaviour
{
    public float speed;
    Rigidbody body;
    [Range(-20,20)]
    [SerializeField]private float offset = 20;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody>();   
        
    }

    // Update is called once per frame



    private void Update()
    {
        float desiredDistanceFromCenter = Planet.Instance.transform.localScale.y * 0.5f + offset;
        Vector3 up = (transform.position - Planet.Instance.GetPlanetPos()).normalized;
        Vector3 targetPosition = Planet.Instance.GetPlanetPos() + up * desiredDistanceFromCenter;
        Vector3 correctedPos = Vector3.Lerp(body.position, targetPosition, 0.1f);
        Vector3 forward = Vector3.Cross(transform.right, up).normalized;
        Vector3 move = forward * speed * Time.deltaTime;
        body.MovePosition(correctedPos + move);

    //    body.AddForce(  (( correctedPos + forward) - transform.position).normalized * speed * Time.deltaTime);
        
        //  body.MovePosition(body.position + (forward*1).normalized *speed*Time.deltaTime);





    }
}
