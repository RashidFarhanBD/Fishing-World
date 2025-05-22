using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [SerializeField]float offset=0;
    [SerializeField]private float speed=1;
    [SerializeField] private float amplitude=1;
    [SerializeField] private float lenght=2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        offset += Time.deltaTime * speed;
    }

    public float GetWaveheight(float _x)
    {

        return amplitude * Mathf.Sin(_x / lenght + offset);
    }
}
