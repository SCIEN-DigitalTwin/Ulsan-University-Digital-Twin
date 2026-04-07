using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HipPositionFix : MonoBehaviour
{
    public Transform hip,whips;
    Transform select,womanselect;
   // public Joint hipJoint,womanhipjoint;

    public List<Transform> standPoints;
    public List<Transform> WstandPoints;
    bool UWBorNot = true;
    bool ManOrWoman = true;


    //public Button BianHuan, LianJie;


    [System.Serializable]
    public class Joint
    {
        public Transform parent;
        public Transform transform;
        public float radius;
        public Joint[] nexts;
    }





    [SerializeField]
    private float m_distanceToFloor;
    /*public float distanceToFloor
    {
        get
        {
            var jointOnGround = hipJoint;
            var cur = jointOnGround;
            for (int i = 0; i< 3;++i)
            {
                var joint = cur;
                if (joint.transform.position.y < jointOnGround.transform.position.y)
                {
                    jointOnGround = joint;
                }
            }
            dfs(hipJoint.nexts[0], joint => { 
                if (joint.transform.position.y < jointOnGround.transform.position.y)
                {
                    jointOnGround = joint;
                }
            });
            dfs(hipJoint.nexts[1], joint => {
                if (joint.transform.position.y < jointOnGround.transform.position.y)
                {
                    jointOnGround = joint;
                }
            });

            var dir = jointOnGround.transform.position - jointOnGround.parent.position;
            var roundRight = Vector3.Cross(dir, Vector3.up);
            var roundDown = Vector3.Cross(dir, roundRight).normalized;

            var offset = Vector3.Dot(new Vector3(dir.x, 0, dir.z).normalized, roundDown * jointOnGround.radius);

            return hip.position.y - jointOnGround.transform.position.y + offset;
        }
    }*/


    void dfs(Joint root, System.Action<Joint> Todo)
    {
        Todo(root);
        if (root.nexts != null && root.nexts.Length > 0) dfs(root.nexts[0], Todo);
    }
    public float distanceToFloor
    {

        get
        {

            float maxDistance = float.MinValue;
            select = default;

            if (ManOrWoman)
            {
                standPoints.ForEach(point =>
                {
                    var distance = hip.position.y - point.position.y;
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        select = point;
                    }
                });
            }

            else
            {
                WstandPoints.ForEach(point =>
                {
                    var distance = whips.position.y - point.position.y;
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        womanselect = point;
                    }
                });
            }
            m_distanceToFloor = maxDistance;
            return maxDistance;
        }
    }

    private Vector3 originCenter;

    void Start()
    {

        // BianHuan.interactable = false;
        // LianJie.interactable = true;
        originCenter = hip.position;
        originCenter.y = 0;
    }



    // public void ModelLianjie()
    // {
    //    BianHuan.interactable = true;
    //    LianJie.interactable = false;
    // }

    // public void ModelChange()
    // {
    //     BianHuan.interactable = false;
    //     LianJie.interactable = true;
    // }

    [ContextMenu("NoUWB")]
    public void NoOrYesUWB()
    {
        if (UWBorNot == false&& ManOrWoman == false)
        {
            UWBorNot = true;
            ManOrWoman = true;

        }
        else if (UWBorNot == true  && ManOrWoman == true)
        {
            UWBorNot = false;
            ManOrWoman = false;

        }

    }
    [ContextMenu("YesUWB")]
    public void YesUWB()
    {
        UWBorNot = false;
        ManOrWoman = false;
    }
    void LateUpdate()
    {
        if (UWBorNot && ManOrWoman)
        {
            var hipPos = originCenter + distanceToFloor * Vector3.up;
            //hip.transform.position = hipPos;

            // 获取当前的hip位置
            Vector3 currentPosition = hip.transform.position;

            // 创建一个新的Vector3来更新y值，保持x和z不变
            Vector3 newPosition = new Vector3(currentPosition.x, hipPos.y, currentPosition.z);

            // 设置hip的新位置
            hip.transform.position = newPosition;
        }
        else if (UWBorNot == false && ManOrWoman == false)
        {
            var hipPos = originCenter + distanceToFloor * Vector3.up;
            whips.transform.position = hipPos;
        }

    }
}
