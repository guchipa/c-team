using UnityEngine;
using System.Collections;

public class Cartridge : MonoBehaviour
{
    public float m_LifeTime = 15f;            // カートリッジが存在する総時間
    public float m_BlinkStartTime = 5f;       // 明滅開始までの時間
    public float m_BlinkInterval = 0.5f;      // 明滅の間隔

    private Renderer m_Renderer;
    private bool m_IsBlinking = false;

    private void Start()
    {
        // Rendererコンポーネントの取得
        m_Renderer = GetComponent<Renderer>();
        
        // LifeTime秒後に破壊
        Destroy(gameObject, m_LifeTime);
        
        // 明滅開始
        StartCoroutine(StartBlinkRoutine());
    }

    private IEnumerator StartBlinkRoutine()
    {
        // BlinkStartTime秒待ってから明滅開始
        yield return new WaitForSeconds(m_BlinkStartTime);
        
        m_IsBlinking = true;
        StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        while (m_IsBlinking)
        {
            // Rendererの有効/無効を切り替え
            m_Renderer.enabled = !m_Renderer.enabled;
            
            // 指定された間隔待機
            yield return new WaitForSeconds(m_BlinkInterval);
        }
    }

    private void OnDestroy()
    {
        // 破壊時に明滅を停止
        m_IsBlinking = false;
    }
}