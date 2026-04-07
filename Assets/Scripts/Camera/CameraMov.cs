using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraMov : MonoBehaviour
{
    public Transform mainCameraTransform;
   

    public Vector3 rawMaterialUnitPosition;
    public Vector3 constructionProcessPosition;
    public Vector3 packagingProcessPosition;
    public Vector3 finishedGoodsUnitPosition;
    public Vector3 homePosition;

    
    public bool objStatus = false;



    public void MoveRawMaterial()
    {
        objStatus = !objStatus;
        if (objStatus == true)
        {
            mainCameraTransform.position = rawMaterialUnitPosition;
            mainCameraTransform.rotation = Quaternion.Euler(0, -90, 0);
        }

        
    }
    public void ConstructionProcess()
    {
        objStatus = !objStatus;
        if (objStatus == true)
        {
            mainCameraTransform.position = constructionProcessPosition;
            mainCameraTransform.rotation = Quaternion.Euler(0, -180, 0);
        }
        
    }
    public void PackagingProcess()
    {
        objStatus = !objStatus;
        if (objStatus == true)
        {
            mainCameraTransform.position = packagingProcessPosition;
            mainCameraTransform.rotation = Quaternion.Euler(0, -180, 0);
        }
        
    }
    public void FinishedGoodsUnit()
    {
        objStatus = !objStatus;
        if (objStatus == true)
        {
            mainCameraTransform.position = finishedGoodsUnitPosition;
            mainCameraTransform.rotation = Quaternion.Euler(0, -90, 0);
        }
       
    }
    public void HOME()
    {
        objStatus = !objStatus;
        if (objStatus == true)
        {
            mainCameraTransform.position = homePosition;
            mainCameraTransform.rotation = Quaternion.Euler(0, -180, 0);
        }

    }

}
