using System.Collections;
using UnityEngine;

namespace Yoziya.manjuu
{
    public class AbstractController : MonoBehaviour, IController
    {
        private IPlayer mState;
        public void Initialize()
        {

        }
        void Start()
        {
            mState = this.GetState<Player>();
            mState.Score.AddListener(a => Debug.Log(mState.Score.Value));
            mState.Score.Value++;
        }
        public IApp GetApp()
        {
            return Manjuu.Interface;
        }

        private void OnDestroy()
        {
            // 8. 将 Model 设置为空
            mState = null;
        }
    }
}