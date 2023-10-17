using UnityEngine;

public class MinimapCameraIndicator : MonoBehaviour
{

    private void Update()
    {
        UpdateCameraIndicator();
    }

    private void UpdateCameraIndicator()
    {
        // 카메라의 월드 좌표를 미니맵의 좌표로 변환
        Vector2 minimapPosition = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.z) * minimapScale;
        
        // 카메라의 크기를 미니맵의 크기로 변환
        float cameraHeight = mainCamera.orthographicSize * 2f * minimapScale;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        
        // 이미지의 위치와 크기를 업데이트
        minimapCameraIndicator.anchoredPosition = minimapPosition;
        minimapCameraIndicator.sizeDelta = new Vector2(cameraWidth, cameraHeight);
    }
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RectTransform minimapCameraIndicator;
    [SerializeField] private float minimapScale = 1f;  // 월드 좌표를 미니맵 좌표로 변환할 때 사용할 스케일
    
}
