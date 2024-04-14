using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnMapGroundCell : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private Image _cellImage;
	[SerializeField] private Image _object;
	[SerializeField] private Image _oreObject;

	[SerializeField] private Sprite _normalCell;
	[SerializeField] private Sprite _waterCell;
	[SerializeField] private Sprite _mountainCell;
	[SerializeField] private Sprite _oreCell;

	[SerializeField] private Sprite _goldImage;
	[SerializeField] private Sprite _oreImage;
	
	public GroundCell cell { get; private set; }
	public Vector3 position { get; private set; }
	
	public void OnPointerClick (PointerEventData eventData)
	{
		if (!PlayerUI.inputIsLocked)
			CameraController.setCameraPosition.Invoke(cell.transform.position);
	}
	
	public void Init(GroundCell cell)
	{
		this.cell = cell;
		position = cell.transform.position;
		transform.localPosition = position;
	}
	
	private void SetColor(GroundCell cell)
	{
		if (cell.terrainType == TerrainType.GoldDeposit)
			_cellImage.color = MapSettings.goldDepositOnMapColor;
		else if (cell.terrainType == TerrainType.OreDeposit)
			_cellImage.color = MapSettings.oreDepositOnMapColor;
		else if (cell.movingType > MovingType.Walk)
			_cellImage.color = MapSettings.unmovableCellOnMapColor;
		else
			_cellImage.color = MapSettings.normalCellOnMapColor;
	}
	
	public void Render()
	{
		SetColor(cell);
		
		if (cell.onCellObject == null)
		{
			_object.gameObject.SetActive(false);
		}
		else
		{
			_object.gameObject.SetActive(true);
			_object.color = cell.onCellObject.player.color;
		}
	}
}
