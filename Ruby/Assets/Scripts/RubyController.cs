using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class RubyController : MonoBehaviour
{
    [Header("physics")]
    Rigidbody2D rigidbody2d;
    public float speed = 5f;
    float horizontal;
    float vertical;

    [Header("health")]
    public int maxHealth = 5;
    int currentHealth;
    public int health { get { return currentHealth; } }

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    [Header("animator")]
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    [Header("projectile")]
    public GameObject projectilePrefab;
    [SerializeField]
    bool _fire = false;
    public bool fire { get { return _fire; } set { _fire = value; }}

    [Header("talk")]
    bool talk = false;

    [Header("audio")]
    AudioSource audioSource;
    public AudioClip shotClip;
    public AudioClip damageClip;

    [Header("Mobile Controls")]
    public RubyInput rubyInputAction; // For movement
    public InputAction movement;

    private void Awake()
    {
        rubyInputAction = new RubyInput();
    }

    private void OnEnable()
    {
        movement = rubyInputAction.Ruby.Move;
        movement.Enable();
    }
    private void OnDisable()
    {
        movement.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        //QualitySettings.vSyncCount = 1;
        //Application.targetFrameRate = 60;
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }
    void Update()
    {
        GetInput();
        CheckInvincible();
        SetAnimator();
        if (fire)
        {
            Launch();
            fire= false;
        }
        if (talk)
        {
            Talk();
            talk=false;
        }
    }


    void GetInput()
    {
#if UNITY_ANDROID
        // Use the joystick's input on Android devices
        Vector2 move = movement.ReadValue<Vector2>();
        horizontal = move.x;
        vertical = move.y;

#else
        // Use keyboard input on non-Android devices
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        // Check if the fire key is pressed on non-Android devices
        fire = Input.GetButtonDown("Fire1");
        talk = Input.GetKeyDown(KeyCode.X);
#endif
    }


    void SetAnimator()
    {
        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
    }

    void Move()
    {
        Vector2 position = rigidbody2d.position;
        position.x += horizontal * speed * Time.deltaTime;
        position.y += vertical * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);

    }

    void CheckInvincible()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;
            animator.SetTrigger("Hit");
            isInvincible = true;
            invincibleTimer = timeInvincible;
            PlayAudio(damageClip);
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        //Debug.Log("HEALTH IS: " + currentHealth + "/" + maxHealth);
        float currentHealthPercent = (float)currentHealth / maxHealth;
        UIHealthBar.Singleton.SetValue(currentHealthPercent);
        //GameObject canvas = GameObject.Find("HealthUI");
        //UIHealthBar uiHealthBar = canvas.GetComponent<UIHealthBar>();
        //Debug.Log("currentHealthPercent" + currentHealthPercent);
        //uiHealthBar.SetValue(currentHealthPercent);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        PlayAudio(shotClip);
    }

    void Talk()
    {
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
        if (hit.collider != null)
        {
            NonPlayerCharacter NPC = hit.collider.GetComponent<NonPlayerCharacter>();
            if (NPC != null)
            {
                NPC.DisplayDialog();
            }
            Debug.Log("Raycast has hit the object " + hit.collider.gameObject);
        }
    }

    public void PlayAudio(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    void ShowTime()
    {
        // debug use only
        Debug.Log(1.0f / Time.deltaTime);
        DateTime currentTime = DateTime.Now;
        Debug.Log("CurrentTime: " + currentTime);
    }


    public void OnFireButtonClicked()
    {
        fire = true;
    }

    public void OnTalkButtonClicked()
    {
        talk = true;
    }
}
