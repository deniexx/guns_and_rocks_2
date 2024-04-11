using UnityEngine;

public static class GameplayStatics
{
    public static Vector2 GetDirectionFromMouseToLocation(Vector3 targetPos)
    {
        Vector3 mouseWorldPos = GameManager.Instance.mainCam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3 direction = targetPos - mouseWorldPos;
        direction.Normalize();
        
        return new Vector2(direction.x, direction.y);
    }
}
