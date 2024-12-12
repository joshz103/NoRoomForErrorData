using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using UnityEngine.SceneManagement;

public class HazardSpawner : MonoBehaviour
{
    [Header("Level Properties")]
    public int numberOfRounds = 20;
    public float dynamicFrictionPlayer = 0.3f;
    public float staticFrictionPlayer = 0.95f;

    [Header("Hazards")]
    public List<GameObject> hazardFloorMarkers;
    public List<GameObject> floorHazards;

    [SerializeField] private List<string> spawnedHazards;

    [Header("Setup")]
    public CinemachineVirtualCamera camera;
    public GameObject playerCameraMarker;
    public Vector3 objectCameraOffset = new Vector3(0, 2, 0);
    private bool isCameraLocked = false;
    public PhysicMaterial playerMaterial;

    [Header("UI Winner")]
    public Canvas mainUI;
    public Canvas winUI;
    public TextMeshProUGUI winRoundNum;
    private bool hasWon = false;
    private bool canEnterMenu = false;


    //public GameObject[] hazardFloor;
    //public GameObject[] hazardCeiling;
    //public GameObject[] hazardAir;

    private GameObject player;
    private Transform newObjectPosition;

    [Header("Seed")]
    [SerializeField]private System.Random seed;
    public string currentSeed;

    public int roundNumber = 0;

    [Header("UI")]
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI deathRoundText;
    public GameObject newObstacleTextContainer;

    [Header("Music")]
    public music_machine musicMachine;

    // Start is called before the first frame update
    void Start()
    {
        hazardFloorMarkers = new List<GameObject>(GameObject.FindGameObjectsWithTag("FloorHazardMarker"));

        player = GameObject.FindGameObjectWithTag("Player");

        roundText.text = "0" + " / " + numberOfRounds;
        deathRoundText.text = "0";

        playerMaterial.dynamicFriction = dynamicFrictionPlayer;
        playerMaterial.staticFriction = staticFrictionPlayer;

        seed = new System.Random(GameObject.FindGameObjectWithTag("Seed").GetComponent<RandomSeed>().seed);

        currentSeed = seed.ToString();

        //eed = GameObject.FindGameObjectWithTag("Seed").GetComponent<RandomSeed>().seed;
        //UnityEngine.Random.InitState(seed);
        //Debug.Log("Hazard spawner got seed " + GameObject.FindGameObjectWithTag("Seed").GetComponent<RandomSeed>().seed);

        //hazardCeiling = GameObject.FindGameObjectsWithTag("CeilingHazardMarker");
        //hazardFloor = GameObject.FindGameObjectsWithTag("AirHazardMarker");

        //spawnFloorHazard();
    }

    private void Update()
    {
        unlockCamera();
        
        if (canEnterMenu && Input.GetButtonDown("Jump"))
        {
            WinReturnToMainMenu();
        }
    }

    public void spawnRandomHazard()
    {
        roundNumber++;

        if (roundNumber > numberOfRounds)
        {
            hasWon = true;
            StartCoroutine(WaitToEnterMenuWin()); //Prevents skipping to menu the same frame you jump or something

            int roundNumberTrim = roundNumber - 1;
            winRoundNum.text = roundNumberTrim.ToString();

            //Stop player input
            player.GetComponent<PlayerMovement>().actionable = false;

            //Enable winner UI, disable main UI
            winUI.enabled = true;
            mainUI.enabled = false;

            //Move Camera to Default Location (looking at entire map)
            newObjectPosition = transform;
            camera.Follow = newObjectPosition;
        }
        else
        {
            //Stop player input
            player.GetComponent<PlayerMovement>().actionable = false;

            //Spawn new object
            spawnFloorHazard();

            //Camera stuff
            camera.Follow = newObjectPosition;
            camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = objectCameraOffset;
            isCameraLocked = true;
            //roundNumber++;
            newObstacleTextContainer.SetActive(true);
            roundText.text = roundNumber.ToString();
            roundText.text = roundText.text + " / " + numberOfRounds;
            deathRoundText.text = roundNumber.ToString();
        }
    }

    public void unlockCamera()
    {
        if (isCameraLocked && Input.GetButtonDown("Jump") && !hasWon)
        {
            camera.Follow = player.transform;
            camera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = objectCameraOffset;
            isCameraLocked= false;
            newObstacleTextContainer.SetActive(false);
            Invoke("unlockPlayerMovement", 0.1f);

            if(roundNumber == 5)
            {
                musicMachine.track2Start();
            }

            if (roundNumber == 10)
            {
                musicMachine.track3Start();
            }

        }
    }

    private void unlockPlayerMovement()
    {
        player.GetComponent<PlayerMovement>().actionable = true;
    }

    private void WinReturnToMainMenu()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        SceneManager.LoadScene(0);
    }

    IEnumerator WaitToEnterMenuWin()
    {
        yield return new WaitForSeconds(2f);
        canEnterMenu = true;
    }

    public void spawnFloorHazard()
    {
        bool isValid = true;

        if(hazardFloorMarkers.Count > 0)
        {
            //Create random values for marker and hazard indexes
            //int randomHazardIndex = UnityEngine.Random.Range(0, floorHazards.Count);
            //int randomHazardMarkerIndex = UnityEngine.Random.Range(0, hazardFloorMarkers.Count);

            int randomHazardIndex = seed.Next(0, floorHazards.Count);
            int randomHazardMarkerIndex = seed.Next(0, hazardFloorMarkers.Count);

            //Check selected marker to see if it is spawning something invalid. If its invalid, set isValid to false, break, and restart.
            foreach (string invalidSpawn in hazardFloorMarkers[randomHazardMarkerIndex].GetComponent<marker>().invalidObjectSpawns)
            {
                if (invalidSpawn == floorHazards[randomHazardIndex].GetComponent<Hazard>().ID)
                {
                    isValid = false;
                    break;
                }
            }
            //If the spawn is valid, spawn the object. Otherwise restart.
            if (isValid)
            {
                GameObject trap = Instantiate(floorHazards[randomHazardIndex], hazardFloorMarkers[randomHazardMarkerIndex].transform.position, hazardFloorMarkers[randomHazardMarkerIndex].transform.rotation);
                Debug.Log("Spawned a " + floorHazards[randomHazardIndex].name + " at " + hazardFloorMarkers[randomHazardMarkerIndex].name + "   |  This marker is no longer valid for spawning hazards.");
                hazardFloorMarkers.RemoveAt(randomHazardMarkerIndex);
                newObjectPosition = trap.transform;
                spawnedHazards.Add(floorHazards[randomHazardIndex].GetComponent<Hazard>().ID); //Keep track of whats spawned in

                //Removes hazard from list of possible spawns if its a limited hazard
                string searchString = floorHazards[randomHazardIndex].GetComponent<Hazard>().ID;
                int count = spawnedHazards.Count(s => s == searchString);

                if (count >= trap.GetComponent<Hazard>().maxAmount)
                {
                    floorHazards.RemoveAt(randomHazardIndex);
                    Debug.Log("The maximum amount of " + searchString + " has been spawned in. Removing from list of spawnable objects.");
                }
            }
            else
            {
                Debug.Log("Couldn't spawn " + floorHazards[randomHazardIndex].name + " at " + hazardFloorMarkers[randomHazardMarkerIndex].name + "... Retrying..." );
                spawnFloorHazard(); //Retry if spawn is invalid
            }
        }
        else //Fallback for if nothing is able to spawn
        {
            Debug.Log("No valid markers found!");
            newObjectPosition = transform;
        }
        
    }


}
