using UnityEngine;
using Zenject;
using Скриптерсы.Datas;

namespace Скриптерсы.Interactable
{
    public class FirstAidKit: HighlightableObject, IClickable
    {
        [SerializeField] private FirstAidKitData firstAidKitData;
        [Inject] private PlayerHealth _playerHealth;
        public ClickResult Click()
        {
            _playerHealth.Heal(firstAidKitData.HealAmount);
            
            Destroy(gameObject);   

            return new ClickResult(true);
        }
    }
}