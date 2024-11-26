using System;
#nullable enable

public class TriggerEffect
    {
        public ICardEffect effect;
        public event Action? subscribers;

        public TriggerEffect(ICardEffect _effect){
            effect = _effect;
        }   

         public void RaiseEvent()
        {
            subscribers?.Invoke();
        }
    }