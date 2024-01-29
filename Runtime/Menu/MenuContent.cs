using System.Collections.Generic;
using UnityEngine;

namespace Devenant
{
    public class MenuContent
    {
        private RectTransform holder;
        private GameObject reference;

        public List<GameObject> elements { get { return _elements; } private set { _elements = value; } }
        private List<GameObject> _elements;

        public MenuContent(RectTransform holder, GameObject reference)
        {
            this.holder = holder;
            this.reference = reference;

            elements = new List<GameObject>();

            reference.SetActive(false);
        }

        public GameObject Create()
        {
            GameObject newElement = Object.Instantiate(reference, holder);
            newElement.SetActive(true);

            elements.Add(newElement);

            return newElement;
        }

        public void Remove(GameObject element)
        {
            elements.Remove(element);

            Object.Destroy(element);
        }

        public void Clear()
        {
            foreach(GameObject element in elements)
            {
                Object.Destroy(element);
            }

            elements = new List<GameObject>();
        }
    }
}