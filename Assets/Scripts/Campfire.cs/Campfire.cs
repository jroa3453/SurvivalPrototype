using UnityEngine;
using System.Collections;

public class Campfire : MonoBehaviour
{
    public static Campfire Instance;
    public bool playerInRange;
    private bool isCooking = false;

    [Header("Fire Effects")]
    public ParticleSystem fireParticles;
    public ParticleSystem smokeParticles;
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
        if (smokeParticles != null) smokeParticles.Stop();
    }

    public void LightFire()
    {
        isLit = true;
        if (fireParticles != null) fireParticles.Play();
        if (smokeParticles != null) smokeParticles.Play();
        if (fireLight != null) fireLight.enabled = true;
    }

    public void ExtinguishFire()
    {
        isLit = false;
        if (fireParticles != null) fireParticles.Stop();
        if (smokeParticles != null) smokeParticles.Stop();
        if (fireLight != null) fireLight.enabled = false;
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