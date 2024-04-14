public interface IEventRowObject
{
	int index { get; }
	int turnOfActivation { get; }
	bool isEnded { get; }
	bool isMission { get; }
	bool isFailed { get; }
}
