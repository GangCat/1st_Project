using UnityEngine;

public class MinimapCameraViewport : MonoBehaviour
{


    private void OnGUI()
    {
        // 카메라의 월드 좌표를 미니맵의 좌표로 변환
        Vector2 minimapPosition = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.z) * minimapScale;

        // 카메라의 크기를 미니맵의 크기로 변환
        float cameraHeight = mainCamera.orthographicSize * 2f * minimapScale;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // 인디케이터의 위치와 크기를 Rect로 정의
        Rect cameraIndicatorRect = new Rect(minimapPosition.x, minimapPosition.y, cameraWidth, cameraHeight);

        // 인디케이터를 화면에 그림
        GUI.DrawTexture(cameraIndicatorRect, cameraIndicatorTexture);
    }
    
    public Camera mainCamera;
    public float minimapScale = 1f;
    public Texture2D cameraIndicatorTexture;
}
