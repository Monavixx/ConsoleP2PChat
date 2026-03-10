namespace ConsoleP2PChat.Domain;

public record Message(string Text, DateTime ReceivedAt, MessageDirection Direction);
public enum MessageDirection {Incoming, Outcoming}