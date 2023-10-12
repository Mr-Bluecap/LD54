using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class RequestRequirementDisplay : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI conditionText;

        [SerializeField]
        Image checkMarkImage;

        [SerializeField]
        Sprite incompleteCheckSprite;

        [SerializeField]
        Sprite completeCheckSprite;

        void Start()
        {
            UpdateProgress(false);
        }

        public void UpdateConditionText(string newCondition)
        {
            conditionText.text = newCondition;
        }

        public void UpdateProgress(bool isComplete)
        {
            checkMarkImage.sprite = isComplete ? completeCheckSprite : incompleteCheckSprite;
        }
    }
}