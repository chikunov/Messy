CREATE PROCEDURE [$SchemaName$].[GetAggregateSnapshot] (
	@AggregateId UNIQUEIDENTIFIER,
	@MaxVersion INT = NULL)
AS
BEGIN
	IF @MaxVersion IS NULL
		BEGIN
			SELECT TOP 1 
				[AggregateId],
				[AggregateName],
				[Version],
				[Data]
			FROM [$SchemaName$].[Snapshots] 
			WHERE AggregateId = @AggregateId
			ORDER BY [Version] DESC
		END
	ELSE
		BEGIN
			SELECT TOP 1 
				[AggregateId],
				[AggregateName],
				[Version],
				[Data]
			FROM [$SchemaName$].[Snapshots] 
			WHERE AggregateId = @AggregateId AND [Version] <= @MaxVersion
			ORDER BY [Version] DESC
		END
END