namespace OKB.Services.Dtos;

public record OpCode2
(
	int? Op,
	OpCode2_D D
);

public record OpCode2_D
(
	int? NegotiatedRpcVersion
);