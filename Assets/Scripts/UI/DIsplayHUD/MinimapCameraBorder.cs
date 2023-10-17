using UnityEngine;

public class MinimapCameraBorder : MonoBehaviour
{


    private void Update()
    {
        DrawMinimapBorder();
    }

    void DrawMinimapBorder()
    {
        // 메인 카메라의 4개 모서리 좌표를 계산
        Vector3[] corners = new Vector3[4];
        corners[0] = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.farClipPlane));
        corners[1] = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, mainCamera.farClipPlane));
        corners[2] = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.farClipPlane));
        corners[3] = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, mainCamera.farClipPlane));

        // 라인렌더러의 점 개수를 설정
        lineRenderer.positionCount = 5;
        
        // 각 꼭지점 설정
        for (int i = 0; i < 4; i++)
        {
            Vector3 minimapPos = WorldToMinimap(corners[i]);
            lineRenderer.SetPosition(i, new Vector3(minimapPos.x, minimapPos.y, 0)); // Z 값은 0으로
        }
        // 첫 번째 꼭지점을 마지막에 다시 추가하여 사각형을 완성
        lineRenderer.SetPosition(4, lineRenderer.GetPosition(0));
    }

    Vector2 WorldToMinimap(Vector3 worldPos)
    {
        Vector3 relativePos = worldPos - minimapWorldCenter;
        float xPercent = (relativePos.x + minimapWorldSize.x * 0.5f) / minimapWorldSize.x;
        float zPercent = (relativePos.z + minimapWorldSize.z * 0.5f) / minimapWorldSize.z;

        return new Vector2(xPercent * minimapRect.sizeDelta.x, zPercent * minimapRect.sizeDelta.y);
    }
    
    
    public Camera mainCamera; // 메인 카메라
    public LineRenderer lineRenderer; // 라인 렌더러
    public RectTransform minimapRect; // 미니맵 RectTransform

    public Vector3 minimapWorldSize; // 미니맵이 표현하는 월드의 크기
    public Vector3 minimapWorldCenter; // 미니맵이 표현하는 월드의 중심 좌표
    
}