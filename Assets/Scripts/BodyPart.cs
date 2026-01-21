using System;
using UnityEngine;
using Скриптерсы;
using Скриптерсы.Datas;

public class BodyPart : MonoBehaviour, IDamageable
{
    [SerializeField] private BodyPartData _bodyPartData;
    public event Action<DamageInfo> OnTakeDamage;
    public void TakeDamage(DamageInfo damageInfo)
    {
        damageInfo.Count *= _bodyPartData.DamageMultiplayer;
        
        OnTakeDamage?.Invoke(damageInfo);
    }
}
