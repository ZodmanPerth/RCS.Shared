namespace OKB.Services.Dtos;

public record OpCode1
(
	int? Op,
	OpCode1_D D
);

public record OpCode1_D
(
	int? RpcVersion,
	string? Authentication,
	int? EventSubscriptions
);