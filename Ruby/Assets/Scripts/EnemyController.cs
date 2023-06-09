using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    public float speed = 3.0f;
    public bool vertical;
    public float changeTime = 3.0f;

    Rigidbody2D rigidbody2d;
    float timer;
    int direction = 1;

    Animator animator;

    bool broken = true;

    public ParticleSystem smokeEffect;

    AudioSource audioSource;
    public AudioClip fixedClip;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        SetTimer();
    }

    void FixedUpdate()
    {
        if (broken) { Patrol(); }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //foreach (var prop in collision.GetType().GetProperties())
        //{
        //    Debug.Log(prop.Name + ": " + prop.GetValue(collision, null));
        //}
        RubyController player = collision.gameObject.GetComponent<RubyController>();
        if (player != null) { player.ChangeHealth(-1); }
    }

    void SetTimer()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    void Patrol()
    {
        Vector2 position = rigidbody2d.position;

        if (vertical)
        {
            animator.SetFloat("moveX", 0);
            animator.SetFloat("moveY", direction);
            position.y = position.y + Time.deltaTime * speed * direction;
        }
        else
        {
            animator.SetFloat("moveX", direction);
            animator.SetFloat("moveY", 0);
            position.x = position.x + Time.deltaTime * speed * direction;
        }

        rigidbody2d.MovePosition(position);
    }

    public void Fix()
    {
        broken = false;
        animator.SetTrigger("fixed");
        smokeEffect.Stop();
        audioSource.Stop();
        audioSource.PlayOneShot(fixedClip);
    }
}

