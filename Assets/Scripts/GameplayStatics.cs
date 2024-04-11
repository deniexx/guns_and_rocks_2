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

    public static Vector3 RotateAngleAxis(float angle, Vector3 axis, Vector3 vector)
    {
        float sin = Mathf.Sin(angle * Mathf.Deg2Rad);
        float cos = Mathf.Sin(angle * Mathf.Deg2Rad);

        float xx = axis.x * axis.x;
        float yy = axis.y * axis.y;
        float zz = axis.z * axis.z;
                    
        float xy = axis.x * axis.y;
        float yz = axis.y * axis.z;
        float zx = axis.z * axis.x;
                    
        float xs = axis.x * sin;
        float ys = axis.y * sin;
        float zs = axis.z * sin;

        float omc = 1f - cos;

        return new Vector3(
            (omc * xx + cos) * vector.x + (omc * xy - zs) * vector.y + (omc * zx + ys) * vector.z,
            (omc * xy + zx) * vector.x + (omc * yy + cos) * vector.y + (omc * yz - xs) * vector.z,
            (omc * zx - ys) * vector.x + (omc * yz + xs) * vector.y + (omc * zz + cos) * vector.z
        );
    }
}
