using UnityEngine;

public static class GameplayStatics
{
    /// <summary>
    /// Finds a direction from the mouse world position to a specified location
    /// </summary>
    /// <param name="targetPos">The location to find the direction TO</param>
    /// <returns>A normalized vector2 containing the direction</returns>
    public static Vector2 GetDirectionFromMouseToLocation(Vector3 targetPos)
    {
        Vector3 mouseWorldPos = GameManager.Instance.mainCam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector3 direction = targetPos - mouseWorldPos;
        direction.Normalize();
        
        return new Vector2(direction.x, direction.y);
    }

    public static Vector2 RotateVector(Vector2 v, float angle)
    {
        float rad = Mathf.Deg2Rad * angle;
        float newX = v.x * Mathf.Cos(rad) - v.y * Mathf.Sin(rad);
        float newY = v.x * Mathf.Sin(rad) + v.y * Mathf.Cos(rad);

        return new Vector2(newX, newY);
    }
}
