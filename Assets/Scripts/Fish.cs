using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
public class Fish : MonoBehaviour
{
    [SerializeField]
    Transform planet;
    Rigidbody rb;
   public SpringJoint joint;
    [SerializeField]
    float moveRadius;
    [SerializeField]
    Animator animator;
    [SerializeField]
    float fishSpeed;
    [SerializeField]
    Vector3 spawnPos;
    [SerializeField]
    Vector3 nextPos;
    [SerializeField]
    fishbehaviour state= fishbehaviour.idle;
    [SerializeField]

    private float waterOffset=.4f;

    [Space(1)]
    [Header("Bait Behaviour")]
    [SerializeField] Bait bait;
    [SerializeField] LayerMask baitLayer;
    [SerializeField] float timeToTriggerBait;
    [SerializeField] float baitTriggerTimer;
    [SerializeField] TextMeshPro txtAttention;
    [SerializeField] float waitToAttackTime = 1;
    [SerializeField] float waitToAttackTimer;
    bool ischarging;
    [SerializeField]
    private float fightStrength = 10f;

    public static event Action<Fish> OnFishCaught;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        spawnPos = transform.position;
        nextPos = FindNextPosition();
        rb = GetComponent<Rigidbody>();
        joint = GetComponent<SpringJoint>();
        joint.connectedBody = null;
        
        //nextPos = spawnPos +  Random.insideUnitSphere * moveRadius * Random.Range(.5f, 1);
        //if (CheckifNextPositionIsUnderWater())
        //{
        //    nextPos = spawnPos + Random.insideUnitSphere * moveRadius * Random.Range(.5f, 1);

        //}
        //transform.DOLookAt(nextPos, .1f);
        //transform.DOMove(nextPos, 3).OnComplete(() => CheckifNextPositionIsUnderWater());
    }
    private void OnDrawGizmos()
    {
        
        Gizmos.DrawWireSphere(spawnPos, moveRadius);
    }

    bool CheckifNextPositionIsUnderWater()
    {
       var nextPosToPlanetCoreDist =   Vector3.Distance(nextPos, planet.position);
        if(nextPosToPlanetCoreDist > (planet.localScale.y*0.5f + (float )waterOffset))
        {

            return true;

        }


        return false;
    }



    public bool CheckForBait()
    {

        var targets = (Physics.OverlapSphere(transform.position, moveRadius, baitLayer));
            
            
        {
            if (targets.Length > 0)
            {
                Debug.Log("niom nom" + targets[0].name);

                bait = targets[0].GetComponent<Bait>();
                return true ;
            }

        }
        bait = null;
        return false ;
    }

  
    // Update is called once per frame
    void FixedUpdate()
    {




        //public Vector3 targetPosition;
        //public float moveSpeed = 5f;
        //public float waveAmplitude = 1f;
        //public float waveFrequency = 2f;

        //private void Update()
        //{
        //    // Smooth movement towards the target
        //    Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        //    // Add the sine wave to the Y-position
        //    float sineValue = Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;
        //    newPosition.y += sineValue;

        //    // Apply the combined position
        //    transform.position = newPosition;
        //}
  



        switch (state)
        {
            case fishbehaviour.idle:

                var dist = Vector3.Distance(transform.position, nextPos);
                var dir = nextPos -transform.position;
                if(dist >0.1f)
                {
                   // transform.DOLookAt(nextPos,1.5f);
                    rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir.normalized),Time.deltaTime*7); 
                   var dirpos = dir.normalized * fishSpeed * Time.fixedDeltaTime;
                  rb.MovePosition(rb.position +  dirpos);
                }
               
                   
                else
                {
                    nextPos = FindNextPosition();
                }


                if (CheckForBait())
                {
                    baitTriggerTimer += Time.fixedDeltaTime;
                    if (baitTriggerTimer >= timeToTriggerBait)
                    {
                        IdleToAttracted();

                    }

                }

            

                break;
            case fishbehaviour.attracted:

                waitToAttackTimer += Time.fixedDeltaTime;

                if (waitToAttackTimer>= waitToAttackTime && CheckForBait() && !ischarging )
                {
                    Debug.Log("1?");
                    //txtAttention.transform.DOJump(txtAttention.transform.position, 1,1,1);
                 //   txtAttention.transform.DOPunchScale(Vector3.one * 3, 1,1,1);
                    waitToAttackTimer = 0;
                    ischarging = true;
                }

                if (ischarging && CheckForBait())
                {
                    if(Vector3.Distance(transform.position,bait.transform.position) >1f )

                    {
                         
                        Vector3 newpos = Vector3.MoveTowards(transform.position, bait.transform.position, fishSpeed * Time.fixedDeltaTime);
                        var posToNewPos = newpos - transform.position;
                        rb.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(posToNewPos.normalized), Time.fixedDeltaTime * 7);
                        rb.MovePosition( newpos);


                    }
                    else
                    {

                        state = fishbehaviour.baited;
                        waitToAttackTimer = 0;
                        baitTriggerTimer = 0;

                        var joint = GetComponent<SpringJoint>();
                        joint.connectedBody = bait.GetComponent<Rigidbody>();
                        rb.isKinematic = false;
                        OnFishCaught.Invoke(this);
                        bait =null;
                        
                    }
                   

                }

                break;
            case fishbehaviour.baited:

                Vector3 randomForce = new Vector3(
    UnityEngine.Random.Range(-1f, 1f),
       UnityEngine.Random.Range(-0.5f, 0.5f),
       UnityEngine.Random.Range(-1f, 1f)
   ).normalized;
                rb.AddForce(  randomForce + (rb.position - joint.connectedBody.position) .normalized * fightStrength, ForceMode.Acceleration);




                break;
            case fishbehaviour.dead:
                break;
        }
    }

    private void IdleToAttracted()
    {
        //attracted
        baitTriggerTimer = 0;
        txtAttention.gameObject.SetActive(true);
        state = fishbehaviour.attracted;
    }

    private Vector3 FindNextPosition()
    {
       return spawnPos +  UnityEngine.Random.insideUnitSphere * moveRadius * UnityEngine.Random.Range(.8f, 1);
    }

    public enum fishbehaviour
{

    idle,
    attracted,
    baited,
    dead,
}
}