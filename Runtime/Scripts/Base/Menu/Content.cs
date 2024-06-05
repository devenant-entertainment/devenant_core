using System.Collections.Generic;
using UnityEngine;

namespace Devenant.Menu
{
    public class Content
    {
        private RectTransform holder;
        private GameObject reference;

        public GameObject[] elements { get { return _elements.ToArray(); } }
        private List<GameObject> _elements;

        public Content(RectTransform holder, GameObject reference)
        {
            this.holder = holder;
            this.reference = reference;

            _elements = new List<GameObject>();

            reference.SetActive(false);
        }

        public GameObject Create()
        {
            GameObject newElement = Object.Instantiate(reference, holder);
            newElement.SetActive(true);

            _elements.Add(newElement);

            return newElement;
        }

        public void Remove(GameObject element)
        {
            _elements.Remove(element);

            Object.Destroy(element);
        }

        public void Clear()
        {
            foreach(GameObject element in _elements)
            {
                Object.Destroy(element);
            }

            _elements = new List<GameObject>();
        }

        public C[] GetComponents<C>() where C : Object
        {
            List<C> results = new List<C>();

            foreach(GameObject element in elements)
            {
                results.Add(element.GetComponent<C>());
            }

            return results.ToArray();
        }
    }
}
