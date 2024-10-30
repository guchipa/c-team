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

        // 1の位の処理
        int singles = stockCount % 10;
        singles = (singles == 0 && stockCount > 0) ? 10 : singles;

        for (int i = 0; i < m_SingleShells.Length; i++)
        {
            bool shouldBeActive = i < singles;
            m_SingleShells[i].gameObject.SetActive(shouldBeActive);
        }

        // 10の位の処理
        int tens = stockCount / 10;
        if (singles == 10) tens--;

        for (int i = 0; i < m_TenShells.Length; i++)
        {
            bool shouldBeActive = i < tens;
            m_TenShells[i].gameObject.SetActive(shouldBeActive);
        }
    }
}