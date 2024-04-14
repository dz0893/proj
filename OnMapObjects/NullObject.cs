using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Mirror;

public abstract class NullObject : MonoBehaviour
{
	public SpriteRenderer spriteRenderer => GetComponent<SpriteRenderer>();

	public BattleMap map { get; private set; }	
	
	public Player player { get; protected set; }
	
	public virtual bool canSurround => false;
	public virtual bool startTurnWithDelay => false;

	public bool isDead { get; protected set; }
	
	[SerializeField] private List<AudioClip> _idleVoiceLine;
	public List<AudioClip> idleVoiceLine => _idleVoiceLine;

	[SerializeField] protected AudioClip _takingDamagePhrase;
	[SerializeField] protected AudioClip _deathPhrase;

	public int idleVoiceIndex { get; private set; }

	[SerializeField] protected ObjectRenderer _objectRenderer;
	public ObjectRenderer objectRenderer => _objectRenderer;
	
	public bool spriteFlipped => _objectRenderer.spriteRenderer.flipX;

	[SerializeField] private int _index;
	public int index => _index;
	
	[SerializeField] protected string _name;
	public string Name => _name;
	
	[SerializeField] protected string _description;
	public string description => _description;
	
	[SerializeField] private int _leadershipCost;
	public int leadershipCost => _leadershipCost;
	
	[SerializeField] protected int _playerIndex;
	public int playerIndex => _playerIndex;
	
	public int team { get; set; }
	
	[SerializeField] protected Sprite _idleSprite;
	public Sprite idleSprite => _idleSprite;
	
	[SerializeField] protected Sprite _icon;
	public Sprite icon => _icon;
	
	public GroundCell position { get; protected set; }
	
	public virtual bool turnEnded { get; set; }
	
	public bool canUseActions => !turnEnded && player == TurnController.currentPlayer;
	
	public ObjectInfo info { get; protected set; }

	public List<Upgrade> upgradeList { get; set; } = new List<Upgrade>();
	
	public static UnityEvent<NullObject> ObjectInited = new UnityEvent<NullObject>();
	public static UnityEvent<NullObject> ObjectDied = new UnityEvent<NullObject>();
	
	public void Init(GroundCell position, Player player)
	{
		map = BattleMap.instance;
		map.objectList.Add(this);
		
		SetPlayer(player);
		_objectRenderer.Init(this);

		LocalInit(position);

		foreach (Upgrade upgrade in player.upgradeList)
			upgrade.SetUpgradeToObject(this);
		
		SetNewPosition(position);
		
		if (player != null)
		{
			player.currentUnitLimit += leadershipCost;
			
			SetDirectionOnInit(player);
		}

		RenderTurnState();
		ObjectInited.Invoke(this);
	}
	
	public void SetPlayer(Player player)
	{
		this.player = player;
		
		if (player != null)
		{
			team = player.team;
			
			this.player.objectList.Add(this);
		}
	}
	
	public void Select()
	{
		LocalSelect();
		
		OpenObjectUI();
	}
	
	public void IncreaseVoiceIndex()
	{
		idleVoiceIndex++;

		if (idleVoiceIndex >= _idleVoiceLine.Count)
			idleVoiceIndex = 0;
	}

	public void DropVoiceIndexToZero()
	{
		idleVoiceIndex = 0;
	}

	public void OpenObjectUI()
	{
		BattleUIManager.onUI.Invoke(this);
	}
	
	protected virtual void LocalSelect() {}
	
	public virtual void StartTurn() {}
	
	public virtual void EndTurn() {}
	
	public abstract void SetNewPosition(GroundCell newPosition);
	
	protected virtual void LocalInit(GroundCell positionCell) {}

	protected abstract void CleanPositionOfThisObject();
	
	public void Death()
	{
		LocalDeath();
		
		map.objectList.Remove(this);
		
		if (player != null)
			player.objectList.Remove(this);
		
		NullObjectDeath();
	}

	private void NullObjectDeath()
	{
		isDead = true;
		
		ObjectDied.Invoke(this);
		
		gameObject.SetActive(false);

		objectRenderer.voiceSource.clip = null;
		objectRenderer.soundSource.clip = null;
	}

	public IEnumerator DeathIEnumerator()
	{
	//	CleanPositionOfThisObject();

		LocalDeath();
		
		map.objectList.Remove(this);
		
		if (player != null)
			player.objectList.Remove(this);

		PlayVoiceClip(_deathPhrase);

		while (objectRenderer.voiceSource.isPlaying)
			yield return null;
		
		NullObjectDeath();
	}

	public void RotateToRight()
	{
		_objectRenderer.spriteRenderer.flipX = true;
	}
	
	public void RotateToLeft()
	{
		_objectRenderer.spriteRenderer.flipX = false;
	}

	public void SetDirection(GroundCell target)
	{
		if (target.transform.position.x < transform.position.x)
			RotateToLeft();
		else if (target.transform.position.x > transform.position.x)
			RotateToRight();
	}

	protected virtual void SetDirectionOnInit(Player player)
	{
		if (player.capital?.transform.position.x < 0)
				RotateToRight();
			else
				RotateToLeft();
	}
	
	public virtual void ActivateTurnEndedAction() {}
	
	public void SetColor()
	{
		_objectRenderer.SetColor();
	}
	
	public abstract void LocalDeath();
	
	protected void InitInfo(ObjectInfo info)
	{
		this.info = info;
		info.Init(this);
	}

	public void PlayIdleVoiceLine()
	{
		if (idleVoiceLine.Count == 0 || objectRenderer.voiceSource.isPlaying || player != TurnController.lastNotComputerPlayer)
			return;

		DropVoiceIndexToZero();
		PlayVoiceClip(idleVoiceLine[idleVoiceIndex]);
	}

	public void PlayIdleVoiceLineWithIncrementingIndex()
	{
		if (idleVoiceLine.Count == 0 || objectRenderer.voiceSource.isPlaying || player != TurnController.lastNotComputerPlayer)
			return;

		IncreaseVoiceIndex();
		PlayVoiceClip(idleVoiceLine[idleVoiceIndex]);
	}

	public void PlayVoiceClip(AudioClip clip)
	{
		if (clip != null && !objectRenderer.voiceSource.isPlaying)
			objectRenderer.PlayVoice(clip);
	}

	public virtual void RenderTurnState()
	{
		objectRenderer.turnStateRenderer.color = new Vector4(0,0,0,0);
	}
}
