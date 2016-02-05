CREATE PROCEDURE [$SchemaName$].[IsAggregateExists]
	@AggregateId UNIQUEIDENTIFIER
AS
BEGIN
    SELECT TOP 1 1 FROM [$SchemaName$].[Events] WHERE AggregateId = @AggregateId;
END