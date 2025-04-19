using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    public GameObject pullBeamPrefab;
    public GameObject pullSwipePrefab;
    public GameObject pushBeamPrefab;
    public GameObject pushSwipePrefab;

    private GameObject activePullBeam = null;
    private GameObject activePullSwipe = null;

    public enum ParticleType
    {
        PullBeam,
        PullSwipe,
        PushBeam,
        PushSwipe
    }

    public void SpawnParticle(ParticleType type)
    {
        switch (type)
        {
            case ParticleType.PullBeam:
                if (activePullBeam == null) // Only spawn if it doesn't exist
                    activePullBeam = Instantiate(pullBeamPrefab, transform.position, Quaternion.identity);
                break;
            case ParticleType.PullSwipe:
                if (activePullSwipe == null) // Only spawn if it doesn't exist
                    activePullSwipe = Instantiate(pullSwipePrefab, transform.position, Quaternion.identity);
                break;
            case ParticleType.PushBeam:
                Instantiate(pushBeamPrefab, transform.position, Quaternion.identity); // Always spawns a new one
                break;
            case ParticleType.PushSwipe:
                Instantiate(pushSwipePrefab, transform.position, Quaternion.identity); // Always spawns a new one
                break;
        }
    }

    public void DestroyPullParticle(ParticleType type)
    {
        switch (type)
        {
            case ParticleType.PullBeam:
                if (activePullBeam != null)
                {
                    Destroy(activePullBeam);
                    activePullBeam = null;
                }
                break;
            case ParticleType.PullSwipe:
                if (activePullSwipe != null)
                {
                    Destroy(activePullSwipe);
                    activePullSwipe = null;
                }
                break;
        }
    }

    #if UNITY_EDITOR
    private void Update()
    {
        // Pull Beam: Exists only while key is held
        if (Input.GetKey(KeyCode.Alpha1))
        {
            SpawnParticle(ParticleType.PullBeam);
        }
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            DestroyPullParticle(ParticleType.PullBeam);
        }

        // Pull Swipe: Exists only while key is held
        if (Input.GetKey(KeyCode.Alpha2))
        {
            SpawnParticle(ParticleType.PullSwipe);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            DestroyPullParticle(ParticleType.PullSwipe);
        }

        // Push Beam: Spawns a new instance every time the key is pressed
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SpawnParticle(ParticleType.PushBeam);
        }

        // Push Swipe: Spawns a new instance every time the key is pressed
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SpawnParticle(ParticleType.PushSwipe);
        }
    }
    #endif
}





//EXAMPLE CLASS ON HOW TO CALL PARTICLES

// using UnityEngine;
//
// public class PlayerController : MonoBehaviour
// {
//     private ParticleSpawner particleSpawner;
//
//     private void Start()
//     {
//         // Find the ParticleSpawner in the scene and store a reference
//         particleSpawner = FindObjectOfType<ParticleSpawner>();
//     }
//
//     private void Update()
//     {
//         if (particleSpawner == null) return; // Safety check
//
//         // Example of calling particle effects from this script
//
//         if (Input.GetKeyDown(KeyCode.Z)) // Example: Spawn a push beam
//         {
//             particleSpawner.SpawnParticle(ParticleSpawner.ParticleType.PushBeam);
//         }
//
//         if (Input.GetKeyDown(KeyCode.X)) // Example: Spawn a push swipe
//         {
//             particleSpawner.SpawnParticle(ParticleSpawner.ParticleType.PushSwipe);
//         }
//
//         if (Input.GetKey(KeyCode.C)) // Example: Hold to spawn pull beam
//         {
//             particleSpawner.SpawnParticle(ParticleSpawner.ParticleType.PullBeam);
//         }
//         if (Input.GetKeyUp(KeyCode.C)) // Example: Release to destroy pull beam
//         {
//             particleSpawner.DestroyPullParticle(ParticleSpawner.ParticleType.PullBeam);
//         }
//
//         if (Input.GetKey(KeyCode.V)) // Example: Hold to spawn pull swipe
//         {
//             particleSpawner.SpawnParticle(ParticleSpawner.ParticleType.PullSwipe);
//         }
//         if (Input.GetKeyUp(KeyCode.V)) // Example: Release to destroy pull swipe
//         {
//             particleSpawner.DestroyPullParticle(ParticleSpawner.ParticleType.PullSwipe);
//         }
//     }
// }

