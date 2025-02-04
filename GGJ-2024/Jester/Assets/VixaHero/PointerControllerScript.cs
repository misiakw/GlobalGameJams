using UnityEngine;

public class PointerControllerScript : MonoBehaviour
{
    public int WhoAmI = 0;
    public GameObject[] pointers;
    private VivaInput input;
    private bool _isInTrigger = false;
    public float rollTime = 3;
    public GameObject _spellObject;
    private GameObject _triggeredObject;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * 360 * rollTime * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _triggeredObject = collision.gameObject;
        
        input.Enable();
        _isInTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        input.Disable();
        _isInTrigger = false;
    }

    private void Awake()
    {
        input = new VivaInput();
    }

    private void OnEnable()
    {
        if (WhoAmI == 1)
            input.Viixa.Key1.performed += ((c) => OnInputPress(1));
        if (WhoAmI == 2)
            input.Viixa.Key2.performed += ((c) => OnInputPress(2));
        if (WhoAmI == 3)
            input.Viixa.Key3.performed += ((c) => OnInputPress(3));
        if (WhoAmI == 4)
            input.Viixa.Key4.performed += ((c) => OnInputPress(4));
    }

    private void OnDisable()
    {
        input.Disable();
    }

    public void OnInputPress(int inp)
    {
        if (_isInTrigger)
        {
            _triggeredObject.gameObject.layer = 6;
            _triggeredObject.gameObject.SetActive(false);
            Debug.Log($"woop: {inp}");
            input.Disable();
        }
    }
}