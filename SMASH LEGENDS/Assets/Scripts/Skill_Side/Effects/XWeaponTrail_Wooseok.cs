using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Wooseok
{

    public class XWeaponTrail_Wooseok : XftWeapon.XWeaponTrail
    {

        public new void Activate()
        {
            if (mActivated)
            {
                return;
            }

            Init();
            mActivated = true;
            //gameObject.SetActive(true);
            //mVertexPool.SetMeshObjectActive(true);

            mFadeT = 1f;
            mIsFading = false;
            mFadeTime = 1f;
            mFadeElapsedime = 0f;
            mElapsedTime = 0f;

            //reset all elemts to head pos.
            for (int i = 0; i < mSnapshotList.Count; i++)
            {
                mSnapshotList[i].PointStart = PointStart.position;
                mSnapshotList[i].PointEnd = PointEnd.position;

                mSpline.ControlPoints[i].Position = mSnapshotList[i].Pos;
                mSpline.ControlPoints[i].Normal = mSnapshotList[i].PointEnd - mSnapshotList[i].PointStart;
            }

            //reset vertex too.
            
            RefreshSpline();
            UpdateVertex();
        }
        public new void Deactivate()
        {
            mVertexPool.Destroy();
            mInited = false;
            mActivated = false;
            //gameObject.SetActive(false);
            mVertexPool.SetMeshObjectActive(false);
        }
        private void OnEnable()
        {
            Activate();
            if (!UseWithSRP)
            {
                Camera.onPostRender += MyPostRender;
                Camera.onPreRender += MyPreRender;
            }
        }
        private void OnDisable()
        {
            Deactivate();
            if (!UseWithSRP)
            {
                Camera.onPostRender -= MyPostRender;
                Camera.onPreRender -= MyPreRender;
            }
        }
    }

    
}