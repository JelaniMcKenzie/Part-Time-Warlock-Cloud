using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewShadowArcher : GameEntity
{
    public Player player;
    public GameObject bowPrefab; 
    public float timer = 0;
    int waitingTime = 5;
    public GameObject arrowPrefab;
    public Transform bowSpawnPoint;
    public static GameObject[] shadowPosition;
    private GameObject bow;
    private SpriteRenderer spriteRenderer;
    public float timeBeforeDisappearing = 2f; // Adjust as needed
    public float timeBeforeRepositioning = 3f; // Adjust as needed
    private Animator animator;



    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = FindAnyObjectByType<Player>();
        animator = GetComponent<Animator>();

        SpawnBow();
        StartCoroutine(EnemyBehavior());
    }

    private void Update() 
    {
        AimAtPlayer();
    }
    private void SpawnBow()
    {
        bow = Instantiate(bowPrefab, bowSpawnPoint.position, Quaternion.identity);
        bow.transform.parent = transform;
    }

    private void AimAtPlayer()
    {
        
        Vector3 directionToPlayer = player.transform.position - transform.position;

        // Flip the sprite based on player position
        if (directionToPlayer.x < 0)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;

        // Rotate the bow toward the player
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        bow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
    }

    // Call this method to make the archer shoot
    public void Shoot()
    {
        // Implement your shooting logic here
        // Instantiate arrows, apply force, etc.
        Animator bowAnim = bowPrefab.GetComponent<Animator>();
        if (bowAnim != null)
        {
            bowAnim.SetTrigger("Shoot");
            GameObject a = Instantiate(arrowPrefab, bowSpawnPoint.transform.position, bow.transform.rotation);
            a.transform.parent = null;
            a.transform.localScale = new Vector3(1, 1, 1);
            a.transform.position = transform.position;
            a.transform.LookAt(player.transform.position);
            Destroy(a, 2.5f);
            
        }
        else
        {
            Debug.LogError("Animator component not found on bowPrefab");
        }
    }

    private IEnumerator EnemyBehavior()
    {

        while (true)
        {
            PlaySpawnAnimation();
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            animator.SetTrigger("Idle");
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            AimAtPlayer();
            Shoot();
            yield return new WaitForSeconds(timeBeforeDisappearing);

            PlayDespawnAnimation();
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            MoveToRandomLocation();
            yield return null;
        }
    }

    private void MoveToRandomLocation()
    {
        // Change this logic based on how you want the enemy to reposition
        shadowPosition = GameObject.FindGameObjectsWithTag("ShadowPosition");
        GameObject selectedObject = shadowPosition[Random.Range(0, shadowPosition.Length-1)];
        transform.position = selectedObject.transform.position;
        Debug.Log("moved");
    }

    private void PlaySpawnAnimation()
    {
        animator.SetTrigger("Spawn");
    }

    private void PlayDespawnAnimation()
    {
        animator.SetTrigger("Despawn");
    }
}
