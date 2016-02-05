CREATE PROCEDURE [$SchemaName$].[SaveEvents]
	@Events [$SchemaName$].[EventsTableType] READONLY
AS
BEGIN
	INSERT INTO [$SchemaName$].[Events] SELECT * FROM @Events
END