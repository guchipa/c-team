using UnityEngine;
using System.Collections;
using Complete; 

public class CartridgeSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject m_CartridgePrefab;    // ShellCartridgeプレハブ

    [SerializeField]
    private float m_SpawnInterval = 10f;     // 生成間隔（秒）

    [SerializeField]
    private float m_SpawnRadius = 20f;       // 生成範囲の半径

    [SerializeField]
    private float m_SpawnHeight = 0.5f;      // 生成する高さ

    private Complete.GameManager m_GameManager;
    private Coroutine m_SpawnCoroutine;
    private void Start()
    {
        m_GameManager = FindObjectOfType<Complete.GameManager>();
        
        if (m_GameManager != null)
        {
            // イベントの購読を開始
            m_GameManager.OnGameStateChanged += HandleGameStateChanged;
        }
        else
        {
            Debug.LogError("GameManager not found!");
        }
    }

    private void HandleGameStateChanged(Complete.GameManager.GameState newState)
    {
        if (newState == Complete.GameManager.GameState.RoundPlaying)
        {
            // プレイ中ならスポーン開始
            if (m_SpawnCoroutine == null)
            {
                m_SpawnCoroutine = StartCoroutine(SpawnRoutine());
            }
        }
        else
        {
            // それ以外なら停止
            if (m_SpawnCoroutine != null)
            {
                StopCoroutine(m_SpawnCoroutine);
                m_SpawnCoroutine = null;
            }
        }
    }

    private void OnDestroy()
    {
        // イベントの購読を解除
        if (m_GameManager != null)
        {
            m_GameManager.OnGameStateChanged -= HandleGameStateChanged;
        }
    }

    // カートリッジ生成メソッド
    private void SpawnCartridge()
    {
        // ランダムな位置を計算
        float randomAngle = Random.Range(0f, 360f);
        float randomRadius = Random.Range(0f, m_SpawnRadius);
        
        // 円形の範囲内でランダムな位置を決定
        float xPos = randomRadius * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
        float zPos = randomRadius * Mathf.Sin(randomAngle * Mathf.Deg2Rad);
        
        // 生成位置を設定
        Vector3 spawnPosition = new Vector3(xPos, m_SpawnHeight, zPos);

        // カートリッジを生成
        Instantiate(m_CartridgePrefab, spawnPosition, Quaternion.identity);
    }

    // スポーン用コルーチン
    private IEnumerator SpawnRoutine()
    {
        while (true) 
        {
            SpawnCartridge();  // カートリッジ生成

            // 次の生成まで待機
            yield return new WaitForSeconds(m_SpawnInterval);
        }
    }

    // デバッグ用：生成範囲の可視化
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_SpawnRadius);
    }
}