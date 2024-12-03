using UnityEngine;

public class TPSCameraController : MonoBehaviour
{
    public Transform turretTransform; // 砲塔のTransform
    public Vector3 offset = new Vector3(0f, 3f, -5f); // カメラの相対位置

    private void LateUpdate()
    {
        if (turretTransform == null)
            return;

        // カメラ位置を更新
        Vector3 desiredPosition = turretTransform.position + turretTransform.TransformDirection(offset);
        transform.position = desiredPosition;

        // カメラの向きを砲塔の方向に設定
        transform.LookAt(turretTransform.position + turretTransform.forward * 10f);
    }
}
