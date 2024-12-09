using UnityEngine;
using UnityEngine.UI;

namespace Complete
{
    public class TankShooting : MonoBehaviour
    {
        public int m_PlayerNumber = 1;              // Used to identify the different players.
        public Rigidbody m_Shell;                   // Prefab of the shell.
        public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
        public Slider m_AimSlider;                  // A child of the tank that displays the current launch force.
        public AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
        public AudioClip m_ChargingClip;            // Audio that plays when each shot is charging up.
        public AudioClip m_FireClip;                // Audio that plays when each shot is fired.
        public float m_MinLaunchForce = 15f;        // The force given to the shell if the fire button is not held.
        public float m_MaxLaunchForce = 30f;        // The force given to the shell if the fire button is held for the max charge time.
        public float m_MaxChargeTime = 0.75f;       // How long the shell can charge for before it is fired at max force.
        private bool m_IsGaugeIncreasing = true;    // ゲージが増加中かどうか

        [Header("Ammo Settings")]
        public int m_InitialAmmoCount = 10;         // ゲーム開始時の砲弾の所持数
        public int m_MaxAmmoCount = 50;             // 所持可能な砲弾の最大数
        public int m_AmmoRefillAmount = 5;          // カートリッジ取得時の補充量
        private int m_CurrentAmmoCount;            // 現在の砲弾の所持数

        private string m_FireButton;                // The input axis that is used for launching shells.
        private float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.
        private float m_ChargeSpeed;                // How fast the launch force increases, based on the max charge time.
        private bool m_Fired;                       // Whether or not the shell has been launched with this button press.
        private TankController tankController;

        public delegate void ShellStockChangedHandler(int currentStock);
        public event ShellStockChangedHandler OnShellStockChanged;

        private void Awake()
        {
            tankController = GetComponent<TankController>();
        }

        private void OnEnable()
        {
            // When the tank is turned on, reset the launch force and the UI
            m_CurrentLaunchForce = m_MinLaunchForce;
            m_AimSlider.value = m_MinLaunchForce;
            m_CurrentAmmoCount = m_InitialAmmoCount;
            NotifyShellStockChanged();
        }


        private void Start ()
        {
            // The fire axis is based on the player number.
            m_FireButton = "Fire" + m_PlayerNumber;

            // The rate that the launch force charges up is the range of possible forces by the max charge time.
            m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;

            m_CurrentAmmoCount = Mathf.Min(m_InitialAmmoCount, m_MaxAmmoCount);
        }


        private void Update ()
        {
            if (m_CurrentAmmoCount <= 0)
            {
                return;
            }
            // The slider should have a default value of the minimum launch force.
            m_AimSlider.value = m_MinLaunchForce;

            // Otherwise, if the fire button has just started being pressed...
            if (Input.GetButtonDown (m_FireButton))
            {
                // 無敵状態中は砲撃を無効化
                if (tankController != null && tankController.IsInvincible)
                {
                    Debug.Log("無敵中のため砲撃できません");
                    return;
                }

                // ... reset the fired flag and reset the launch force.
                m_Fired = false;
                m_CurrentLaunchForce = m_MinLaunchForce;
                m_IsGaugeIncreasing = true;

                // Change the clip to the charging clip and start it playing.
                m_ShootingAudio.clip = m_ChargingClip;
                m_ShootingAudio.Play ();
            }
            // 発射ボタンが押されている間、かつ未発射の場合
            else if (Input.GetButton(m_FireButton) && !m_Fired)
            {
                // ゲージが増加中の場合
                if (m_IsGaugeIncreasing)
                {
                    // 発射力を増加
                    m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
                    
                    // 最大値に達したらフラグを切り替え
                    if (m_CurrentLaunchForce >= m_MaxLaunchForce)
                    {
                        m_CurrentLaunchForce = m_MaxLaunchForce;
                        m_IsGaugeIncreasing = false;
                    }
                }
                else  // ゲージが減少中の場合
                {
                    // 発射力を減少
                    m_CurrentLaunchForce -= m_ChargeSpeed * Time.deltaTime;
                    
                    // 最小値に達したらフラグを切り替え
                    if (m_CurrentLaunchForce <= m_MinLaunchForce)
                    {
                        m_CurrentLaunchForce = m_MinLaunchForce;
                        m_IsGaugeIncreasing = true;
                    }
                }

                // スライダーの値を更新
                m_AimSlider.value = m_CurrentLaunchForce;
            }
            // Otherwise, if the fire button is released and the shell hasn't been launched yet...
            else if (Input.GetButtonUp (m_FireButton) && !m_Fired)
            {
                // ... launch the shell.
                Fire ();
            }
        }

        private void NotifyShellStockChanged()
        {
            OnShellStockChanged?.Invoke(m_CurrentAmmoCount);
        }

        private void Fire ()
        { 
            // Set the fired flag so only Fire is only called once.
            m_Fired = true;

            // Create an instance of the shell and store a reference to it's rigidbody.
            Rigidbody shellInstance =
                Instantiate (m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

            // Set the shell's velocity to the launch force in the fire position's forward direction.
            shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward; 

            // Change the clip to the firing clip and play it.
            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play ();

            // Reset the launch force.  This is a precaution in case of missing button events.
            m_CurrentLaunchForce = m_MinLaunchForce;
            m_CurrentAmmoCount--;
            NotifyShellStockChanged();
        }
        public void RefillAmmo()
        {
            // 現在の弾薬数に補充量を加えた値と最大値を比較し、
            // 小さい方を新しい弾薬数とする
            int beforeRefill = m_CurrentAmmoCount;
            m_CurrentAmmoCount = Mathf.Min(
                m_CurrentAmmoCount + m_AmmoRefillAmount, 
                m_MaxAmmoCount
            );
            Debug.Log($"Refill: {beforeRefill} -> {m_CurrentAmmoCount} (Max: {m_MaxAmmoCount})");
            NotifyShellStockChanged();
        }
        private void OnCollisionEnter(Collision collision)
        {
            // 衝突したオブジェクトのタグがShellCartridgeかチェック
            if (collision.gameObject.CompareTag("ShellCartridge"))
            {
                // 弾薬を補充
                RefillAmmo();
                
                // カートリッジを破壊
                Destroy(collision.gameObject);
            }
        }
    }
}