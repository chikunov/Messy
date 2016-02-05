CREATE PROCEDURE [$SchemaName$].[GetEventsByAggregateId] (
	@AggregateId UNIQUEIDENTIFIER, 
	@SinceVersion INT, 
	@ToVersion INT = NULL)
AS
BEGIN
	IF @ToVersion IS NOT NULL
		BEGIN
			SELECT 
				[AggregateId],
				[AggregateName],
				[Version],
				[EventName],
				[DateTime],
				[Data]
			FROM [$SchemaName$].[Events] 
			WHERE AggregateId = @AggregateId AND [Version] > @SinceVersion AND [Version] <= @ToVersion
			ORDER BY [Version] ASC
		END
	ELSE
		BEGIN
			SELECT 
				[AggregateId],
				[AggregateName],
				[Version],
				[EventName],
				[DateTime],
				[Data]
			FROM [$SchemaName$].[Events] 
			WHERE AggregateId = @AggregateId AND [Version] > @SinceVersion
			ORDER BY [Version] ASC
		END
END