namespace SharedDatabase.Contracts;

public record GoalTemplateResponse(Guid Id, string Name, IEnumerable<GoalResponse> Goals);