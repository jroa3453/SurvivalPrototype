using UnityEngine;
using System.Collections;

public class Campfire : MonoBehaviour
{
    public static Campfire Instance;
    public bool playerInRange;
    private bool isCooking = false;

    [Header("Fire Effects")]
    public ParticleSystem fireParticles;
    public GameObject smokeEffect;
    public GameObject newLightEffect;
    public bool isLit = false;

    [Header("Flicker Light")]
    public Light fireLight;
    public float minIntensity = 2f;
    public float maxIntensity = 4f;
    public float flickerSpeed = 0.1f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (fireParticles != null) fireParticles.Stop();
        if (smokeEffect != null) smokeEffect.SetActive(false);
        if (newLightEffect != null) newLightEffect.SetActive(false);
        if (fireLight != null) fireLight.enabled = false;
    }

    public void LightFire()
    {
        isLit = true;
        if (fireParticles != null) fireParticles.Play();
        if (fireLight != null) fireLight.enabled = true;
        if (smokeEffect != null) smokeEffect.SetActive(false);
        if (newLightEffect != null) newLightEffect.SetActive(true);
        
    }

    public void ExtinguishFire()
    {
        isLit = false;
        if (fireParticles != null) fireParticles.Stop();
        if (fireLight != null) fireLight.enabled = false;
        if (newLightEffect != null) newLightEffect.SetActive(false);

        if(smokeEffect != null)
        {
           Debug.Log("Smoke Slot is filled - Playing Now!");
           smokeEffect.SetActive(true);
           StartCoroutine(StopSmokeAfterDelay(3f));
        }
        else
        {
            Debug.Log("Smoke Slot is EMPTY");
        }
    }


    IEnumerator StopSmokeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (smokeEffect != null) smokeEffect.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerInRange)
        {
            if (isLit) ExtinguishFire();
            else LightFire();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && playerInRange && !isCooking)
        {
            bool holdingAxe = EquipSystem.Instance.selectedItem != null && 
                            EquipSystem.Instance.selectedItem.name.Contains("Axe");

            if (!holdingAxe && InventorySystem.Instance.itemList.Contains("RawMeat") && isLit)
            {
                StartCoroutine(CookMeat());
            }
        }
        if(isLit && fireLight != null)
            {
                fireLight.intensity = Mathf.Lerp(
                    fireLight.intensity,
                    Random.Range(minIntensity, maxIntensity),
                    flickerSpeed
                );
            }
    }

    IEnumerator CookMeat()
    {
        isCooking = true;
        
        if (!InventorySystem.Instance.itemList.Contains("RawMeat"))
        {
            isCooking = false;
            yield break;
        }
        
        InventorySystem.Instance.RemoveItem("RawMeat", 1);
        yield return new WaitForSeconds(1f);
        InventorySystem.Instance.AddToInventory("CookedMeat");
        isCooking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = false;
    }
}