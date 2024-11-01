using UnityEngine;
using System.Collections;

namespace Complete 
{
    public class CartridgeSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_CartridgePrefab;    // ShellCartridgeプレハブ

        [SerializeField]
        private float m_SpawnInterval = 10f;     // 生成間隔（秒）

        [SerializeField]
        private float m_AreaSize = 100f;          // エリアのサイズ（100x100）

        [SerializeField]
        private float m_SpawnHeight = 0.5f;      // 生成する高さ

        private GameManager m_GameManager;
        private Coroutine m_SpawnCoroutine;

        private void Start()
        {
            m_GameManager = FindObjectOfType<GameManager>();
            
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

        private void HandleGameStateChanged(GameManager.GameState newState)
        {
            if (newState == GameManager.GameState.RoundPlaying)
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
            // 正方形エリア内でランダムな位置を計算
            float halfSize = m_AreaSize * 0.5f;
            float xPos = Random.Range(-halfSize, halfSize);
            float zPos = Random.Range(-halfSize, halfSize);
            
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
            // 正方形の範囲を表示
            Gizmos.color = Color.yellow;
            float halfSize = m_AreaSize * 0.5f;
            Vector3 center = transform.position;
            
            Vector3 p1 = center + new Vector3(-halfSize, 0, -halfSize);
            Vector3 p2 = center + new Vector3(-halfSize, 0, halfSize);
            Vector3 p3 = center + new Vector3(halfSize, 0, halfSize);
            Vector3 p4 = center + new Vector3(halfSize, 0, -halfSize);
            
            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p2, p3);
            Gizmos.DrawLine(p3, p4);
            Gizmos.DrawLine(p4, p1);
        }
    }
}