using UnityEngine;
using UnityEngine.UI;

public class PlayerStockArea : MonoBehaviour
{
    [SerializeField]
    private Image[] m_SingleShells;  // Shell1～Shell10の参照を保持

    [SerializeField]
    private Image[] m_TenShells;    // Shells10～Shells40の参照を保持

    private void Start()
    {
        // 初期状態では全て非表示
        UpdatePlayerStockArea(0);
    }

    public void UpdatePlayerStockArea(int stockCount)
    {
        // 1の位の弾数を処理（Shell1～Shell10）
        int singles = stockCount % 10;
        singles = (singles == 0 && stockCount > 0) ? 10 : singles;
        
        for (int i = 0; i < m_SingleShells.Length; i++)
        {
            m_SingleShells[i].gameObject.SetActive(i < singles);
        }

        // 10の位の弾数を処理（Shells10～Shells40）
        int tens = stockCount / 10;
        for (int i = 0; i < m_TenShells.Length; i++)
        {
            // i番目のTenShellsを、i < tensの場合のみ表示
            m_TenShells[i].gameObject.SetActive(i < tens);
        }
    }
}