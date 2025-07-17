using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using Unity.VisualScripting;
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
    [SerializeField] Vector3 randomFightPos;
    bool ischarging;
    [SerializeField]
    private float fightStrength = 10f;

    public static event Action<Fish> OnFishCaught;

    [Space(1)]
    [Header("FightBack")]
    [SerializeField]
    bool isFighting;
    [SerializeField] Vector3 FightTargetpos;
    [SerializeField] float fightDuration = 1f;
    [SerializeField] float fightCooldownTimer = 0;
    [SerializeField] float nextFightDelay;
    [SerializeField] float fightTimer;
    [SerializeField] GameObject debugprefab;
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


    public Rigidbody GetRB()
    {

        return rb;
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
               // Debug.Log("niom nom" + targets[0].name);

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
                    txtAttention.text = "!";
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
                        ischarging=false;
                        txtAttention.text = "<3";
                        randomFightPos =( Planet.Instance.GetPlanetScale() * .5f)* UnityEngine.Random.onUnitSphere;
                        Instantiate(debugprefab, randomFightPos, Quaternion.identity);
                    }
                   

                }

                break;
            case fishbehaviour.baited:
                Debug.DrawLine(transform.position, randomFightPos);
                OrbiterLogic2();

             /*   if (!isFighting)
                {
                    fightCooldownTimer += Time.fixedDeltaTime;
                    if (fightCooldownTimer >= nextFightDelay)
                    {
                        isFighting = true;
                        fightTimer = fightDuration;
                        fightCooldownTimer = 0;
                        nextFightDelay = UnityEngine.Random.Range(1f, 2f);



                    }



                }

                if (isFighting)
                {
                    fightTimer -= Time.fixedDeltaTime;
                    FightBack();
                    if (fightTimer <= 0)
                    {
                        isFighting = false;
                    }


                }*/


                // we will be back here 

                break;
            case fishbehaviour.dead:
                break;
        }
    }


    public static bool IsNear(Vector3 current, Vector3 target, float threshold)
    {
        return Vector3.Distance(current, target) <= threshold;
    }


    private void OrbiterLogic2()
    {

        // lets climb up to the debugs level, probably use lerp
        if (Vector3.Distance(transform.position, randomFightPos) < 2) return;
        Vector3 center = Vector3.zero;

        // "Up" is from planet center to fish
        Vector3 up = (transform.position - center).normalized;

        // Direction from fish to target
        Vector3 toTarget = (randomFightPos - transform.position).normalized;

        // Project movement onto tangent plane (to keep movement on the sphere)
        Vector3 tangentDir = Vector3.ProjectOnPlane(toTarget, up).normalized;

        // Move in the tangent direction (around the sphere)
        Vector3 newPos = rb.position + tangentDir * fishSpeed*.5f * Time.fixedDeltaTime;

        // Re-project to maintain fixed radius from planet center
        Vector3 fixedRadiusPos = center + (newPos - center).normalized * Planet.Instance.GetDistFromPlanet(randomFightPos);
        rb.MovePosition(newPos);

        // Rotate to face movement direction
        Quaternion targetRot = Quaternion.LookRotation(tangentDir, up);
        rb.rotation = Quaternion.Slerp(rb.rotation, targetRot, Time.fixedDeltaTime * 5f);
    }






    private void FightBack()
    {

        //Vector3 planetCenter = Planet.Instance.GetPlanetPos();

        //// Gravity direction at fish's position (towards planet center)
        //Vector3 gravityDir = (planetCenter - transform.position).normalized;

        //// Direction from fish to boat (we’ll orbit around this)
        //Vector3 toBoat = (joint.connectedBody.position - transform.position).normalized;

        //// Get orbit direction (perpendicular to gravity and toBoat)
        //Vector3 orbitDir = Vector3.Cross(gravityDir, toBoat).normalized;

        //// Optional: flip direction randomly every few seconds for variety
        //if (UnityEngine.Random.value < 0.5f)
        //    orbitDir = -orbitDir;

        //// Apply orbiting force along the curved surface
        //rb.AddForce(orbitDir * 3, ForceMode.VelocityChange);

        //// Keep fish submerged (gently correct height if it floats too high)
        //float waterRadius = Planet.Instance.GetPlanetScale() * 0.5f + 0.2f + WaveManager.Instance.GetWaveheight(transform.position.x); // adjust offset
        //float currentRadius = Vector3.Distance(transform.position, planetCenter);
        //float depthError = waterRadius - currentRadius;

        //rb.AddForce(gravityDir * depthError * 2f, ForceMode.Acceleration);


        //// rb.AddForce(  randomForce + (rb.position - joint.connectedBody.position) .normalized * fightStrength, ForceMode.Acceleration);
        //Vector3 toBoat = joint.connectedBody.position - rb.position;
        //Vector3 awayFromBoat = -toBoat.normalized;
        //Vector3 gravdir = (Planet.Instance.GetPlanetPos()- transform.position).normalized;
        //Vector3 tangentDir = Vector3.ProjectOnPlane(UnityEngine.Random.onUnitSphere,gravdir).normalized;
        //Vector3 randomOffset = UnityEngine.Random.insideUnitSphere * 0.2f;
        //Vector3 noise = UnityEngine.Random.insideUnitSphere * 0.05f;
        //tangentDir = Vector3.ProjectOnPlane(tangentDir + noise, gravdir).normalized;

        ////   Vector3 finalDirection = (tangentDir + randomOffset).normalized;
        //Vector3 finalDirection = (tangentDir + randomOffset).normalized;
        //rb.AddForce(finalDirection * fightStrength , ForceMode.VelocityChange);
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