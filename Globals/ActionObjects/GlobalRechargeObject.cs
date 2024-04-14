public class GlobalRechargeObject : GlobalActionObject
{
	public override GlobalAction GetGlobalAction()
	{
		return new GlobalRecharge(this);
	}
}
