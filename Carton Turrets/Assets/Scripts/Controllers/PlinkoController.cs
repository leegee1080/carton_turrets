using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlinkoController : MonoBehaviour
{
    public static PlinkoController singlton;

    private PiaMainControls PlayerInputActions;
    public InputAction move, activate;
    [SerializeField] GameObject _onKbuttonsContainer;

    [SerializeField]HighlighterPackage _mainPackage;
    [SerializeField]HighlighterPackage _quitPackage;

    [Header("Bumpers")]
    [SerializeField]int _bumperNum;
    [SerializeField]GameObject _bumperGO;
    [SerializeField]GameObject _bumperContainer;
    [SerializeField] Vector3 _centerLocationforBumperSpawn;
    [SerializeField] float _radiusAroundBumperSpawnLocation;
    [SerializeField]float _bumperMinDistance;
    private List<GameObject> _spawnedBumpers = new List<GameObject>(); // a list to keep track of the spawned objects

    [Header("Ball")]
    [SerializeField]TMP_Text _walletText;
    [SerializeField]int _ballToPoolNum;
    [SerializeField]GameObject _ballGO;
    [SerializeField]GameObject _ballContainer;
    public ObjectPooler BallPooler;
    [SerializeField]Transform[] _ballSpawnLocations;
    [SerializeField] float _ballSpawnOffset;
    [SerializeField]bool _ballDropped;

    private void Awake()
    {
        singlton = this;
        PlayerInputActions = new PiaMainControls();
    }
    public void OnEnable()
    {
        move = PlayerInputActions.MainMap.PlayerMovement;
        move.Enable();
        activate = PlayerInputActions.MainMap.PlaceTurret;
        activate.Enable();
    }
    private void OnDisable()
    {
        move.Disable();
        activate.Disable();
    }

    private void Start()
    {
        Time.timeScale = 1;

        // _onKbuttonsContainer.SetActive(GlobalDataStorage.singleton.ControllerUsed == ControllerUsed.kb);

        AudioController.singleton.FadeSoundIn(0.05f,"music_plinko");

        StageMoneyEarnedIndicatorUI.singlton.GiveGlobalMoneyToTrack();
        StageMoneyEarnedIndicatorUI.singlton.UpdateInterface();
        UpdateTempWalletUI();
        GlobalVolumeController.singleton.ShowScene();

        GenerateBumperField();

        BallPooler = new ObjectPooler(_ballGO, _ballToPoolNum, _ballContainer,false);

        ShowPlinko();
    }
    private void Update()
    {
        if(_ballDropped){return;}
        switch (FindActivateControlsIndex())
        {
            case 0://right
                LaunchBall(2);
                break;
            case 1://up
                LaunchBall(0);
                break;
            case 2://left
                LaunchBall(1);
                break;
            case 3://down Exit menu
                break;
            default:
                break;
        }
    }

    public void ShowPlinko()
    {
        ControlsController.singleton.CurrentHighligherPackage = _mainPackage;
    }
    public void ShowQuit()
    {
        ControlsController.singleton.CurrentHighligherPackage = _quitPackage;
    }

    [ContextMenu("Gen Bumper Field")]
    public void GenerateBumperField()
    {
        if(_radiusAroundBumperSpawnLocation <= _bumperMinDistance){return;}

        int childCount = _bumperContainer.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            GameObject child = _bumperContainer.transform.GetChild(i).gameObject;
            Destroy(child);
        }

        _spawnedBumpers.Clear();

        // spawn the objects
        for (int i = 0; i < _bumperNum; i++)
        {
            // generate a random position within the spawn radius
            Vector3 spawnPosition = _centerLocationforBumperSpawn + new Vector3(Random.insideUnitSphere.x,Random.insideUnitSphere.y,0)  * _radiusAroundBumperSpawnLocation;

            // check if the distance between the new position and any other objects is greater than the minimum distance
            bool tooClose = false;
            foreach (GameObject obj in _spawnedBumpers)
            {
                if (Vector3.Distance(spawnPosition, obj.transform.position) < _bumperMinDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            // if the distance is greater than the minimum distance, instantiate the prefab at the new position
            if (!tooClose)
            {
                GameObject spawnedObject = Instantiate(_bumperGO, spawnPosition, Quaternion.identity);
                spawnedObject.transform.parent = _bumperContainer.transform;
                _spawnedBumpers.Add(spawnedObject); // add the spawned object to the list
            }
        }
    }

    private void UpdateTempWalletUI()
    {
        _walletText.text = GlobalDataStorage.singleton.PlayerTempWallet.ToString();
    }

    public void LaunchBall(int pos)
    {
        if(GlobalDataStorage.singleton.PlayerTempWallet <= 0){return;}

        AudioController.singleton.PlaySound("player_turret_build");
        
        _ballDropped = true;
        GlobalDataStorage.singleton.PlayerTempWallet -= 1;
        UpdateTempWalletUI();
        StageMoneyEarnedIndicatorUI.singlton.UpdateInterface();

        GameObject ball = BallPooler.ActivateNextObject(default);
        ball.transform.position = _ballSpawnLocations[pos].position + new Vector3(Random.insideUnitSphere.x,Random.insideUnitSphere.y,0) * _ballSpawnOffset;

        StartCoroutine(ButtonReset());
    }

    IEnumerator ButtonReset()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        _ballDropped = false;
    }

    public void LeavePlinko()
    {
        AudioController.singleton.FadeSoundOut(0.1f,"music_plinko");
        GlobalDataStorage.singleton.PlayerMoney += StageMoneyEarnedIndicatorUI.singlton.PublicMoneyAmountEarnedInLevel + GlobalDataStorage.singleton.PlayerTempWallet;
        GlobalVolumeController.singleton.NewScene(1);
    }

    public int FindActivateControlsIndex()//looks at the controls on the right side for the button pressed
    {
        int GetActivatedSlot(Vector2 v)
        {
            if (v[0] > 0) return 0;
            if (v[0] < 0) return 1;
            if (v[1] > 0) return 2;
            if (v[1] < 0) return 3;
            return -1;
        }
        int index = GetActivatedSlot(activate.ReadValue<Vector2>().normalized);
        if(index == -1){return -1;}
        return index;
    }
}
