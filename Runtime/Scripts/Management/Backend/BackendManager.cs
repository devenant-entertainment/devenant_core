using System;
using System.Collections.Generic;
using UnityEngine;
namespace Devenant
{
    public class BackendManager : Singleton<BackendManager>
    {
        public BackendData data { get { if (_data == null) { _data = new BackendData(backendData); } return _data; } }
        private BackendData _data;

        [SerializeField] private BackendAsset backendData;
    }
}
