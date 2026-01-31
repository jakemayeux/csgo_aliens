using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

partial struct CameraSystem : ISystem
{
    float yaw;
    float pitch;

    public void OnUpdate(ref SystemState state)
    {
        float mouseSensitivity = 20f;

        Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerData>();
        LocalTransform playerTransform = SystemAPI.GetComponent<LocalTransform>(playerEntity);

        Camera.main.transform.position = new Vector3(playerTransform.Position.x, playerTransform.Position.y, playerTransform.Position.z) + new Vector3(0, 1, 0);

        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        Camera.main.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}
