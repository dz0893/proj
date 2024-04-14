using System.Collections;
using UnityEngine;

public class AnimationObject : MonoBehaviour
{
	[SerializeField] private Animator _animator;
	[SerializeField] private SpriteRenderer _objectSprite;

	[SerializeField] private AnimationCurve _flyingCurve;
	
	[SerializeField] private Vector3 _offsetPosition;
	private Vector3 scaledOffsetPosition => new Vector3(_offsetPosition.x * scale, _offsetPosition.y * scale, _offsetPosition.z);

	public float scale { get; set; }
	public float animationTime { get; private set; }

	private Vector3 normalVector = new Vector3(1,0,0);

	public IEnumerator OnCellAnimation(AnimationClip animation, GroundCell startedCell, float angle, bool flipX)
	{
		_objectSprite.flipX = !flipX;
		transform.up = new Vector3(0,0,0);
		transform.rotation *= Quaternion.Euler(0, 0, angle);
		animationTime = animation.length;
		
		Vector3 position = new Vector3(startedCell.transform.position.x, startedCell.transform.position.y, 0) + scaledOffsetPosition;
		transform.position = position;
		_animator.Play(animation.name);
		yield return new WaitForSeconds(animationTime);
		Destroy(gameObject);
	}
	
	public IEnumerator FlyingAnimation(AnimationClip animation, GroundCell startedCell, GroundCell targetCell, bool flyingWithCurve)
	{
		float angle = Vector3.Angle(targetCell.transform.position - startedCell.transform.position, normalVector);
		
		int sign = 1;
		if (targetCell.transform.position.y - startedCell.transform.position.y < 0)
			sign = -1;

		transform.rotation *= Quaternion.Euler(0, 0, angle * sign);

		_animator.Play(animation.name);
		
		Vector3 startedPosition = new Vector3(startedCell.transform.position.x, startedCell.transform.position.y, 0) + scaledOffsetPosition;
		Vector3 goalPosition = new Vector3(targetCell.transform.position.x, targetCell.transform.position.y, 0) + scaledOffsetPosition;
		Vector3 deltaPos = goalPosition - startedPosition;
		Vector3 moveProgress = new Vector3(0,0,0);

		if (flyingWithCurve)
		{
			animationTime = ActionSettings.ARROWFLYINGTIME / scale;
			
			int range = (int)deltaPos.magnitude + 1;

			for (int i = 1; i < range; i++)
			{
				animationTime += animationTime * 1.5f / (1 + i);
			}
		}
		else
		{
			animationTime = deltaPos.magnitude * ActionSettings.ARROWFLYINGTIME / scale;
		}
		
		float progress = 0;
		
		while (progress < 1)
		{
			progress += Time.deltaTime / animationTime;
			
			moveProgress.x = startedPosition.x + deltaPos.x * progress;
			moveProgress.y = startedPosition.y + deltaPos.y * progress;
			
			if (flyingWithCurve)
				moveProgress.y += _flyingCurve.Evaluate(progress);

			transform.position = moveProgress;
			yield return null;
		}

		Destroy(gameObject);
	}
}
