using UnityEngine;

namespace Completed
{
    public class TpsCameraControl : MonoBehaviour
    {
        // 追従対象のタンクのTransformコンポーネントを保持する変数
        public Transform targetTank;

        // 戦車の背後にカメラを配置するオフセット位置
        [SerializeField]
        private float distanceBehind = 10f; // 戦車の後方距離

        // 戦車からカメラの高さ
        [SerializeField]
        private float heightAbove = 5f; // 戦車の上方高さ

        private void FixedUpdate()
        {
            // ターゲットが設定されていない場合、処理をスキップ
            if (targetTank == null) return;

             // カメラの目標位置を計算
            Vector3 targetPosition = targetTank.position 
                                    - targetTank.forward * distanceBehind // 戦車の背後
                                    + Vector3.up * heightAbove;          // 戦車の上方

            // カメラの位置を更新
            transform.position = targetPosition;

             // 戦車の向きにカメラを向ける（回転を更新）
            transform.rotation = Quaternion.LookRotation(targetTank.position - transform.position);
        }
    }
}
