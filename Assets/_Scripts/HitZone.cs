using System.Collections.Generic;
using UnityEngine;

public class HitZone : MonoBehaviour
{
    [Header("Cài đặt Phím")]
    public KeyCode inputKey; 

    [Header("Cài đặt 16 SFX theo độ cao")]
    public AudioClip[] pitchSFXFiles = new AudioClip[16]; 
    [Range(0f, 2f)] public float sfxVolume = 1.0f; 
    private AudioSource sfxSource;

    private List<GameObject> enemiesInZone = new List<GameObject>();
    private bool isHolding = false;
    private GameObject currentHoldEnemy = null;

    void Start()
    {
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.volume = sfxVolume;
    }

    void Update()
    {
        if (Input.GetKeyDown(inputKey))
        {
            if (enemiesInZone.Count > 0)
            {
                GameObject enemyToHit = enemiesInZone[0];
                float noteY = 192f;
                EnemyController enemyComp = enemyToHit.GetComponent<EnemyController>();
                if (enemyComp != null) { noteY = enemyComp.originalYPos; }
                
                if (enemyToHit.CompareTag("HoldEnemy"))
                {
                    isHolding = true;
                    currentHoldEnemy = enemyToHit;
                    PlayPitchHitSound(noteY); 
                }
                else if (enemyToHit.CompareTag("Enemy"))
                {
                    enemiesInZone.RemoveAt(0);
                    Destroy(enemyToHit);
                    PlayPitchHitSound(noteY); 

                    // TRUYỀN VỊ TRÍ CHÉM VÀO ĐÂY
                    if (ScoreManager.instance != null) ScoreManager.instance.AddHit("Perfect", transform.position);
                }
            }
            else
            {
                if (ScoreManager.instance != null) ScoreManager.instance.MissHit(transform.position);
            }
        }

        if (Input.GetKeyUp(inputKey) && isHolding)
        {
            isHolding = false; 
            if (currentHoldEnemy != null && enemiesInZone.Contains(currentHoldEnemy))
            {
                enemiesInZone.Remove(currentHoldEnemy);
                Destroy(currentHoldEnemy); 
                if (ScoreManager.instance != null) ScoreManager.instance.MissHit(transform.position); 
            }
        }
    }

    void PlayPitchHitSound(float yPos)
    {
        if (pitchSFXFiles == null || pitchSFXFiles.Length == 0 || sfxSource == null) return;
        int sfxIndex = Mathf.Clamp(Mathf.FloorToInt((384f - yPos / 384f) * pitchSFXFiles.Length), 0, pitchSFXFiles.Length - 1);
        AudioClip clipToPlay = pitchSFXFiles[sfxIndex];
        
        if (clipToPlay != null) sfxSource.PlayOneShot(clipToPlay, sfxVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("HoldEnemy"))
        {
            enemiesInZone.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (enemiesInZone.Contains(other.gameObject))
        {
            enemiesInZone.Remove(other.gameObject);
            
            if (other.CompareTag("HoldEnemy") && isHolding && other.gameObject == currentHoldEnemy)
            {
                isHolding = false;
                Destroy(other.gameObject);
                if (ScoreManager.instance != null) ScoreManager.instance.AddHit("Perfect", transform.position); 
            }
            else
            {
                isHolding = false;
                if (ScoreManager.instance != null) ScoreManager.instance.MissHit(transform.position); 
            }
        }
    }
}