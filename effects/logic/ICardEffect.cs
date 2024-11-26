public interface ICardEffect
{
    public ICard? ownerCard {get; set;}
    public ActivationState activationState {get; set;}
    public SpellSpeed spellSpeed {get; set;}
    public bool negatable {get; set;}
    public RequirementType requirementType {get; set;}
    public int originalActivations {get; set;}
    public int activationsRemaining {get; set;}
    public EffectDependency dependency {get; set;}
    public bool isAnimated {get; set;}

    public bool DeleteConditional();
    public bool Conditional(); // make sure it does not activate infinitly 
    public void Activate(int cardeffectActivationChainIndex); //cardeffectActivationChainIndex is used to mark effect events to activations // activateIndex is the index of the function that will be the start point of executing the list of effect functions that was registered  ! cardEffectActivationChainIndex not used?
    public void Refresh();
    public void Effect(int cardEffectActivationChainIndex); //effect index is the index of the function that will be the start point of executing the list of effect functions that was registered ! cardEffectActivationChainIndex not used?
    public ICard GetCard();

    public int GetActivationsRemaining();
    public void SetActivationsRemaining(int _activationsRemaining);



    //public ICardEffect? GetRefreshEffect();
}