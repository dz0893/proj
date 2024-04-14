using System.Collections.Generic;

public class GlobalBurning : GlobalAction, IAreaAction
{
	public TerrainType newTerrainType { get; private set; }
	public DamageType damageType { get; private set; }
	public bool terrainChanger { get; private set; }
	
	public int area => 1;
	
	public GlobalBurning(GlobalBurningObject actionObject)
	{
		this.actionObject = actionObject;
		
		newTerrainType = actionObject.newTerrainType;
		damageType = actionObject.damageType;
		terrainChanger = actionObject.terrainChanger;
		
		InitDescriptionList();
	}
	
	public override GroundCell GetAITarget(Player player)
	{
		GroundCell target = null;
		int bestCounter = 0;
		
		foreach (GroundCell targetCell in areaList)
		{
			int currentCounter = 0;
			
			foreach (GroundCell cell in GetAreaDistance(targetCell))
			{
				if (cell.onCellObject != null)
				{
					if (cell.onCellObject.team != player.team)
						currentCounter++;
					else
						currentCounter -= 2;
				}
			}
			
			if (currentCounter > bestCounter)
			{
				target = targetCell;
				bestCounter = currentCounter;
			}
		}
		
		return target;
	}
	
	protected override void CurrentActivate(Player player, GroundCell target)
	{
		foreach (GroundCell cell in GetAreaDistance(target))
			OnCellEffect(cell);
	}
	
	private void OnCellEffect(GroundCell target)
	{
		if (target.onCellObject != null)
			target.onCellObject.GetAttack(actionValue, damageType);
		if (target.unmaterialOnCellObject != null && target.unmaterialOnCellObject.initer == null)
			target.unmaterialOnCellObject.Death();
		if (target.canBeTerraformated && target.terrainType != newTerrainType && terrainChanger)
			target.SetTerrainType(BattleMap.instance.groundFactory.GetTerrain(newTerrainType));
	}
	
	protected override List<GroundCell> GetRemoovedCells(Player player, List<GroundCell> areaList)
	{
		return new List<GroundCell>();
	}
	
	protected override string GetActionValueString()
	{
		return UISettings.damage + actionValue + " " + UISettings.GetDamageTypeName(damageType);
	}
	
	public override string GetRenderedText(GroundCell cell)
	{
		return actionTextGetter.GetDamageText(cell.onCellObject, actionValue, damageType);
	}
}
