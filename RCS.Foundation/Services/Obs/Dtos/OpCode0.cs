namespace RCS.Services.Dtos;

public record OpCode0
(
	int? Op,
	OpCode0_D D
);

public record OpCode0_Authentication
(
	string Challenge,
	string Salt
);

public record OpCode0_D
(
	OpCode0_Authentication Authentication,
	string ObsStudioVersion,
	string ObsWebSocketVersion,
	int? RpcVersion
);