using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;

    private GameObject currentFloor;
    private GameObject oldFloor;


    [SerializeField] private int Hp;
    [SerializeField] GameObject HpBar;

    [SerializeField]  Text  scoreText;
    private int score;

    private float scoreTime;

    private Animator anim;

    private SpriteRenderer render;

    private AudioSource deathSound;

    [SerializeField] private GameObject replayButton;
    // Start is called before the first frame update
    void Start()
    {
        Hp = 10;
        score = 0;
        scoreTime = 0f;
        anim = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        deathSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(moveSpeed*Time.deltaTime,0,0);
            render.flipX =false;
            anim.SetBool("run",true);

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-moveSpeed*Time.deltaTime,0,0);
            render.flipX = true;
            anim.SetBool("run",true);

        }
        else
        {
            anim.SetBool("run",false);

        }
        UpdateScore();

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Normal")
        {
            if (other.contacts[0].normal == new Vector2(0f, 1f))
            {
                Debug.Log("this is floor 1");
                currentFloor = other.gameObject;
                ModifyHp(1);
                other.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else if (other.gameObject.tag == "Nails")
        {
            if (other.contacts[0].normal == new Vector2(0f, 1f))
            {
                Debug.Log("this is NAIL!!!!");
                currentFloor = other.gameObject;
                ModifyHp(-3);
                anim.SetTrigger("hurt");
                other.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else if (other.gameObject.tag == "Celing")
        {
            Debug.Log("hit celing");
            currentFloor.GetComponent<BoxCollider2D>().enabled = false;
            ModifyHp(-3); 
            anim.SetTrigger("hurt");
            other.gameObject.GetComponent<AudioSource>().Play();
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    { 
        if (other.gameObject.tag == "DeathLine")
        {
            Debug.Log("you are dead");
            die();

        }
    }

    void ModifyHp(int num)
    {
        Hp += num;
        if (Hp > 10)
        {
            Hp = 10;
        }
        else if (Hp <= 0)
        {
            Hp = 0;
            die();
        }
        UpdateHpBar();
    }

    void UpdateHpBar()
    {
        for (int i = 0; i < HpBar.transform.childCount; i++)
        {
            if (Hp > i)
            {
                HpBar.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                HpBar.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    void UpdateScore()
    {
        scoreTime += Time.deltaTime;
        if (scoreTime > 2f)
        {
            score++;
            scoreTime = 0f;
            scoreText.text = "LEVEL" + score.ToString();
        }
    }

    void die()
    {
        deathSound.Play();
        Time.timeScale = 0f;
        replayButton.SetActive(true);
    }

    public void Replay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }
}
