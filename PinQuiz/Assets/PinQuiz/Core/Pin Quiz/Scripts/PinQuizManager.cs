using DG.Tweening;
using HongQuan;
using QuanUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace PinQuiz
{
    public class PinQuizManager : MonoBehaviour
    {
        public static PinQuizManager instance;

        [SerializeField] PinQuizEndGamePopup popupEndLevel;
        [SerializeField] int levelIndex;
        [SerializeField] PinQuizLevel[] levels;
        [SerializeField] FitSpriteRendererWithMainCamera bg;
        [SerializeField] Sprite[] bgSprites;
        [SerializeField] Sprite[] coinSprites;
        [SerializeField] ParticleSystem winEff;

        List<Pin> pins = new List<Pin>();
        List<PinQuizCoin> coins = new List<PinQuizCoin>();
        PinQuizLevel currentLevel;

        [Header("Lava")]
        [SerializeField] public GameObject lavaStone;
       // [SerializeField] public ParticleEffect lavaSteamEffect;
        [SerializeField] public AudioClip lavaEvaporationSound;

        [Header("LayerMasks")]
        [SerializeField] private LayerMask itemLayerMask;
        [SerializeField] private LayerMask characterLayerMask;
        [SerializeField] private LayerMask clickLayerMask;
        [SerializeField] private LayerMask waterLayerMask;
        [SerializeField] private LayerMask lavaLayerMask;
        [SerializeField] private LayerMask slimeLayerMask;
        [SerializeField] private LayerMask stoneLayerMask;

        [Header("Camera")]
        [SerializeField] private GameObject slimeCamera;
        [SerializeField] private GameObject lavaCamera;
        [SerializeField] private GameObject waterCamera;

        public PinQuizPrincess princess;
        public bool isDream;

        private Camera mainCam;

        public int LevelIndex => levelIndex;
        public List<PinQuizCoin> Coins => coins;
        public LayerMask StoneLayerMask => stoneLayerMask;

        public GameObject effectChangeSkinPrincessWin, effectCoinCollect;

        public float camereaOrthographicSize;

        public PinQuizPrincessEndGameAnim endPrincessAnim;
        public Vector3 winAnimLocalScale;
        public List<PinQuizFluid> fluids = new List<PinQuizFluid>();
        public Sprite[] CoinSprites => coinSprites;

        [SerializeField] AudioClip loseSound;
        private void Awake()
        {
            Application.targetFrameRate = 60;
            mainCam = Camera.main;
            instance = this;
            InitPhysic();
            
            if (camereaOrthographicSize == 0)
                camereaOrthographicSize = mainCam.orthographicSize;
            PinQuizFluid.isLowRes = false;
            if (PinQuizFluid.isLowRes)
                TurnOnLowResolution();
        }


        private void InitPhysic()
        {
            Physics2D.IgnoreLayerCollision((int)Mathf.Log(itemLayerMask, 2), (int)Mathf.Log(characterLayerMask, 2));
            Physics2D.IgnoreLayerCollision((int)Mathf.Log(itemLayerMask, 2), (int)Mathf.Log(waterLayerMask, 2));

            Physics2D.IgnoreLayerCollision((int)Mathf.Log(clickLayerMask, 2), (int)Mathf.Log(characterLayerMask, 2));
            Physics2D.IgnoreLayerCollision((int)Mathf.Log(clickLayerMask, 2), (int)Mathf.Log(itemLayerMask, 2));
            Physics2D.IgnoreLayerCollision((int)Mathf.Log(clickLayerMask, 2), (int)Mathf.Log(waterLayerMask, 2));
            Physics2D.IgnoreLayerCollision((int)Mathf.Log(clickLayerMask, 2), (int)Mathf.Log(slimeLayerMask, 2));
            Physics2D.IgnoreLayerCollision((int)Mathf.Log(clickLayerMask, 2), (int)Mathf.Log(lavaLayerMask, 2));
            Physics2D.IgnoreLayerCollision((int)Mathf.Log(clickLayerMask, 2), (int)Mathf.Log(stoneLayerMask, 2));

            Physics2D.IgnoreLayerCollision((int)Mathf.Log(stoneLayerMask, 2), (int)Mathf.Log(waterLayerMask, 2));
            Physics2D.IgnoreLayerCollision((int)Mathf.Log(stoneLayerMask, 2), (int)Mathf.Log(slimeLayerMask, 2));
            Physics2D.IgnoreLayerCollision((int)Mathf.Log(stoneLayerMask, 2), (int)Mathf.Log(lavaLayerMask, 2));
        }

        private void OnDestroy()
        {
            Physics2D.IgnoreLayerCollision((int)Mathf.Log(itemLayerMask, 2), (int)Mathf.Log(characterLayerMask, 2), false);
            Physics2D.IgnoreLayerCollision((int)Mathf.Log(itemLayerMask, 2), (int)Mathf.Log(waterLayerMask, 2), false);

            Physics2D.IgnoreLayerCollision((int)Mathf.Log(clickLayerMask, 2), (int)Mathf.Log(characterLayerMask, 2), false);
            Physics2D.IgnoreLayerCollision((int)Mathf.Log(clickLayerMask, 2), (int)Mathf.Log(itemLayerMask, 2), false);
            Physics2D.IgnoreLayerCollision((int)Mathf.Log(clickLayerMask, 2), (int)Mathf.Log(waterLayerMask, 2), false);
            Physics2D.IgnoreLayerCollision((int)Mathf.Log(clickLayerMask, 2), (int)Mathf.Log(slimeLayerMask, 2), false);
            Physics2D.IgnoreLayerCollision((int)Mathf.Log(clickLayerMask, 2), (int)Mathf.Log(lavaLayerMask, 2), false);
            Physics2D.IgnoreLayerCollision((int)Mathf.Log(clickLayerMask, 2), (int)Mathf.Log(stoneLayerMask, 2), false);

            Physics2D.IgnoreLayerCollision((int)Mathf.Log(stoneLayerMask, 2), (int)Mathf.Log(waterLayerMask, 2), false);
            Physics2D.IgnoreLayerCollision((int)Mathf.Log(stoneLayerMask, 2), (int)Mathf.Log(slimeLayerMask, 2), false);
            Physics2D.IgnoreLayerCollision((int)Mathf.Log(stoneLayerMask, 2), (int)Mathf.Log(lavaLayerMask, 2), false);

            levels = null;
            if (endPrincessAnim != null)
                Destroy(endPrincessAnim.gameObject);
            if (princess != null) Destroy(princess.gameObject);
            bgSprites = null;
            
            instance = null;

    
            //Resources.UnloadUnusedAssets();
            /*System.GC.WaitForPendingFinalizers();
            System.GC.Collect();*/
        }

        private void Start()
        {
            if (!mainCam)
                mainCam = Camera.main;
            levelIndex = 0;
            StartLevel();
            //Time.timeScale = 1.5f;
        }
        public enum TypeEff {
            CoinCollect = 1,
            ChangeSkinPrincessWin  =2,
        }
        public void ShowEff(float t,Vector3 pos, TypeEff typeEff)
        {
           
            if (typeEff == TypeEff.CoinCollect)
            {
                effectCoinCollect.SetActive(true);
                effectCoinCollect.transform.position = pos;
                DOVirtual.DelayedCall(t, () => { effectCoinCollect.SetActive(false); });
            }
            else if (typeEff == TypeEff.ChangeSkinPrincessWin)
            {
                effectChangeSkinPrincessWin.SetActive(true);
                effectChangeSkinPrincessWin.transform.position = pos;
                DOVirtual.DelayedCall(1, () => { effectChangeSkinPrincessWin.SetActive(false); });
            }

        }
        private void StartLevel()
        {
           LunaIntegrationManager.instance.LogEventLvStart(levelIndex+1);
            SetActiveLavaCamera(false);
            SetActiveSlimeCamera(false);
            SetActiveWaterCamera(false);
            if (currentLevel != null) Destroy(currentLevel.gameObject);
            currentLevel = Instantiate(levels[levelIndex]);
            isDoneGame = false;
            SetBG();
            canRemovePin = 0;
            SetRemovePins(true);
            mainCam.transform.DOMove(new Vector3(0, 0, -10), 1);
            mainCam.DOOrthoSize(camereaOrthographicSize, 1);
            winEff.Stop();

            endPrincessAnim.StopAnimation();
            endPrincessAnim.transform.position = Vector2.one * 10000;

            ////MyAnalytics.GetInstance().LogStartGame();
            ////MyAnalytics.GetInstance().LogLevelGame(levelIndex);
        }

        public void NextLevel()
        {
            Time.timeScale = 1f;
            levelIndex = (levelIndex + 1) % levels.Length;
            StartLevel();
        }

        public void LastLevel()
        {
            levelIndex = Mathf.Abs((levelIndex - 1) % levels.Length);
            StartLevel();
        }

        bool isDoneGame;

        public void WinGame()
        {
            if (isDoneGame) return;
            isDoneGame = true;
            LunaIntegrationManager.instance.LogEventLvAchieved(levelIndex+1);
            if (levelIndex >=2 || LunaIntegrationManager.instance.OverTimeLimt)
            {
                LunaIntegrationManager.instance.OnGameFinished();
                return;
            }
            SetRemovePins(false);
            MoveCamToTarget(princess.transform);
            mainCam.DOOrthoSize(4, 1);
            winEff.transform.position = princess.transform.position + Vector3.up*3;
            winEff.Play();
            SoundManager_BabyGirl.Instance.PlayOneShot("QuanSounds/Children Yay");
            this.DelayFunction(3, () => NextLevel() /*popupEndLevel.Show(EndGameType.Win)*/);
            //princess.SetOverlay();
            ////MyAnalytics.GetInstance().LogWinGame();
        }

        public void LoseGame(EndGameType loseType = EndGameType.CoinStuck)
        {
            

            if (isDoneGame) return;
            isDoneGame = true;
           LunaIntegrationManager.instance.LogEventlevelLose(levelIndex + 1);
            if (LunaIntegrationManager.instance.OverTimeLimt)
            {
                LunaIntegrationManager.instance.OnGameFinished();
                return;
            }
            levelIndex = 0;
            Time.timeScale = 1.5f;
            SetRemovePins(false);
            mainCam.DOOrthoSize(4, 1);
            MoveCamToTarget(princess.transform);
            SoundManager_BabyGirl.Instance.PlayOneShot(loseSound);
            this.DelayFunction(3, () => StartLevel()/* popupEndLevel.Show(loseType)*/);
            ////MyAnalytics.GetInstance().LogLoseGame();
        }

        public void LoseGame(Vector3 targetCamPos, EndGameType loseType = EndGameType.CoinStuck)
        {
            if (isDoneGame) return;
            isDoneGame = true;

            SetRemovePins(false);
            mainCam.transform.DOMove(targetCamPos + new Vector3(0, 0, -10), 1);
            mainCam.DOOrthoSize(4, 1);
            this.DelayFunction(3, () => popupEndLevel.Show(loseType));
            //MyAnalytics.GetInstance().LogLoseGame();
        }

        public void LoseGame(Transform targetTransform, EndGameType loseType = EndGameType.CoinStuck, float delay = 3)
        {
            if (isDoneGame) return;
            isDoneGame = true;

            SetRemovePins(false);
            MoveCamToTarget(targetTransform);
            mainCam.DOOrthoSize(4, 1);
            this.DelayFunction(delay, () => popupEndLevel.Show(loseType));
            //MyAnalytics.GetInstance().LogLoseGame();
        }

        public void ReplayLevel()
        {
            StartLevel();
        }

        public void AddPin(Pin pin)
        {
            pins.Add(pin);
        }

        public void RemovePin(Pin pin)
        {
            pins.Remove(pin);
        }

        public void SetRemovePins(bool canRemove)
        {
            Pin.canRemove = canRemove;
            //Debug.Log("Set remove pin " + canRemove);
        }

        public int canRemovePin = 0;
        public void DisableRemovePins()
        {
            canRemovePin--;
            if (canRemovePin > 0) return;
            SetRemovePins(false);
        }

        public void EnableRemovePin()
        {
            canRemovePin++;
            if (canRemovePin < 0) return;
            SetRemovePins(true);
        }

        public void AddCoin(PinQuizCoin coin)
        {
            coins.Add(coin);
        }

        public void RemoveCoin(PinQuizCoin coin)
        {
            coins.Remove(coin);
        }

        public void DestroyAllCoin()
        {
            foreach (var coin in coins)
            {
                coin.Die();
            }
            coins.Clear();
        }

        private void SetBG()
        {
            bg.spriteRenderer.sprite = bgSprites[Random.Range(0, bgSprites.Length)];
            bg.FitWithMainCam(camereaOrthographicSize, Vector3.zero);
        }

        public void Out()
        {
            Manager.instance.transition.FullScreen(() =>
            {
                //LoadSceneManager.Instance.LoadScene("PinQuiz");
            }, null);
        }

        private void MoveCamToTarget(Transform target)
        {
            StartCoroutine(MovingCamToPrincess(1, target));
        }

        private IEnumerator MovingCamToPrincess(float time, Transform target)
        {
            float clk = 0f;
            Vector3 startPos = mainCam.transform.position;
            while (clk <= time)
            {
                clk += Time.deltaTime;
                mainCam.transform.position = Vector3.Lerp(startPos, target.position + new Vector3(0, 0, -10) + Vector3.up, clk / time);
                yield return null;
            }
            mainCam.transform.position = target.position + new Vector3(0, 0, -10) + Vector3.up;
        }

        public void SetCamSize(float size)
        {
            camereaOrthographicSize = size;
        }

        public void SetActiveWaterCamera(bool isActive)
        {
            if (PinQuizFluid.isLowRes) return;
            waterCamera.SetActive(isActive);
        }
        public void SetActiveLavaCamera(bool isActive)
        {
            if (PinQuizFluid.isLowRes) return;
            lavaCamera.SetActive(isActive);
        }
        public void SetActiveSlimeCamera(bool isActive)
        {
            if (PinQuizFluid.isLowRes) return;
            slimeCamera.SetActive(isActive);
        }

        private void Update()
        {
            CheckLowFps();
        }

        private float timer = 0f;
        private int frames = 0;
        private bool lowFPSDetected = false;

        private void CheckLowFps()
        {
            //#if UNITY_EDITOR
            //            if(Input.GetKeyDown(KeyCode.Space))
            //            {
            //                Application.targetFrameRate = 8;
            //            }
            //#endif

            //            if(lowFPSDetected) return;
            //            timer += Time.deltaTime;
            //            frames++;

            //            if (timer > 5f)
            //            {
            //                float fps = frames / timer;
            //                if (fps <= 15f)
            //                {
            //#if UNITY_EDITOR
            //                    Debug.Log("LowFPS");
            //#endif
            //                    lowFPSDetected = true;
            //                    TurnOnLowResolution();
            //                }
            //                timer = 0f;
            //                frames = 0;
            //            }
        }

        public void TurnOnLowResolution()
        {
            PinQuizFluid.isLowRes = true;
            foreach (var f in fluids)
            {
                f.ChangeToLowResolution();
            }
            SetActiveLavaCamera(false);
            SetActiveSlimeCamera(false);
            SetActiveWaterCamera(false);
            Application.targetFrameRate = 60;
        }

    }

}