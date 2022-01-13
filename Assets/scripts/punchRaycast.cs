using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class punchRaycast
{
    public static void hit(Vector3 hitPosition, Vector3 hitDirection)
    {
        RaycastHit2D raycastHit2d = Physics2D.Raycast(hitPosition,hitDirection);

        if(raycastHit2d.collider != null){
            
        }
    }
}
