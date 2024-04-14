public class Stun : ActionEffect
{
	public override EffectType effectType => EffectType.Stun;

	public override void Dot(Unit target, CurrentEffect effect)
	{
		target.EndTurn();
	}
}
