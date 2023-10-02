using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileTurret : MonoBehaviour
{
    public void Init(Vector3 _spawnPos)
    {
        StartCoroutine(LaunchMissileCoroutine(_spawnPos));
    }

    private IEnumerator LaunchMissileCoroutine(Vector3 _spawnPos)
    {
        Vector3 launchDirection = transform.forward;
        float launchAngle = Vector3.Angle(launchDirection, Vector3.up) * Mathf.Deg2Rad;

        // 초기 속도를 X, Y, Z 방향으로 계산
        float initialVelocityX = initialSpeed * Mathf.Cos(launchAngle) * launchDirection.x;
        float initialVelocityY = initialSpeed * Mathf.Sin(launchAngle);
        float initialVelocityZ = initialSpeed * Mathf.Cos(launchAngle) * launchDirection.z;

        Vector3 initialVelocity = new Vector3(initialVelocityX, initialVelocityY, initialVelocityZ);

        Vector3 prevPos = transform.position;
        float time = 0f;
        float xPos = 0f;
        float yPos = 0f;
        float zPos = 0f;

        while (true)
        {
            // 포물선 운동 계산
            xPos = initialVelocity.x * time;
            yPos = initialVelocity.y * time - 0.5f * gravity * time * time;
            zPos = initialVelocity.z * time;

            // 물체의 위치를 업데이트
            transform.position = new Vector3(xPos, yPos, zPos) + _spawnPos;

            // 시간 업데이트
            time += Time.deltaTime;
            if(!transform.position.Equals(prevPos))
                transform.rotation = Quaternion.LookRotation(transform.position - prevPos);

            prevPos = transform.position;
            // 포물선 운동이 목표 지점에 도달하면 루프를 종료
            if (transform.position.y <= 0f)
            {
                break;
            }
            yield return null;
        }

        // 포물선 운동이 끝나면 물체를 파괴
        Destroy(gameObject);
    }

    [SerializeField]
    private float initialSpeed = 10f;
    [SerializeField]
    private float gravity = 9.81f;
}
