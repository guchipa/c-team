using UnityEngine;

public class Mine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WarpGate"))
        {
            // 地雷の挙動を変更または何も起こらないように設定
            // 必要に応じて追加ロジック
        }
    }
}
