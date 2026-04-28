using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;



public class Animal : MonoBehaviour
{
    public string animalName;
    
 
     public bool playerInRange;
    [SerializeField] int MaxHealth;
    [SerializeField] int currentHealth; 
    public GameObject meatPreFab;
    public GameObject RawHide_Model;
    
    [Header("Sounds")]
     [SerializeField] AudioSource soundChannel;
    [SerializeField] AudioClip rabbitHitScream;
    [SerializeField] AudioClip rabbitDeathScream;

    private Animator animator;
    public bool isDead;

    [SerializeField] ParticleSystem bloodSplashedParticles;

    enum AnimalType
    {
        Rabbit,
        Deer,
        Boar
    }
     [SerializeField] AnimalType thisAnimalType;
     [SerializeField] GameObject bloodPuddle;
    

    void Start()
    {
        currentHealth = MaxHealth;
        animator = GetComponent<Animator>();
    }
    public void TakeDamage(int Damage)
    {
          Debug.Log("TakeDamage called! Damage: " + Damage);
        if(isDead == false)
        {
            currentHealth -= Damage;
             Debug.Log("Blood particles playing: " + bloodSplashedParticles);

            bloodSplashedParticles.Play();
             
            if(currentHealth <= 0)
            {
                 PlayDyingSound();
                
                animator.SetTrigger("DIE");
                GetComponent<AI_Movement>().enabled = false;


                bloodPuddle.SetActive(true);
                isDead = true;
            }
            else
            {
                PlayHitSound();  
            }
        }    
    }

    private void PlayDyingSound()
    {
        switch (thisAnimalType)
        {
            case AnimalType.Rabbit:
                soundChannel.PlayOneShot(rabbitDeathScream);
                break;
            case AnimalType.Deer:
                //soundChannel.PlayOneShot(deerDeathScream);
                break;
            case AnimalType.Boar:
                //soundChannel.PlayOneShot(boarDeathScream);
                break;
            default:
                //soundChannel.PlayOneShot(defaultDeathScream);
                break;
        }
    }
    private void PlayHitSound()
    {
      soundChannel.PlayOneShot(rabbitHitScream);
    }

 private void OnTriggerEnter(Collider other)
    {
        
      if (other.CompareTag("Player"))
        {         
            playerInRange = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
