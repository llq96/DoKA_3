using System;
using System.Linq;
using UnityEngine;
using VladB.EffectsSystem;

namespace VladB.Doka
{
    public class UnitEffectsReceiver : EffectsReceiver
    {
        private Unit _unit;

        [SerializeField] private EffectData_ChangeValue_Float _effectData_baseMoveSpeed;
        [SerializeField] private EffectData_ChangeValue_Float _effectData_baseMaxHp;

        public void Init(Unit unit)
        {
            _unit = unit;

            OnAddAnyEffectWithType += RecalculateChecks;
            OnRemoveAnyEffectWithType += RecalculateChecks;

            AddEffect<UnitEffect_MoveSpeed, EffectData_ChangeValue_Float>(_effectData_baseMoveSpeed);
            AddEffect<UnitEffect_MaxHP, EffectData_ChangeValue_Float>(_effectData_baseMaxHp);
        }

        public void RecalculateChecks(Type t)
        {
            //TODO
            // if (t == typeof(FYW_PersonEffect_DamageBuster))
            // {
            //     Recalculate_Damage();
            // }
            // else if (t == typeof(FYW_PersonEffect_ModelSize))
            // {
            //     Recalculate_ModelSize();
            // }
            // else if (t == typeof(FYW_PersonEffect_StringValueEnable))
            // {
            //     Recalculate_Particles();
            // }
            // else if (t == typeof(UnitEffect_MoveSpeed))
            // {
            Recalculate_MoveSpeed();
            Recalculate_MaxHp();
            // }
            // else if (t == typeof(FYW_PersonEffect_CanAttack))
            // {
            //     Recalculate_CanAttack();
            // }
        }

        public void Recalculate_MoveSpeed()
        {
            float moveSpeed = GetEffectsWithType<UnitEffect_MoveSpeed>().OfType<Effect_ChangeValue_Float>()
                .ToList().Calc();
            _unit.Mover.Speed = moveSpeed;
        }

        public void Recalculate_MaxHp()
        {
            float maxHp = GetEffectsWithType<UnitEffect_MaxHP>().OfType<Effect_ChangeValue_Float>()
                .ToList().Calc();
            _unit.Stats.SetMaxHp(maxHp);
        }
    }
}