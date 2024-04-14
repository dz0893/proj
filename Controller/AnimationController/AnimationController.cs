using UnityEngine;
using System;
using System.Collections;

public class AnimationController : MonoBehaviour
{
	[SerializeField] private AnimationObject _animationPref;
	[SerializeField] private Transform _textCotainer;
	[SerializeField] private Transform _mapContainer;
	[SerializeField] private FloatedText _textPref;

	[SerializeField] private AnimationClip _nullFlyingAnimation;

	[SerializeField] private Vector3 _offsetPosition;
	
	private Vector3 scale => _mapContainer.localScale;

	public delegate void Play(IAnimateIniter action, GroundCell startedPosition, GroundCell targetPosition, bool flipX);
	public static Play play;
	
	public delegate void Write(Vector3 position, string text, Color color);
	public static Write write;

	public static bool flyAnimationIsActive { get; private set; }

	private Vector3 normalVector = new Vector3(1,0,0);

	private void Start()
	{
		play = PlayAnimation;
		write = WriteText;
	}
	
	private void PlayAnimation(IAnimateIniter action, GroundCell startedPosition, GroundCell targetPosition, bool flipX)
	{
		StartCoroutine(FullAnimationCicle(action, startedPosition, targetPosition, flipX));
	}

	private IEnumerator FullAnimationCicle(IAnimateIniter action, GroundCell startedCell, GroundCell targetCell, bool flipX)
	{
		if (action.flyAnimation != null)
			flyAnimationIsActive = true;

		yield return new WaitForSeconds(action.afterCastSoundDelay);
		
		if (action.onCasterAnimation != null)
		{
			AnimationObject onCasterAnimationObject = InitAnimationObject();

			float angle = GetAngleOfAnimationObject(action, onCasterAnimationObject, startedCell, targetCell);

			if (angle != 0 && action.startedOnCasterAnimationAngle == 0)
				flipX = true;

			StartCoroutine(onCasterAnimationObject.OnCellAnimation(action.onCasterAnimation, startedCell, angle, flipX));
		}

		if (action.flyAnimation == _nullFlyingAnimation)
		{
			yield return new WaitForSeconds(ActionSettings.NULLFLYINGANIMATIONDELAY);
		}
		else if (action.flyAnimation != null)
		{
			AnimationObject flyAnimationObject = InitAnimationObject();

			StartCoroutine(flyAnimationObject.FlyingAnimation(action.flyAnimation, startedCell, targetCell, action.shotFlyingWithCurve));
			yield return new WaitForSeconds(flyAnimationObject.animationTime);
		}

		action.PlaySound(action.contactSound);

		if (action.contactAnimation != null)
		{
			AnimationObject contactAnimationObject = InitAnimationObject();

			StartCoroutine(contactAnimationObject.OnCellAnimation(action.contactAnimation, targetCell, 0,  flipX));
		}

		flyAnimationIsActive = false;
		yield return null;
	}
	
	private AnimationObject InitAnimationObject()
	{
		AnimationObject animationObject = Instantiate(_animationPref, transform);
		animationObject.transform.localScale = scale;
		animationObject.scale = scale.x;
		return animationObject;
	}

	private void WriteText(Vector3 position, string text, Color color)
	{
		if (!DataSaver.GameInLoad)
		{
			FloatedText floatedText = Instantiate(_textPref, _textCotainer);
		
			floatedText.Play(position, text, color);
		}
	}

	private float GetAngleOfAnimationObject(IAnimateIniter action, AnimationObject obj, GroundCell startedPosition, GroundCell targetPosition)
	{
		float angle = 0;
		
		if (action.needToRotateOnCasterAnimationToTarget)
		{
			angle = Vector3.Angle(targetPosition.transform.localPosition - startedPosition.transform.localPosition, normalVector);

			if (targetPosition.transform.localPosition.y - startedPosition.transform.localPosition.y < 0)
				angle *= -1;

		}
		else if (action.startedOnCasterAnimationAngle != 0)
		{
			angle = action.startedOnCasterAnimationAngle;

			if (!startedPosition.onCellObject.spriteFlipped)
				angle *= -1;
		}
		
		return angle;
	}
}
